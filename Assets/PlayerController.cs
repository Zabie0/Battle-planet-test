using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public Transform planet;
    public FixedJoystick joystick;
    Animator animator;
    float vt, hz;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        vt = joystick.Vertical;
        hz = joystick.Horizontal;
        if(Mathf.Abs(vt) > 0.05f || Mathf.Abs(hz) > 0.05f){
            if(!IsObstacleInFront()) PlanetRotation();
            PlayerRotation();
            CalculateAnimation();
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
    void PlanetRotation()
    {
        planet.Rotate(new Vector3(-vt, 0, hz), Space.World);
    }
    void PlayerRotation()
    {
        float angle = Mathf.Atan2(hz, vt) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }
    void CalculateAnimation()
    {
        float animationSpeed = Mathf.Sqrt(hz * hz + vt * vt);
        animator.SetBool("isMoving", true);
        animator.SetFloat("runSpeed", animationSpeed);
    }
    bool IsObstacleInFront()
    {
        if (Physics.Raycast(new Vector3(transform.position.x+0.3f, transform.position.y, transform.position.z), transform.forward, 1f)
        || Physics.Raycast(new Vector3(transform.position.x-0.3f, transform.position.y, transform.position.z), transform.forward, 1f)) // Cast a ray from the player forward
        {
            return true;
        }
        return false;
    }
}
