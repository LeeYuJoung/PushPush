using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketEffect : MonoBehaviour
{
    public GameObject bucketEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GameObject _effect = Instantiate(bucketEffect,transform.position, transform.rotation);
            Destroy(_effect, 0.35f);
        }
    }
}
