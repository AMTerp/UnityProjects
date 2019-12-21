using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Helps displays the current value of a UI slider.
public class DisplaySliderValue : MonoBehaviour
{
    // The formatting of the text is specified in the inspector.
    public string format;

    // The slider whose value is being displayed.
    public Slider slider;

    // The text field to display the value in.
    public Text text;

    // Use this for initialization
    void Start()
    {
        slider.onValueChanged.AddListener(OnValueChange);
        OnValueChange(slider.value);
    }

    // Called every time the associated slider has its value changed.
    private void OnValueChange(float newValue)
    {
        text.text = newValue.ToString(format);
    }
}