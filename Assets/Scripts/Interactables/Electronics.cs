using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electronics : TaskInteractable, IInteractable
{
    bool isOn;
    [SerializeField] Sprite offSprite;
    [SerializeField] GameObject objLight;

    protected override void InteractionCompleted()
    {
        if(!isBeingCompleted) return;

        if (offSprite != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = offSprite;            
        }
        objLight.SetActive(false);

        print($"{gameObject.name}'s task has been completed!");
        isCompleted = true;
        isBeingCompleted = false;

        EnableMovement();
    }
}
