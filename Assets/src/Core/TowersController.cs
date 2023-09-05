using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowersController : MonoBehaviour
{
    [SerializeField] private GridObjects gridObjects;
    public GameObject buildingPrefab; // Префаб объекта, который игрок будет строить.
    public LayerMask buildableLayer; // Слой, на котором можно строить.

    private bool isBuilding = false; // Флаг, указывающий, что игрок сейчас строит объект.
    private GameObject currentBuilding; // Текущий объект, который игрок строит.

    private void Update()
    {
        // Обработка нажатия кнопки мыши.
        if (Input.GetMouseButtonDown(0))
        {
            // Определение позиции в мировых координатах, на которую кликнул игрок.
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // Установите z-координату в 0 (плоскость игры).

            // Проверка, можно ли строить на этой клетке.
            if (CanBuildAtPosition(clickPosition))
            {
                // Создание нового экземпляра объекта для строительства.
                currentBuilding = Instantiate(buildingPrefab, clickPosition, Quaternion.identity);
                isBuilding = true;
            }
        }

        // Завершение строительства объекта при отпускании кнопки мыши.
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

    // Проверка, можно ли строить на данной позиции.
    private bool CanBuildAtPosition(Vector3 position)
    {
        // Проверьте, нет ли коллизий на слое, на котором можно строить.
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(1f, 1f), 0, buildableLayer);

        // Если нет коллизий, то можно строить.
        return colliders.Length == 0;
    }

    private int BuildingType(int _type)
    {
        return _type;
    }
}
