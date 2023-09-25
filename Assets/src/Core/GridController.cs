using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public List<Sprite> sprites;
    public List<Sprite> decor;
    public GameObject nodePrefab;
    public GameObject playerHp;
    public int gridSizeX, gridSizeY;

    public List<Transform> waypoints;

    public GameObject[,] grid;

    private int activeRoad;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridSizeX, gridSizeY];
        List<WavePath> pathList = PathController.paths;
        activeRoad = Random.Range(0, pathList.Count);
        Debug.Log("[Core] [Grid] Selected new Path for generating road ID: %" + activeRoad + "%");

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 nodePosition = new Vector3(x - 15, y - 8, 0);
                GameObject newNode = Instantiate(nodePrefab, nodePosition, Quaternion.identity);
                Node nodeComponent = newNode.AddComponent<Node>();

                

                Vector2Int currentPos = new Vector2Int(x, y);

                if (currentPos == new Vector2Int(35, 22))
                {
                    Instantiate(playerHp, nodePosition, Quaternion.identity);
                }

                bool isPath = false;

                bool leftHasPath = (x > 0 && pathList[activeRoad].pathPoints.Exists(p => p.x == x - 1 && p.y == y));
                bool rightHasPath = (x < gridSizeX - 1 && pathList[activeRoad].pathPoints.Exists(p => p.x == x + 1 && p.y == y));
                bool topHasPath = (y < gridSizeY - 1 && pathList[activeRoad].pathPoints.Exists(p => p.x == x && p.y == y + 1));
                bool downHasPath = (y > 0 && pathList[activeRoad].pathPoints.Exists(p => p.x == x && p.y == y - 1));

                bool topLeftCorner = (x > 0 && y < gridSizeY - 1 && pathList[activeRoad].pathPoints.Exists(p => p.x == x - 1 && p.y == y + 1));
                bool topRightCorner = (x < gridSizeX - 1 && y < gridSizeY - 1 && pathList[activeRoad].pathPoints.Exists(p => p.x == x + 1 && p.y == y + 1));
                bool bottomLeftCorner = (x > 0 && y > 0 && pathList[activeRoad].pathPoints.Exists(p => p.x == x - 1 && p.y == y - 1));
                bool bottomRightCorner = (x < gridSizeX - 1 && y > 0 && pathList[activeRoad].pathPoints.Exists(p => p.x == x + 1 && p.y == y - 1));

                if (isPath)
                {
                    nodeComponent.haveSomething = true;
                }
                else if (leftHasPath && topHasPath || leftHasPath && downHasPath || downHasPath && rightHasPath || topHasPath && rightHasPath)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[0];
                    nodeComponent.haveSomething = true;
                }
                else if (leftHasPath && !topHasPath && !rightHasPath && !downHasPath)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[2];
                    nodeComponent.haveSomething = true;
                }
                else if (rightHasPath && !topHasPath && !leftHasPath && !downHasPath)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[3];
                    nodeComponent.haveSomething = true;
                }
                else if (topHasPath && !rightHasPath && !leftHasPath && !downHasPath)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[4];
                    nodeComponent.haveSomething = true;
                }
                else if (downHasPath && !rightHasPath && !leftHasPath && !topHasPath)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[5];
                    nodeComponent.haveSomething = true;
                }
                else if (topLeftCorner)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[6];
                    nodeComponent.haveSomething = true;
                }
                else if (topRightCorner)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[7];
                    nodeComponent.haveSomething = true;
                }
                else if (bottomLeftCorner)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[8];
                    nodeComponent.haveSomething = true;
                }
                else if (bottomRightCorner)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[9];
                    nodeComponent.haveSomething = true;
                }
                else if (!isPath && !nodeComponent.haveSomething)
                {
                    nodeComponent.spriteRenderer.sprite = sprites[1];

                    bool applyOverlay = Random.Range(0f, 1f) > 0.7f;

                    if (applyOverlay)
                    {
                        int randomDecorIndex = Random.Range(0, decor.Count);
                        Sprite selectedDecor = decor[randomDecorIndex];

                        GameObject overlaySprite = new GameObject("OverlaySprite");
                        SpriteRenderer overlayRenderer = overlaySprite.AddComponent<SpriteRenderer>();
                        overlayRenderer.sprite = selectedDecor;
                        overlayRenderer.sortingOrder = 1;
                        overlaySprite.transform.SetParent(newNode.transform);
                        overlaySprite.transform.localPosition = Vector3.zero;
                    }


                }

                newNode.gameObject.name = "X: " + x + " Y: " + y;
                nodeComponent.x = x;
                nodeComponent.y = y;
                newNode.transform.SetParent(gameObject.transform);

                grid[x, y] = newNode;
            }
        }
        foreach (PathPoint _pathPoint in pathList[activeRoad].pathPoints)
        {
            int x = _pathPoint.x;
            int y = _pathPoint.y;

            Node nodeComponent = grid[x, y].GetComponent<Node>();
            nodeComponent.haveSomething = true;
            nodeComponent.spriteRenderer.sprite = sprites[0];
            waypoints.Add(grid[x, y].transform);
        }

        Debug.Log("[Core] [Grid] Grid generated!");
    }

    public List<Node> GetNodeNeighbors(Node _node)
    {
        List<Node> nodes = new List<Node>();

        if (grid[_node.x + 1, _node.y] != null)
            nodes.Add(grid[_node.x + 1, _node.y].GetComponent<Node>());

        if (grid[_node.x - 1, _node.y] != null)
            nodes.Add(grid[_node.x - 1, _node.y].GetComponent<Node>());

        if (grid[_node.x, _node.y + 1] != null)
            nodes.Add(grid[_node.x, _node.y + 1].GetComponent<Node>());

        if (grid[_node.x, _node.y - 1] != null)
            nodes.Add(grid[_node.x, _node.y - 1].GetComponent<Node>());

        if (grid[_node.x + 1, _node.y + 1] != null)
            nodes.Add(grid[_node.x + 1, _node.y + 1].GetComponent<Node>());

        if (grid[_node.x + 1, _node.y - 1] != null)
            nodes.Add(grid[_node.x + 1, _node.y - 1].GetComponent<Node>());

        if (grid[_node.x - 1, _node.y + 1] != null)
            nodes.Add(grid[_node.x - 1, _node.y + 1].GetComponent<Node>());

        if (grid[_node.x - 1, _node.y - 1] != null)
            nodes.Add(grid[_node.x - 1, _node.y - 1].GetComponent<Node>());


        return nodes;
    }

}
