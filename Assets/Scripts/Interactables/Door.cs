using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable, IInteractable
{
    [SerializeField] bool isOpened;
    public bool IsLocked {  get; private set; }

    [SerializeField] bool showColourWhenOpen;
    [SerializeField] GameObject colourLayer;

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
        //Randomly sets each one to be open or closed
        int randNum = UnityEngine.Random.Range(0, 2);

        if (randNum == 0)
            isOpened = false;
        else
            isOpened = true;

        //If door is locked, set the text to say it is locked
        if(IsLocked) 
        {
            UIText = lockedText;
        }
        //Sets door and text to show door is open
        else if(isOpened)
        {
            openedDoor.SetActive(true);
            closedDoor.SetActive(false);
            UIText = openText;
        }
        //Sets door and text to show door is closed
        else if (!isOpened)
        {
            openedDoor.SetActive(false);
            closedDoor.SetActive(true);
            UIText = closedText;
        }

        ShowColour();
    }

    //Updates the door and text when it is interacted with
    public void Interact()
    {
        //Stops player interacting if door is locked
        if (IsLocked)
            return;

        //Closes the door
        else if (isOpened)
        {
            //Unselects trigger on collider to stop player from walking through the door
            Collider2D col = gameObject.GetComponent<Collider2D>();
            if (col.isTrigger)
                col.isTrigger = false;

            openedDoor.SetActive(false);
            closedDoor.SetActive(true);
            UIText = closedText;
            isOpened = false;
        }
        //Opens the door
        else
        {
            //Sets collider to trigger so player can walk through it
            Collider2D col = gameObject.GetComponent<Collider2D>();
            if (!col.isTrigger)
                col.isTrigger = true;

            openedDoor.SetActive(true);
            closedDoor.SetActive(false);
            UIText = openText;
            isOpened = true;
        }

        ShowColour();
    }
    
    //Manages the locking and unlocking of doors
    public void ChangeLockState()
    {
        //Can only lock/unlock if door is closed
        if (!isOpened)
        {
            //Prevents player from locking/unlocking door without correct key
            if (!keyManager.KeysPickedUp.ContainsKey(correctKey) || !keyManager.KeysPickedUp[correctKey] == true)
                return;

            //Locks the door
            if (!IsLocked)
            {
                IsLocked = true;
                StartCoroutine(LockedText(true));
            }
            //Unlocks the door
            else
            {
                IsLocked = false;
                StartCoroutine(LockedText(true));
            }
        }
        //Tells player they need to close door before locking
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
            if(IsLocked)
            {
                beforeText = nowLockedText;
                afterText = lockedText;
            }
            else
            {
                beforeText = unlockedText;
                afterText = closedText;
            }
        }
        //Unable to open door because it is locked
        else if(IsLocked)
        {
            beforeText = lockedText;
            afterText = lockedText;
        }
        //Locking the door
        else
        {
            beforeText = nowLockedText;
            afterText = closedText;
        }

        //Shows new lock state text, waits 2 seconds and then goes to passive text
        UIText = beforeText;
        yield return new WaitForSeconds(2);
        UIText = afterText;
    }

    //Shows/hides colour on door that corresponds to the key, depending on orientation of door
    void ShowColour()
    {
        //If colour can be seen when door is open, and door is open: show colour
        //If colour can be seen when door is closed, and door is closed: show colour
        if((showColourWhenOpen && isOpened) || (!showColourWhenOpen && !isOpened))
            colourLayer.SetActive(true);
        else
            colourLayer.SetActive(false);
    }
}
