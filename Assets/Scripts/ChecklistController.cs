using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistController : MonoBehaviour
{
    private ChecklistStep[] steps;
    private Button btn;
    
    private void Awake() {
        steps = GetComponentsInChildren<ChecklistStep>();
        btn = GetComponentInChildren<Button>();
    }

    private void Start() {
        btn.onClick.AddListener(HandleButtonClick);
    }

    private void Update() {
            
    }

    private void HandleButtonClick() {
        Debug.Log("CLICK");
    }
}
