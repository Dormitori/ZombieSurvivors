using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Sound
{
    [SerializeField] private AudioClip audioClip;
    public string soundName;
    
    [Range(0f, 1f), SerializeField] private float volume = 1;
    [Range(0f, 1f), SerializeField] private float volumeRandomSpread = 0; 

    [Range(-3f, 3f), SerializeField] private float pitch = 1;
    [Range(0f, 3f), SerializeField] private float pitchRandomSpread = 0;

    private AudioSource _audioSource;

    public void SetSource(AudioSource audioSource)
    {
        
        _audioSource = audioSource;
        _audioSource.clip = audioClip;
    }
    
    public void Play()
    {
        _audioSource.volume = Random.Range(volume - volumeRandomSpread, volume + volumeRandomSpread);
        _audioSource.pitch = Random.Range(pitch - pitchRandomSpread, pitch + pitchRandomSpread);
        _audioSource.PlayOneShot(audioClip);
    }
}


public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] _sounds;
    public static AudioManager instance;

    private void Start()
    {
        for (int i = 0; i < _sounds.Length; i++) {
            var go = new GameObject($"Sound_{i}");
            _sounds[i].SetSource(go.AddComponent<AudioSource>());
            go.transform.parent = transform;
        }
        
        instance = this;
    }

    public void Play(string soundName)
    {
        foreach (var sound in _sounds)
        {
            if (sound.soundName == soundName)
            {
                sound.Play();
                return;
            }
        }
    }
}