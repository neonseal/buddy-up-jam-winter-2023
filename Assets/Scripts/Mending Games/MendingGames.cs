using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

// Primary Mending Repair Game Class
// Handles generation and updating state during mini games
public class MendingGames : MonoBehaviour {
    const string targetColliderName = "GameTarget";
    const string dashColliderName = "DashComponent";

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
    private bool lineComplete;

    private void Awake() {
        lenseSpriteRenderer = GetComponent<SpriteRenderer>();
        dashSetCollections = new List<List<Dash>>();
        targetColliders = new List<BoxCollider2D>();

        dashSize = 0.1f;
        delta = 0.5f;

        activeDashSetIndex = -1;
        nextTargetIndex = 0;
        gameActive = false;
        lineComplete = false;
    }

    void Update() {
        // Get the mouse position on the screen and send a raycast into the game world from that position.
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0) && hit.collider != null) {

            // Hit taret point, check which state
            if (hit.collider.name == targetColliderName) {
                // Player clicks starting target - start game
                if (hit.collider.Equals(startingTargetCollider) && !gameActive) {
                    // Activate first dash collection
                    gameActive = true;
                    activeDashSetIndex++;
                    nextTargetIndex++;
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    dashSetCollections[activeDashSetIndex].ForEach(dash => dash.Active = true);
                } else if (hit.collider.Equals(targetColliders[nextTargetIndex])) {
                    // Player is starting the next line
                    activeDashSetIndex++;
                    nextTargetIndex++;
                    dashSetCollections[activeDashSetIndex].ForEach(dash => dash.Active = true);
                }
            }
        }
        
        // When the left mouse button is lifed, if on a target, we check if the dash set is complete
        if (Input.GetMouseButtonUp(0) && hit.collider != null) {
            if (hit.collider.name == targetColliderName && hit.collider.Equals(targetColliders[nextTargetIndex])) {
                if (lineComplete) {
                    // Complete any incomplete dashes in current line
                    foreach (Dash dash in dashSetCollections[activeDashSetIndex].Where(dash => !dash.Complete).ToList()) {
                        dash.CompleteDash();
                    }
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

                    // Check if the target index was the final target to complete the game
                    if (hit.collider.Equals(endingingTargetCollider)) {
                        // Triggger game complete
                        Debug.Log("GAME COMPLETE");
                    }
                }
            } 
        }

        // If a dash set is active, check if > 90% of dashes are complete
        if (gameActive) {
            List<Dash> currentDashSet = dashSetCollections[activeDashSetIndex];
            int countComplete = currentDashSet.Where(dash => dash.Complete == true).ToList().Count;
            float percentComplete = (float)countComplete / currentDashSet.Count;
            lineComplete = percentComplete >= 0.8f;
        }
    }

    public void GenerateNewSewingGame(List<Vector3> targetPositions) {
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
        while ((end - start).magnitude > (dash - start).magnitude) {
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

        return dash;
    }


    /* 
     private PlushieDamage plushieDamage;


     private bool gameComplete;


     private void Update() {
         // Toggle mouse held state
         if (Input.GetMouseButton(0)) {
             CustomEventManager.Current.mouseHoldStatusToggle(true);
         } else {
             CustomEventManager.Current.mouseHoldStatusToggle(false);
         }

         // Check for complete dash collections
         if (dashes.Count > 0 && !gameComplete) {
             checkIfAllDashesComplete();
         }
     }

     private void checkIfAllDashesComplete() {
         var incomplete = dashes.Where(dash => dash.Complete == false).ToList();
         if (incomplete.Count == 0) {
             gameComplete = true;
             CustomEventManager.Current.repairCompletionEvent(this.plushieDamage);
         }
     }

     private void DestroyAllDashes() {
         foreach (Dash dash in dashes) {
             Destroy(dash);
         }
         dashPositions.Clear();
     }
    */
}
