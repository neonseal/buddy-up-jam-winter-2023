using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffingGameController : MonoBehaviour
{
    [SerializeField] private Texture2D mainTex;
    [SerializeField] private Texture2D unstuffedTexBase;
    [SerializeField] private Texture2D stuffingBrush;
    [SerializeField] private Material material;
    

    private Color[] unstuffedPixels;
    SpriteRenderer spriteRenderer;

    private Texture2D stuffingMaskTexture;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;

    public int Width { get { return spriteRenderer.sprite.texture.width; } }
    public int Height { get { return spriteRenderer.sprite.texture.height; } }

    private void Start() {
        stuffingMaskTexture = new Texture2D(unstuffedTexBase.width, unstuffedTexBase.height);
        stuffingMaskTexture.SetPixels(unstuffedTexBase.GetPixels());
        stuffingMaskTexture.Apply();
        material.SetTexture("_Mask", stuffingMaskTexture);
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            Vector3 position = Input.mousePosition;
            position.z = 0.0f;
            position = Camera.main.ScreenToWorldPoint(position);
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

            if (hit.collider != null) {
                // Determine mouse position as percentage of collider extents 
                Vector2 targetPosition = this.transform.position;
                Debug.Log("TARGET POSITION: " + targetPosition);
                Debug.Log("PARENT POSITION: " + this.transform.parent.transform.position);
                Debug.Log("MOUSE POSITION: " + position);

                float percentX = position.x < 0 ? (position.x + 2.8f) / 2.8f : position.x / 2.8f;
                float percentY = position.y < 0 ? (position.y + 2.8f) / 2.8f : position.y / 2.8f;

                int pixelX = (int)(percentX * 300);
                int pixelY = (int)(percentY * 300);
                Debug.Log("X: " + percentX + " | Y: " + percentY);


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
                        //Debug.Log(pixelDirtMask);

                        float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                        dirtAmount -= removedAmount;

                        stuffingMaskTexture.SetPixel(
                            pixelXOffset + x,
                            pixelYOffset + y,
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                        );
                    }
                }


                stuffingMaskTexture.Apply();
            }
        }
    }
}
