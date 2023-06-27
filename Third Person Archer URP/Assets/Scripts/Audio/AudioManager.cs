using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip         = s.clip;
            s.source.volume       = s.volume;
            s.source.pitch        = s.pitch;
            s.source.spatialBlend = s.spatialBlend;
            s.source.maxDistance  = s.maxDistance;
            s.source.loop         = s.loop;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.rolloffMode = AudioRolloffMode.Linear;
        }
    }

    private void Start()
    {
        PlaySound("Theme");
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound <" + name + "> not found");
            return;
        }
        s.source.Play();
    }
}
