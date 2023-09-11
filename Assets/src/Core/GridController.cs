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
            bool leftHasPath = (x > 0 && pathList.Exists(p => p.x == x - 1 && p.y == y));
            bool rightHasPath = (x < gridSizeX - 1 && pathList.Exists(p => p.x == x + 1 && p.y == y));
            bool topHasPath = (y < gridSizeY - 1 && pathList.Exists(p => p.x == x && p.y == y + 1));
            bool downHasPath = (y > 0 && pathList.Exists(p => p.x == x && p.y == y - 1));

            bool topLeftCorner = (x > 0 && y < gridSizeY - 1 && pathList.Exists(p => p.x == x - 1 && p.y == y + 1));
            bool topRightCorner = (x < gridSizeX - 1 && y < gridSizeY - 1 && pathList.Exists(p => p.x == x + 1 && p.y == y + 1));
            bool bottomLeftCorner = (x > 0 && y > 0 && pathList.Exists(p => p.x == x - 1 && p.y == y - 1));
            bool bottomRightCorner = (x < gridSizeX - 1 && y > 0 && pathList.Exists(p => p.x == x + 1 && p.y == y - 1));

            if (isPath)
            {
                nodeComponent.spriteRenderer.sprite = sprites[0];
                waypoints.Add(newNode.transform);
                nodeComponent.haveSomething = true;
            }
            else if (leftHasPath && topHasPath || leftHasPath && downHasPath || downHasPath && rightHasPath || topHasPath && rightHasPath) 
            {
                nodeComponent.spriteRenderer.sprite = sprites[0];
            }
            else if (leftHasPath && !topHasPath && !rightHasPath && !downHasPath)
            {
                nodeComponent.spriteRenderer.sprite = sprites[2]; 
            }
             else if (rightHasPath && !topHasPath && !leftHasPath && !downHasPath)
            {
                nodeComponent.spriteRenderer.sprite = sprites[3]; 
            }
              else if (topHasPath && !rightHasPath && !leftHasPath && !downHasPath)
            {
                nodeComponent.spriteRenderer.sprite = sprites[4]; 
            }
              else if (downHasPath && !rightHasPath && !leftHasPath && !topHasPath)
            {
                nodeComponent.spriteRenderer.sprite = sprites[5]; 
            }
              else if (topLeftCorner)
            {
                nodeComponent.spriteRenderer.sprite = sprites[6]; 
            }
             else if (topRightCorner)
            {
                nodeComponent.spriteRenderer.sprite = sprites[7]; 
            }
             else if (bottomLeftCorner)
            {
                nodeComponent.spriteRenderer.sprite = sprites[8]; 
            }
             else if (bottomRightCorner)
            {
                nodeComponent.spriteRenderer.sprite = sprites[9]; 
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
