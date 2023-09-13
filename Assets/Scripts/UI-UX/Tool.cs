
using Scriptables;
using System;
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
            toolRollSlotImage = GetComponent<Image>();
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

        public void Pickup() {
            pickupSound.Play();
        }

        public void Place() {
            placeSound.Play();
        }

        public void PlayMouseClickSound() {
            singleMouseClickSound.Play();
        }

        public void PlayMouseHoldSound() {
            contionuousMouseHoldSound.Play();
        }

        /* Public Properties */
        public ToolScriptableObject ToolScriptableObject { get => toolScriptableObject; }
    }
}
