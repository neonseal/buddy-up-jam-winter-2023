using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUI
{
    public class CanvasManager : MonoBehaviour
    {
        internal GameObject currentTool;
        private EventSystem eventSystem;
        private PointerEventData pointerEventData;
        private GraphicRaycaster raycaster;

        void Start()
        {
            this.currentTool = null;
            this.raycaster = this.GetComponent<GraphicRaycaster>();

            this.eventSystem = EventSystem.current;
        }

        void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Set up the new Pointer Event
            pointerEventData = new PointerEventData(eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            pointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            raycaster.Raycast(pointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);
            }
        }
    }
    }
}

