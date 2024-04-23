using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] bool isInteracting;
    [SerializeField] float interactionRadius;
    [SerializeField] LayerMask interactionMask;

    [SerializeField] Collider2D[] interactableCols = new Collider2D[3];

    int numFound;
    int randNum;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Interact.started += _ => 
        { 
            isInteracting = true;
            randNum = Random.Range(0, numFound);
        };

        playerControls.Player.Interact.canceled += _ => { isInteracting = false; };
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        numFound = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, interactableCols, interactionMask);

        if(numFound > 0 && isInteracting) 
        { 
            Interactable interactable = interactableCols[randNum].GetComponent<Interactable>();

            if(interactable != null) 
            {
                interactable.Interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
        {
        }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
