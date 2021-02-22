using UnityEngine;

public class getAudioManager : MonoBehaviour
{
    private AudioManager instance;

    // Start is called before the first frame update
    public void Play(string in_string)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.Play(in_string);
    }

    public void Stop(string in_string)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.Stop(in_string);
    }

    public void FadeOut(string in_string)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.FadeOut(in_string);
    }

    public void FadeIn(string in_string)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.FadeIn(in_string);
    }

    public void setMusicVolume(float in_vol)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.setVolume("MusicVolume", in_vol);
    }

    public void setMasterVolume(float in_vol)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.setVolume("MasterVolume", in_vol);
    }

    public void setSFXVolume(float in_vol)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.setVolume("SFXVolume", in_vol);
    }

    public void setAmbientVolume(float in_vol)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.setVolume("AmbientVolume", in_vol);
    }

    public void setOneShotVolume(float in_vol)
    {
        instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instance.setVolume("OneShotVolume", in_vol);
    }
}