using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using GameData;
using GameUI;


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

    [Header("Game Component Collections")]
    // Set of nodes the player must make contact with while completing a dashed line
    List<GameObject> nodes;
    // Set of dashes making up the line to be followed
    List<List<GameObject>> dashSets;

    [Header("Game Progression Variables")]
    int targetNodeIndex;
    ToolType requiredToolType;

    private void Awake() {
        // Instantiate lists
        nodes = new List<GameObject>();
        dashSets = new List<List<GameObject>>();

        // Set rendering variables 
        dashSize = 0.1f;
        delta = 0.5f;

        // Set progression variables
        targetNodeIndex = 0;

        // Setup mending game event listeners
        MendingGameEventManager.Current.onNodeTriggered += HandleNodeTrigger;
    }

    private void Update() {

    }

    public void CreateSewingGame(List<Vector3> targetPositions) {
        gameInProgress = true;
        requiredToolType = ToolType.Needle;

        // Reset node collection
        if (nodes.Count > 0 || dashSets.Count > 0) {
            Reset();
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


    /* Instantiate new node prefabs at the designated positions,
    *  setting up the starting node */
    private List<GameObject> GenerateNodes(List<Vector3> positions) {
        List<GameObject> outputNodes = new List<GameObject>();

        for (int i = 0; i < positions.Count; i++) {
            GameObject node = Instantiate(targetPrefab, positions[i], Quaternion.identity, this.transform);

            if (i == 0) {
                node.GetComponent<Node>().targetNode = true;
            }
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
        // Check if line of dashes completed successfully

        // Update current target node
        nodes[targetNodeIndex].GetComponent<SpriteRenderer>().color = Color.blue;
        nodes[targetNodeIndex].GetComponent<Node>().targetNode = false;

        // Check if the last node was triggered - end mini game
        if (targetNodeIndex == nodes.Count - 1) {
            MendingGameEventManager.Current.MendingGameComplete();
        } else {
            // Activate corresponding line of dashes
            dashSets[targetNodeIndex].ForEach(dObj => dObj.GetComponent<Dash>().dashActive = true);

            // Increment target node
            targetNodeIndex++;

            // Update target node properties
            nodes[targetNodeIndex].GetComponent<Node>().targetNode = true;
        }
    }

    /* Clear all game elements and reset indexes */
    private void Reset() {
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












    /* const string targetColliderName = "GameNode";
     const string dashColliderName = "DashComponent";

     *//* Plushie damage component to be repaired and passed on when game is complete *//*
     private PlushieDamage plushieDamage;
     *//* Required tool to perform repair *//*
     private ToolType requiredToolType;
     *//* Lense Sprite on which we'll render the dashed line *//*
     private SpriteRenderer lenseSpriteRenderer;
     *//* Collection of dashed lines, each represented by a list of Dash objects *//*
     private List<List<Dash>> dashSetCollections;
     *//* Collection of game node colliders *//*
     private List<BoxCollider2D> targetColliders;
     *//* Starting and ending node collider to begin game *//*
     private BoxCollider2D startingNodeCollider;
     private BoxCollider2D endingingNodeCollider;

     *//* Dash Rendering Variables *//*
     [Range(0.01f, 1f)]
     [SerializeField] private float dashSize;
     [Range(0.1f, 2f)]
     [SerializeField] private float delta;

     [SerializeField] private GameObject targetPrefab;

     // Gameplay variables
     private int activeDashSetIndex;
     private int nextNodeIndex;
     private bool gameActive;

     private void Awake() {
         lenseSpriteRenderer = GetComponent<SpriteRenderer>();
         dashSetCollections = new List<List<Dash>>();
         targetColliders = new List<BoxCollider2D>();

         dashSize = 0.1f;
         delta = 0.5f;

         activeDashSetIndex = -1;
         nextNodeIndex = 0;
         gameActive = false;
     }

     void Update() {
         // Get the mouse position on the screen and send a raycast into the game world from that position.
         Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

         //If the left mouse button is clicked.
         if (Input.GetMouseButtonDown(0) && hit.collider != null) {
             TryStartNextDashSection(hit);
         }

         // When the left mouse button is lifed, check collider
         if (Input.GetMouseButtonUp(0) && gameActive) {
             // If on a node, we check if the dash set is complete
             if (hit.collider != null) {
                 CheckDashSectionCompletion(hit);
             } else {
                 // If released off a line or node collider, reset current line
                 foreach (Dash dash in dashSetCollections[activeDashSetIndex]) {
                     dash.Reset();
                 }
             }
         }

     }

     private void TryStartNextDashSection(RaycastHit2D hit) {
         // Hit taret point, check which state
         if (hit.collider.name == targetColliderName) {
             // Check if player is holding the correct tool
             if (CanvasManager.currentTool == null || CanvasManager.toolType == this.requiredToolType) {
                 // Player clicks starting node - start game
                 if (hit.collider.Equals(startingNodeCollider) && !gameActive) {
                     // Activate first dash collection
                     gameActive = true;
                     activeDashSetIndex++;
                     nextNodeIndex++;
                     hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                     dashSetCollections[activeDashSetIndex].ForEach(dash => dash.Active = true);
                 } else if (hit.collider.Equals(targetColliders[nextNodeIndex]) && nextNodeIndex != targetColliders.Count - 1) {
                     // Player is starting the next line
                     activeDashSetIndex++;
                     nextNodeIndex++;
                     dashSetCollections[activeDashSetIndex].ForEach(dash => dash.Active = true);
                 }
             } else {
                 // Show player tool tip to remind them to use the correct tool
             }
         }
     }

     private void CheckDashSectionCompletion(RaycastHit2D hit) {
         if (hit.collider.name == targetColliderName && hit.collider.Equals(targetColliders[nextNodeIndex])) {
             bool lineComplete = CheckCurrentLineStatus();
             if (lineComplete) {
                 // Complete any incomplete dashes in current line
                 foreach (Dash dash in dashSetCollections[activeDashSetIndex].Where(dash => !dash.Complete).ToList()) {
                     dash.CompleteDash();
                 }
                 hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

                 // Check if the node index was the final node to complete the game
                 if (hit.collider.Equals(endingingNodeCollider)) {
                     // Triggger game complete
                     DamageLifeCycleEventManager.Current.repairDamage_Complete(this.plushieDamage);
                 }
             }
         }

     }

     private bool CheckCurrentLineStatus() {
         bool complete = false;
         // If a dash set is active, check if > 90% of dashes are complete
         if (gameActive) {
             List<Dash> currentDashSet = dashSetCollections[activeDashSetIndex];
             int countComplete = currentDashSet.Where(dash => dash.Complete == true).ToList().Count;
             float percentComplete = (float)countComplete / currentDashSet.Count;
             complete = percentComplete >= 0.8f;
         }
         return complete;
     }

     public void GenerateNewSewingGame(List<Vector3> targetPositions, PlushieDamage damage) {
         this.plushieDamage = damage;
         this.requiredToolType = ToolType.Needle;

         // Clear game elements if necessary
         if (dashSetCollections.Count > 0) {
             DestroyAllGameElements();
         }

         // Create nodes and dashed lines
         for (int i = 0; i < targetPositions.Count; i++) {
             // Establish node points
             GenerateNode(targetPositions[i], i == 0, i == targetPositions.Count - 1);

             // Generate dash positions between each pair of nodes and render to screen
             if (i != targetPositions.Count - 1) {
                 List<Vector3> dashPositions = GenerateDashPositions(targetPositions[i], targetPositions[i + 1]);
                 List<Dash> dashes = RenderLine(dashPositions, targetPositions[i], targetPositions[i + 1]);
                 dashSetCollections.Add(dashes);
             }
         }
     }
     public void DestroyAllGameElements() {
         foreach (List<Dash> dashes in dashSetCollections) {
             foreach (Dash dash in dashes) {
                 Debug.Log("DESTROYING");
                 Destroy(dash);
             }
         }
         dashSetCollections = new List<List<Dash>>();
         targetColliders = new List<BoxCollider2D>();
     }

     private void GenerateNode(Vector3 targetPosition, bool startingNode = false, bool finalNode = false) {
         GameObject node = Instantiate(targetPrefab, targetPosition, Quaternion.identity, this.transform);
         BoxCollider2D targetCollider = node.GetComponent<BoxCollider2D>();
         targetCollider.name = targetColliderName;

         // Set start and end points
         if (startingNode) {
             startingNodeCollider = targetCollider;
         } else if (finalNode) {
             endingingNodeCollider = targetCollider;
         }

         targetColliders.Add(node.GetComponent<BoxCollider2D>());
     }

     private List<Vector3> GenerateDashPositions(Vector3 start, Vector3 end) {
         List<Vector3> positions = new List<Vector3>();
         // Triangulate a straight line between both point
         Vector3 direction = (end - start).normalized;
         Vector3 dash = start += (direction * delta);


         // Incrementally calculate dash positions until we reach the end position
         while ((end - start).magnitude > (dash - start + (direction * delta * 0.45f)).magnitude) {
             // If within threshold of the ending position

             positions.Add(dash);
             dash += (direction * delta);
         }

         return positions;
     }

     private List<Dash> RenderLine(List<Vector3> dashPositions, Vector3 start, Vector3 end) {
         List<Dash> dashes = new List<Dash>();
         foreach (Vector3 dashPosition in dashPositions) {
             Dash dashObject = GenerateDash(start, end);
             dashObject.transform.position = dashPosition;
             dashes.Add(dashObject);
         }
         return dashes;
     }

     private Dash GenerateDash(Vector3 start, Vector3 end) {
         // Generate base object
         GameObject gameObject = new GameObject();
         gameObject.name = "DashComponent";
         gameObject.transform.localScale = Vector3.one * dashSize;
         gameObject.transform.parent = this.transform;


         Vector3 normalizedNode = (end - start).normalized;
         float angle = Mathf.Atan2(normalizedNode.x, normalizedNode.y) * Mathf.Rad2Deg;
         gameObject.transform.rotation = Quaternion.Euler(Vector3.forward * (-Mathf.Sign(end.x) * angle));

         // Add dash sprite
         SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
         spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/dash");
         spriteRenderer.sortingLayerName = this.lenseSpriteRenderer.sortingLayerName;
         spriteRenderer.sortingOrder = 10;
         spriteRenderer.color = Color.black;

         // Add collider for completing mini game
         BoxCollider2D dashCollider = gameObject.AddComponent<BoxCollider2D>();
         dashCollider.size = new Vector2(3f, 2f);
         dashCollider.name = dashColliderName;

         // Add dash C# class
         Dash dash = gameObject.AddComponent<Dash>();
         dash.RequiredToolType = this.requiredToolType;

         return dash;
     }*/
}
