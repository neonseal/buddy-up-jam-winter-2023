using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/* User-defined Namespaces */
using Scriptables;

/// <summary>
/// Tool Class
/// 
/// The Tool class is responsible for alerting the canvas manager when the user 
/// clicks on a tool roll slot to select or drop a tool
/// </summary>
namespace PlayArea {
    public enum ToolType {
        Scissors,
        Needle,
        Stuffing,
        Cleaning,
        None
    }

    public class Tool : MonoBehaviour, IPointerClickHandler {
        [SerializeField]
        private ToolScriptableObject toolScriptableObject;

        /* Tool Selected Event */
        public static event Action<Tool, ToolType> OnToolClicked;

        // Set the selected tool as the player's cursor
        public void OnPointerClick(PointerEventData eventData) {
            OnToolClicked?.Invoke(this, toolScriptableObject.toolType);
        }

        /* Public Properties */
        public ToolScriptableObject ToolScriptableObject { get => ToolScriptableObject; }
    }
}
