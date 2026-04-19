using UnityEngine;
using UnityEngine.AI;

public class PenguinAI : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float detectRange = 10f;
    public float stopDistance = 1.5f;

    private NavMeshAgent agent;
    private Transform player;
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = moveSpeed;
        agent.angularSpeed = 360;
        agent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectRange)
        {
            agent.SetDestination(player.position);
        }

        bool walking = agent.velocity.magnitude > 0.1f;
        anim.SetBool("Walk", walking);
    }
}