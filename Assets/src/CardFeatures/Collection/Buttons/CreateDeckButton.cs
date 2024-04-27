using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDeckButton : MonoBehaviour
{
    private Hero selectedHero;

    private void CreateNewDeck()
    {


        if (selectedHero == null)
        {
            Debug.LogError("[Collection] No Hero Selected For Creating Deck!");
            return;
        }
    }
}
