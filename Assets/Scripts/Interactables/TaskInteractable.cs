using System;
using UnityEngine;
using UnityEngine.UI;

public class TaskInteractable : Interactable, IInteractable
{
    [SerializeField] Slider fillAmount;
    [SerializeField] Task task;
    public Task TaskSO { get { return task; } private set { task = value; } }

    SpriteRenderer spriteRend;

    float completionMaxTime;
    float completionCurrentTime;

    protected bool isBeingCompleted;
    protected bool isCompleted;

    public static event Action DisablePlayerMovement;
    public static event Action EnablePlayerMovement;


    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        if (task != null) 
        { 
            spriteRend.sprite = task.SpriteAsset;
            UIText = task.UIText;
            completionMaxTime = task.CompletionTime;
        }

        completionCurrentTime = 0;
        fillAmount.value = 0;
    }

    private void Update() 
    {
        //Run if the player is holding the interaction key on the object
        if (isBeingCompleted)
        {
            //Add the time to the completion time
            completionCurrentTime += Time.deltaTime;
            print(completionCurrentTime);

            //Increase the visual interaction bar by the percentage of completion
            fillAmount.value = (1 / completionMaxTime) * completionCurrentTime;

            //If the completion reaches the max time, the interaction is done
            if(completionCurrentTime >= completionMaxTime) 
                InteractionCompleted();
        }
    }

    //Starts interaction and prevents player from moving during this
    public void Interact()
    {
        //If completed, prevent the object from being interacted with again
        if (isCompleted)
            return;

        isBeingCompleted = true;
        //print($"{gameObject.name} is a task, being completed");

        DisableMovement();
    }

    //Stops the interaction and enables player movement
    public override void StopInteracting()
    {
        isBeingCompleted = false;
        completionCurrentTime = 0;
        //print($"Cancelled interacting with the task object; {gameObject.name}");

        EnableMovement();
    }

    //Runs if the interaction is done
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.01f, 0.01f, 0.1f));
    }
}
