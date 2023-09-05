using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowersController : MonoBehaviour
{
    [SerializeField] private GridObjects gridObjects;
    public GameObject buildingPrefab; // ������ �������, ������� ����� ����� �������.
    public LayerMask buildableLayer; // ����, �� ������� ����� �������.

    private bool isBuilding = false; // ����, �����������, ��� ����� ������ ������ ������.
    private GameObject currentBuilding; // ������� ������, ������� ����� ������.

    private void Update()
    {
        // ��������� ������� ������ ����.
        if (Input.GetMouseButtonDown(0))
        {
            // ����������� ������� � ������� �����������, �� ������� ������� �����.
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // ���������� z-���������� � 0 (��������� ����).

            // ��������, ����� �� ������� �� ���� ������.
            if (CanBuildAtPosition(clickPosition))
            {
                // �������� ������ ���������� ������� ��� �������������.
                currentBuilding = Instantiate(buildingPrefab, clickPosition, Quaternion.identity);
                isBuilding = true;
            }
        }

        // ���������� ������������� ������� ��� ���������� ������ ����.
        if (isBuilding && Input.GetMouseButtonUp(0))
        {
            isBuilding = false;
            Tower towerType = currentBuilding.AddComponent<Tower>();
            currentBuilding = null;

        }

        if (isBuilding)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            currentBuilding.transform.position = mousePosition;
        }
    }

    // ��������, ����� �� ������� �� ������ �������.
    private bool CanBuildAtPosition(Vector3 position)
    {
        // ���������, ��� �� �������� �� ����, �� ������� ����� �������.
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(1f, 1f), 0, buildableLayer);

        // ���� ��� ��������, �� ����� �������.
        return colliders.Length == 0;
    }

    private int BuildingType(int _type)
    {
        return _type;
    }
}
