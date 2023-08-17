using System;
using UnityEngine;

public class RadioButton : MonoBehaviour {
    public static event Action<RadioButton> OnRadioButtonClick;
    private void OnMouseDown() {
        OnRadioButtonClick?.Invoke(this);
    }
}
