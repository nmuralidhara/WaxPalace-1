using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    //made with guidance from https://www.youtube.com/watch?v=7TVXCa2Nwj4 tutorial
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public bool blind;
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        public GameObject hearing_radius;

        public enum State
        {
            WANDER,
            FOLLOW,
            INVESTIGATE
        }
        public GameObject floor;
        public State state;
        private bool is_alive = true;

        //********************\\
        public GameObject[] waypoints;
        private int waypoint_index;
        public float wander_speed = 0.5f;
        //**********************\\
        public float follow_speed = 1f;

        //***********************\\
        public float height;
        public float distance = 20;
        public float hearing_distance = 10;
        //***********************\\
        private Vector3 spot_to_investigate;
        private float timer = 0;
        private float investigate_wait = 10;


        private void Start()
        {
            if (blind)
            {
                hearing_radius = Instantiate(hearing_radius);
                hearing_radius.transform.localScale = new Vector3(hearing_distance, .0000001f, hearing_distance);
                hearing_radius.transform.position = new Vector3(hearing_radius.transform.position.x, floor.transform.position.y +.000000001f - 0.15f, hearing_radius.transform.position.z);
            }
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            height = 1.36f;
        }


        private void Update()
        {
            if (blind)
            {
                hearing_radius.transform.position = new Vector3(transform.position.x, - 0.15f, transform.position.z);
            }

            if (is_alive)
            {
                if (state == State.WANDER)
                {
                    Wander();
                }
                if (state == State.FOLLOW)
                {
                    Follow();
                }
                if (state == State.INVESTIGATE)
                {
                    Investigate();
                }
            }
            
            else
                character.Move(Vector3.zero, false, false);
        }

        void Wander()
        {
            agent.speed = wander_speed;
            if (Vector3.Distance(transform.position, waypoints[waypoint_index].transform.position) >= 2)
            {
                agent.SetDestination(waypoints[waypoint_index].transform.position);
                character.Move(agent.desiredVelocity, false, false);
            }
            else if (Vector3.Distance(transform.position, waypoints[waypoint_index].transform.position) < 2)
            {
                waypoint_index = waypoint_index > waypoints.Length ? 0 : waypoint_index + 1;
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }
        void Follow()
        {
            agent.speed = follow_speed;
            agent.SetDestination(target.position);
            character.Move(agent.desiredVelocity, false, false);
        }
        void Investigate()
        {
            timer += Time.deltaTime;
            agent.SetDestination(transform.position);
            character.Move(Vector3.zero, false, false);
            transform.LookAt(spot_to_investigate);

            if(timer >= investigate_wait)
            {
                state = State.WANDER;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collided with something");
            if (collision.collider.tag.Equals("Player"))
            {
                Debug.Log("Collided with player");
                state = State.INVESTIGATE;
                spot_to_investigate = collision.collider.gameObject.transform.position;
            }
        }

        void FixedUpdate()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position + Vector3.up * height, transform.forward * distance, Color.green);
            Debug.DrawRay(transform.position + Vector3.up * height, (transform.forward + transform.right).normalized * distance, Color.green);
            Debug.DrawRay(transform.position + Vector3.up * height, (transform.forward - transform.right).normalized * distance, Color.green);
            if (!blind)
            {
                if (Physics.Raycast(transform.position + Vector3.up * height, transform.forward, out hit, distance))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        state = State.FOLLOW;
                        target = hit.collider.gameObject.transform;
                    }
                }
                if (Physics.Raycast(transform.position + Vector3.up * height, (transform.forward + transform.right).normalized, out hit, distance))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        state = State.FOLLOW;
                        target = hit.collider.gameObject.transform;
                    }
                }
                if (Physics.Raycast(transform.position + Vector3.up * height, (transform.forward + transform.right).normalized, out hit, distance))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        state = State.FOLLOW;
                        target = hit.collider.gameObject.transform;
                    }
                }
            }
            if (blind)
            {
                if (Vector3.Distance(transform.position, target.position) < hearing_distance/2)
                {
                    state = State.FOLLOW;
                    target = target;
                }
            }

        }
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
