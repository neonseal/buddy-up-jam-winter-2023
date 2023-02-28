using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MendingGames: MonoBehaviour
{
    private SpriteRenderer lenseSpriteRenderer;
    private LineRenderer lineRenderer;
    private List<Vector3> dashPositions;

    [SerializeField] private Button drawLineButton;
    [SerializeField] private List<GameObject> dashes;
    [Range(0.01f, 1f)]
    [SerializeField] private float dashSize;
    [Range(0.1f, 2f)]
    [SerializeField] private float delta;

    private void Awake() {
        this.lenseSpriteRenderer = GetComponent<SpriteRenderer>();
        this.lineRenderer = GetComponent<LineRenderer>();
        dashes = new List<GameObject>();
        dashPositions = new List<Vector3>();

        dashSize = 0.125f;
        delta = 0.5f;
    }

    private void Start() {
        drawLineButton.onClick.AddListener(CreateSewingMiniGame);
    }

    private GameObject GenerateDash() {
        // Generate base object
        GameObject dash = new GameObject();
        dash.transform.localScale = Vector3.one * dashSize;
        dash.transform.parent = this.transform;

        // Add dash sprite
        SpriteRenderer spriteRenderer = dash.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/dash");
        spriteRenderer.sortingLayerName = this.lenseSpriteRenderer.sortingLayerName;
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.color = Color.black;

        // Add collider for completing mini game
        dash.AddComponent<BoxCollider2D>();

        // Add dash C# class

        return dash;
    }

    private void DestroyAllDashes() {
        foreach (GameObject dash in dashes) {
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
            GameObject dashObject = GenerateDash();
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
