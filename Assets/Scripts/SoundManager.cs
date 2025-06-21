using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public float volume = 1;
    private Dictionary<string, Dictionary<string, AudioSource>> audios = new Dictionary<string, Dictionary<string, AudioSource>>();
    private AudioSource currentMusic;
    private Dictionary<string, bool> coolDown = new Dictionary<string, bool>();

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
            Transform typeTransform = transform.GetChild(i);
            audios.Add(typeTransform.name, new Dictionary<string, AudioSource>());
            for (int j = 0; j < typeTransform.childCount; j++)
            {
                Transform audio = typeTransform.GetChild(j);
                audios[typeTransform.name].Add(audio.name, audio.GetComponent<AudioSource>());
            }
        }
    }

    public void ChangeSceneMusic()
    {
        if (GameManager.instance.currentSceneName == "MenuScene")
        {
            PlayAudio("Music", "MenuMusic");
        }
        else if (GameManager.instance.currentSceneName == "CaveMap")
        {
            PlayAudio("Music", "CaveMusic");
        }
        else if (GameManager.instance.currentSceneName == "ForestMap")
        {
            PlayAudio("Music", "ForestMusic");
        }
    }

    public void PlayAudio(string audioType, string audioName)
    {
        if (audioType == "Music")
        {
            FadeOutMusic();
            currentMusic = audios[audioType][audioName];
        }

        audios[audioType][audioName].Play();
    }

    public void PlayAudioWithDelay(string audioType, string audioName, float delay)
    {
        if (!coolDown.ContainsKey(audioName))
        {
            coolDown.Add(audioName, false);
        }

        if (!coolDown[audioName])
        {
            PlayAudio(audioType, audioName);
            StartCoroutine(DelayCoroutine(audioName, delay));
        }
    }

    private IEnumerator DelayCoroutine(string particleName, float time)
    {
        coolDown[particleName] = true;
        yield return new WaitForSeconds(time);
        coolDown[particleName] = false;
    }

    public void SetVolume()
    {
        List<String> typeKeys = audios.Keys.ToList();
        for (int i = 0; i < audios.Count; i++)
        {
            string type = typeKeys[i];
            List<String> nameKeys = audios[type].Keys.ToList();
            for (int j = 0; j < audios[type].Count; j++)
            {
                audios[type][nameKeys[j]].volume = volume;
            }
        }
    }

    public void FadeOutMusic()
    {
        if (currentMusic == null)
        {
            return;
        }
        StartCoroutine(FadeOutCoroutine(currentMusic));
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
