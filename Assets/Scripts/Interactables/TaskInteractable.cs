using System;
using UnityEngine;
using UnityEngine.UI;

public class TaskInteractable : Interactable, IInteractable
{
    [SerializeField] Sprite sprite;

    [SerializeField] float completionMaxTime;
    float completionCurrentTime;

    protected bool isBeingCompleted;
    protected bool isCompleted;

    public static event Action DisablePlayerMovement;
    public static event Action EnablePlayerMovement;

    [SerializeField] Slider fillAmount;

    private void Awake()
    {
        if(sprite != null)
            GetComponent<SpriteRenderer>().sprite = sprite;

        completionCurrentTime = 0;
        fillAmount.value = 0;
    }

    private void Update() 
    {
        if (isBeingCompleted)
        {
            completionCurrentTime += Time.deltaTime;

            fillAmount.value = (1 / completionMaxTime) * completionCurrentTime;

            if(completionCurrentTime >= completionMaxTime) 
            {
                InteractionCompleted();
            }
        }
    }

    public void Interact()
    {
        if (isCompleted)
            return;

        isBeingCompleted = true;
        print($"{gameObject.name} is a task, being completed");

        DisableMovement();
    }

    public override void StopInteracting()
    {
        isBeingCompleted = false;
        completionCurrentTime = 0;
        print($"Cancelled interacting with the task object; {gameObject.name}");

        EnableMovement();
    }

    protected virtual void InteractionCompleted()
    {
        print($"{gameObject.name}'s task has been completed!");
        Destroy(gameObject);
        isCompleted = true;
        isBeingCompleted = false;

        EnableMovement();
    }

    //This is so any child classes can call the event
    protected void EnableMovement()
    {
        fillAmount.value = 0;
        EnablePlayerMovement?.Invoke();
    }

    protected virtual void DisableMovement() 
    {
        DisablePlayerMovement?.Invoke();
    }
}
