using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataHolderController : MonoBehaviour
{
    [SerializeField] GameObject dataHolder;

    public static event Action<int, int, FamilyMember[]> SendDataEvent;

    private void Start()
    {
        DataHolder[] dataHolders = FindObjectsOfType<DataHolder>();
        if (dataHolders.Length > 0)
            foreach (DataHolder holder in dataHolders)
                Destroy(holder.gameObject);
    }

    // Update is called once per frame
    void OnEnable()
    {
        FamilyConditions.ReloadScene += SendData;
    }

    void OnDisable()
    {
        FamilyConditions.ReloadScene -= SendData;
    }

    void SendData()
    {
        Instantiate(dataHolder);
        SendDataEvent?.Invoke(ShiftManager.Instance.ShiftCount, MoneyManager.Instance.NewMoney, FamilyConditions.Instance.Family);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
