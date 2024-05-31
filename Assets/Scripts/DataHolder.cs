using System;
using UnityEngine;

public class DataHolder : MonoBehaviour
{ 
    public int ShiftNum { get; set; }
    public int Money { get; set; }
    public FamilyMember[] FamilyMembers { get; set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        FamilyMembers = new FamilyMember[3];
    }

    private void OnEnable()
    {
        DataHolderController.SendDataEvent += SaveData;
    }

    private void OnDisable()
    {
        DataHolderController.SendDataEvent -= SaveData;
    }

    //Saves all the data that is passed in
    public void SaveData(int _shiftNum, int _money, FamilyMember[] family) 
    { 
        ShiftNum = _shiftNum;
        Money = _money;

        for(int i = 0; i < family.Length; i++) 
        {
            FamilyMembers[i] = family[i];
        }
    }
}
