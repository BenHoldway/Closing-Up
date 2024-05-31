using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;

    [SerializeField] bool isInteracting;

    [SerializeField] float interactionRadius;
    [SerializeField] LayerMask interactionMask;

    [SerializeField] Collider2D[] interactableCols = new Collider2D[3];

    [SerializeField] GameObject promptUI;

    public static event Action<GameObject, GameObject> GetInteractPrompt;

    int numFound;
    int closest;

    Interactable interactable;
    IInteractable iInteractable;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Interact.started += Interact;
        playerControls.Player.Interact.canceled += StopInteract;

        playerControls.Player.Lock.started += ChangeDoorLockState;

        TaskInteractable.EnablePlayerMovement += EnableMovement;
        TaskInteractable.DisablePlayerMovement += DisableMovement;
        Key.HidePromptUI += HideUI;

        ShiftManager.CompleteShiftEvent += DisableInteract;
    }

    private void OnDisable()
    {
        playerControls.Disable();

        playerControls.Player.Interact.started -= Interact;
        playerControls.Player.Interact.canceled -= StopInteract;

        playerControls.Player.Lock.started -= ChangeDoorLockState;

        TaskInteractable.EnablePlayerMovement -= EnableMovement;
        TaskInteractable.DisablePlayerMovement -= DisableMovement;
        Key.HidePromptUI -= HideUI;

        ShiftManager.CompleteShiftEvent -= DisableInteract;
    }

    // Update is called once per frame
    void Update()
    {
        //Get amount of interactables in range
        numFound = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, interactableCols, interactionMask);

        if (numFound > 0)
            HandleClosestInteractable();
        else if (promptUI.activeSelf)
            HideUI(false);
    }

    void HandleClosestInteractable()
    {
        GetClosest();

        Collider2D closestCol = interactableCols[closest];
        if (closestCol == null)
            return;

        TaskInteractable taskInteractable = closestCol.gameObject.GetComponent<TaskInteractable>();
        if (taskInteractable != null && taskInteractable.IsCompleted)
            return;

        //Gets the Interface component of the object
        iInteractable = closestCol.GetComponent<IInteractable>();

        //Notify that interact UI is needed to be shown
        GetInteractPrompt?.Invoke(closestCol.gameObject, promptUI);
    }

    void Interact(InputAction.CallbackContext context)
    {
        //Call the interactable interface Interact() method
        if (iInteractable != null)
        {
            iInteractable.Interact();
            isInteracting = true;
        }
    }

    void StopInteract(InputAction.CallbackContext context)
    {
        if (!isInteracting)
            return;

        isInteracting = false;
        interactable = interactableCols[closest].GetComponent<Interactable>();

        //If interactable is a task, call the stop interacting method
        if (interactable.IsATaskInteractable)
            interactable.StopInteracting();
    }

    //Change the door lock state if the interactable object is a door
    void ChangeDoorLockState(InputAction.CallbackContext context)
    {
        if (interactableCols[closest] == null)
            return;

        Door door = interactableCols[closest].gameObject.GetComponent<Door>();

        if (door != null)
            door.ChangeLockState();
    }

    void GetClosest()
    {
        float dis = 1000;

        //Run through each interactable, and see which one is closest
        foreach (Collider2D col in interactableCols)
        {
            if (col == null)
                continue;

            float newDis = Vector2.Distance(transform.position, col.gameObject.transform.position);
            if (newDis < dis)
            {
                dis = newDis;
                closest = Array.IndexOf(interactableCols, col);
            }
        }
    }

    void EnableMovement()
    {
        playerMovement.enabled = true;
        isInteracting = false;
    }

    void DisableMovement()
    {
        playerMovement.enabled = false;
    }

    //Called to hide the interact prompt UI
    void HideUI(bool gotKey)
    {
        Collider2D closestCol = interactableCols[closest];

        //Hide if closest interactable does not exist anymore
        if (closestCol == null)
            promptUI.SetActive(false);
        //Return if interactable is a key and has not been fully collected yet
        else if (closestCol.gameObject.GetComponent<Key>() != null && !gotKey)
            return;

        promptUI.SetActive(false);
    }

    void DisableInteract()
    {
        this.enabled = false;
    }


    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}
