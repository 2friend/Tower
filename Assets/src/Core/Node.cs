using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public GridObjects gridObjects;

    public int x = 0;
    public int y = 0;

    public bool haveSomething;

    private void Awake()
    {
        gridObjects = GameObject.Find("MainCamera").GetComponent<GridObjects>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (gridObjects.isBuilding)
        {
            gridObjects.currentBuilding.transform.position = gameObject.transform.position;
            if (haveSomething)
                gridObjects.currentBuilding.GetComponent<SpriteRenderer>().color = Color.red;
            else
                gridObjects.currentBuilding.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnMouseExit()
    {
        
    }
}
