using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPower : MonoBehaviour
{
    public GameObject projectile;
    public GameObject player;
    public GameObject[] enemies;
    GameObject closestEnemy;
    float closestDistance;
    public GameObject hand;
    bool thrown = false;
    bool inHand = true;
    bool calledBack;
    // Start is called before the first frame update
    void Start()
    {
        closestDistance = 100000f;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        projectile.transform.position = hand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!thrown)
        {
            transform.position = hand.transform.position;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!thrown)
            {
                Debug.Log("hello");
                foreach (GameObject enemy in enemies)
                {

                    if (Vector3.Distance(enemy.transform.position, player.transform.position) < closestDistance)
                    {
                        closestEnemy = enemy;
                    }
                }
                projectile.transform.position = Vector3.MoveTowards(projectile.transform.position,
                closestEnemy.transform.position, .1f * Time.deltaTime);
                thrown = true;
                inHand = false;
            }
            else if (thrown)
            {
                calledBack = true;
            }
        }
        if (thrown && !calledBack)
        {
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position,
            new Vector3(closestEnemy.transform.position.x, hand.transform.position.y, closestEnemy.transform.position.z),
                 2f * Time.deltaTime);
        }
        if (calledBack)
        {
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, hand.transform.position,
                 2f * Time.deltaTime);
            if (Vector3.Distance(hand.transform.position, projectile.transform.position) < .01)
            {
                thrown = false;
                calledBack = false;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {

            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), player.GetComponent<CapsuleCollider>());
        }
    }
}
