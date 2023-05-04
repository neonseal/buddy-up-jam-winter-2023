using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameData;
using GameLoop;

// Primary Mending Repair Game Class
// Handles generation and updating state during mini games
public class MendingGames : MonoBehaviour {
    // Game Active state to ensure we don't start extra games on accident
    public bool gameInProgress { get; private set; }

    [Header("Game Component Rendering")]
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private GameObject dashPrefab;
    [Range(0.01f, 1f)]
    [SerializeField] private float dashSize;
    [Range(0.1f, 2f)]
    [SerializeField] private float delta;
    private SpriteRenderer spriteRenderer;

    [Header("Game Component Collections")]
    // Set of nodes the player must make contact with while completing a dashed line
    List<GameObject> nodes;
    // Set of dashes making up the line to be followed
    List<List<GameObject>> dashSets;
    // Current type of plushie damange being repaired
    PlushieDamage currentPlushieDamage;

    [Header("Game Progression Variables")]
    int targetNodeIndex;
    int activeDashSetIndex;
    bool linePreviouslyReset;
    ToolType requiredToolType;

    private void Awake() {
        // Instantiate lists
        nodes = new List<GameObject>();
        dashSets = new List<List<GameObject>>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set rendering variables 
        dashSize = 0.1f;
        delta = 0.5f;

        // Set progression variables
        targetNodeIndex = 0;
        activeDashSetIndex = 0;
        linePreviouslyReset = false;

        // Setup mending game event listeners
        MendingGameEventManager.Current.onNodeTriggered += HandleNodeTrigger;
    }

    public void CreateSewingGame(List<Vector3> targetPositions, PlushieDamage plushieDamage) {
        PlushieScriptableObject currentPlushie = GameLoopManager.currentPlushieScriptableObject;

        currentPlushieDamage = plushieDamage;
        gameInProgress = true;
        requiredToolType = ToolType.Needle;

        this.spriteRenderer.sprite = currentPlushie.damageSprite;
        int zRotation = currentPlushie.spriteZRotationValue;

        // Reset node collection
        if (nodes.Count > 0 || dashSets.Count > 0) {
            ResetAllElements();
        }

        // Create the set of nodes for this game
        nodes = GenerateNodes(targetPositions);


        // Generate dashes in between each pair of nodes
        for (int i = 0; i < nodes.Count - 1; i++) {
            Vector3 startingNodePos = nodes[i].transform.position;
            Vector3 endingNodePos = nodes[i + 1].transform.position;
            List<Vector3> dashPositions = GenerateDashPositions(startingNodePos, endingNodePos);
            List<GameObject> dashes = RenderLine(dashPositions, startingNodePos, endingNodePos);

            // Add to total dash set 
            dashSets.Add(dashes);
        }
    }

    /* Clear all game elements and reset indexes */
    public void ResetAllElements() {
        // Destroy elements on screen
        for (int i = nodes.Count - 1; i >= 0; i--) {
            Destroy(nodes[i]);
        }
        for (int j = dashSets.Count - 1; j >= 0; j--) {
            for (int k = dashSets[j].Count - 1; k >= 0; k--) {
                Destroy(dashSets[j][k]);
            }
        }

        // Clear element collections
        nodes.Clear();
        dashSets.Clear();

        // Reset target index
        targetNodeIndex = 0;
    }

    /* Instantiate new node prefabs at the designated positions,
    *  setting up the starting node */
    private List<GameObject> GenerateNodes(List<Vector3> positions) {
        List<GameObject> outputNodes = new List<GameObject>();

        for (int i = 0; i < positions.Count; i++) {
            GameObject node = Instantiate(targetPrefab, positions[i], Quaternion.identity, this.transform);

            // Set starting node as first target
            if (i == 0) {
                node.GetComponent<Node>().targetNode = true;
            }
            node.GetComponent<Node>().requiredToolType = this.requiredToolType;
            outputNodes.Add(node);
        }

        return outputNodes;
    }

    /* Calculate dash positions between pairs of nodes where we will generate dash objects */
    private List<Vector3> GenerateDashPositions(Vector3 start, Vector3 end) {
        List<Vector3> positions = new List<Vector3>();
        // Triangulate a straight line between both point
        Vector3 direction = (end - start).normalized;
        Vector3 dash = start += (direction * delta);


        // Incrementally calculate dash positions until we reach the end position
        while ((end - start).magnitude > (dash - start + (direction * delta * 0.65f)).magnitude) {
            // If within threshold of the ending position

            positions.Add(dash);
            dash += (direction * delta);
        }

        return positions;
    }

