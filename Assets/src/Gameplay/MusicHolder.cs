using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHolder : MonoBehaviour
{
    public List<AudioClip> sounds = new List<AudioClip>();

    public AudioClip GetSound(string name)
    {
        AudioClip _audio = null;

        for (int i = 0; i < sounds.Count; i++)
        {
            if (sounds[i].name == name)
            {
                _audio = sounds[i];
            }
        }

        if(_audio == null)
            Debug.LogWarning("[Core] [Errors] WRONG Audio ID called or NO SUCH Audio: %" + name + "%");

        return _audio;
    }
}
