using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    #region Variables

    public float moveSpeed = 6;
    
    [HideInInspector]
    public Controller2D controller;
   
    [Tooltip("Define Controller: P1, P2, P3, P4, KB")]
    public string playerAxis;

    #region Player Vitals
    private LivingEntity playerVitals;

    [Header("Player Vitals:")]
    public float maxHealth;
    public string name;
    [Tooltip("Only used if its not set in gamerules")]
    public float basicAttackDamage;
    #endregion

    Vector2 input;

    #region Jumping
    [Header ("Jumping:")]
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    [Tooltip("Time it takes to jump (Used to calculate gravity)")]
    public float timeToJumpApex = .4f;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    [HideInInspector]
    public Vector3 velocity;
    float velocityXSmoothing;
    #endregion

    #region Walljump & Sliding
    [Header("Wall Jumping & Sliding:")]
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public float wallLeap;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;

    float timeToWallUnstick;
    #endregion

    #region Condition Variables
    private bool disabled = false;
    [HideInInspector]
    public bool stunned = false;
    [HideInInspector]
    public bool knockUp = false;
    #endregion

    #region Animations 
    private Animator _animator;
    private bool mirror = false;
    private bool death = false;
    #endregion
    #endregion

    void Start() {
        controller = GetComponent<Controller2D>();
        /*_animator = GetComponent<Animator>();*/

        // create playerVitals
        playerVitals = new LivingEntity(maxHealth, name, basicAttackDamage * (Gamerules._instance.damageModifier / 100));

        // calculate gravity
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        /*print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);*/
    }

    void Update() {

        // imobelised
        if (stunned || knockUp) { disabled = true; }
        else { disabled = false; }

        // get movement input ( controler / keyboard )
        input = new Vector2(Input.GetAxisRaw(playerAxis + "_Horizontal"), Input.GetAxisRaw(playerAxis + "_Vertical"));
        /*_animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw(playerAxis + "_Horizontal")));*/

        Movement();

        // add gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        // stop jumping / falling when colliding top / bottom
        if (controller.collisions.above || controller.collisions.below) { velocity.y = 0; }

        #region Flipping
        if (Input.GetAxis(playerAxis + "_Horizontal") > 0 && !mirror) {
            Flip();
        }
        else if (Input.GetAxis(playerAxis + "_Horizontal") < 0 && mirror) {
            Flip();
        }
        #endregion
    }

    void Movement() {
        if (!disabled) {

            // horizontal movement
            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            int wallDirX = (controller.collisions.left) ? -1 : 1;
            bool wallSliding = false;
            
            // sitcked to wall
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
                wallSliding = true;

                // regulate sliding speed
                if (velocity.y < -wallSlideSpeedMax) { velocity.y = -wallSlideSpeedMax; }
                
                // stic to wall
                if (timeToWallUnstick > 0) {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if (input.x != wallDirX && input.x != 0) {
                        timeToWallUnstick -= Time.deltaTime; }

                    else {
                        timeToWallUnstick = wallStickTime; }
                }

                else {
                    timeToWallUnstick = wallStickTime;
                }
            }
            
            // jump
            if (Input.GetButtonDown(playerAxis + "_Jump")) {
                if (wallSliding) {
                    
                    // wall climb
                    if ((wallDirX < 0 && input.x < 0) || (wallDirX > 0 && input.x > 0))
                    {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }
                    
                    // wall jump
                    else if (input.x == 0) {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }
                    
                    // wall leap
                    else {
                        if (Mathf.Abs(input.y) > 0.2) {
                            velocity.x = -wallDirX * wallLeap / 2;
                            velocity.y = wallLeap;
                        }
                        
                        else {
                            velocity.x = -wallDirX * wallLeap;
                            velocity.y = wallLeap / 4;
                        }
                    }
                }

                // jump on floor
                if (controller.collisions.below) {
                    /*_animator.SetTrigger("Jump");*/
                    velocity.y = maxJumpVelocity;
                }
            }

            if (Input.GetButtonUp(playerAxis + "_Jump")) {
                if (velocity.y > minJumpVelocity) { velocity.y = minJumpVelocity; }
            }

            /*if (velocity.y < -0.1) { _animator.SetTrigger("Fall"); }*/
            /*if (velocity.y == 0) { _animator.SetTrigger("Land"); }*/
        }
    }
    
    void Flip() {
        mirror = !mirror;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}