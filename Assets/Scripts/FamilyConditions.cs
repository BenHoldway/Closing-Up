using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class FamilyMember
{
    public enum ConditionType
    {
        Hunger,
        Warmth,
        Health,
        None
    }

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

    public void IncreaseAmounts(ConditionType type, float _amount)
    {
        switch (type)
        {
            case ConditionType.Hunger:
                hunger += _amount;
                break;
            case ConditionType.Warmth:
                warmth += _amount;
                break;
            case ConditionType.Health:
                health -= _amount;
                break;
        }
    }
}

public class FamilyConditions : MonoBehaviour
{
    public static FamilyConditions Instance;

    [SerializeField] FamilyMember[] family;
    public FamilyMember[] Family { get { return family; } set { family = value; } }
    int familyMembers = 3;

    [SerializeField] List<TMP_Text> familyUI = new List<TMP_Text>();
    List<FamilyMember.ConditionType> conditionsToIncrease = new List<FamilyMember.ConditionType>();

    [SerializeField] float reductionAmount;
    [SerializeField] float increaseAmount;

    public static event Action ReloadScene;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        family = new FamilyMember[familyMembers];
        
        DataHolder shuttleObj = FindObjectOfType<DataHolder>();
        if (shuttleObj == null)
        {
            for (int i = 0; i < familyMembers; i++)
                family[i] = new FamilyMember(100, 100, 100);
        }
        else
        {
            for (int i = 0; i < familyMembers; i++)
                family[i] = shuttleObj.FamilyMembers[i];
        }
    }

    private void OnEnable()
    {
        ShiftManager.CompleteShiftEvent += ReduceConditions;
        ShiftManager.NextShiftEvent += IncreaseConditions;

        ButtonSelection.Selected += AddToIncreasingConditions;
        ButtonSelection.Deselected += RemoveIncreasingConditions;
    }

    private void OnDisable()
    {
        ShiftManager.CompleteShiftEvent -= ReduceConditions;
        ShiftManager.NextShiftEvent -= IncreaseConditions;

        ButtonSelection.Selected -= AddToIncreasingConditions;
        ButtonSelection.Deselected -= RemoveIncreasingConditions;
    }


    void AddToIncreasingConditions(int ignore, FamilyMember.ConditionType type)
    {
        conditionsToIncrease.Add(type);
    }

    void RemoveIncreasingConditions(int ignore, FamilyMember.ConditionType type)
    {
        for(int i = 0; i < conditionsToIncrease.Count; i++)
            if (conditionsToIncrease[i] == type)
            {
                conditionsToIncrease.RemoveAt(i);
                return;
            }
    }

    void ReduceConditions()
    {
        for (int i = 0; i < familyMembers; i++)
            family[i].ReduceAmounts(reductionAmount);

        DisplayConditions();
    }

    void IncreaseConditions()
    {
        for (int i = 0; i < conditionsToIncrease.Count; i++)
            for(int member = 0; member < familyMembers; member++)
                family[member].IncreaseAmounts(conditionsToIncrease[i], increaseAmount);

        ReloadScene?.Invoke();
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
