using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsuedoSocket : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private bool shouldMove, shouldRotate;


    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
            transform.position = pivot.position;

        if (shouldRotate)
            transform.rotation = pivot.rotation;
    }
}
