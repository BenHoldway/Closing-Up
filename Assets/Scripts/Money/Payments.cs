using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Payments : MonoBehaviour
{
    public enum PaymentType 
    { 
        Rent,
        Heating,
        Water,
        Food,
        Medicine,
        AmountOfPayments
    }

    Dictionary<PaymentType, int> paymentAmounts = new Dictionary<PaymentType, int>();
    [SerializeField] List<TMP_Text> paymentTexts = new List<TMP_Text>();

    public static event Action<int, bool> PaymentEvent;

    private void Start()
    {
        paymentAmounts.Add(PaymentType.Rent, 20);
        paymentAmounts.Add(PaymentType.Heating, 10);
        paymentAmounts.Add(PaymentType.Water, 10);
        paymentAmounts.Add(PaymentType.Food, 15);
        paymentAmounts.Add(PaymentType.Medicine, 0);

        for (int i = 0; i < (int)PaymentType.AmountOfPayments; i++)
        {
            paymentTexts[i].text = paymentAmounts[(PaymentType)i].ToString();
        }
    }

    private void OnEnable()
    {
        ShiftManager.CompleteShiftEvent += CompletedShift;
        ButtonSelection.Selected += SelectPayment;
        ButtonSelection.Deselected += DeselectPayment;
    }

    private void OnDisable()
    {
        ShiftManager.CompleteShiftEvent -= CompletedShift;
        ButtonSelection.Selected -= SelectPayment;
        ButtonSelection.Deselected -= DeselectPayment;
    }

    void CompletedShift()
    {
        SelectPayment(0, FamilyMember.ConditionType.None);
    }

    void SelectPayment(int enumIndex, FamilyMember.ConditionType ignore)
    {
        PaymentEvent?.Invoke(paymentAmounts[(PaymentType)enumIndex], true);
    }

    void DeselectPayment(int enumIndex, FamilyMember.ConditionType ignore) 
    {
        PaymentEvent?.Invoke(paymentAmounts[(PaymentType)enumIndex], false);
    }
}
