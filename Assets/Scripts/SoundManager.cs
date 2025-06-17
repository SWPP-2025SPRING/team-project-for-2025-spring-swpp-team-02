using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public float volume = 1;
    private Dictionary<string, AudioSource> audios = new Dictionary<string, AudioSource>(); 
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            audios.Add(child.name, child.GetComponent<AudioSource>());
        }
    }

    public void PlayAudio(string audioName)
    {
        audios[audioName].Play();
    }

    public void SetVolume()
    {
        List<String> keys = audios.Keys.ToList();
        for (int i = 0; i < audios.Count; i++)
        {
            audios[keys[i]].volume = volume;
        }
    }

    public void FadeOut(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            return;
        }
        Debug.Log("run");
        StartCoroutine(FadeOutCoroutine(audioSource));
    }

    IEnumerator FadeOutCoroutine(AudioSource audioSource)
    {
        float initialVolume = audioSource.volume;

        for (float t = 1; t > 0; t -= Time.deltaTime / TransitionEffect.instance.transitionTime)
        {
            audioSource.volume = initialVolume * t;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = initialVolume;
    }
}
