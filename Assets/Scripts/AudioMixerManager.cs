using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider mainVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundEffectsVolumeSlider;

    private void Start()
    {
        //Set value sliders to be the same as volumes
        mainVolumeSlider.value = GetAudioValue("mainVolume");
        musicVolumeSlider.value = GetAudioValue("musicVolume");
        soundEffectsVolumeSlider.value = GetAudioValue("soundEffectsVolume");
    }

    //Return volume
    float GetAudioValue(string name)
    {
        float value = 0f;
        bool isThere = audioMixer.GetFloat(name, out value);

        if (isThere)
            //Opposite of Log10
            return Mathf.Pow(10, value);
        //Return lowest value
        else
            return 0.0001f;
    }

    //Changes the Main Volume
    public void UpdateMainVolume(float val)
    {
        //Mathf.Log10(val) * 20f will convert the logarithmic decibel value to increase linearly
        audioMixer.SetFloat("mainVolume", Mathf.Log10(val) * 20f);
    }

    //Changes the Music Volume
    public void UpdateMusicVolume(float val)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(val) * 20f);
    }

    //Changes the Sound Effects Volume
    public void UpdateSoundEffectsVolume(float val)
    {
        audioMixer.SetFloat("soundEffectsVolume", Mathf.Log10(val) * 20f);
    }
}
