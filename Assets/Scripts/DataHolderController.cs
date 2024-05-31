using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataHolderController : MonoBehaviour
{
    [SerializeField] GameObject dataHolder;

    public static event Action<int, int, FamilyMember[]> SendDataEvent;
    public static event Action LoadNextShift;

    private void Start()
    {
        //All data is fetched in awake. Any data containers are deleted when Start is called
        DataHolder[] dataHolders = FindObjectsOfType<DataHolder>();
        if (dataHolders.Length > 0)
            foreach (DataHolder holder in dataHolders)
                Destroy(holder.gameObject);
    }

    // Update is called once per frame
    void OnEnable()
    {
        ShiftManager.EndDay += SendData;
    }

    void OnDisable()
    {
        ShiftManager.EndDay -= SendData;
    }

    //Create a data container and send all the data into it
    void SendData()
    {
        Instantiate(dataHolder);
        SendDataEvent?.Invoke(ShiftManager.Instance.ShiftCount, MoneyManager.Instance.NewMoney, FamilyConditions.Instance.Family);
        LoadNextShift?.Invoke();
    }
}
