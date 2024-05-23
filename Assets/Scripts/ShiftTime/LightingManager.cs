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
        Light2D[] children = gameObject.GetComponentsInChildren<Light2D>();
        outsideLights = new Light2D[children.Length];

        for (int i = 0; i < children.Length; i++)
        {
            outsideLights[i] = children[i].GetComponent<Light2D>();
            outsideLights[i].color = startingColour;
        }

        startColourChange = ShiftManager.Instance.ShiftStartTime + startBuffer;
        endColourChange = ShiftManager.Instance.ShiftEndTime - endBuffer;

        ambientLight.intensity = startingAmbience;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpedColour == endColour)
            return;

        currentColourValue = Mathf.Clamp(ShiftManager.Instance.ShiftCurrentTime - startColourChange, 0, endColourChange);

        lerpedColour = Color.Lerp(startingColour, endColour, currentColourValue / (endColourChange - startColourChange));
        ambientLight.intensity = Mathf.Lerp(startingAmbience, finalAmbience, currentColourValue / (endColourChange - startColourChange));

        foreach (Light2D light in outsideLights)
        {
            light.color = lerpedColour;
        }
    }
}
