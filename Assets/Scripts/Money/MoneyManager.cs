using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] int startingMoney;
    public int moneyAmout {  get; private set; }
    public int NewMoney {  get; set; }


    int moneyPerTask;

    [SerializeField] TMP_Text totalMoneyText;
    [SerializeField] TMP_Text shiftMoneyText;
    [SerializeField] TMP_Text newMoneyText;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DataHolder shuttleObj = FindObjectOfType<DataHolder>();
        if (shuttleObj == null)
            moneyAmout = startingMoney;
        else
            moneyAmout = shuttleObj.Money;
        
        
        NewMoney = moneyAmout;
        moneyPerTask = 5;

        ShiftManager.CompleteShiftEvent += IncreaseMoney;
        Payments.PaymentEvent += ChangeMoney;
    }

    void IncreaseMoney()
    {
        int shiftMoney = moneyPerTask * TaskTracker.Instance.GetCompletedTasks();
        shiftMoneyText.text = shiftMoney.ToString();

        NewMoney += shiftMoney;
        newMoneyText.text = NewMoney.ToString();
        totalMoneyText.text = moneyAmout.ToString();

    }

    void ChangeMoney(int amount, bool isReduced)
    {
        if(isReduced)
            NewMoney -= amount;
        else
            NewMoney += amount;

        newMoneyText.text = NewMoney.ToString();
    }
}
