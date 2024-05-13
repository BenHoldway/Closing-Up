using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{
    Image image;
    bool isSelected;

    [SerializeField] Payments.PaymentType paymentType;
    [SerializeField] Sprite unselectedSprite;
    [SerializeField] Sprite selectedSprite;

    public static event Action<int> Selected;
    public static event Action<int> Deselected;

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    public void OnClick()
    {
        isSelected = !isSelected;

        if(isSelected)
        {
            Selected?.Invoke((int)paymentType);
            image.sprite = selectedSprite;
        }
        else
        { 
            Deselected?.Invoke((int)paymentType);
            image.sprite = unselectedSprite;
        }
    }
}
