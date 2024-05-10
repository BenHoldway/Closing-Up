using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    MoneyManager Instance;

    public int moneyAmout {  get; private set; }

    int moneyPerTask;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        moneyPerTask = 5;

        ShiftManager.CompleteShiftEvent += IncreaseMoney;
    }

    void IncreaseMoney()
    {
        moneyAmout += moneyPerTask * TaskTracker.Instance.GetCompletedTasks();
    }

    void ReduceMoney(int amount)
    {
        moneyAmout -= amount;
    }
}
