using System;
using UnityEngine;

public class Electronics : TaskInteractable, IInteractable
{
    public bool IsOn { get; set; }
    [SerializeField] GameObject offComputer;
    [SerializeField] GameObject onComputer;

    [SerializeField] GameObject objLight;

    public static event Action<TaskInteractable> NewComputer;

    void OnEnable()
    {
        RoomManager.ComputersInitialised += ChangeComputerState;
        PlayerInteract.GetInteractPrompt += GetUIPrompt;
    }

    void OnDisable()
    {
        RoomManager.ComputersInitialised -= ChangeComputerState;
        PlayerInteract.GetInteractPrompt -= GetUIPrompt;
    }

    void ChangeComputerState(int ignore)
    {
        //If computer is on, show sprite for on and enable glow
        if (IsOn)
        {
            onComputer.SetActive(true);
            offComputer.SetActive(false);
            objLight.SetActive(true);

            //Event to add on to total task count
            NewComputer?.Invoke(this);
        }
        //Set computer to be off, and hide glow. Set interactable to be done to disable any interaction
        else
        {
            onComputer.SetActive(false);
            offComputer.SetActive(true);
            objLight.SetActive(false);

            IsCompleted = true;
        }
    }

    //Call parent method to stop interaction
    //For some reason, without this, it wouldn't call this method
    public override void StopInteracting()
    {
        base.StopInteracting();
    }

    //Call parent method to show UI prompt
    //For some reason, without this, it wouldn't call this method
    public override void GetUIPrompt(GameObject interactObj, GameObject promptUI)
    {
        base.GetUIPrompt(interactObj, promptUI);
    }

    protected override void InteractionCompleted()
    {
        if(!isBeingCompleted) 
            return;

        //Disable box collider to stop interaction
        GetComponent<BoxCollider2D>().enabled = false;
        //Stop interacting sound
        AudioManager.Instance.StopSound(task.Clip);

        //Set computer to be off, and hide glow
        onComputer.SetActive(false);
        offComputer.SetActive(true);
        objLight.SetActive(false);

        IsCompleted = true;
        isBeingCompleted = false;

        EnableMovement();
        //Notifies that this task has been completed
        CommunicateCompletion();
    }
}
