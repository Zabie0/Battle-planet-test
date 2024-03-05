using UnityEngine;
using UnityEngine.AI;

public class FollowerController : MonoBehaviour
{
    public Transform target, planet, player;
    public float turnSpeed = 5f, maxSpeed = 13;
    public FixedJoystick joystick;
    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        EngageTarget();
    }
    private void EngageTarget()
    {
        FacePlayer();
        if(distanceToTarget >= navMeshAgent.stoppingDistance+0.1f)
        {
            ChaseTarget();
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
    private void ChaseTarget()
    {
        navMeshAgent.speed = CalculateSpeed(joystick.Horizontal, joystick.Vertical);
        navMeshAgent.SetDestination(target.position);
        animator.SetBool("isMoving", true);
    }
    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    float CalculateSpeed(float horizontalInput, float verticalInput)
    {
        float inputMagnitude = Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput);
        animator.SetFloat("runSpeed", inputMagnitude);
        float speed = inputMagnitude * maxSpeed;
        speed += speed <= 1 ? 1 : 0;
        return speed;
    }
}
