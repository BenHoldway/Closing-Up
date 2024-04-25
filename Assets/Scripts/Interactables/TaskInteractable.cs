using UnityEngine;

public class TaskInteractable : Interactable, IInteractable
{
    [SerializeField] Sprite sprite;

    [SerializeField] float completionMaxTime;
    float completionCurrentTime;

    protected bool isBeingCompleted;
    protected bool isCompleted;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        completionCurrentTime = 0;
    }

    private void Update() 
    {
        if (isBeingCompleted)
        {
            completionCurrentTime += Time.deltaTime;

            if(completionCurrentTime >= completionMaxTime) 
            {
                CompleteTask();
            }
        }
    }

    public void Interact()
    {
        if (isCompleted)
            return;

        isBeingCompleted = true;
        print($"{gameObject.name} is a task, being completed");
    }

    public override void StopInteracting()
    {
        isBeingCompleted = false;
        completionCurrentTime = 0;
        print($"Cancelled interacting with the task object; {gameObject.name}");
    }

    protected virtual void CompleteTask()
    {
        print($"{gameObject.name}'s task has been completed!");
        Destroy(gameObject);
        isCompleted = true;
        isBeingCompleted = false;
    }
}
