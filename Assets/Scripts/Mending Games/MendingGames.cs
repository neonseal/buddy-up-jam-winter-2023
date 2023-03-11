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
    /* Starting target collider to begin game */
    private BoxCollider2D startingTargetCollider;

    /* Dash Rendering Variables */
    [Range(0.01f, 1f)]
    [SerializeField] private float dashSize;
    [Range(0.1f, 2f)]
    [SerializeField] private float delta;

    [SerializeField] private GameObject targetPrefab;

    private void Awake() {
        lenseSpriteRenderer = GetComponent<SpriteRenderer>();
        dashSetCollections = new List<List<Dash>>();

        dashSize = 0.1f;
        delta = 0.5f;
    }

    void Update() {
        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0)) {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, check what we hit
            if (hit.collider != null) {
                if (hit.collider.name == targetColliderName && hit.collider == startingTargetCollider) {
                    Debug.Log(hit.collider.name);
                } 
            }
        }
    }

    public void GenerateNewSewingGame(List<Vector3> targetPositions) {
        // Create targets and dashed lines
        for (int i = 0; i < targetPositions.Count; i++) {
            // Establish target points
            GenerateTarget(targetPositions[i], i == 0);

            // Generate dash positions between each pair of targets and render to screen
            if (i != targetPositions.Count - 1) {
                List<Vector3> dashPositions = GenerateDashPositions(targetPositions[i], targetPositions[i + 1]);
                List<Dash> dashes = RenderLine(dashPositions, targetPositions[i], targetPositions[i + 1]);
                dashSetCollections.Add(dashes);
            }
        }

    }

    private void GenerateTarget(Vector3 targetPosition, bool startingTarget = false) {
        GameObject target = Instantiate(targetPrefab, targetPosition, Quaternion.identity, this.transform);
        BoxCollider2D targetCollider = target.GetComponent<BoxCollider2D>();
        targetCollider.name = targetColliderName;
        if (startingTarget) {
            startingTargetCollider = targetCollider;
        }
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
