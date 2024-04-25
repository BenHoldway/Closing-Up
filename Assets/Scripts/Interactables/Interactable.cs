using UnityEngine;

public interface IInteractable 
{
    public void Interact();
}

public class Interactable : MonoBehaviour
{
    [SerializeField] bool isATaskInteractable;
    public bool IsATaskInteractable { get { return isATaskInteractable; } private set { isATaskInteractable = value; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.01f, 0.01f, 0.1f));
    }

    public virtual void StopInteracting() { }
}
