using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHitGround : MonoBehaviour
{
    Rigidbody rb;
    Coroutine resetRotCoroutine;
    bool isGround;
    public bool setGround { get => isGround;
        set 
            { 
                if(value == true)
                {
                    resetRotCoroutine = StartCoroutine(IEResetRotation());
                }
                else
                {
                    if(resetRotCoroutine != null)
                    {
                        StopCoroutine(resetRotCoroutine);
                    }
                }
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        setGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        setGround = false;
    }

    IEnumerator IEResetRotation()
    {
        yield return new WaitForSeconds(2f);
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
}
