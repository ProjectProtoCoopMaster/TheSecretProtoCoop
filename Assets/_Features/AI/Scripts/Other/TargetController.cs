using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

public class TargetController : MonoBehaviour
{
    public Vector3Variable targetPosition;
    public float speed;

    void Update()
    {
        Vector3 rawMovement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Vector3 clampMovement = Vector3.ClampMagnitude(rawMovement, 1f);

        Move(clampMovement);

        targetPosition.Value = transform.position;
    }

    void Move(Vector3 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
