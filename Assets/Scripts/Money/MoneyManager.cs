using System;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    const int moneyPerTask = 5;
    const int moneyPerHiddenTask = 2;

    public static MoneyManager Instance { get; private set; }

    [SerializeField] int startingMoney;
    public int moneyAmout {  get; private set; }
    public int NewMoney {  get; set; }

    [SerializeField] TMP_Text totalMoneyText;
    [SerializeField] TMP_Text shiftMoneyText;
    [SerializeField] TMP_Text newMoneyText;

    public static event Action MoneyChanged;
    public static event Action<int> ShowReductionAmount;
    public static event Action<bool> Fail;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DataHolder shuttleObj = FindObjectOfType<DataHolder>();
        //Set money to be starting value if "Data Container" object doesn't exist
        if (shuttleObj == null)
            moneyAmout = startingMoney;
        //Get values from Data Container after scene is reloaded
        else
            moneyAmout = shuttleObj.Money;
        
        NewMoney = moneyAmout;
    }

    private void OnEnable()
    {
        TaskTracker.ShiftReductionsEvent += GetShiftMoney;
        Payments.PaymentEvent += ChangeMoney;
    }

    private void OnDisable()
    {
        TaskTracker.ShiftReductionsEvent -= GetShiftMoney;
        Payments.PaymentEvent -= ChangeMoney;
    }

    //Get shift task money and extra money from the shift
    void GetShiftMoney(int reductions)
    {
        //reductions = 3
        int shiftMoney = moneyPerTask * TaskTracker.Instance.GetCompletedTasks(); //70
        int extraMoney = moneyPerHiddenTask * TaskTracker.Instance.GetHiddenCompletedTasks();
        shiftMoneyText.text = (shiftMoney + extraMoney).ToString();

        //Notify to show reductions, with value being clamped from amount of money from shift up to full reductions.
        //Clamp prevents reductions being more than money made from shift
        if(shiftMoney < reductions)
            reductions = shiftMoney;

        ShowReductionAmount(reductions);
        shiftMoney -= reductions;
        if(shiftMoney < 0)
            shiftMoney = 0;

        //Set new/remaining money after payments to be from shift, and display it
        NewMoney += shiftMoney + extraMoney;
        newMoneyText.text = NewMoney.ToString();
        totalMoneyText.text = moneyAmout.ToString();
    }

    //Reduce/Increase money from amount passed in
    void ChangeMoney(int amount, bool isReduced)
    {
        if(isReduced)
        {
            NewMoney -= amount;
        }
        else
            NewMoney += amount;

        if (NewMoney < 0)
            Fail?.Invoke(true);

        newMoneyText.text = NewMoney.ToString();
        //Notify that the money has changed
        MoneyChanged?.Invoke();
    }
}
