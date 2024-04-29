using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable, IInteractable
{
    [SerializeField] bool lightIsOn;

    [SerializeField] GameObject[] lights;

    [SerializeField][TextArea] string onText;
    [SerializeField][TextArea] string offText;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject light in lights)
            light.SetActive(lightIsOn);

        if (lightIsOn)
            UIText = onText;
        else
            UIText = offText;
    }

    public void Interact()
    {
        if (lightIsOn)
        {
            print($"{gameObject.name} was switched off");
            lightIsOn = false;
            UIText = offText;

            foreach(GameObject light in lights)
                light.SetActive(false);
        }
        else
        {
            print($"{gameObject.name} was switched on");
            lightIsOn = true;
            UIText = onText;

            foreach (GameObject light in lights)
                light.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
