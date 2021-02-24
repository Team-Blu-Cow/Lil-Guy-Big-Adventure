using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Music[] music;
    public OneShot[] oneShots;
    private List<string> currentlyPlaying;

    public static AudioManager instance;
    public AudioMixer mixer;

    // Start is called before the first frame update
    private void Awake()
    {
        currentlyPlaying = new List<string>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Music s in music)
        {
            s.startSource = gameObject.AddComponent<AudioSource>();
            s.startSource.clip = s.start;
            s.startSource.loop = false;
            s.startSource.playOnAwake = false;
            s.startSource.volume = s.volume;
            s.loopSource = gameObject.AddComponent<AudioSource>();
            s.loopSource.clip = s.musicLoop;
            s.loopSource.loop = true;
            s.loopSource.playOnAwake = false;
            s.loopSource.volume = s.volume;
            s.endSource = gameObject.AddComponent<AudioSource>();
            s.endSource.clip = s.end;
            s.endSource.loop = false;
            s.endSource.playOnAwake = false;
            s.endSource.volume = s.volume;
            s.initAudio();
        }

        foreach (OneShot s in oneShots)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loops;
            s.source.playOnAwake = false;
            s.source.volume = s.volume;
            s.initAudio();
        }
    }

    // Update is called once per frame

    public void Play(string in_name)
    {
        Music m = Array.Find(music, sounds => sounds.name == in_name);
        if (m != null)
        {
            StartChildCoroutine(m.Play());
            currentlyPlaying.Add(in_name);
            return;
        }
        OneShot o = Array.Find(oneShots, sounds => sounds.name == in_name);
        if (o != null)
        {
            o.Play();

            currentlyPlaying.Add(in_name);
            return;
        }
        Debug.LogWarning("No valid Audio matches name: " + in_name);
        return;
    }

    public void Stop(string in_name)
    {
        Music m = Array.Find(music, sounds => sounds.name == in_name);
        if (m != null)
        {
            m.Stop();

            currentlyPlaying.Remove(in_name);
            return;
        }
        OneShot o = Array.Find(oneShots, sounds => sounds.name == in_name);
        if (o != null)
        {
            o.Stop();

            currentlyPlaying.Remove(in_name);
            return;
        }
    }

    public void StopAll()
    {
        foreach (string s in currentlyPlaying)
        {
            Music m = Array.Find(music, sounds => sounds.name == s);
            if (m != null)
            {
                m.Stop();
                return;
            }
            OneShot o = Array.Find(oneShots, sounds => sounds.name == s);
            if (o != null)
            {
                o.Stop();

                return;
            }
        }

        currentlyPlaying.Clear();
    }

    public void FadeOut(string in_name)
    {
        Music m = Array.Find(music, sounds => sounds.name == in_name);
        if (m != null)
        {
            StartChildCoroutine(m.FadeOut());

            currentlyPlaying.Remove(in_name);
            return;
        }
        OneShot o = Array.Find(oneShots, sounds => sounds.name == in_name);
        if (o != null)
        {
            StartChildCoroutine(o.FadeOut());

            currentlyPlaying.Remove(in_name);
            return;
        }
    }

    public void FadeIn(string in_name)
    {
        Music m = Array.Find(music, sounds => sounds.name == in_name);
        if (m != null)
        {
            StartChildCoroutine(m.FadeIn());

            currentlyPlaying.Add(in_name);
            return;
        }
        OneShot o = Array.Find(oneShots, sounds => sounds.name == in_name);
        if (o != null)
        {
            StartChildCoroutine(o.FadeIn());

            currentlyPlaying.Add(in_name);
            return;
        }
    }

    public void StartChildCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }

    public void setVolume(string in_name, float in_vol)
    {
        float logValue = Mathf.Log10(in_vol) * 20; // converts linear slider value to decibel log curve
        mixer.SetFloat(in_name, logValue);
    }
}