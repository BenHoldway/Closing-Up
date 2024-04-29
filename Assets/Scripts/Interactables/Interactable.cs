using System;
using UnityEngine;

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

    protected void GetUIPrompt(GameObject interactObj, GameObject promptUI)
    {
        if(interactObj == gameObject)
        {
            promptUI.SetActive(true);
            promptUI.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = UIText;
        }
    }

    public virtual void StopInteracting() { }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.01f, 0.01f, 0.1f));
    }
}
