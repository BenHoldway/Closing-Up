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
        //Set slider value to 0 to begin with
        slider = GetComponent<Slider>();
        slider.value = 0;
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
        //Update slider value with value passed in
        slider.value = _value;
    }
}
