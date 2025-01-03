using System;
using UnityEngine;

//Interface for any objects that can be interacted with
public interface IInteractable 
{
    public void Interact();
}

public class Interactable : MonoBehaviour
{
    [SerializeField] bool isATaskInteractable;
    public bool IsATaskInteractable { get { return isATaskInteractable; } private set { isATaskInteractable = value; } }

    [SerializeField][TextArea] protected string UIText;

    private void OnEnable()
    {
        PlayerInteract.GetInteractPrompt += GetUIPrompt;
    }

    private void OnDisable()
    {
        PlayerInteract.GetInteractPrompt -= GetUIPrompt;
    }

    //Enables prompt UI and sets text to be the interaction
    public virtual void GetUIPrompt(GameObject interactObj, GameObject promptUI)
    {
        if(interactObj == gameObject)
        {
            promptUI.SetActive(true);
            promptUI.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = UIText;
        }
    }

    //Method to stop the interaction
    public virtual void StopInteracting() { }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.01f, 0.01f, 0.1f));
    }
}
