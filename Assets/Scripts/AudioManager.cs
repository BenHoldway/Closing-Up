using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource audioSource;
    List<AudioSource> sources = new List<AudioSource>();

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    //Plays sound until length float has been reached
    public void PlaySound(AudioClip clip, Transform position, float volume)
    {
        //Create the source object, and set the clip to play and volume
        AudioSource source = Instantiate(audioSource, position);
        source.clip = clip;
        source.volume = volume;

        source.Play();

        sources.Add(source);
    }

    //Runs through each source until clip is found, and destroy it to stop playing it
    public void StopSound(AudioClip clip) 
    {
        for(int i = 0; i < sources.Count; i++) 
        {
            if (sources[i].clip == clip)
            {
                Destroy(sources[i]);
                sources.RemoveAt(i);
                CancelInvoke();
            }
        }
    }
}
