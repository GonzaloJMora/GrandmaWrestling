using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    //important references for input
    private InputActionAsset inputAsset;
    private InputAction movement;
    private InputActionMap player;
    private Rigidbody rb;

    //references to hitboxes
    public GameObject swipeBox;
    public GameObject jabBox;
    public GameObject blockBox;

    //lock to keep players from activating block, jab, and swipe at the same time
    private bool attLock = true;

    //lock used to keep the player from indefinitely sticking to the wall outside the stage
    private bool fallLock = false;

    //movement values
    [SerializeField]
    public float movementForce = 1f;
    [SerializeField]
    public float jumpForce = 5f;
    [SerializeField]
    public float maxSpeed = 5f;

    //force to be applied onto player's rigidbody
    private Vector3 forceDir = Vector3.zero;

    //will be force applied when hit, is a placeholder
    private Vector3 hitVec = Vector3.zero;
    public bool isBlocking = false;

    //used when player is off map to not allow them to stick on walls
    private Vector3 lastForce = Vector3.zero;

    //camera that determines which direction we move when joystick is moved
    [SerializeField]
    public Camera cam;

    //initialization
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        inputAsset = this.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    //also initialization but only when object becomes active
    private void OnEnable()
    {
        player.FindAction("Jump").started += DoJump;
        player.FindAction("Block").started += DoBlock;
        player.FindAction("Swipe").started += DoSwipe;
        player.FindAction("Jab").started += DoJab;
        movement = player.FindAction("Movement");
        player.Enable();
    }

    //used when object is destroyed
    private void OnDisable()
    {
        player.FindAction("Jump").started -= DoJump;
        player.FindAction("Block").started -= DoBlock;
        player.FindAction("Swipe").started -= DoSwipe;
        player.FindAction("Jab").started -= DoJab;
        player.Disable();
    }

    public void getHit(int force, Vector3 attacker_pos)
    {
        //hitDir = Vector3.zero;
        if (isBlocking)
        {
            return;
        }

        hitVec = Vector3.Normalize(gameObject.transform.position - attacker_pos);
        hitVec *= force;
        rb.AddForce(hitVec, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        //only gives the player control when they are not off of the map (will need to fix this later most likely but works for now)
        //joystick direction to movement is based off of the camera position
        if (!isOffMap())
        {
            forceDir += movement.ReadValue<Vector2>().x * GetCameraRight(cam) * movementForce;
            forceDir += movement.ReadValue<Vector2>().y * GetCameraForward(cam) * movementForce;

            rb.AddForce(forceDir, ForceMode.Impulse);
            lastForce = forceDir;
            forceDir = Vector3.zero;
            fallLock = true;
        }

        //if player is off the map, the previous force will be constantly applied
        else
        {
            //Debug.Log(rb.velocity.y);
            if (fallLock && lastForce.x == 0 && lastForce.z == 0) {
                lastForce.x = -rb.position.x; 
                lastForce.z = -rb.position.z;
                fallLock = false;
            } 

            rb.AddForce(lastForce, ForceMode.Impulse);

            lastForce = Vector3.zero;
        }

        //player will fall faster and faster if they are not grounded
        if (rb.velocity.y < 0f || !isGrounded())
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        //if player has reached max speed, then cap them at that speed
        Vector3 horizontalVel = rb.velocity;
        horizontalVel.y = 0f;
        if (horizontalVel.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVel.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        LookAt();
    }

    //rotates the player so that they are facing in the direction of motion
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
            //gameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    //gets vector values for the camera to calculate side to side motion
    private Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0f;
        return right.normalized;
    }

    //gets vector values for the camera to calculate top to bottom motion
    private Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0f;
        return forward.normalized;
    }

    //callback that adds the jump force to the players forces
    private void DoJump(InputAction.CallbackContext obj)
    {
        if (isGrounded())
        {
            forceDir += Vector3.up * jumpForce;
        }
    }

    //callback that handles the block action
    private void DoBlock(InputAction.CallbackContext obj)
    {
        if (attLock) {
            attLock = false;
            blockBox.SetActive(true);
            isBlocking = true;
            Debug.Log("Block");
            Invoke("deactivateBlock", 1);
            Invoke("releaseLock", 1);
        }
    }

    //callback that handles the swipe action
    private void DoSwipe(InputAction.CallbackContext obj)
    {
        if (attLock) {
            attLock = false;
            swipeBox.SetActive(true);
            Debug.Log("Swipe");
            Invoke("deactivateSwipe", 1);
            Invoke("releaseLock", 1);
        }
    }

    //callback that handles the jab action
    private void DoJab(InputAction.CallbackContext obj)
    {
        if (attLock) {
            attLock = false;
            jabBox.SetActive(true);
            Debug.Log("Jab");
            Invoke("deactivateJab", 1);
            Invoke("releaseLock", 1);
        }
    }

    //check for if the player is currently on the ground
    private bool isGrounded()
    {

        Ray ray = new Ray(this.transform.position + Vector3.up * 0.00025f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.35f))
        {
            return true;
        }

        return false;
    }

    //check for if the player has moved off of the map completely
    private bool isOffMap()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.00025f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return true;
    }

    //method that will deactivate block hitbox called after a small delay
    private void deactivateBlock()
    {
        Debug.Log("Deactivating block");
        isBlocking = false;
        blockBox.SetActive(false);
    }

    //method that will deactivate jab hitbox called after a small delay
    private void deactivateJab()
    {
        Debug.Log("Deactivating jab");
        jabBox.SetActive(false);
    }

    //method that will deactivate swipe hitbox called after a small delay
    private void deactivateSwipe()
    {
        Debug.Log("Deactivating swipe");
        swipeBox.SetActive(false);
    }

    private void releaseLock() 
    {
        attLock = true;
    }
}
