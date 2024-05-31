using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Payment", menuName = "Payment Types", order = 0)]
public class PaymentTypes : ScriptableObject
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

    public PaymentType type;
    public int value;

    public bool isActive;
}