    private List<GameObject> RenderLine(List<Vector3> dashPositions, Vector3 start, Vector3 end) {
        List<GameObject> dashes = new List<GameObject>();

        // Instantiate line of dashes between nodes
        foreach (Vector3 dashPosition in dashPositions) {
            GameObject dashObject = GenerateDash(start, end);
            dashObject.transform.position = dashPosition;
            dashes.Add(dashObject);
        }

        return dashes;
    }

    /* Instantiate a new Dash object and calculate rotation between given node posisitons */
    private GameObject GenerateDash(Vector3 startingNodePos, Vector3 endingNodePos) {
        // Generate base object
        GameObject gameObject = new GameObject();
        gameObject.name = "DashComponent";
        gameObject.transform.localScale = Vector3.one * dashSize;
        gameObject.transform.parent = this.transform;

        // Calculate the appropriate rotation for the given node positions
        Vector3 normalizedNode = (endingNodePos - startingNodePos).normalized;
        float angle = Mathf.Atan2(normalizedNode.x, normalizedNode.y) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(Vector3.forward * (-Mathf.Sign(endingNodePos.x) * angle));

        // Add dash sprite
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/dash");
        spriteRenderer.sortingLayerName = this.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.color = Color.black;

        // Add collider for completing mini game
        BoxCollider2D dashCollider = gameObject.AddComponent<BoxCollider2D>();
        dashCollider.size = new Vector2(3f, 2f);

        // Add dash C# class
        Dash dash = gameObject.AddComponent<Dash>();
        dash.requiredToolType = this.requiredToolType;

        return gameObject;
    }

    /* When a given node is triggered, we need to update the goal node and 
    *  corresponding set of dashes */
    private void HandleNodeTrigger(Node triggeredNode) {
        // Check if we are activating the first node to start the game or restarting a reset line
        if (targetNodeIndex == 0 || linePreviouslyReset) {
            // Set all dashes in new set to active
            dashSets[activeDashSetIndex].ForEach(d => d.GetComponent<Dash>().Activate());
            nodes[targetNodeIndex].GetComponent<Node>().TriggerNode(false);

            targetNodeIndex++;
            nodes[targetNodeIndex].GetComponent<Node>().targetNode = true;

            if (linePreviouslyReset) {
                linePreviouslyReset = false;
            }
        } else {
            // Triggered target node -> check if active line was completed >= 80%
            if (CalculatePercentComplete() >= 0.8f) {
                // Make sure all dashes are colored in
                dashSets[activeDashSetIndex].ForEach(d => d.GetComponent<Dash>().spriteRenderer.color = Color.yellow);

                // Line complete -> Check if final node was triggered to complete game
                if (targetNodeIndex == nodes.Count - 1) {
                    // Update current target node
                    nodes[targetNodeIndex].GetComponent<Node>().TriggerNode(false);
                    MendingGameEventManager.Current.MendingGameComplete(currentPlushieDamage);
                } else {
                    // Update current target node
                    nodes[targetNodeIndex].GetComponent<Node>().TriggerNode(false);

                    // Increment targets
                    activeDashSetIndex++;
                    dashSets[activeDashSetIndex].ForEach(d => d.GetComponent<Dash>().Activate());

                    targetNodeIndex++;
                    nodes[targetNodeIndex].GetComponent<Node>().targetNode = true;
                }
            } else {
                // Line not completed sufficiently -> Reset line and target node
                ResetDashSet(activeDashSetIndex, true);

                // Update current target node
                nodes[targetNodeIndex].GetComponent<Node>().Reset(false);

                // Decrement target node to reset line and previous node as target
                targetNodeIndex--;
                nodes[targetNodeIndex].GetComponent<Node>().Reset(true);

                linePreviouslyReset = true;
            }
        }
    }

    private float CalculatePercentComplete() {
        // Get current active dash set
        List<GameObject> dashes = dashSets[activeDashSetIndex];
        // Get how many dashes were triggered
        float countComplete = dashes.Where(d => d.GetComponent<Dash>().triggered == true).Count();
        // Calculate percentage based on total number of dashes in line
        return countComplete / dashes.Count();
    }

    /* Reset a given set of dashes */
    private void ResetDashSet(int index, bool active) {
        Debug.Assert(index < dashSets.Count);

        foreach (GameObject dash in dashSets[index]) {
            dash.GetComponent<Dash>().Reset(active);
        }
    }
}
