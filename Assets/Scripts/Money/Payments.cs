using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Payments : MonoBehaviour
{
    [SerializeField] List<PaymentTypes> paymentAmounts = new List<PaymentTypes>();
    [SerializeField] List<TMP_Text> paymentTexts = new List<TMP_Text>();
    [SerializeField] TMP_Text reductionText;

    public static event Action<int, bool> PaymentEvent;

    private void Start()
    {
        int rentAmount = 0;
        int shift = ShiftManager.Instance.ShiftCount;

        //Change rent amount depending on shift number
        if (shift < 5)
            rentAmount = 20;
        else if (shift < 10)
            rentAmount = 30;
        else if (shift < 15)
            rentAmount = 40;
        else
            rentAmount = 50;

        //Set rent amount
        foreach(PaymentTypes paymentType in paymentAmounts)
        {
            if(paymentType.type == PaymentTypes.PaymentType.Rent)
                paymentType.value = rentAmount;
        }
    }

    private void OnEnable()
    {
        ShiftManager.CompleteShiftEvent += CompletedShift;
        ButtonSelection.Selected += SelectPayment;
        ButtonSelection.Deselected += DeselectPayment;
        MoneyManager.ShowReductionAmount += ReductionText;
    }

    private void OnDisable()
    {
        ShiftManager.CompleteShiftEvent -= CompletedShift;
        ButtonSelection.Selected -= SelectPayment;
        ButtonSelection.Deselected -= DeselectPayment;
        MoneyManager.ShowReductionAmount -= ReductionText;
    }

    void CompletedShift()
    {
        //Make rent payment
        SelectPayment(0, FamilyMember.ConditionType.None);

        CheckFamilyHealth();

        int paymentIndex = 0;
        //Run through each payment type
        for (int i = 0; i < paymentAmounts.Count; i++)
        {
            //If this payment is not available, continue on to the next one
            if (!paymentAmounts[i].isActive)
                continue;

            //Show the value of each visible payment
            paymentTexts[paymentIndex].text = paymentAmounts[i].value.ToString();
            paymentIndex++;
        }
    }

    //Show reduction amount
    void ReductionText(int amount)
    {
        reductionText.text = $"- {amount}";
    }

    void CheckFamilyHealth()
    {
        //Run through each family member
        for (int i = 0; i < FamilyConditions.Instance.Family.Length; i++)
        {
            FamilyMember familyMember = FamilyConditions.Instance.Family[i];
            //If their health is below 40, set up the medicine payment for them
            if (familyMember.health < 40)
            {
                int medicineCost = 5;

                //If their health is more critical, increase the cost of medicine
                if (familyMember.health < 20)
                    medicineCost = 15;

                //Instantiate the payment UI, and set the child order
                GameObject newPayment = familyMember.medicineUI;
                newPayment.SetActive(true);

                //Update the payment name to tell the player which family member it is
                newPayment.transform.GetChild(0).GetComponent<TMP_Text>().text = $"Medicine ({FamilyConditions.Instance.Family[i].name})";

                //Change the type of the medicine depending on which family member it is
                PaymentTypes.PaymentType type;
                if (i == 0)
                    type = PaymentTypes.PaymentType.Wife_Medicine;
                else if (i == 1)
                    type = PaymentTypes.PaymentType.Son_Medicine;
                else
                    type = PaymentTypes.PaymentType.Daughter_Medicine;

                //Update the payment amount for medicine
                foreach(PaymentTypes paymentType in paymentAmounts)
                {
                    if (paymentType.type == type)
                    {
                        paymentType.value = medicineCost;
                        paymentType.isActive = true;
                    }
                }

                //Pass in the payment type to the button
                newPayment.transform.GetChild(2).GetComponent<ButtonSelection>().PaymentTypeVar = paymentAmounts[i+4];
            }
            //Disable medicine payment if not needed
            else if(familyMember.medicineUI.activeSelf) 
                familyMember.medicineUI.SetActive(false);
        }
    }

    //Notify that payment has been selected
    void SelectPayment(int enumIndex, FamilyMember.ConditionType ignore)
    {
        PaymentEvent?.Invoke(paymentAmounts[enumIndex].value, true);
    }

    //Notify that payment has been deselected
    void DeselectPayment(int enumIndex, FamilyMember.ConditionType ignore) 
    {
        PaymentEvent?.Invoke(paymentAmounts[enumIndex].value, false);
    }
}