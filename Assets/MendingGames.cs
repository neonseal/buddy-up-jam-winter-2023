using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MendingGames: MonoBehaviour
{
    private SpriteRenderer lenseSpriteRenderer;
    private LineRenderer lineRenderer;
   
    [Range(0.01f, 1f)]
    [SerializeField] private float dashSize;
    [Range(0.1f, 2f)]
    [SerializeField] private float delta;

    private void Awake() {
        this.lenseSpriteRenderer = GetComponent<SpriteRenderer>();
        this.lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start() {
        CreateSewingMiniGame();
    }

    private GameObject GenerateDash() {
        GameObject dash = new GameObject();
        dash.transform.localScale = Vector3.one * dashSize;
        dash.transform.parent = this.transform;

        SpriteRenderer spriteRenderer = dash.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/dash");
        return dash;
    }

    private void GenerateTarget(Vector3 targetPosition) {
        GameObject startingObject = new GameObject();
        startingObject.transform.localPosition = targetPosition;
        startingObject.transform.localScale = new Vector3(3f, 3f);
        SpriteRenderer targetSprite = startingObject.AddComponent<SpriteRenderer>();
        targetSprite.sprite = Resources.Load<Sprite>("Sprites/target");
        targetSprite.color = Color.black;
        targetSprite.sortingLayerName = this.lenseSpriteRenderer.sortingLayerName;
        targetSprite.sortingOrder = 10;
        startingObject.transform.SetParent(this.lenseSpriteRenderer.transform);
    }

    private void CreateSewingMiniGame() {
        // Define two points
        Vector3 startingPoint = new Vector3(-2, 0, -1);
        Vector3 endingPoint = new Vector3(2, 0, -1);

        // Generate two target sprites representing the target points
        this.GenerateTarget(startingPoint);
        this.GenerateTarget(endingPoint);

        // Triangulate a straight line between both point
        List<Vector3> dashPositions = new List<Vector3>();
        Vector3 dash = startingPoint;
        Vector3 direction = (endingPoint - startingPoint).normalized;
        while ((endingPoint - startingPoint).magnitude > (dash - startingPoint).magnitude) {
            dashPositions.Add(dash);
            dash += (direction * delta);
        }
        Debug.Log(dashPositions);


        this.lineRenderer.sortingLayerName = this.lenseSpriteRenderer.sortingLayerName;
        this.lineRenderer.sortingOrder = 11;
        Vector3[] lineArray = new Vector3[2] { startingPoint, endingPoint };
        this.lineRenderer.positionCount = 2;
        this.lineRenderer.startWidth = 0.2f;
        this.lineRenderer.endWidth = 0.2f;
        this.lineRenderer.SetPositions(lineArray);
        // Iteratively render lines until you reach that point
        // Add each set of lines into a bucketed collection
    }
}
