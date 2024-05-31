using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingManager : MonoBehaviour
{
    Light2D[] outsideLights;

    [SerializeField] Color startingColour;
    [SerializeField] Color endColour;

    [SerializeField] Light2D ambientLight;
    [SerializeField] float startingAmbience;
    [SerializeField] float finalAmbience;

    Color lerpedColour;

    float startColourChange;
    float endColourChange;

    [SerializeField] float startBuffer;
    [SerializeField] float endBuffer;

    float currentColourValue;

    // Start is called before the first frame update
    void Start()
    {
        //Get each outside light object and set them to be the starting colour
        Light2D[] children = gameObject.GetComponentsInChildren<Light2D>();
        outsideLights = new Light2D[children.Length];

        for (int i = 0; i < children.Length; i++)
        {
            outsideLights[i] = children[i].GetComponent<Light2D>();
            outsideLights[i].color = startingColour;
        }

        //Add buffers to the shift time so that the light doesn't change colour for the first and last sections of the shift
        startColourChange = ShiftManager.Instance.ShiftStartTime + startBuffer;
        endColourChange = ShiftManager.Instance.ShiftEndTime - endBuffer;

        //Set ambient intensity to starting ambience
        ambientLight.intensity = startingAmbience;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpedColour == endColour)
            return;

        //Clamp current value between 2 buffer values
        currentColourValue = Mathf.Clamp(ShiftManager.Instance.ShiftCurrentTime - startColourChange, 0, endColourChange);

        //Lerp the colour of the outside lights and ambient intensity
        lerpedColour = Color.Lerp(startingColour, endColour, currentColourValue / (endColourChange - startColourChange));
        ambientLight.intensity = Mathf.Lerp(startingAmbience, finalAmbience, currentColourValue / (endColourChange - startColourChange));

        //Set each light colour to be current lerped colour
        foreach (Light2D light in outsideLights)
        {
            light.color = lerpedColour;
        }
    }
}
