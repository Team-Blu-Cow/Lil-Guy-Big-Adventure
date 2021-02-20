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
}

[System.Serializable]
public class OneShot
{
    public string name;

    [Range(0, 1)] public float volume = 1;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}