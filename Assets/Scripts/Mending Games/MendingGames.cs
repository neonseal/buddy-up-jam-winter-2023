using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MendingGames: MonoBehaviour
{
    private SpriteRenderer lenseSpriteRenderer;
    private List<Vector3> dashPositions;

    [SerializeField] private Button drawLineButton;
    [SerializeField] private List<Dash> dashes;
    [Range(0.01f, 1f)]
    [SerializeField] private float dashSize;
    [Range(0.1f, 2f)]
    [SerializeField] private float delta;

    private bool gameComplete;

    private void Awake() {
        this.lenseSpriteRenderer = GetComponent<SpriteRenderer>();
        dashes = new List<Dash>();
        dashPositions = new List<Vector3>();

        dashSize = 0.125f;
        delta = 0.5f;
    }

    private void Start() {
        drawLineButton.onClick.AddListener(CreateSewingMiniGame);
    }

    private void Update() {
        // Toggle mouse held state
        if (Input.GetMouseButton(0))
            CustomEventManager.Current.mouseHoldStatusToggle(true);
        else
            CustomEventManager.Current.mouseHoldStatusToggle(false);

        // Check for complete dash collections
        if (dashes.Count > 0 && !gameComplete) {
            checkIfAllDashesComplete();
        }
    }

    private void checkIfAllDashesComplete() {
        var incomplete = dashes.Where(dash => dash.Complete == false).ToList();
        if (incomplete.Count == 0) {
            gameComplete = true;
            //CustomEventManager.Current.repairCompletionEvent();
        }
    }
    private Dash GenerateDash() {
        // Generate base object
        GameObject gameObject = new GameObject();
        gameObject.transform.localScale = Vector3.one * dashSize;
        gameObject.transform.parent = this.transform;

        // Add dash sprite
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/dash");
        spriteRenderer.sortingLayerName = this.lenseSpriteRenderer.sortingLayerName;
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.color = Color.black;

        // Add collider for completing mini game
        BoxCollider2D addCollider = gameObject.AddComponent<BoxCollider2D>();

        // Add dash C# class
        Dash dash = gameObject.AddComponent<Dash>();

        return dash;
    }

    private void DestroyAllDashes() {
        foreach (Dash dash in dashes) {
            Destroy(dash);
        }
        dashPositions.Clear();
    }

    private void GenerateTarget(Vector3 targetPosition) {
        GameObject targetObject = new GameObject();
        targetObject.transform.localPosition = targetPosition;
        targetObject.transform.localScale = new Vector3(3f, 3f);
        SpriteRenderer targetSprite = targetObject.AddComponent<SpriteRenderer>();
        targetSprite.sprite = Resources.Load<Sprite>("Sprites/target");
        targetSprite.color = Color.black;
        targetSprite.sortingLayerName = this.lenseSpriteRenderer.sortingLayerName;
        targetSprite.sortingOrder = 10;
        targetObject.transform.SetParent(this.lenseSpriteRenderer.transform);
    }

    private void GenerateDashPositions(Vector3 start, Vector3 end) {
        // Triangulate a straight line between both point
        Vector3 direction = (end - start).normalized;
        Vector3 dash = start += (direction * delta);
        while ((end - start).magnitude > (dash - start).magnitude) {
            dashPositions.Add(dash);
            dash += (direction * delta);
        }
    }

    private void RenderLine() {
        foreach (Vector3 dashPosition in dashPositions) {
            Dash dashObject = GenerateDash();
            dashObject.transform.position = dashPosition;
            dashes.Add(dashObject);
        }
    }

    private void CreateSewingMiniGame() {
        DestroyAllDashes();

        // Define two points
        Vector3 startingPoint = new Vector3(-2, 0, -1);
        Vector3 endingPoint = new Vector3(2, 0, -1);

        // Generate two target sprites representing the target points
        this.GenerateTarget(startingPoint);
        this.GenerateTarget(endingPoint);

        GenerateDashPositions(startingPoint, endingPoint);

        RenderLine();
    }
}
