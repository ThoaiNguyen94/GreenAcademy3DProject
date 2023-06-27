using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [Range(0.1f, 3f)] public float pitch;
    [Range(0f, 1f)] public float spatialBlend;
    public float maxDistance;
    public bool loop;
    public AudioMixerGroup mixer;
    [HideInInspector] public AudioSource source;
}
