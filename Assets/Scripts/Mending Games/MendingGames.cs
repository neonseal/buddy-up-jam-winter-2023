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
    const string targetColliderName = "GameTarget";
    const string dashColliderName = "DashComponent";

    /* Plushie damage component to be repaired and passed on when game is complete */
    private PlushieDamage plushieDamage;
    /* Required tool to perform repair */
    private ToolType requiredToolType;
    /* Lense Sprite on which we'll render the dashed line */
    private SpriteRenderer lenseSpriteRenderer;
    /* Collection of dashed lines, each represented by a list of Dash objects */
    private List<List<Dash>> dashSetCollections;
    /* Collection of game target colliders */
    private List<BoxCollider2D> targetColliders;
    /* Starting and ending target collider to begin game */
    private BoxCollider2D startingTargetCollider;
    private BoxCollider2D endingingTargetCollider;

    /* Dash Rendering Variables */
    [Range(0.01f, 1f)]
    [SerializeField] private float dashSize;
    [Range(0.1f, 2f)]
    [SerializeField] private float delta;

    [SerializeField] private GameObject targetPrefab;

    // Gameplay variables
    private int activeDashSetIndex;
    private int nextTargetIndex;
    private bool gameActive;

    private void Awake() {
        lenseSpriteRenderer = GetComponent<SpriteRenderer>();
        dashSetCollections = new List<List<Dash>>();
        targetColliders = new List<BoxCollider2D>();

        dashSize = 0.1f;
        delta = 0.5f;

        activeDashSetIndex = -1;
        nextTargetIndex = 0;
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
            // If on a target, we check if the dash set is complete
            if (hit.collider != null) {
                CheckDashSectionCompletion(hit);
            } else {
                // If released off a line or target collider, reset current line
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
                // Player clicks starting target - start game
                if (hit.collider.Equals(startingTargetCollider) && !gameActive) {
                    // Activate first dash collection
                    gameActive = true;
                    activeDashSetIndex++;
                    nextTargetIndex++;
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    dashSetCollections[activeDashSetIndex].ForEach(dash => dash.Active = true);
                } else if (hit.collider.Equals(targetColliders[nextTargetIndex]) && nextTargetIndex != targetColliders.Count - 1) {
                    // Player is starting the next line
                    activeDashSetIndex++;
                    nextTargetIndex++;
                    dashSetCollections[activeDashSetIndex].ForEach(dash => dash.Active = true);
                }
            } else {
                // Show player tool tip to remind them to use the correct tool
            }
        }
    }

    private void CheckDashSectionCompletion(RaycastHit2D hit) {
        if (hit.collider.name == targetColliderName && hit.collider.Equals(targetColliders[nextTargetIndex])) {
            bool lineComplete = CheckCurrentLineStatus();
            if (lineComplete) {
                // Complete any incomplete dashes in current line
                foreach (Dash dash in dashSetCollections[activeDashSetIndex].Where(dash => !dash.Complete).ToList()) {
                    dash.CompleteDash();
                }
                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

                // Check if the target index was the final target to complete the game
                if (hit.collider.Equals(endingingTargetCollider)) {
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

        // Create targets and dashed lines
        for (int i = 0; i < targetPositions.Count; i++) {
            // Establish target points
            GenerateTarget(targetPositions[i], i == 0, i == targetPositions.Count - 1);

            // Generate dash positions between each pair of targets and render to screen
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

    private void GenerateTarget(Vector3 targetPosition, bool startingTarget = false, bool finalTarget = false) {
        GameObject target = Instantiate(targetPrefab, targetPosition, Quaternion.identity, this.transform);
        BoxCollider2D targetCollider = target.GetComponent<BoxCollider2D>();
        targetCollider.name = targetColliderName;

        // Set start and end points
        if (startingTarget) {
            startingTargetCollider = targetCollider;
        } else if (finalTarget) {
            endingingTargetCollider = targetCollider;
        }

        targetColliders.Add(target.GetComponent<BoxCollider2D>());
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


        Vector3 normalizedTarget = (end - start).normalized;
        float angle = Mathf.Atan2(normalizedTarget.x, normalizedTarget.y) * Mathf.Rad2Deg;
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
    }
}
