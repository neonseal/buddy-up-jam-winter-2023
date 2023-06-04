using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameData;
using TutorialSequence;

namespace GameUI {
    public class Tool : MonoBehaviour, IPointerClickHandler {
        public Image image;
        [SerializeField]
        public ToolScriptableObject toolScriptableObject;
        [SerializeField]
        private Canvas canvas;
        private CanvasManager canvasManager;
        private AudioSource[] audioSources;
        private ToolRoll toolRoll;
        private GameObject crosshair;

        [Header("Tutorial Interaction Variables")]
        private bool tutorialActionRequired;
        private ToolType requiredToolType;

        void Awake() {
            this.tutorialActionRequired = false;

            this.canvasManager = this.canvas.GetComponent<CanvasManager>();
            this.image = GetComponent<Image>();
            this.toolRoll = GetComponentInParent<ToolRoll>();
            // Array of tool pickup, place, and apply sounds
            this.audioSources = GetComponents<AudioSource>();

            TutorialSequenceEventManager.Current.onRequireToolPickupContinueAction += SetupToolInteractionRequiredAction;
            TutorialSequenceEventManager.Current.onRequireToolDropContinueAction += SetupToolInteractionRequiredAction;
        }

        private void FixedUpdate() {
            if (crosshair != null) {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                float speed = 150f;
                crosshair.transform.position = Vector3.Lerp(transform.position, mousePos, speed * Time.deltaTime);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            // Check if there is an required tutorial tool pickup action and user selecting wrong tool
            if ((TutorialSequenceManager.isTutorialActive && !tutorialActionRequired) ||
                (tutorialActionRequired && this.toolScriptableObject.toolType != this.requiredToolType)
            ) { 
                return;
            }

            // If nothing is held, set this gameobject/tool as the tool being held
            if (CanvasManager.currentTool == null) {
                CanvasManager.currentTool = this.gameObject;
                CanvasManager.toolType = this.toolScriptableObject.toolType;

                this.SetToolCursor();
                // Play pickup sound
                audioSources[0].Play();

                // Instantiate a new crosshair only when no tool is held
                this.crosshair = new GameObject();
                this.crosshair.name = "ToolCursorCrosshair";
                SpriteRenderer crosshairSprite = this.crosshair.AddComponent<SpriteRenderer>();
                crosshairSprite.transform.SetParent(this.canvas.transform);
                crosshairSprite.sprite = Resources.Load<Sprite>("Sprites/Circle");
                crosshairSprite.sortingLayerName = "UILayer";
                crosshairSprite.sortingOrder = 50;
                crosshairSprite.color = Color.black;

                // Set crosshair position
                Vector2 mousePosition = Input.mousePosition;
                Camera mainCam = Camera.main;
                Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(
                    new Vector3(mousePosition.x, mousePosition.y, mainCam.nearClipPlane)
                );
                this.crosshair.transform.position = mouseWorldPosition;
                this.crosshair.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            } else if (this.gameObject == CanvasManager.currentTool) {
                // If you're clicking on the original slot, drops the tool
                this.deselectTool();
                Destroy(crosshair);
            } else {
                // If you're click on a new tool, swap to the new tool and return the old tool
                CanvasManager.currentTool = this.gameObject;
                CanvasManager.toolType = this.toolScriptableObject.toolType;

                this.toolRoll.ResetOtherToolSprite(this);
                this.SetToolCursor();
                // Play pickup sound
                audioSources[0].Play();
            }
            ToggleToolSprite();

            // Check if tutorial interaction is required
            if (tutorialActionRequired) {
                tutorialActionRequired = false;
                StartCoroutine(TutorialSequenceEventManager.Current.HandleTutorialRequiredActionCompletion());
            }
        }

        public void deselectTool() {
            CanvasManager.currentTool = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            audioSources[1].Play();
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
                    CursorMode.ForceSoftware
                );
            } else {
                // Set cursor at crosshair
                Cursor.SetCursor(
                    this.toolScriptableObject.toolCursorTexture,
                    new Vector2(
                        40f,
                        29f
                    ),
                    CursorMode.ForceSoftware
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

        private void SetupToolInteractionRequiredAction(ToolType toolType) {
            tutorialActionRequired = true;
            requiredToolType = toolType;
        }
    }
}