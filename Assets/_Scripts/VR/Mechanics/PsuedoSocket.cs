using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PsuedoSocket : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private bool moveX, moveY, moveZ;
    [SerializeField] private bool rotateX, rotateY, rotateZ;


    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        if (moveX) newPosition.x = pivot.position.x;
        if (moveY) newPosition.y = pivot.position.y;
        if (moveZ) newPosition.z = pivot.position.z;
        transform.position = newPosition;

        Vector3 newRotation = transform.eulerAngles;
        if (rotateX) newRotation.x = pivot.eulerAngles.x;
        if (rotateY) newRotation.y = pivot.eulerAngles.y;
        if (rotateZ) newRotation.z = pivot.eulerAngles.z;
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
