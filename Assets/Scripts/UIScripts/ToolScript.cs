using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tool;

namespace GameUI
{
    public class ToolScript : MonoBehaviour, IPointerClickHandler
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
                Cursor.SetCursor(
                    this.toolScriptableObject.toolCursorTexture, 
                    new Vector2(
                        this.toolScriptableObject.toolCursorTexture.width, 
                        this.toolScriptableObject.toolCursorTexture.height
                        ) / 2f, 
                    CursorMode.Auto);
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
                Cursor.SetCursor(
                    this.toolScriptableObject.toolCursorTexture, new Vector2(
                        this.toolScriptableObject.toolCursorTexture.width, 
                        this.toolScriptableObject.toolCursorTexture.height
                        ) / 2f,  
                    CursorMode.Auto);
                // Play pickup sound
                audioSources[0].Play();
            }
        }

        public void deselectTool() {
            CanvasManager.currentTool = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            audioSources[1].Play();
        }
    }
}