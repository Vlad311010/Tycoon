using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSliderUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text text;

    private void Awake()
    {
        UpdateValue(slider.value);
        slider.onValueChanged.AddListener(UpdateValue);
    }

    private void UpdateValue(float value)
    {
        if (slider.wholeNumbers)
        {
            text.text = value.ToString();
        }
        else
        {
            text.text = value.ToString("P0");
        }
    }
}
