using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Vector2 swipeStartPos;
    private bool isSwiping = false;

    public float swipeSpeed = 17.0f;
    public float minX = -6.0f;
    public float maxX = 6.0f;
    public float minY = 2.0f;
    public float maxY = 10.0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }

        if (isSwiping)
        {
            Vector2 swipeEndPos = Input.mousePosition;
            Vector2 swipeDirection = (swipeEndPos - swipeStartPos).normalized;

            // ����������� ������ �� ���� X � Y.
            float newX = transform.position.x - swipeDirection.x * swipeSpeed * Time.deltaTime;
            float newY = transform.position.y - swipeDirection.y * swipeSpeed * Time.deltaTime;

            // ����������� ������� ������ �� ���� X � Y.
            newX = Mathf.Clamp(newX, minX, maxX);
            newY = Mathf.Clamp(newY, minY, maxY);

            transform.position = new Vector3(newX, newY, transform.position.z);

            swipeStartPos = swipeEndPos;
        }
    }
}
