using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OutsideLighting : MonoBehaviour
{
    Light2D[] outsideLights;

    [SerializeField] Color startingColour;
    [SerializeField] Color endColour;

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
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Light2D light in outsideLights)
            light.color = Color.Lerp(startingColour, endColour, Mathf.Clamp(Time.deltaTime, ShiftManager.Instance.ShiftEndTime, ShiftManager.Instance.ShiftStartTime));
    }
}
