using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public List<Sprite> sprites;
    public GameObject nodePrefab;
    public int gridSizeX, gridSizeY;

    public GameObject[,] grid;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 nodePosition = new Vector3(x, y, 0); 
                GameObject newNode = Instantiate(nodePrefab, nodePosition, Quaternion.identity);
                Node nodeComponent = newNode.AddComponent<Node>();

                if (x % 2 == 0)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[0];
                    nodeComponent.haveSomething = true;
                }
                else if (x % 2 > 0) {
                    nodeComponent.spriteRenderer.sprite = sprites[1];
                }
                   


                newNode.gameObject.name = "X: " + nodePosition.x + " Y: " + nodePosition.y;
                nodeComponent.x = x;
                nodeComponent.y = y;
                newNode.transform.SetParent(gameObject.transform);

                grid[x, y] = newNode;
            }
        }
    }
}
