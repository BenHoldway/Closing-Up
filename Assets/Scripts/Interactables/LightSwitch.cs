using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : Interactable, IInteractable
{
    [SerializeField] bool lightIsOn;

    [SerializeField] GameObject lightParent;
    GameObject[] lights;

    [SerializeField][TextArea] string onText;
    [SerializeField][TextArea] string offText;

    // Start is called before the first frame update
    void Start()
    {
        GetAllLights();

        foreach (GameObject light in lights)
            light.SetActive(lightIsOn);

        if (lightIsOn)
            UIText = onText;
        else
            UIText = offText;
    }

    void GetAllLights()
    {
        Light2D[] children = lightParent.GetComponentsInChildren<Light2D>();
        lights = new GameObject[children.Length];

        for (int i = 0; i < children.Length; i++)
            lights[i] = children[i].gameObject;
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
