using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string name;

    [Range(0, 1)] public float volume = 1;
    [SerializeField] public AudioClip start;
    [SerializeField] public AudioClip musicLoop;
    [SerializeField] public AudioClip end;
    [HideInInspector] public AudioSource startSource;
    [HideInInspector] public AudioSource loopSource;
    [HideInInspector] public AudioSource endSource;

    public AudioMixerGroup group;

    public void initAudio()
    {
        if (group != null)
        {
            startSource.outputAudioMixerGroup = group;
            loopSource.outputAudioMixerGroup = group;
            endSource.outputAudioMixerGroup = group;
        }
        else
        {
            Debug.Log("WARNING: " + name + "has not been assigned a mixer group!");
        }
    }

    public IEnumerator Play()
    {
        startSource.Play();
        yield return new WaitForSeconds(start.length);
        loopSource.Play();
    }

    public void Stop()
    {
        loopSource.Stop();
        endSource.Play();
    }

    public IEnumerator FadeOut()
    {
        //bool done = false;
        //while (!done)
        //{
        //    volume -= Time.deltaTime;
        //    if (volume <= 0f)
        //    {
        //        //Stop();
        //        //volume = 1f;
        //        done = true;
        //    }
        //}
        Debug.LogWarning("Deprecated lmao");
        yield return null;
    }
}

[System.Serializable]
public class OneShot
{
    public string name;
    public bool loops;
    [Range(0, 1)] public float volume = 1;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;
    public AudioMixerGroup group;

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void initAudio()
    {
        if (group != null)
        {
            source.outputAudioMixerGroup = group;
        }
        else
        {
            Debug.Log("WARNING: " + name + "has not been assigned a mixer group!");
        }
    }

    public IEnumerator FadeOut()
    {
        //bool done = false;
        //while (!done)
        //{
        //    volume -= Time.deltaTime * 0.1f;
        //    if (volume <= 0f)
        //    {
        //        Stop();
        //        volume = 1f;
        //        done = true;
        //    }
        //}
        //
        Debug.LogWarning("Deprecated lmao");
        yield return null;
    }
}