using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable, IInteractable
{
    [SerializeField] bool isOpened;
    [SerializeField] bool isLocked;

    [SerializeField] KeyManager keyManager;

    [SerializeField] GameObject closedDoor;
    [SerializeField] GameObject openedDoor;

    [SerializeField] KeyManager.KeyTypes correctKey;

    [SerializeField][TextArea] string closedText;
    [SerializeField][TextArea] string openText;
    [SerializeField][TextArea] string lockedText;

    [SerializeField][TextArea] string nowLockedText;
    [SerializeField][TextArea] string unlockedText;

    // Start is called before the first frame update
    void Start()
    {
        if(isLocked) 
        {
            UIText = lockedText;
        }
        else if(isOpened)
        {
            //openedDoor.SetActive(true);
            closedDoor.SetActive(false);
            UIText = openText;
        }
        else if(!isOpened && !isLocked)
        {
            //openedDoor.SetActive(false);
            closedDoor.SetActive(true);
            UIText = closedText;
        }
    }

    public void Interact()
    {
        if (isLocked)
        {
            print($"{gameObject.name} is locked, unable to open");
            UIText = lockedText;
            StartCoroutine(LockedText(false));
            return;
        }

        else if (isOpened)
        {
            Collider2D col = gameObject.GetComponent<Collider2D>();
            if (col.isTrigger)
                col.isTrigger = false;

            print($"{gameObject.name} was closed");
            //openedDoor.SetActive(false);
            closedDoor.SetActive(true);
            UIText = closedText;
            isOpened = false;
        }
        else
        {
            Collider2D col = gameObject.GetComponent<Collider2D>();
            if (!col.isTrigger)
                col.isTrigger = true;

            print($"{gameObject.name} was opened");
            //openedDoor.SetActive(true);
            closedDoor.SetActive(false);
            //gameObject.SetActive(false);
            UIText = openText;
            isOpened = true;
        }
    }
    
    public void ChangeLockState()
    {
        if (!isOpened)
        {
            if (!keyManager.KeysPickedUp.ContainsKey(correctKey) || !keyManager.KeysPickedUp[correctKey] == true)
                return;

            if (!isLocked)
            {
                isLocked = true;
                StartCoroutine(LockedText(true));
                print($"{gameObject.name} is now locked");
            }
            else
            {
                isLocked = false;
                StartCoroutine(LockedText(true));
                print($"{gameObject.name} is now unlocked");
            }
        }
        else if (isOpened)
            print($"Please close {gameObject.name} before attempting to lock");
    }

    IEnumerator LockedText(bool changingState)
    {
        string beforeText = "";
        string afterText = "";

        //Locking/Unlocking Door
        if(changingState)
        {
            if(isLocked)
                beforeText = nowLockedText;
            else
                beforeText = unlockedText;

            afterText = openText;
        }
        //Unable to open door because it is locked
        else if(isLocked)
        {
            beforeText = lockedText;
            afterText = closedText;
        }
        //Locking the door
        else
        {
            beforeText = nowLockedText;
            afterText = closedText;
        }

        UIText = beforeText;

        yield return new WaitForSeconds(2);

        UIText = afterText;
    }
}
