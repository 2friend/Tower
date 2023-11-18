using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHolder : MonoBehaviour
{
    public List<GameObject> effectsList = new List<GameObject>();

    public GameObject GetEffect(string name)
    {
        GameObject effect = null;

        for(int i = 0; i < effectsList.Count; i++)
        {
            if (effectsList[i].name == name)
                effect = effectsList[i];

        }

        if (effect == null)
            Debug.LogError("[!] [Gameplay] [Effects] WRONG EFFECT ID CALLED OR NO SUCH EFFECT: %" + name + "%");

        return effect;
    }
}
