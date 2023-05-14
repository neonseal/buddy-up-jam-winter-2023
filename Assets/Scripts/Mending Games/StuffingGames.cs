using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using GameUI;

public class StuffingGames : MonoBehaviour {
    [Header("Texture Variables")]
    [SerializeField] private Texture2D mainTex;
    [SerializeField] private Texture2D unstuffedTexBase;
    [SerializeField] private Texture2D stuffingBrush;
    [SerializeField] private Material material;
    [SerializeField] private Material defaultMaterial;

    [Header("Game Components")]
    [SerializeField] GameObject stuffingTargetPrefab;
    [SerializeField] float texturePosModifier = 3f;
    List<StuffingTarget> targets;
    PlushieDamage currentPlushieDamage;

    [Header("State Management Variables")]
    private Texture2D stuffingMaskTexture;
    private Vector2Int lastPaintPixelPosition;
    private float unstuffedAreaTotal;
    private float unstuffedAreaCurrent;
    private bool gameActive;
    private ToolType requiredToolType;

    private MendingGames mendingGameForTransfer;

    // Update for a single large target
    private void Update() {
        if (Input.GetMouseButton(0) && gameActive && CanvasManager.toolType == requiredToolType) {
            if (GetStuffedAmount() > 0.1f) {
                Vector3 position = Input.mousePosition;
                position.z = 0.0f;
                position = Camera.main.ScreenToWorldPoint(position);
                RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

                if (hit.collider != null) {
                    // Determine mouse position as percentage of collider extents 
                    float percentX = position.x < 0 ? (position.x + texturePosModifier) / texturePosModifier : position.x / texturePosModifier;
                    float percentY = position.y < 0 ? (position.y + texturePosModifier) / texturePosModifier : position.y / texturePosModifier;

                    int pixelX = (int)(percentX * 300);
                    int pixelY = (int)(percentY * 300);

                    Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);

                    int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                    int maxPaintDistance = 7;
                    if (paintPixelDistance < maxPaintDistance) {
                        // Painting too close to last position
                        return;
                    }
                    lastPaintPixelPosition = paintPixelPosition;

                    int pixelXOffset = pixelX - (stuffingBrush.width / 2);
                    int pixelYOffset = pixelY - (stuffingBrush.height / 2);

                    for (int x = 0; x < stuffingBrush.width; x++) {
                        for (int y = 0; y < stuffingBrush.height; y++) {
                            Color pixelDirt = stuffingBrush.GetPixel(x, y);
                            Color pixelDirtMask = stuffingMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);

                            float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                            unstuffedAreaCurrent -= removedAmount;

                            stuffingMaskTexture.SetPixel(
                                pixelXOffset + x,
                                pixelYOffset + y,
                                new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                            );
                        }
                    }

                    stuffingMaskTexture.Apply();
                }

            } else {
                // More than 95% cleaned -> Set remaining pixels and end game
                gameActive = false;
                Color32 stuffedColor = new Color32(0, 0, 0, 0);
                Color32[] colors = stuffingMaskTexture.GetPixels32();

                for (int i = 0; i < colors.Length; i++) {
                    colors[i] = stuffedColor;
                }

                stuffingMaskTexture.SetPixels32(colors);
                stuffingMaskTexture.Apply();

                TransferToSewingGame();
            }
        }
    }

    private void TransferToSewingGame() {
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material = defaultMaterial;
        Vector3 lensePosition = spriteRenderer.transform.position;
        Debug.Log(lensePosition);

        List<Vector3> targetPositions = new List<Vector3> {
                    new Vector3(lensePosition.x - 1, lensePosition.y + 2, -1),
                    new Vector3(lensePosition.x + 1, lensePosition.y, -1),
                    new Vector3(lensePosition.x - 1, lensePosition.y - 2, -1),
                };
        mendingGameForTransfer.CreateSewingGame(targetPositions, currentPlushieDamage);
    }

    public void CreateStuffingGameMultipleTargets(List<Vector3> targetPositions, PlushieDamage plushieDamage) {
        gameActive = true;
        int counter = 1;

        // Create a new target for each position
        foreach (Vector3 targetPosition in targetPositions) {
            // Set stuffing texture mask to be updated by game
            Texture2D stuffingMaskTexture = new Texture2D(unstuffedTexBase.width, unstuffedTexBase.height);
            stuffingMaskTexture.SetPixels(unstuffedTexBase.GetPixels());
            stuffingMaskTexture.Apply();

            // Set unique mask for each target 
            string maskName = "_Mask" + counter;
            counter++;
            Material stuffingMaterial = material;
            stuffingMaterial.SetTexture(maskName, stuffingMaskTexture);

            // Calculate total unstuffed area
            float unstuffedAreaTotal = 0f;
            for (int x = 0; x < unstuffedTexBase.width; x++) {
                for (int y = 0; y < unstuffedTexBase.height; y++) {
                    unstuffedAreaTotal += unstuffedTexBase.GetPixel(x, y).g;
                }
            }

            GameObject target = Instantiate(stuffingTargetPrefab, targetPosition, Quaternion.identity, this.transform);
            target.GetComponentInChildren<SpriteRenderer>().material.SetTexture(maskName, stuffingMaskTexture);

            StuffingTarget stuffingTarget = new StuffingTarget(target, unstuffedAreaTotal, stuffingMaskTexture);
            targets.Add(stuffingTarget);
        }
    }

    public void StartGameRoutine(MendingGames mendingGame, PlushieDamage plushieDamage) {
        StartCoroutine(CreateStuffingGame(mendingGame, plushieDamage));
    }

    public IEnumerator CreateStuffingGame(MendingGames mendingGame, PlushieDamage plushieDamage) {
        requiredToolType = ToolType.Stuffing;
        mendingGameForTransfer = mendingGame;
        currentPlushieDamage = plushieDamage;
        targets = new List<StuffingTarget>();

        stuffingMaskTexture = new Texture2D(unstuffedTexBase.width, unstuffedTexBase.height);
        stuffingMaskTexture.SetPixels(unstuffedTexBase.GetPixels());
        stuffingMaskTexture.Apply();

        material.SetTexture("_Mask", stuffingMaskTexture);

        this.gameObject.GetComponent<SpriteRenderer>().material = material;

        unstuffedAreaTotal = 0f;
        for (int x = 0; x < unstuffedTexBase.width; x++) {
            for (int y = 0; y < unstuffedTexBase.height; y++) {
                unstuffedAreaTotal += unstuffedTexBase.GetPixel(x, y).g;
            }
        }
        unstuffedAreaCurrent = unstuffedAreaTotal;

        // Wait briefly to allow game to animate and populate
        yield return new WaitForSeconds(0.25f);
        gameActive = true;
    }

    private float GetStuffedAmount() {
        return this.unstuffedAreaCurrent / unstuffedAreaTotal;
    }
}
