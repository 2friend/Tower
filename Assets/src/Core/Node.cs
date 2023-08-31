using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int x = 0;
    public int y = 0;

    public bool haveSomething;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
