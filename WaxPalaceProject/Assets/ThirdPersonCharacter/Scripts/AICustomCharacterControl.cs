using System;
using UnityEngine;
using System.Collections;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICustomCharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent;             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character; // the character we are controlling
        public Transform target;                                    // target to aim for
        public enum State
        {
            WANDER,
            FOLLOW
        }

        public State state;
        private bool is_alive;

        //********************\\
        public GameObject[] waypoints;
        private int waypoint_index;
        public float wander_speed = 0.5f;

        //**********************\\
        public float follow_speed = 1f;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updateRotation = false;
            agent.updatePosition = true;

            state = State.WANDER;

            is_alive = true;

            // Start FINITE STATE MACHINE
            StartCoroutine("FSM");
        }

        IEnumerator FSM()
        {
            while (is_alive)
            {
                if (state == State.WANDER)
                {
                    
                    Wander();
                }
                if (state == State.FOLLOW)
                {
                    Follow();
                }
                yield return null;
            }
        }
        void Wander()
        {
            agent.speed = wander_speed;
            if (Vector3.Distance(transform.position, waypoints[waypoint_index].transform.position) >= 2)
            {
                Debug.Log("Why no anim");
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
        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag.Equals("Player"))
            {
                state = State.FOLLOW;
                target = collider.gameObject.transform;
            }
        }
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
