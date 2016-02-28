using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : PlayerBase {

    #region Events
    public delegate void ChargeDashAction();
    public static event ChargeDashAction OnChargeDashAction;

    public delegate void ReleaseDashAction();
    public static event ReleaseDashAction OnReleaseDashAction;
    #endregion

    [Header("Movement")]
    public float movementSpeed;
    public float turnSpeed;
    public float groundCheckDistance = 1.2f;
    public bool isGrounded;

    [Header("Movement switches")]
    public bool canMoveBackwards = false;
    public bool canMoveForward = true;
    public bool canTurn = true;
    public bool canMove { get { return canTurn && canMoveForward; } }
    
    [Header("Dash")]
    public float maxDashChargeAmount;
    public float dashChargeMultiplier = 1f;
    public float chargeForce = 10f;

    // Inputs
    private float horizontal, vertical;

    // Dash variables
    private bool isChargingDash;
    [HideInInspector]
    public bool isDashing;
    [HideInInspector]
    public float dashChargeTime;
    private float _endofDash = 0;

    private Rigidbody _body;

    void Awake () {
        _body = GetComponent<Rigidbody>();
	}
	
	void Update () {
        // Get inputs
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
            StartChargingDash();

        if (Input.GetKeyUp(KeyCode.Space) && !isDashing)
            ReleaseDash();

        // Disable backwards key
        if (vertical < 0 && !canMoveBackwards)
            vertical = 0;

        // Calculate stuff related to dash
        HandleDash();

        // Raycast ground check
        Ray groundCheck = new Ray(transform.position, transform.up * -1);
        RaycastHit hitInfo;
        if (Physics.Raycast(groundCheck, out hitInfo, 1.2f))
        {
            Debug.DrawLine(transform.position, transform.position + transform.up * -1 * groundCheckDistance, Color.red);
            isGrounded = true;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.up * -1 * groundCheckDistance, Color.green);
            isGrounded = false;
        }

    }

    void FixedUpdate()
    {
        if (canTurn)
        {
            // Calculate new rotation
            Vector3 newRotation = new Vector3();
            newRotation = transform.rotation.eulerAngles;
            newRotation.y += horizontal * turnSpeed;

            // Apply it
            transform.rotation = Quaternion.Euler(newRotation);
        }

        if (canMoveForward)
        {
            // Apply movement only if we're not already going above movementSpeed
            if (_body.velocity.magnitude <= movementSpeed)
            {
                // Calculate direction
                Vector3 moveDirection = transform.rotation * Vector3.forward * movementSpeed * vertical;
                Vector3 newVelocity = new Vector3(_body.velocity.x, _body.velocity.y, _body.velocity.z);

                newVelocity.x = moveDirection.x;
                newVelocity.z = moveDirection.z;
                _body.velocity = newVelocity;
            }
        }
    }

    private void HandleDash()
    {
        // Look at how long we press the charge button
        if (isChargingDash && dashChargeTime <= maxDashChargeAmount)
        {
            dashChargeTime += Time.deltaTime * dashChargeMultiplier;
        }

        // Check when to end the dash
        if ((_endofDash > 0 && Time.time >= _endofDash) || _body.velocity.magnitude <= movementSpeed)
        {
            StopDash();
        }
    }

    private void StopDash()
    {
        canMoveForward = true;
        canTurn = true;
        isDashing = false;
        _endofDash = 0;

        this.PlayerAnimationController.SetCharge(false); // Ewwwww bad
    }

    private void StartChargingDash()
    {
        // Fire event
        if (OnChargeDashAction != null)
            OnChargeDashAction();

        // Start charging, disable moving forward
        isChargingDash = true;
        dashChargeTime = 0;
    }

    private void ReleaseDash()
    {
        // Fire event
        if (OnReleaseDashAction != null)
            OnReleaseDashAction();

        // Release charge
        _endofDash = Time.time + dashChargeTime;
        isChargingDash = false;
        isDashing = true;
        canTurn = false;

        _body.velocity = transform.forward * chargeForce * dashChargeTime;
        dashChargeTime = 0;

        this.PlayerAnimationController.SetCharge(true); // Ewwwww bad
    }
}
