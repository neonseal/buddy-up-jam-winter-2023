/* User-defined Namespaces */
using Scriptables;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

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

        /* Tool Selected Event */
        public static event Action<Tool, ToolType> OnToolClicked;

        // Set the selected tool as the player's cursor
        public void OnPointerClick(PointerEventData eventData) {
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
