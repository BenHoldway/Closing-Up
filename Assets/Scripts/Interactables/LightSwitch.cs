using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable, IInteractable
{
    bool lightIsOn;

    [SerializeField] GameObject[] lights;

    // Start is called before the first frame update
    void Start()
    {
        lightIsOn = false;

        foreach (GameObject light in lights)
            light.SetActive(false);
    }

    public void Interact()
    {
        if (lightIsOn)
        {
            print($"{gameObject.name} was switched off");
            lightIsOn = false;

            foreach(GameObject light in lights)
                light.SetActive(false);
        }
        else
        {
            print($"{gameObject.name} was switched on");
            lightIsOn = true;

            foreach (GameObject light in lights)
                light.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
