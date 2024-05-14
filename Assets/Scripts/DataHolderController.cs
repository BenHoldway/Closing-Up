using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolderController : MonoBehaviour
{
    [SerializeField] GameObject dataHolder;

    // Update is called once per frame
    void OnEnable()
    {
        ShiftManager.NextShiftEvent += SendData;
    }

    void SendData()
    {
        Instantiate(dataHolder);

    }
}
