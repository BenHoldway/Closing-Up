using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class FamilyMember
{
    public float hunger;
    public float warmth;
    public float health;

    public FamilyMember(float _hunger, float _warmth, float _health)
    {
        hunger = _hunger;
        warmth = _warmth;
        health = _health;
    }

    public void ReduceAmounts(float _amount)
    {
        hunger -= _amount; warmth -= _amount; health -= _amount;
    }
}

public class FamilyConditions : MonoBehaviour
{
    [SerializeField] FamilyMember[] family;
    int familyMembers = 3;

    [SerializeField] List<TMP_Text> familyUI = new List<TMP_Text>();

    [SerializeField] float reductionAmount;

    // Start is called before the first frame update
    void Start()
    {
        family = new FamilyMember[familyMembers];

        for (int i = 0; i < familyMembers; i++)
            family[i] = new FamilyMember(100, 100, 100);
        DisplayConditions();
    }

    private void OnEnable()
    {
        ShiftManager.CompleteShiftEvent += ReduceConditions;
    }

    void ReduceConditions()
    {
        for (int i = 0; i < familyMembers; i++)
            family[i].ReduceAmounts(reductionAmount);

        DisplayConditions();
    }

    void DisplayConditions()
    {
        for (int i = 0; i < familyMembers; i++)
        {
            string conditions = "";
            int badConditions = 0;

            if (family[i].hunger < 40)
            {
                conditions += "Hungry";
                badConditions++;
            }

            if (family[i].warmth < 40)
            {
                if (badConditions > 0)
                    conditions += ", ";
                conditions += "Cold";
                badConditions++;
            }

            if (family[i].health < 40)
            {
                if (badConditions > 0)
                    conditions += ", ";
                conditions += "Ill";
            }

            if (badConditions == 0)
                conditions = "Ok";

            familyUI[i].text = conditions;
        }
    }
}
