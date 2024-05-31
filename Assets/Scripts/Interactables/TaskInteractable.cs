using System;
using UnityEngine;
using UnityEngine.UI;

public class TaskInteractable : Interactable, IInteractable
{
    [SerializeField] protected Task task;
    public Task TaskSO { get { return task; } private set { task = value; } }

    SpriteRenderer spriteRend;

    float completionMaxTime;
    float completionCurrentTime;

    protected bool isBeingCompleted;
    public bool IsCompleted { get; protected set; }

    bool isPlayingSound;

    public static event Action DisablePlayerMovement;
    public static event Action EnablePlayerMovement;
    public static event Action<TaskInteractable> TaskCompleted;
    public static event Action<float> ChangeSliderFill;

    private void Awake()
    {
        //Get variables from task scriptable object
        spriteRend = GetComponent<SpriteRenderer>();
        if (task != null) 
        { 
            //Get random sprite from available list
            if(task.SpriteAsset.Count > 0)
            {
                int randSprite = UnityEngine.Random.Range(0, task.SpriteAsset.Count);
                spriteRend.sprite = task.SpriteAsset[randSprite];
            }

            UIText = task.UIText;
            completionMaxTime = task.CompletionTime;
        }

        completionCurrentTime = 0;
        isPlayingSound = false;
    }

    private void Update() 
    {
        //Run if the player is holding the interaction key on the object
        if (isBeingCompleted)
        {
            if(!isPlayingSound) 
            {
                AudioManager.Instance.PlaySound(task.Clip, gameObject.transform, 1f);
                isPlayingSound = true;
            }

            //Add the time to the completion time
            completionCurrentTime += Time.deltaTime;

            //Increase the visual interaction bar by the percentage of completion
            ChangeSliderFill?.Invoke((1 / completionMaxTime) * completionCurrentTime);

            //If the completion reaches the max time, the interaction is done
            if(completionCurrentTime >= completionMaxTime) 
                InteractionCompleted();
        }
    }

    //Starts interaction and prevents player from moving during this
    public void Interact()
    {
        //If completed, prevent the object from being interacted with again
        if (IsCompleted)
            return;

        isBeingCompleted = true;

        DisableMovement();
    }

    //Stops the interaction and enables player movement
    public override void StopInteracting()
    {
        isBeingCompleted = false;
        completionCurrentTime = 0;

        AudioManager.Instance.StopSound(task.Clip);
        isPlayingSound = false;

        EnableMovement();
    }

    //Runs if the interaction is done
    protected virtual void InteractionCompleted()
    {
        AudioManager.Instance.StopSound(task.Clip);

        IsCompleted = true;
        isBeingCompleted = false;
        isPlayingSound = false;

        EnableMovement();
        CommunicateCompletion();
        Destroy(gameObject);
    }

    //Notify that the task is completed
    protected void CommunicateCompletion()
    {
        TaskCompleted?.Invoke(this);
    }

    //This is so any child classes can call the event
    protected void EnableMovement()
    {
        ChangeSliderFill?.Invoke(0);
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
