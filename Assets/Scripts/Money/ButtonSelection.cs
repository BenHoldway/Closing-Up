using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{
    Image image;
    bool isSelected;

    [SerializeField] PaymentTypes paymentType;
    public PaymentTypes PaymentTypeVar { get { return paymentType; } set { paymentType = value; } }
    [SerializeField] Sprite unselectedSprite;
    [SerializeField] Sprite selectedSprite;

    Color colour;

    [SerializeField] FamilyMember.ConditionType condition;

    public static event Action<int, FamilyMember.ConditionType> Selected;
    public static event Action<int, FamilyMember.ConditionType> Deselected;

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        colour = new Color(1.0f, 1.0f, 1.0f);

        CheckPaymentState();
    }

    private void OnEnable()
    {
        MoneyManager.MoneyChanged += CheckPaymentState;
    }

    private void OnDisable()
    {
        MoneyManager.MoneyChanged -= CheckPaymentState;
    }

    public void OnClick()
    {
        isSelected = !isSelected;

        //Set button sprite to selected, and notify that a payment has been made
        if(isSelected)
        {
            Selected?.Invoke((int)paymentType.type, condition);
            image.sprite = selectedSprite;
        }
        //Set button sprite to not selected, and notify that payment has been refunded
        else
        { 
            Deselected?.Invoke((int)paymentType.type, condition);
            image.sprite = unselectedSprite;
        }
    }

    void CheckPaymentState()
    {
        if (isSelected)
            return;

        //Disable button if player does not have enough money
        if(MoneyManager.Instance.NewMoney < paymentType.value)
        {
            gameObject.GetComponent<Button>().interactable = false;
            colour = new Color(0.5f, 0.5f, 0.5f);
        }
        //Make sure button is enabled when player has enough money
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
            colour = new Color(1.0f, 1.0f, 1.0f);
        }

        //Set each payment part to be the correct colour
        GameObject parent = gameObject.transform.parent.gameObject;
        parent.transform.GetChild(0).GetComponent<TMP_Text>().color = colour;
        parent.transform.GetChild(1).GetComponent<TMP_Text>().color = colour;

        gameObject.GetComponent<Image>().color = colour;
    }
}
