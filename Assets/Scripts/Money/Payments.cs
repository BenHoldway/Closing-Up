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
        Wife_Medicine,
        Son_Medicine,
        Daughter_Medicine,
        AmountOfPayments
    }

    [SerializeField] GameObject paymentPrefab;
    [SerializeField] GameObject paymentParent;

    Dictionary<PaymentType, int> paymentAmounts = new Dictionary<PaymentType, int>();
    [SerializeField] List<TMP_Text> paymentTexts = new List<TMP_Text>();

    public static event Action<int, bool> PaymentEvent;

    private void Start()
    {
        int rentAmount = 0;
        int shift = ShiftManager.Instance.ShiftCount;

        if (shift < 5)
            rentAmount = 20;
        else if (shift < 10)
            rentAmount = 30;
        else if (shift < 15)
            rentAmount = 40;
        else
            rentAmount = 50;

        paymentAmounts.Add(PaymentType.Rent, rentAmount);
        paymentAmounts.Add(PaymentType.Heating, 10);
        paymentAmounts.Add(PaymentType.Water, 10);
        paymentAmounts.Add(PaymentType.Food, 15);
    }

    private void OnEnable()
    {
        FamilyConditions.FamilyUpdated += CompletedShift;
        ButtonSelection.Selected += SelectPayment;
        ButtonSelection.Deselected += DeselectPayment;
    }

    private void OnDisable()
    {
        FamilyConditions.FamilyUpdated -= CompletedShift;
        ButtonSelection.Selected -= SelectPayment;
        ButtonSelection.Deselected -= DeselectPayment;
    }

    void CompletedShift()
    {
        SelectPayment(0, FamilyMember.ConditionType.None);

        int index = 6;
        for (int i = 0; i < FamilyConditions.Instance.Family.Length; i++)
        {
            if (FamilyConditions.Instance.Family[i].health < 40)
            {
                int medicineCost = 5;

                if (FamilyConditions.Instance.Family[i].health < 20)
                    medicineCost = 15;

                GameObject newPayment = Instantiate(paymentPrefab, paymentParent.transform, false);
                newPayment.transform.SetSiblingIndex(index);
                index++;

                paymentTexts.Add(newPayment.transform.GetChild(1).GetComponent<TMP_Text>());
                newPayment.transform.GetChild(0).GetComponent<TMP_Text>().text = $"Medicine ({FamilyConditions.Instance.Family[i].name})";

                PaymentType type;
                if (i == 0)
                    type = PaymentType.Wife_Medicine;
                else if (i == 1)
                    type = PaymentType.Son_Medicine;
                else
                    type = PaymentType.Daughter_Medicine;

                paymentAmounts.Add(type, medicineCost);
                newPayment.transform.GetChild(2).GetComponent<ButtonSelection>().PaymentTypeVar = type;
            }
        }

        int paymentIndex = 0;
        for (int i = 0; i < (int)PaymentType.AmountOfPayments; i++)
        {
            if (!paymentAmounts.ContainsKey((PaymentType)i))
                continue;

            paymentTexts[paymentIndex].text = paymentAmounts[(PaymentType)i].ToString();
            paymentIndex++;
        }
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
