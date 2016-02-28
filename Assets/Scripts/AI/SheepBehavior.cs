using UnityEngine;
using System.Collections;

public class SheepBehavior : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float rotationSpeed;

    [Header("AI")]
    public float distanceCheck = .5f;

    [Header("Idle state")]
    public float maximumIdleRadius;
    public float maxTimeToGetToNewPos = 3f; // how long before we consider our pos unreachable

    private float newposPickTime; // Time at which we picked a new position to go to

    // Implémentation de la FSM
    private FSM brain;
    private Rigidbody _body;

    private Vector3 idleNextPoint;

    // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody>();

        brain = new FSM(AI_EnterIdle);
    }

    void Update()
    {
        brain.RunFSM();
    }

    void AI_EnterIdle()
    {
        idleNextPoint = PickNewPosition();
        brain.SetState(AI_Idle);
    }

    void AI_Idle()
    {
        // If we're not on our next idle point, move to it
        if (XZDistance(transform.position, idleNextPoint) >= distanceCheck)
            MoveTo(idleNextPoint);
        // Else generate a new one
        else
        {
            idleNextPoint = PickNewPosition();
        }

        // If we take too long to reach point, change point
        if (Time.time >= newposPickTime + maxTimeToGetToNewPos)
        {
            idleNextPoint = PickNewPosition();
        }
    }

    Vector3 PickNewPosition()
    {
        float x = Random.Range(transform.position.x - maximumIdleRadius, transform.position.x + maximumIdleRadius);
        float z = Random.Range(transform.position.z - maximumIdleRadius, transform.position.z + maximumIdleRadius);
        float y = 0f;

        newposPickTime = Time.time;

        return new Vector3(x, y, z);
    }

    void MoveTo(Vector3 destination)
    {
        if (XZDistance(transform.position, destination) >= distanceCheck)
        {
            // Slowly move towards our destination
            MoveDirection((destination - transform.position).normalized);
        }
    }

    void MoveDirection(Vector3 direction)
    {
        // Set new velocity to the direction times movement speed but keep Y velocity so we don't erase gravity
        Vector3 newVelocity = _body.velocity;
        newVelocity.x = direction.x * movementSpeed;
        newVelocity.z = direction.z * movementSpeed;

        _body.velocity = newVelocity;

        // Set the rotation to look at our destination but keeping y = 0 so that the sheep doesn't move his head up and down
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), Time.deltaTime * rotationSpeed);
    }

    float XZDistance (Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;

        return Vector3.Distance(a, b);
    }

    public void OnDrawGizmos()
    {
        // Idle destination
        Gizmos.DrawWireSphere(idleNextPoint, 1f);

        // Idle radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maximumIdleRadius);
    }
}
