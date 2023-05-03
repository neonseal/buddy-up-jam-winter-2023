using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bell : MonoBehaviour
{
    private Button _bellButton;

    private void Awake()
    {
        this._bellButton = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        this._bellButton.onClick.AddListener(this.bellClick);
    }

    private void bellClick()
    {
        Debug.Log("Ding!");
        PlushieLifeCycleEventManager.Current.ringBell();
    }
}
