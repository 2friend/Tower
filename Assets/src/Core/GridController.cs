using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public List<Sprite> sprites;
    public GameObject nodePrefab;
    public int gridSizeX, gridSizeY;

    public List<Transform> waypoints;

    public GameObject[,] grid;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridSizeX, gridSizeY];

        List<Vector2Int> path = FindPath(); 

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 nodePosition = new Vector3(x, y, 0); 
                GameObject newNode = Instantiate(nodePrefab, nodePosition, Quaternion.identity);
                Node nodeComponent = newNode.AddComponent<Node>();

                if (path.Contains(new Vector2Int(x, y)))
                {
                    nodeComponent.spriteRenderer.sprite = sprites[0];
                    waypoints.Add(newNode.transform);
                }
                else
                {
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

    List<Vector2Int> FindPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        int currentX = 0;
        int currentY = 0;

        while (currentX < gridSizeX - 1 || currentY < gridSizeY - 1)
        {
            path.Add(new Vector2Int(currentX, currentY));

            if (Random.value > 0.5f && currentX < gridSizeX - 1)
            {
                currentX++;
            }
            else if (currentY < gridSizeY - 1)
            {
                currentY++;
            }
        }

        path.Add(new Vector2Int(gridSizeX - 1, gridSizeY - 1));
        return path;
    }
}
