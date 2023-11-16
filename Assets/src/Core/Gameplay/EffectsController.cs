using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public bool looped;
    public MusicHolder sounds;
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sounds = GameObject.Find("MusicHolder").GetComponent<MusicHolder>();
    }

    private void Update()
    {
        
    }

    public void EndLife()
    {
        Destroy(this.gameObject);
    }

    public void PlaySound(string _name)
    {
        audioSource.PlayOneShot(sounds.GetSound(_name));
    }
}
