using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitCollision : MonoBehaviour
{
    public GameObject player, border;
    public float distance;

    void Update()
    {
        distance = Vector3.Distance(player.transform.position, border.transform.position);
    }
}
