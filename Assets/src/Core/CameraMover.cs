using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Vector2 swipeStartPos;
    private bool isSwiping = false;
    public bool canMove = true;

    public float swipeSpeed = 17.0f;
    public float minX = -0.5f;
    public float maxX = 6.0f;
    public float minY = -.5f;
    public float maxY = 10.0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            swipeStartPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && canMove)
        {
            isSwiping = false;
        }

        if (isSwiping)
        {
            Vector2 swipeEndPos = Input.mousePosition;
            Vector2 swipeDirection = (swipeEndPos - swipeStartPos).normalized;

            // Перемещение камеры по осям X и Y.
            float newX = transform.position.x - swipeDirection.x * swipeSpeed * Time.deltaTime;
            float newY = transform.position.y - swipeDirection.y * swipeSpeed * Time.deltaTime;

            // Ограничение позиции камеры по осям X и Y.
            newX = Mathf.Clamp(newX, minX, maxX);
            newY = Mathf.Clamp(newY, minY, maxY);

            transform.position = new Vector3(newX, newY, transform.position.z);

            swipeStartPos = swipeEndPos;
        }
    }
}
