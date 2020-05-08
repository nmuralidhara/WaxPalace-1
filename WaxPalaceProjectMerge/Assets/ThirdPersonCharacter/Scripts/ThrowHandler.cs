using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHandler : MonoBehaviour
{
    public GameObject projectile;
    public State state;
    public float projectile_speed = 10;
    public float time_to_throw = 10;
    public float throw_distance;
    Vector3 target;
    public GameObject hand;
    public GameObject cursor;
    public Transform cross_hair;
    bool go;
    Vector3 Vo = new Vector3(0, 0, 0);
    public enum State
    {
        INHAND,
        MOVING_TOWARDS_TARGET,
        MOVING_TOWARDS_HAND

    }
    public void Throw()
    {
        go = true;
    }
    public void OnThrowCompletion()
    {
        GetComponent<Animator>().SetBool("Throw", false);
    }
    Vector3 GetCrossHairPosition()
    {
        return cross_hair.position;
    }
    void Start()
    {
        projectile.transform.position = hand.transform.position;
    }

    void Update()
    {


        if (state == State.MOVING_TOWARDS_TARGET)
        {
            if (Input.GetMouseButtonDown(0))
            {
                state = State.MOVING_TOWARDS_HAND;
            }
            //projectile.transform.position = Vector3.MoveTowards(transform.position, target, projectile_speed * Time.deltaTime);
        }
        else if (state == State.MOVING_TOWARDS_HAND)
        {
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, hand.transform.position, projectile_speed*Time.deltaTime);
            if (Vector3.Distance(projectile.transform.position, hand.transform.position) < .1)
            {
                projectile.transform.position = hand.transform.position;
                state = State.INHAND;
            }
        }
        else if (state == State.INHAND)
        {

            projectile.transform.position = hand.transform.position;

            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(camRay, out hit, 100f))
                {
                    cursor.transform.position = hit.point + Vector3.up * 0.1f;
                    Vo = CalculateVelocity(cursor.transform.position, transform.position, .5f);
                }
                if (Vector3.Distance(cursor.transform.position, transform.position) < throw_distance)
                {
                    cursor.SetActive(true);
                    if (Input.GetMouseButtonDown(0))
                    {

                        GetComponent<Animator>().SetBool("Throw", true);
                    }
                }
                else
                {
                    cursor.SetActive(false);
                }

            }
            if (go)
            {

                projectile.GetComponent<Rigidbody>().velocity = Vo;
                state = State.MOVING_TOWARDS_TARGET;
                cursor.SetActive(false);
                go = false;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            Physics.IgnoreCollision(projectile.GetComponent<CapsuleCollider>(), GetComponent<CapsuleCollider>());
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ = new Vector3(distanceXZ.x, 0f, distanceXZ.z);

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float VXZ = Sxz / time;
        float VY = (Sy / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXZ.normalized;
        result *= VXZ;
        result = new Vector3(result.x, VY, result.z);

        return result;

    }
}
