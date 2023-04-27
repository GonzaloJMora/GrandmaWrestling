using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    //for cams movement settings
    //This is a structure for storing the max speed, acceleration, and deceleration
    //in different contexts. On the ground we may want different move settings than in air
    //also this is familiar to cameron from earlier works.
    //this allows us to make multiple movement settings for any other reasons too, maybe a minigame!
    [System.Serializable]
    public class MovementSettings
    {
        public float MaxSpeed;
        public float Acceleration;
        public float Deceleration;

        public MovementSettings(float maxSpeed, float accel, float decel)
        {
            MaxSpeed = maxSpeed;
            Acceleration = accel;
            Deceleration = decel;
        }
    }
    
    //**************new cameron variables************************************
    [Header("Cams New Movement Stuff")]
    [SerializeField] private float m_Friction = 6;
    [SerializeField] private float m_Drag = 6;
    [SerializeField] private float m_Gravity = 20;
    [SerializeField] private float m_JumpForce = 7;
    [SerializeField] private MovementSettings m_GroundSettings = new MovementSettings(4, 14, 3);
    [SerializeField] private MovementSettings m_AirSettings = new MovementSettings(4, 14, 3);
    public bool m_WasAirborne = false;

    [Header("Cams Testing")]
    [SerializeField]
    public Vector3 m_PlayerVelocity = Vector3.zero;
    [SerializeField]
    public Vector3 m_MoveDirectionNorm = Vector3.zero;
    private Vector3 m_MoveInput;
    private CharacterController m_Character;
    [SerializeField]
    private bool m_JumpNeeded = false;
    

    //********************end new cameron variables******************

    //important references for input
    private InputActionAsset inputAsset;
    private InputAction movement;
    private InputActionMap player;
    private Rigidbody rb;

    //references to hitboxes
    [Header("Hitboxes")]
    public GameObject swipeBox;
    public GameObject jabBox;
    public GameObject blockBox;

    //lock to keep players from activating block, jab, and swipe at the same time
    private bool attLock = true;

    //lock used to keep the player from indefinitely sticking to the wall outside the stage
    private bool fallLock = false;

    //movement values
    [Header("Old Movement Values")]
    [SerializeField]
    public float movementForce = 1f;
    private const float cmovementForce = 1f;
    [SerializeField]
    public float jumpForce = 5f;
    [SerializeField]
    public float maxSpeed = 5f;

    //force to be applied onto player's rigidbody
    private Vector3 forceDir = Vector3.zero;

    //stuff for fighting
    public bool isBlocking = false;
    public bool canAct = true;
    public bool inLaunched = false;
    public int smokeRemaining = 0;
    
    [Header("Knockback Scaling")]
    [Tooltip("How many 'hits' the player starts with")]
    [SerializeField] public int hitInitial = 1;
    [Tooltip("How many hits it takes to recieve 100% knockback")]
    [SerializeField] public int hitNorm = 5;

    private int hitCount; //update in player start

    //used when player is off map to not allow them to stick on walls
    private Vector3 lastForce = Vector3.zero;

    //camera that determines which direction we move when joystick is moved
    [Header("Camera")]
    [SerializeField]
    public Camera cam;

    [Header("Effects")]
    [SerializeField] private GameObject SmokeEffect;

    [Header("Player Manager")]
    //access to lastCollision
    [SerializeField] private PlayerManager playerManager;

    //what player are we
    private Color c;

    [Header("Audio")]
    [SerializeField]
    private SoundTicketManager attackSound;

    [SerializeField]
    private SoundTicketManager hitSound;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip jumpSFX;

    [SerializeField]
    private AudioClip blockUpSFX;

    //initialization
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        inputAsset = this.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    private void Start() {
        c = GetComponent<MeshRenderer>().material.color;
        m_Character = GetComponent<CharacterController>();
        hitCount = hitInitial;
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
        hitSound.playSound();

        int launchPopup = 5; //determines how much additional vertical launch the attack will cause.
        
        inLaunched = true; //getting hit should cause smoke effect

        if (isBlocking)
        {
            return;
        }

        hitCount++;

        var launchVec = Vector3.Normalize(gameObject.transform.position - attacker_pos);
        
        float knockback_scale = ((float)hitCount/(float)hitNorm);
        launchVec *= force * knockback_scale;

        launchVec.y = launchVec.y + launchPopup;

        AddVelocity(launchVec);
    }

    private void FixedUpdate()
    {
        //only gives the player control when they are not off of the map (will need to fix this later most likely but works for now)
        //joystick direction to movement is based off of the camera position
        //m_MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //if (!isOffMap())
        if (true)
        {
            forceDir += movement.ReadValue<Vector2>().x * GetCameraRight(cam) * movementForce;
            forceDir += movement.ReadValue<Vector2>().y * GetCameraForward(cam) * movementForce;
            //not touching forceDir in case it does other things elsewhere

            if (inLaunched)
            {
                smokeRemaining = 30;
                inLaunched = false;
                SmokeEffect.GetComponent<ParticleSystem>().Play();
            }

            if (smokeRemaining>0)
            {
                smokeRemaining--;
            }
            else
            {
                //var newParticles = SmokeEffect.GetComponent<ParticleSystem>();
                SmokeEffect.GetComponent<ParticleSystem>().Stop();
            }

            //cams movement root
            if (m_Character.isGrounded)
            {
                GroundMove();
            }
            else
            {
                m_WasAirborne = true;
                AirMove();
            }
            
            m_Character.Move(m_PlayerVelocity * Time.deltaTime);



            
            //end cam new stuff

            //rb.AddForce(forceDir, ForceMode.Impulse);
            lastForce = forceDir;
            forceDir = Vector3.zero;
            fallLock = true;
        }

        // //if player is off the map, the previous force will be constantly applied
        // else
        // {
        //     //Debug.Log(rb.velocity.y);
        //     if (fallLock && lastForce.x == 0 && lastForce.z == 0) {
        //         lastForce.x = -rb.position.x; 
        //         lastForce.z = -rb.position.z;
        //         fallLock = false;
        //     } 

        //     //rb.AddForce(lastForce, ForceMode.Impulse);

        //     lastForce = Vector3.zero;
        // }

        LookAt();
    }


    private void AirMove()
    {
        ApplyDrag(1.0f);
        
        var wishdir = Vector3.zero;
        wishdir += movement.ReadValue<Vector2>().x * GetCameraRight(cam) * movementForce;
        wishdir += movement.ReadValue<Vector2>().y * GetCameraForward(cam) * movementForce;

        //wishdir.Normalize();
        m_MoveDirectionNorm = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= m_AirSettings.MaxSpeed;

        Accelerate(wishdir, wishspeed, m_AirSettings.Acceleration);

        // Apply gravity
        m_PlayerVelocity.y -= m_Gravity * Time.deltaTime;

    }
    
    // Handle ground movement.
    private void GroundMove()
    {

        ApplyFriction(1.0f);

        //var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
        //wishdir = m_Tran.TransformDirection(wishdir);

        var wishdir = Vector3.zero;
        wishdir += movement.ReadValue<Vector2>().x * GetCameraRight(cam) * movementForce;
        wishdir += movement.ReadValue<Vector2>().y * GetCameraForward(cam) * movementForce;


        //wishdir.Normalize();
        m_MoveDirectionNorm = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= m_GroundSettings.MaxSpeed;

        Accelerate(wishdir, wishspeed, m_GroundSettings.Acceleration);



        

        // Reset the gravity velocity
        if (m_WasAirborne)
        {
            m_PlayerVelocity.y = -m_Gravity * Time.deltaTime;
            m_WasAirborne = false;
        }
        //m_PlayerVelocity.y = -m_Gravity * Time.deltaTime;

        if (m_JumpNeeded)
        {
            m_PlayerVelocity.y = m_JumpForce;
            m_JumpNeeded = false;
        }
    }


    //rotates the player so that they are facing in the direction of motion
    private void LookAt()
    {
        //Vector3 direction = rb.velocity;
        Vector3 direction = m_PlayerVelocity;
        direction.y = 0f;

        if (movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            //this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
            gameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        else
        {
            //rb.angularVelocity = Vector3.zero;

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
        //if (isGrounded())
        if (m_Character.isGrounded)
        //if (true)
        {
            //forceDir += Vector3.up *  jumpForce;
            //signal the fixed update that a jump should be performed on the next update
            m_JumpNeeded = true;
            audio.PlayOneShot(jumpSFX);
            //Debug.Log("DoJump entered");
            //m_PlayerVelocity.y = m_JumpForce;
            Invoke("resetLastCollision", 0.75f);
        }
        else
        {
            //Debug.Log("char was not grounded");
        }
    }

    //last collision is reset if the player jumps
    private void resetLastCollision() {
        if (c == Color.blue) {
            playerManager.lastCollision[0] = -1;
        }

        else if (c == Color.red) {
            playerManager.lastCollision[1] = -1;
        }

        else if (c == Color.green) {
            playerManager.lastCollision[2] = -1;
        }

        else if (c == Color.yellow) {
            playerManager.lastCollision[3] = -1;
        }
    }

    //callback that handles the block action
    private void DoBlock(InputAction.CallbackContext obj)
    {

        /*if (canAct)
        {
            canAct = false;
            blockBox.SetActive(true);
            isBlocking = true;
            Debug.Log("Block");
            Invoke("deactivateBlock", 1);            
        }*/


        if (attLock) {
            attLock = false;
            blockBox.SetActive(true);
            audio.PlayOneShot(blockUpSFX);
            isBlocking = true;
            //Debug.Log("Block");
            Invoke("deactivateBlock", 1);
            Invoke("releaseLock", 1);
        }

    }

    //callback that handles the swipe action
    private void DoSwipe(InputAction.CallbackContext obj)
    {

        /*if (canAct)
        {
            canAct = false;
            swipeBox.SetActive(true);
            Debug.Log("Swipe");
            Invoke("deactivateSwipe", 1);
        }*/

        if (attLock) {
            attLock = false;
            swipeBox.SetActive(true);
            attackSound.playSound();
            //Debug.Log("Swipe");
            Invoke("deactivateSwipe", 1);
            Invoke("releaseLock", 1);
        }
    }

    //callback that handles the jab action
    private void DoJab(InputAction.CallbackContext obj)
    {

        /*if (canAct)
        {
            canAct = false;
            jabBox.SetActive(true);
            Debug.Log("Jab");
            Invoke("deactivateJab", 1);
        }*/

        if (attLock) {
            attLock = false;
            jabBox.SetActive(true);
            attackSound.playSound();
            //Debug.Log("Jab");
            Invoke("deactivateJab", 1);
            Invoke("releaseLock", 1);
        }
    }

    //check for if the player is currently on the ground
    public bool isGrounded()
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
        //Debug.Log("Deactivating block");
        isBlocking = false;
        blockBox.SetActive(false);
        canAct = true;
    }

    //method that will deactivate jab hitbox called after a small delay
    private void deactivateJab()
    {
        //Debug.Log("Deactivating jab");
        jabBox.SetActive(false);
        canAct = true;
    }

    //method that will deactivate swipe hitbox called after a small delay
    private void deactivateSwipe()
    {
        //Debug.Log("Deactivating swipe");
        swipeBox.SetActive(false);
        canAct = true;
    }

    private void releaseLock() 
    {
        attLock = true;
    }
    
    public IEnumerator PauseMovementForce(float time, int count)
    {
        //Debug.Log("START PAUSING: " + count);
        if (this.movementForce == 0f) { /*Debug.Log("BREAKING EARLY " + count);*/ yield break; }
        float temp = this.movementForce;
        this.movementForce = 0f;
        yield return new WaitForSeconds(0.5f);
        this.movementForce = 1f;
        //Debug.Log("STOPPED PAUSING: " + count);
    }

    //Starting adding helpers for camerons physics calculations

    // Calculates acceleration based on desired speed and direction.
    private void Accelerate(Vector3 targetDir, float targetSpeed, float accel)
    {
        float currentspeed = Vector3.Dot(m_PlayerVelocity, targetDir);
        float addspeed = targetSpeed - currentspeed;
        if (addspeed <= 0)
        {
            return;
        }

        float accelspeed = accel * Time.deltaTime * targetSpeed;
        if (accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }

        m_PlayerVelocity.x += accelspeed * targetDir.x;
        m_PlayerVelocity.z += accelspeed * targetDir.z;
    }


    public void AddVelocity(Vector3 vector)
    {
        //Debug.Log("Velocity Added");
        m_PlayerVelocity.x += vector.x;
        m_PlayerVelocity.y += vector.y;
        m_PlayerVelocity.z += vector.z;
        //Debug.Log("New Y: " + m_PlayerVelocity.y.ToString());
    }

    private void ApplyFriction(float t)
    {
        // Equivalent to VectorCopy();
        Vector3 vec = m_PlayerVelocity; 
        vec.y = 0;
        float speed = vec.magnitude;
        float drop = 0;

        // Only apply friction when grounded.
        //if (m_Character.isGrounded)
        if (true) //temp
        {
            float control = speed < m_GroundSettings.Deceleration ? m_GroundSettings.Deceleration : speed;
            drop = control * m_Friction * Time.deltaTime * t;
        }

        float newSpeed = speed - drop;
        //m_PlayerFriction = newSpeed;
        if (newSpeed < 0)
        {
            newSpeed = 0;
        }

        if (speed > 0)
        {
            newSpeed /= speed;
        }

        m_PlayerVelocity.x *= newSpeed;
        // playerVelocity.y *= newSpeed;
        m_PlayerVelocity.z *= newSpeed;
    }

    private void ApplyDrag(float t)
    {
        // Equivalent to VectorCopy();
        Vector3 vec = m_PlayerVelocity; 
        vec.y = 0;
        float speed = vec.magnitude;
        float drop = 0;

        // Only apply drag when airborne.
        if (!m_Character.isGrounded)
        {
            float control = speed < m_AirSettings.Deceleration ? m_AirSettings.Deceleration : speed;
            drop = control * m_Drag * Time.deltaTime * t;
        }

        float newSpeed = speed - drop;
        //m_PlayerFriction = newSpeed;
        if (newSpeed < 0)
        {
            newSpeed = 0;
        }

        if (speed > 0)
        {
            newSpeed /= speed;
        }

        m_PlayerVelocity.x *= newSpeed;
        // playerVelocity.y *= newSpeed;
        m_PlayerVelocity.z *= newSpeed;
    }

    public void ResetPhysics()
    {
        m_PlayerVelocity = Vector3.zero;
    }

    public void ResetHits()
    {
        hitCount = hitInitial;
    }

}
