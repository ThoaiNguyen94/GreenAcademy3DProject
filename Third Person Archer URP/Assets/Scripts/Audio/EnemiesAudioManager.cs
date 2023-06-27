using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
            s.source.rolloffMode = AudioRolloffMode.Linear;
            s.source.maxDistance = s.maxDistance;
        }
    }

    public void EnemiesSound(string name)
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
