using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameData;

namespace GameUI
{
    public class Tool : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        public ToolScriptableObject toolScriptableObject;
        [SerializeField]
        private Canvas canvas;
        private CanvasManager canvasManager;
        private AudioSource[] audioSources;

        void Awake()
        {
            this.canvasManager = this.canvas.GetComponent<CanvasManager>();
            // Array of tool pickup, place, and apply sounds
            this.audioSources = GetComponents<AudioSource>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // If nothing is held, set this gameobject/tool as the tool being held
            if (CanvasManager.currentTool == null)
            {
                CanvasManager.currentTool = this.gameObject;
                this.setToolCursor();
                // Play pickup sound
                audioSources[0].Play();
            }
            // If you're clicking on the original slot, drops the tool
            else if (this.gameObject == CanvasManager.currentTool)
            {
                this.deselectTool();
            }
            // If you're click on a new tool, swap to the new tool and return the old tool
            else
            {
                CanvasManager.currentTool = this.gameObject;
                this.setToolCursor();
                // Play pickup sound
                audioSources[0].Play();
            }
        }

        private void setToolCursor()
        {
            if (this.toolScriptableObject.toolType.Equals(ToolType.Cleaning) || this.toolScriptableObject.toolType.Equals(ToolType.Stuffing))
            {
                // Set cursor at center of sprite
                Cursor.SetCursor(
                    this.toolScriptableObject.toolCursorTexture,
                    new Vector2(
                        this.toolScriptableObject.toolCursorTexture.width,
                        this.toolScriptableObject.toolCursorTexture.height
                    ) / 2f,
                    CursorMode.Auto
                );
            }
            else {
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

        private void setCursorBottomLeft()
        {

        }

        public void deselectTool()
        {
            CanvasManager.currentTool = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            audioSources[1].Play();
        }
    }
}