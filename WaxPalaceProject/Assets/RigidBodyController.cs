using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Enemy")){
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
