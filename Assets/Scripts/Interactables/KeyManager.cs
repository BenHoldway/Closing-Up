using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public enum KeyTypes 
    { 
        General,
        Office_Space,
        Meeting_Room
    }

    Dictionary<KeyTypes, bool> keysPickedUp = new Dictionary<KeyTypes, bool>();

    public Dictionary<KeyTypes, bool> KeysPickedUp {  get { return keysPickedUp; } }

    public KeyTypes KeyType {  get; private set; }

    [SerializeField] GameObject promptUI;

    // Start is called before the first frame update
    void OnEnable()
    {
        Key.pickedUp += UpdateKey;
    }

    void OnDisable()
    {
        Key.pickedUp -= UpdateKey;
    }

    private void Awake()
    {
        keysPickedUp.Add(KeyTypes.General, false);
        keysPickedUp.Add(KeyTypes.Office_Space, false);
        keysPickedUp.Add(KeyTypes.Meeting_Room, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateKey(KeyTypes type, string keyText)
    {
        //Using the door key type as a key in the dictionary, and sets the corresponding collected bool to true
        switch(type) 
        {
            case KeyTypes.General:
                keysPickedUp[KeyTypes.General] = true;
                break;
            case KeyTypes.Office_Space:
                keysPickedUp[KeyTypes.Office_Space] = true;
                break;
            case KeyTypes.Meeting_Room:
                keysPickedUp[KeyTypes.Meeting_Room] = true;
                break;
        }

        PickedUpKeyNotif(keyText);
    }


    void PickedUpKeyNotif(string keyText)
    {
        promptUI.SetActive(true);

        //Set prompt text to notify player what key was picked up
        promptUI.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = keyText;
    }
}
