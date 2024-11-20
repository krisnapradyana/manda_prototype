
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioTriggerReceiver : MonoBehaviour
{
    [System.Serializable]
    public class MultiData
    {
        public string targetTag; // String data
        public bool onTrigger = true;  // First boolean
        public bool onCollision = true;  // Second boolean
        public AudioPlayer player;
    }


    public MultiData[] dataArray;

    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            if (dataArray[i].onCollision && collision.gameObject.tag == dataArray[i].targetTag)
            {
                dataArray[i].player.PlayRandomSoundEffect();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            if (dataArray[i].onTrigger && other.tag == dataArray[i].targetTag)
            {
                dataArray[i].player.PlayRandomSoundEffect();
            }
        }
    }
}
