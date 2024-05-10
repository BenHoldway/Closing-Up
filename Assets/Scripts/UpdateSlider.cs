using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSlider : MonoBehaviour
{
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        TaskInteractable.ChangeSliderFill += UpdateFill;
    }

    void OnDisable()
    {
        TaskInteractable.ChangeSliderFill -= UpdateFill;
    }

    void UpdateFill(float _value)
    {
        slider.value = _value;
    }
}
