using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayArea {
    public enum ToolType {
        Scissors,
        Needle,
        Stuffing,
        Cleaning,
        None
    }
    public class Tool : MonoBehaviour, IPointerClickHandler {


        private void Awake() {
            
        }

        public void OnPointerClick(PointerEventData eventData) {
            Debug.Log(this.gameObject.name);
        }
    }
}
