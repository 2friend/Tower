using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    MusicHolder musicHolder;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicHolder = GameObject.Find("MusicHolder").GetComponent<MusicHolder>();
    }
    public void Die () 
    {
        Destroy(gameObject);
    }
    public void PlaySound(string name)
    {
        audioSource.clip = musicHolder.GetSound(name);
        audioSource.Play();
    }
}
