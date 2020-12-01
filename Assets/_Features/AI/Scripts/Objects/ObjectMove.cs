using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public float speed;

    public Transform startPoint;
    public Transform endPoint;

    private Vector3 currentDestination { get { if (onReturn) return startPoint.position; else return endPoint.position; } }

    public bool onReturn { get; set; }

    void Update()
    {
        MoveTo(currentDestination);
    }

    void MoveTo(Vector3 destination)
    {
        Vector3 localDestination = destination - transform.position;
        Vector3 direction = Vector3.ClampMagnitude(localDestination, 1f);

        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position == destination) onReturn = !onReturn;
    }
}
