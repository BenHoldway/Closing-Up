using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : Interactable, IInteractable
{
    public bool LightIsOn {  get; private set; }

    [SerializeField] GameObject lightParent;
    GameObject[] lights;

    [SerializeField][TextArea] string onText;
    [SerializeField][TextArea] string offText;

    // Start is called before the first frame update
    void Start()
    {
        //Randomly sets each one to be on or off
        int randNum = UnityEngine.Random.Range(0, 2);

        if (randNum == 0)
            LightIsOn = false;
        else
            LightIsOn = true;

        GetAllLights();

        //Set each light to be on/off depending on bool state
        foreach (GameObject light in lights)
            light.SetActive(LightIsOn);

        //Prompt UI text set to "Turn Off"
        if (LightIsOn)
            UIText = onText;
        //Prompt UI text set to "Turn On"
        else
            UIText = offText;
    }

    //Gets all the lights related to this switch
    void GetAllLights()
    {
        Light2D[] children = lightParent.GetComponentsInChildren<Light2D>();
        lights = new GameObject[children.Length];

        for (int i = 0; i < children.Length; i++)
            lights[i] = children[i].gameObject;
    }

    public void Interact()
    {
        //Turn all lights off
        if (LightIsOn)
        {
            LightIsOn = false;
            UIText = offText;

            foreach(GameObject light in lights)
                light.SetActive(false);
        }
        //Turn all lights on
        else
        {
            LightIsOn = true;
            UIText = onText;

            foreach (GameObject light in lights)
                light.SetActive(true);
        }
    }
}
