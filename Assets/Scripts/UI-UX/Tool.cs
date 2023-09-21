
using Scriptables;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Tool Class
/// 
/// The Tool class is responsible for alerting the canvas manager when the user 
/// clicks on a tool roll slot to select or drop a tool
/// </summary>
namespace PlayArea {
    public enum ToolType {
        None,
        Scissors,
        Needle,
        Stuffing,
        Cleaning
    }

    public class Tool : MonoBehaviour, IPointerClickHandler {
        [SerializeField]
        private ToolScriptableObject toolScriptableObject;
        private bool held;
        private bool usingTool;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource pickupSound;
        [SerializeField] private AudioSource placeSound;
        [SerializeField] private AudioSource singleMouseClickSound;
        [SerializeField] private AudioSource contionuousMouseHoldSound;

        [Header("Tool Image Assets")]
        private Image toolRollSlotImage;
        [SerializeField] private Sprite defaultToolImage;
        [SerializeField] private Sprite pickedUpToolImage;

        /* Tool Selected Event */
        public static event Action<Tool, ToolType> OnToolClicked;

        private void Awake() {
            held = false;
            toolRollSlotImage = GetComponent<Image>();
        }

        private void Update() {
            if (held && Input.GetMouseButton(0) && !usingTool) {
                StartCoroutine("PlayMouseHoldSound");
            }

            if (Input.GetMouseButtonUp(0)) {
                contionuousMouseHoldSound.Stop();
                usingTool = false;
                StopAllCoroutines();
            }
        }

        // Set the selected tool as the player's cursor
        public void OnPointerClick(PointerEventData eventData) {
            if (pickedUpToolImage != null) {
                toolRollSlotImage.sprite = toolRollSlotImage.sprite == defaultToolImage ? pickedUpToolImage : defaultToolImage;
            } else {
                Color tempColor = toolRollSlotImage.color;
                tempColor.a = toolRollSlotImage.color.a == 1f ? 0f : 1f;
                toolRollSlotImage.color = tempColor;
            }

            OnToolClicked?.Invoke(this, toolScriptableObject.ToolType);
        }

        public void Reset() {
            held = false;
            usingTool = false;

            if (pickedUpToolImage != null) {
                toolRollSlotImage.sprite = defaultToolImage;
            } else {
                Color tempColor = toolRollSlotImage.color;
                tempColor.a = 1f;
                toolRollSlotImage.color = tempColor;
            }
        }

        public void Pickup() {
            pickupSound.Play();
            held = true;
        }

        public void Place() {
            held = false;
            contionuousMouseHoldSound.Stop();
            usingTool = false;
            StopAllCoroutines();
            placeSound.Play();
        }

        public void PlayMouseClickSound() {
            singleMouseClickSound.Play();
        }

        public IEnumerator PlayMouseHoldSound() {
            usingTool = true;
            contionuousMouseHoldSound.Play();
            yield return new WaitForSeconds(2f);
            usingTool = false;
        }

        /* Public Properties */
        public ToolScriptableObject ToolScriptableObject { get => toolScriptableObject; }
    }
}
