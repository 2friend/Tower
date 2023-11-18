using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public GridObjects _gridObjects;

    public int x = 0;
    public int y = 0;

    public bool haveSomething;

    private void Awake()
    {
        _gridObjects = GameConstant._gridObjects;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (_gridObjects.isBuilding)
        {
            _gridObjects.currentBuilding.transform.position = gameObject.transform.position;
            if (haveSomething)
                _gridObjects.currentBuilding.GetComponent<SpriteRenderer>().color = Color.red;
            else
                _gridObjects.currentBuilding.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnMouseExit()
    {
        
    }
}
