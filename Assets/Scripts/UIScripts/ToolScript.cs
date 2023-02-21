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
        private Image toolImage;

        void Awake()
        {
            this.canvasManager = this.canvas.GetComponent<CanvasManager>();

            this.toolImage = this.gameObject.GetComponent<Image>();
            this.toolImage.sprite = this.toolScriptableObject.toolSlotSprite;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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
                this.toolImage.color = new Color(1, 1, 1, 0.5f);
            }
            // If you're clicking on the original slot, drops the tool
            else if (this.gameObject == CanvasManager.currentTool)
            {
                this.deselectTool();
            }
            // If you're click on a new tool, swap to the new tool and return the old tool
            else
            {
                CanvasManager.currentTool.GetComponent<ToolScript>().toolImage.color = Color.white;
                CanvasManager.currentTool = this.gameObject;
                Cursor.SetCursor(
                    this.toolScriptableObject.toolCursorTexture, new Vector2(
                        this.toolScriptableObject.toolCursorTexture.width, 
                        this.toolScriptableObject.toolCursorTexture.height
                        ) / 2f,  
                    CursorMode.Auto);
                this.toolImage.color = new Color(1, 1, 1, 0.5f);
            }
        }

        public void deselectTool() {
            CanvasManager.currentTool = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            this.toolImage.color = Color.white;
        }
    }
}