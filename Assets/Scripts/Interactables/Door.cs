using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable, IInteractable
{
    [SerializeField] bool isOpened;
    [SerializeField] bool isLocked;

    [SerializeField] GameObject keyManagerObj;

    [SerializeField] GameObject closedDoor;
    [SerializeField] GameObject openedDoor;

    [SerializeField] KeyManager.KeyTypes correctKey;

    // Start is called before the first frame update
    void Start()
    {
        if(isOpened)
        {
            openedDoor.SetActive(true);
            closedDoor.SetActive(false);
        }
        else
        {
            openedDoor.SetActive(false);
            closedDoor.SetActive(true);
        }
    }

    public void Interact()
    {
        if (isLocked)
        {
            print($"{gameObject.name} is locked, unable to open");
            return;
        }

        if (isOpened)
        {
            print($"{gameObject.name} was closed");
            openedDoor.SetActive(false);
            closedDoor.SetActive(true);
            isOpened = false;
        }
        else
        {
            print($"{gameObject.name} was opened");
            openedDoor.SetActive(true);
            closedDoor.SetActive(false);
            isOpened = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpened && Input.GetMouseButtonDown(1))
        {
            KeyManager keyManager = keyManagerObj.GetComponent<KeyManager>();
            if (!keyManager.KeysPickedUp.ContainsKey(correctKey) || !keyManager.KeysPickedUp[correctKey] == true)
                return;

            if (!isLocked)
            {
                isLocked = true;
                print($"{gameObject.name} is now locked");
            }
            else
            {
                isLocked = false;
                print($"{gameObject.name} is now unlocked");
            }
        }
        else if (isOpened && Input.GetMouseButtonDown(1))
            print($"Please close {gameObject.name} before attempting to lock");
    }
}
