using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tool;

namespace GameUI {
    public class ToolScript : MonoBehaviour, IPointerClickHandler {
        public Image image;
        [SerializeField]
        public ToolScriptableObject toolScriptableObject;
        [SerializeField]
        private Canvas canvas;
        private CanvasManager canvasManager;
        private AudioSource[] audioSources;
        private ToolRoll toolRoll;

        void Awake() {
            this.canvasManager = this.canvas.GetComponent<CanvasManager>();
            this.image = GetComponent<Image>();
            this.toolRoll = GetComponentInParent<ToolRoll>();
            // Array of tool pickup, place, and apply sounds
            this.audioSources = GetComponents<AudioSource>();
        }

        public void OnPointerClick(PointerEventData eventData) {
            // If nothing is held, set this gameobject/tool as the tool being held
            if (CanvasManager.currentTool == null) {
                CanvasManager.currentTool = this.gameObject;
                this.SetToolCursor();
                // Play pickup sound
                audioSources[0].Play();
            }
            // If you're clicking on the original slot, drops the tool
            else if (this.gameObject == CanvasManager.currentTool) {
                this.deselectTool();
            }
            // If you're click on a new tool, swap to the new tool and return the old tool
            else {
                CanvasManager.currentTool = this.gameObject;
                this.toolRoll.ResetOtherToolSprite(this);
                this.SetToolCursor();
                // Play pickup sound
                audioSources[0].Play();
            }

            ToggleToolSprite();
        }

        private void SetToolCursor() {
            if (this.toolScriptableObject.toolType.Equals(ToolType.Cleaning) || this.toolScriptableObject.toolType.Equals(ToolType.Stuffing)) {
                // Set cursor at center of sprite
                Cursor.SetCursor(
                    this.toolScriptableObject.toolCursorTexture,
                    new Vector2(
                        this.toolScriptableObject.toolCursorTexture.width,
                        this.toolScriptableObject.toolCursorTexture.height
                    ) / 2f,
                    CursorMode.Auto
                );
            } else {
                // Set cursor at bottom left of sprite
                Cursor.SetCursor(
                    this.toolScriptableObject.toolCursorTexture,
                    new Vector2(
                        0f,
                        this.toolScriptableObject.toolCursorTexture.height
                    ),
                    CursorMode.Auto
                );
            }
        }

        private void ToggleToolSprite() {
            switch (this.toolScriptableObject.toolType) {
                case ToolType.Needle:
                    if (this.image.sprite == this.toolScriptableObject.standardToolImage) {
                        this.image.sprite = this.toolScriptableObject.selectedToolImage;
                    } else {
                        this.image.sprite = this.toolScriptableObject.standardToolImage;
                    }
                    break;
                case ToolType.Scissors:
                    if (this.image.color.a > 0) {
                        Color transparent = this.image.color;
                        transparent.a = 0;
                        this.image.color = transparent;
                    } else {
                        Color visible = this.image.color;
                        visible.a = 255;
                        this.image.color = visible;
                    }
                    break;
                case ToolType.Stuffing:
                    if (this.image.sprite == this.toolScriptableObject.standardToolImage) {
                        this.image.sprite = this.toolScriptableObject.selectedToolImage;
                    } else {
                        this.image.sprite = this.toolScriptableObject.standardToolImage;
                    }
                    break;
            }
        }

        public void deselectTool() {
            CanvasManager.currentTool = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            audioSources[1].Play();
        }
    }
}