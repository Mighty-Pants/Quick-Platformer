using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> points;
    int goalPoint;
    public float moveSpeed = 5;

    void Update()
    {
        moveToNextPoint();
    }

    public void moveToNextPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[goalPoint].position, Time.deltaTime * moveSpeed);

        if(Vector2.Distance(transform.position, points[goalPoint].position) < 0.1f)
        {
            if(goalPoint == points.Count - 1)
            {
                goalPoint = 0;
            }
            else
            {
                goalPoint++;
            }
        }
    }
}
