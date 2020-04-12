using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] listOfSounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in listOfSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.soundClip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string soundName)
    {
        Sound sounds = Array.Find(listOfSounds, sound => sound.name == soundName);
        sounds.source.Play();
    }
}
