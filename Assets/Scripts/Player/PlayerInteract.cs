using System;
using Unity.VisualScripting;
using UnityEngine;

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

        playerControls.Player.Interact.started += _ => 
        { 
            isInteracting = true;
        };

        playerControls.Player.Interact.canceled += _ => 
        { 
            isInteracting = false;

            HandleClosestInteractable(true);
        };

        TaskInteractable.EnablePlayerMovement += EnableMovement;
        TaskInteractable.DisablePlayerMovement += DisableMovement;
    }

    private void OnDisable()
    {
        playerControls.Disable();

        TaskInteractable.EnablePlayerMovement -= EnableMovement;
        TaskInteractable.DisablePlayerMovement -= DisableMovement;
    }

    // Update is called once per frame
    void Update()
    {
        numFound = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, interactableCols, interactionMask);

        if (numFound > 0)
            HandleClosestInteractable(false);
        else
        {
            if(promptUI.activeSelf)
                promptUI.SetActive(false);
        }
                
    }

    void HandleClosestInteractable(bool stopInteracting)
    {
        int closestIndex = GetClosest();
        
        if (interactableCols[closestIndex] == null)
            return;

        iInteractable = interactableCols[closestIndex].GetComponent<IInteractable>();


        GetInteractPrompt?.Invoke(interactableCols[closestIndex].gameObject, promptUI);

        if (isInteracting)
        {
            if (iInteractable != null)
            {
                iInteractable.Interact();
                isInteracting = false;
            }
        }
        else if(stopInteracting)
        {
            interactable = interactableCols[closestIndex].GetComponent<Interactable>();

            if (interactable.IsATaskInteractable)
                interactable.StopInteracting();
        }
    }

    private int GetClosest()
    {
        float dis = 1000;
        int closestCol = 0;
        foreach(Collider2D col in interactableCols) 
        {
            if (col == null)
                continue;

            float newDis = Vector2.Distance(transform.position, col.gameObject.transform.position);
            if (newDis < dis)
            {
                dis = newDis;
                closestCol = System.Array.IndexOf(interactableCols, col);
            }

        }

        return closestCol;
    }

    void EnableMovement()
    {
        playerMovement.enabled = true;
    }

    void DisableMovement()
    {
        playerMovement.enabled = false;
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