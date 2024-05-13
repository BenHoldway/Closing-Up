using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] int startingMoney;
    public int moneyAmout {  get; private set; }
    int newMoney;


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

        moneyAmout = startingMoney;
        newMoney = moneyAmout;
        moneyPerTask = 5;

        ShiftManager.CompleteShiftEvent += IncreaseMoney;
        ShiftManager.NextShiftEvent += NextShift;
        Payments.PaymentEvent += ChangeMoney;
    }

    void IncreaseMoney()
    {
        int shiftMoney = moneyPerTask * TaskTracker.Instance.GetCompletedTasks();
        shiftMoneyText.text = shiftMoney.ToString();

        newMoney += shiftMoney;
        newMoneyText.text = newMoney.ToString();
        totalMoneyText.text = moneyAmout.ToString();

    }

    void ChangeMoney(int amount, bool isReduced)
    {
        if(isReduced)
            newMoney -= amount;
        else
            newMoney += amount;

        newMoneyText.text = newMoney.ToString();
    }

    void NextShift()
    {
        moneyAmout = newMoney;
    }
}
