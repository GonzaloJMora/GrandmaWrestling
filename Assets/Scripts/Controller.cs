using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private GrandmaActions playerActions;
    private InputAction movement;

    private Rigidbody rb;
    [SerializeField]
    public float movementForce = 1f;
    [SerializeField]
    public float jumpForce = 5f;
    [SerializeField]
    public float maxSpeed = 5f;
    private Vector3 forceDir = Vector3.zero;

    [SerializeField]
    public Camera cam;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActions = new GrandmaActions();
    }

    private void OnEnable()
    {
        //playerActions.Player.Jump.started += doJump;
        movement = playerActions.Player.Movement;
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        //playerActions.Player.Jump.started -= doJump;
        playerActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        forceDir += movement.ReadValue<Vector2>().x * GetCameraRight(cam) * movementForce;
        forceDir += movement.ReadValue<Vector2>().y * GetCameraForward(cam) * movementForce;

        rb.AddForce(forceDir, ForceMode.Impulse);
        forceDir = Vector3.zero;

        if (rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVel = rb.velocity;
        horizontalVel.y = 0;
        if (horizontalVel.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVel.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    /*private void doJump(InputAction.CallbackContext obj)
    {
        if (isGrounded())
        {
            forceDir += Vector3.up * jumpForce;
        }
    }

    private bool isGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }

        return false;
    }*/
}
