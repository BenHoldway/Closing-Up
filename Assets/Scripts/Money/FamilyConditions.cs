using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PaymentTypes;

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

    public string name;

    public float hunger;
    public float warmth;
    public float health;

    public GameObject medicineUI;

    public FamilyMember(string _name, float _hunger, float _warmth, float _health, GameObject _medicineUI)
    {
        name = _name;
        hunger = _hunger;
        warmth = _warmth;
        health = _health;
        medicineUI = _medicineUI;
    }

    public void ReduceAmounts(float _amount, bool healthIncreased)
    {
        hunger -= _amount; warmth -= _amount; 
        
        if(!healthIncreased)
            health -= _amount;
    }

    public void IncreaseAmounts(ConditionType type, float _amount)
    {
        switch (type)
        {
            case ConditionType.Hunger:
                hunger = Mathf.Clamp(hunger + _amount, 0, 100);
                break;
            case ConditionType.Warmth:
                warmth = Mathf.Clamp(warmth + _amount, 0, 100);
                break;
            case ConditionType.Health:
                health = Mathf.Clamp(health + _amount, 0, 100);
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
    [SerializeField] List<GameObject> medicineUI = new List<GameObject>();
    List<FamilyMember.ConditionType> conditionsToIncrease = new List<FamilyMember.ConditionType>();

    [SerializeField] float reductionAmount;
    [SerializeField] float increaseAmount;

    bool healthIncreased;
    List<int> familyToGetHealth = new List<int>();

    public static event Action ConditionsUpdated;
    public static event Action<bool> Fail;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        family = new FamilyMember[familyMembers];
        
        DataHolder shuttleObj = FindObjectOfType<DataHolder>();
        //Initialise each family member if "Data Container" object doesn't exist
        if (shuttleObj == null)
        {
            for (int i = 0; i < familyMembers; i++)
            {
                string name = "";
                switch (i)
                { 
                    case 0:
                        name = "Wife";
                        break;
                    case 1:
                        name = "Son";
                        break;
                    case 2:
                        name = "Daughter";
                        break;
                }

                family[i] = new FamilyMember(name, 100, 100, 100, medicineUI[i]);
            }
        }
        //Get values from Data Container after scene is reloaded
        else
        {
            for (int i = 0; i < familyMembers; i++)
                family[i] = shuttleObj.FamilyMembers[i];
        }

        //Fetches and sets corresponding Medicine Payment UI to each family member
        for(int i = 0; i < familyMembers; i++)
        {
            family[i].medicineUI = medicineUI[i];
            medicineUI[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        ShiftManager.CompleteShiftEvent += DisplayConditions;
        ShiftManager.NextShiftEvent += IncreaseConditions;

        ButtonSelection.Selected += AddToIncreasingConditions;
        ButtonSelection.Deselected += RemoveIncreasingConditions;
    }

    private void OnDisable()
    {
        ShiftManager.CompleteShiftEvent -= DisplayConditions;
        ShiftManager.NextShiftEvent -= IncreaseConditions;

        ButtonSelection.Selected -= AddToIncreasingConditions;
        ButtonSelection.Deselected -= RemoveIncreasingConditions;
    }

    //Add condition to be increased (will be increased later on)
    void AddToIncreasingConditions(int _paymentType, FamilyMember.ConditionType type)
    {
        if (type == FamilyMember.ConditionType.Health)
            familyToGetHealth.Add(_paymentType - 4);

        conditionsToIncrease.Add(type);
    }

    //Remove the condition from the list to be increased
    void RemoveIncreasingConditions(int ignore, FamilyMember.ConditionType type)
    {
        for(int i = 0; i < conditionsToIncrease.Count; i++)
        {
            if (conditionsToIncrease[i] == type)
            {
                conditionsToIncrease.RemoveAt(i);
                return;
            }
        }
    }

    //Increase the values of the conditions
    void IncreaseConditions()
    {
        for (int i = 0; i < conditionsToIncrease.Count; i++)
        {
            for (int member = 0; member < familyMembers; member++)
            {
                if (conditionsToIncrease.Count == 0)
                    break;

                //If health is being increased, do this seperately for each family member.
                //This will override health decreasing
                if (conditionsToIncrease[i] == FamilyMember.ConditionType.Health)
                {
                    for (int family = 0; family < familyToGetHealth.Count; family++)
                    {
                        if (familyToGetHealth[family] == member)
                        {
                            IncreaseFamilyHealth(member);
                            conditionsToIncrease.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    if (conditionsToIncrease.Count == 0)
                        break;

                    family[member].IncreaseAmounts(conditionsToIncrease[i], increaseAmount);
                }

            }
        }

        ReduceConditions();
    }

    void IncreaseFamilyHealth(int familyMember)
    {
        //Increase health for the specific family member
        family[familyMember].IncreaseAmounts(FamilyMember.ConditionType.Health, increaseAmount + 25);
        healthIncreased = true;
    }

    //Reduce conditions when shift is completed
    void ReduceConditions()
    {
        for (int i = 0; i < familyMembers; i++)
        {
            //healthIncreased used to prevent health draining when using medicine
            family[i].ReduceAmounts(reductionAmount, healthIncreased);

            //If any conditions are 0 or below, player has failed
            if (family[i].hunger <= 0 || family[i].warmth <= 0 || family[i].health <= 0)
            {
                Fail?.Invoke(false);
                //This return stops any other event or action being taken
                return;
            }
        }

        if (healthIncreased)
            healthIncreased = false;

        //Notify that the conditions have been increased
        ConditionsUpdated?.Invoke();
    }

    //Show each condition, depending on value. If below 40, show Hungry/Cold/Ill. If below 20, add "Really" in front of that
    void DisplayConditions()
    {
        for (int i = 0; i < familyMembers; i++)
        {
            string conditions = "";
            int badConditions = 0;

            if (family[i].hunger < 40)
            {
                if (family[i].hunger < 20)
                    conditions += "Really ";

                conditions += "Hungry";
                badConditions++;
            }

            if (family[i].warmth < 40)
            {
                if (badConditions > 0)
                    conditions += ", ";

                if (family[i].warmth < 20)
                    conditions += "Really ";

                conditions += "Cold";
                badConditions++;
            }

            if (family[i].health < 40)
            {
                if (badConditions > 0)
                    conditions += ", ";

                if (family[i].health < 20)
                    conditions += "Really ";

                conditions += "Ill";
                badConditions++;
            }

            if (badConditions == 0)
                conditions = "Ok";

            familyUI[i].text = conditions;
        }
    }
}
