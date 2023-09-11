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

    List<Path> pathList = PathController.path;

    for (int x = 0; x < gridSizeX; x++)
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            Vector3 nodePosition = new Vector3(x-9, y-5, 0); 
            GameObject newNode = Instantiate(nodePrefab, nodePosition, Quaternion.identity);
            Node nodeComponent = newNode.AddComponent<Node>();

            Vector2Int currentPos = new Vector2Int(x, y);

            bool isPath = false;

            foreach (Path pathInstance in pathList)
            {
                if (pathInstance.x == currentPos.x && pathInstance.y == currentPos.y)
                {
                    isPath = true;
                    break;
                }
            }

            if (isPath)
            {
                nodeComponent.spriteRenderer.sprite = sprites[0];
                waypoints.Add(newNode.transform);
                nodeComponent.haveSomething = true;
            }
            else
            {
                nodeComponent.spriteRenderer.sprite = sprites[1]; 
            }

            newNode.gameObject.name = "X: " + x + " Y: " + y;
            nodeComponent.x = x;
            nodeComponent.y = y;
            newNode.transform.SetParent(gameObject.transform);

            grid[x, y] = newNode;
        }
    }
}

}
