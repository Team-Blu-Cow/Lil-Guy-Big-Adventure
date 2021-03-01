using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string name;

    [Range(0, 1)] public float volume = 1;
    [Range(0, 20)] public float leadInTime = 1;
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
            if (start != null)
            {
                startSource.outputAudioMixerGroup = group;
            }
            loopSource.outputAudioMixerGroup = group;
            if (end != null)
            {
                endSource.outputAudioMixerGroup = group;
            }
        }
        else
        {
            Debug.Log("WARNING: " + name + "has not been assigned a mixer group!");
        }
    }

    public IEnumerator Play()
    {
        if (start != null)
        {
            // Debug.Log("hit");
            startSource.Play();
            yield return new WaitForSeconds(start.length);
        }
        else
        {
            // Debug.Log("leadin");
            yield return new WaitForSeconds(leadInTime);
        }

        loopSource.Play();
    }

    public void Stop()
    {
        loopSource.Stop();
        if (end != null)
        {
            endSource.Play();
        }
    }

    public IEnumerator FadeOut()
    {
        {
            float currentTime = 0;
            float start = loopSource.volume;

            while (currentTime < 1)
            {
                currentTime += Time.deltaTime;
                if (start != null)
                {
                    startSource.volume = Mathf.Lerp(start, 0f, currentTime / 1);
                }
                loopSource.volume = Mathf.Lerp(start, 0f, currentTime / 1);
                if (end != null)
                {
                    endSource.volume = Mathf.Lerp(start, 0f, currentTime / 1);
                }
                yield return null;
            }
            Stop();
            if (start != null)
            {
                startSource.volume = 1f;
            }
            loopSource.volume = 1f;
            if (end != null)
            {
                endSource.volume = 1f;
            }
            yield break;
        }
    }

    public IEnumerator FadeIn()
    {
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(name);
            float currentTime = 0;
            float start = 0f;

            loopSource.volume = start;
            yield return new WaitForSeconds(leadInTime);

            while (currentTime < 1)
            {
                currentTime += Time.deltaTime;
                if (start != null)
                {
                    startSource.volume = Mathf.Lerp(start, 1f, currentTime / 1);
                }
                loopSource.volume = Mathf.Lerp(start, 1f, currentTime / 1);
                if (end != null)
                {
                    endSource.volume = Mathf.Lerp(start, 1f, currentTime / 1);
                }
                yield return null;
            }
            yield break;
        }
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
        {
            float currentTime = 0;
            float start = source.volume;

            while (currentTime < 1)
            {
                currentTime += Time.deltaTime;
                source.volume = Mathf.Lerp(start, 0f, currentTime / 1);
                yield return null;
            }
            Stop();
            source.volume = 1f;
            yield break;
        }
    }

    public IEnumerator FadeIn()
    {
        {
            Play();
            float currentTime = 0;
            float start = 0f;

            while (currentTime < 1)
            {
                currentTime += Time.deltaTime;
                source.volume = Mathf.Lerp(start, 1f, currentTime / 1);
                yield return null;
            }
            yield break;
        }
    }
}