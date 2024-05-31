using System;
using System.Collections;
using UnityEngine;

public class Key : Interactable, IInteractable
{
    [SerializeField] KeyManager.KeyTypes keyType;

    [SerializeField][TextArea] string keyPickedUpText;

    public static event Action<KeyManager.KeyTypes, string> pickedUp;
    public static event Action<bool> HidePromptUI;

    // Start is called before the first frame update
    void Start()
    {
        //Sets position to a one of the children positions set up in Unity
        int randNum = UnityEngine.Random.Range(1, transform.childCount);
        transform.position = transform.GetChild(randNum).position;
    }

    public void Interact()
    {
        //Calls the key pick up event to notify observers
        pickedUp?.Invoke(keyType, keyPickedUpText);

        //Disables sprite, collider and glow
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        StartCoroutine(Delay());
    }

    //Waits for 3 seconds before destroying key object and notifying for the UI to be hidden
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);

        //Sends out notification to hide the prompt UI
        HidePromptUI?.Invoke(true);
        Destroy(gameObject);
    }
}
