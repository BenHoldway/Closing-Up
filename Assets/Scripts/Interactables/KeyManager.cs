using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public enum KeyTypes 
    { 
        office1, office2, office3, office4,
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
        keysPickedUp.Add(KeyTypes.office1, false);
        keysPickedUp.Add(KeyTypes.office2, false);
        keysPickedUp.Add(KeyTypes.office3, false);
        keysPickedUp.Add(KeyTypes.office4, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateKey(KeyTypes type)
    {
        switch(type) 
        {
            case KeyTypes.office1:
                keysPickedUp[KeyTypes.office1] = true;
                break;
            case KeyTypes.office2:
                keysPickedUp[KeyTypes.office2] = true;
                break;
            case KeyTypes.office3:
                keysPickedUp[KeyTypes.office3] = true;
                break;
            case KeyTypes.office4:
                keysPickedUp[KeyTypes.office4] = true;
                break;
        }
    }
}
