using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GridTEmp grid;
    private Animator animator;
    float speed = 3.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        grid = GameObject.Find("Grid").GetComponent<GridTEmp>();
    }

    private int currentWaypointIndex = 0;

    private void Update()
    {
        if (currentWaypointIndex < grid.waypoints.Count)
        {
            Transform targetWaypoint = grid.waypoints[currentWaypointIndex];

            Vector3 moveDirection = (targetWaypoint.position - transform.position).normalized;

            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Y", moveDirection.y);

            transform.Translate(moveDirection * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
