using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable, IInteractable
{
    [SerializeField] KeyManager.KeyTypes keyType;

    public static event Action<KeyManager.KeyTypes> pickedUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        pickedUp?.Invoke(keyType);
        Destroy(gameObject);
    }
}
