using UnityEngine;

public class DataHolder : MonoBehaviour
{ 
    public int ShiftNum { get { return ShiftNum; } set { ShiftNum = value; } }
    public int Money { get { return Money; } set { Money = value; } }
    public FamilyMember[] FamilyMembers { get { return FamilyMembers; } set { FamilyMembers = value; } }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    DataHolder(int _shiftNum, int _money) 
    { 
        ShiftNum = _shiftNum;
        Money = _money;
    }
}
