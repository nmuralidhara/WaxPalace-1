using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class FlowerPower : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), collision.collider.gameObject.GetComponent<CapsuleCollider>());
        }
        if (collision.collider.tag.Equals("Enemy"))
        {
            Rigidbody[] rigidbodies = collision.collider.gameObject.GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }
            Debug.Log("Nice Shot");
            collision.collider.gameObject.GetComponent<Animator>().enabled = false;
            Destroy(collision.collider.gameObject, 5);
            collision.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            collision.collider.gameObject.GetComponent<AICharacterControl>().enabled = false;
            collision.collider.gameObject.GetComponent<ThirdPersonCharacter>().enabled = false;

        }
    }
}
