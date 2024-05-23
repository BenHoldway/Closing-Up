using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public enum KeyTypes 
    { 
        Entrance_WorkSpace, Entrance_BreakRoom, BreakRoom_WorkSpace, WorkSpace_Kitchen, WorkSpace_MeetingRoom
    }

    Dictionary<KeyTypes, bool> keysPickedUp = new Dictionary<KeyTypes, bool>();

    public Dictionary<KeyTypes, bool> KeysPickedUp {  get { return keysPickedUp; } }

    public KeyTypes KeyType {  get; private set; }

    // Start is called before the first frame update
    void OnEnable()
    {
        Key.pickedUp += UpdateKey;
    }

    private void Awake()
    {
        keysPickedUp.Add(KeyTypes.Entrance_WorkSpace, false);
        keysPickedUp.Add(KeyTypes.Entrance_BreakRoom, false);
        keysPickedUp.Add(KeyTypes.BreakRoom_WorkSpace, false);
        keysPickedUp.Add(KeyTypes.WorkSpace_Kitchen, false);
        keysPickedUp.Add(KeyTypes.WorkSpace_MeetingRoom, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateKey(KeyTypes type)
    {
        switch(type) 
        {
            case KeyTypes.Entrance_WorkSpace:
                keysPickedUp[KeyTypes.Entrance_WorkSpace] = true;
                break;
            case KeyTypes.Entrance_BreakRoom:
                keysPickedUp[KeyTypes.Entrance_BreakRoom] = true;
                break;
            case KeyTypes.BreakRoom_WorkSpace:
                keysPickedUp[KeyTypes.BreakRoom_WorkSpace] = true;
                break;
            case KeyTypes.WorkSpace_Kitchen:
                keysPickedUp[KeyTypes.WorkSpace_Kitchen] = true;
                break;
            case KeyTypes.WorkSpace_MeetingRoom:
                keysPickedUp[KeyTypes.WorkSpace_MeetingRoom] = true;
                break;
        }
    }
}
