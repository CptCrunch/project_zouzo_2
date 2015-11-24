using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public Controller2D controller;
    [HideInInspector]
    public Vector2 input;

    [Tooltip("Define Controller: P1, P2, P3, P4, KB")]
    public string playerAxis;

    #region Player Vitals
    [HideInInspector]
    public LivingEntity playerVitals;

    [Header("Player Vitals:")]
    public string name = "";
    public float moveSpeed = 6;
    public float slowedSpeed = 3;
    public float maxHealth;
    #endregion

    #region Jumping
    [Header("Jumping:")]
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    [Tooltip("Time it takes to jump (Used to calculate gravity)")]
    public float timeToJumpApex = .4f;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float gravity;
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
    #endregion

    // ability variables
    private Attacks[] abilityArray = new Attacks[4];
    
    #region Animations 
    private Animator _animator;
    private bool mirror = false;
    private bool death = false;
    #endregion
    #endregion

    void Start()
    {
        // set starter abilities
        abilityArray[0] = AbilityManager._instance.CreateBasic();
        abilityArray[1] = AbilityManager._instance.CreateCapricorn();

        controller = GetComponent<Controller2D>();
        _animator = GetComponent<Animator>();

        // create playerVitals
        playerVitals = new LivingEntity(gameObject, name, moveSpeed, slowedSpeed, maxHealth);
        
        // calculate gravity
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        
        /*print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);*/
    }

    void Update()
    {
        if (playerAxis != "KB")
        {
            if (Input.GetKeyDown("joystick " + playerAxis.Substring(1, 1) + " button " + "1"))
            {
                Debug.Log(playerAxis.Substring(1, 1));
            }
        }

        #region Testing Conditions
        if (Input.GetKeyDown(KeyCode.P)) { StartCoroutine(playerVitals.Stun(3f)); }
        if (Input.GetKeyDown(KeyCode.O)) { StartCoroutine(playerVitals.SlowOverTime(3f)); }
        if (Input.GetKeyDown(KeyCode.L)) { playerVitals.Slow(true); }
        if (Input.GetKeyDown(KeyCode.K)) { playerVitals.Slow(false); }
        if (Input.GetKeyDown(KeyCode.I)) { StartCoroutine(playerVitals.PlayerKnockUp(5f, 0.2f)); }
        if (Input.GetKeyDown(KeyCode.U)) { StartCoroutine(playerVitals.PlayerKnockBack(5f, 0f, 0.2f)); }
        #endregion

        // Imobelised
        if (playerVitals.Stunned || playerVitals.KnockUped || playerVitals.KnockBacked) { disabled = true; }
        else { disabled = false; }

        // use ability

        // basic
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (abilityArray[0].IsMeele)
            {
                meleeAttack(abilityArray[0]);
            }
        }

        // ability 1
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (abilityArray[1].IsMeele)
            {
                meleeAttack(abilityArray[1]);
            }
        }

        // ability 2
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (abilityArray[2].IsMeele)
            {
                meleeAttack(abilityArray[2]);
            }
        }

        // ability 3
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (abilityArray[3].IsMeele)
            {
                meleeAttack(abilityArray[3]);
            }
        }

        //Debug.Log(name + " health: " + playerVitals.CurrHealth);

        // Get movement input ( controler / keyboard )
        input = new Vector2(Input.GetAxisRaw(playerAxis + "_Horizontal"), Input.GetAxisRaw(playerAxis + "_Vertical"));
        _animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw(playerAxis + "_Horizontal")));

        Movement();

        // add gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        // Stop jumping / falling when colliding top / bottom
        if (controller.collisions.above || controller.collisions.below) { velocity.y = 0; }

        #region Flipping
        if (Input.GetAxis(playerAxis + "_Horizontal") > 0 && !mirror)
        {
            Flip();
        }
        else if (Input.GetAxis(playerAxis + "_Horizontal") < 0 && mirror)
        {
            Flip();
        }
        #endregion
    }

    void Movement()
    {
        if (!disabled)
        {
            // horizontal movement
            float targetVelocityX = input.x * playerVitals.MoveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            int wallDirX = (controller.collisions.left) ? -1 : 1;
            bool wallSliding = false;

            // sticked to wall
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                wallSliding = true;
                _animator.SetBool("WallSlide", true);
                _animator.SetBool("Fall", false);

                // regulate sliding speed
                if (velocity.y < -wallSlideSpeedMax) { velocity.y = -wallSlideSpeedMax; }

                // stick to wall
                if (timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if (input.x != wallDirX && input.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }

                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }

                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                _animator.SetBool("WallSlide", false);
            }

            // jump
            if (Input.GetButtonDown(playerAxis + "_Jump"))
            {
                if (wallSliding)
                {
                    _animator.SetBool("WallSlide", true);
                    _animator.SetBool("Falling", false);

                    // wall climb
                    if ((wallDirX < 0 && input.x < 0) || (wallDirX > 0 && input.x > 0))
                    {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }

                    // wall jump
                    else if (input.x == 0)
                    {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }

                    // wall leap
                    else
                    {
                        if (Mathf.Abs(input.y) > 0.2)
                        {
                            velocity.x = -wallDirX * wallLeap / 2;
                            velocity.y = wallLeap;
                        }

                        else
                        {
                            velocity.x = -wallDirX * wallLeap;
                            velocity.y = wallLeap / 4;
                        }
                    }
                } else
                {
                    _animator.SetBool("WallSlide", true);
                }

                // jump on floor
                if (controller.collisions.below)
                {
                    _animator.SetTrigger("Jump");
                    velocity.y = maxJumpVelocity;
                }
            }

            if (Input.GetButtonUp(playerAxis + "_Jump"))
            {
                if (velocity.y > minJumpVelocity) { velocity.y = minJumpVelocity; }
            }


            if (velocity.y < 0.1 || !controller.collisions.below || !controller.collisions.left || !controller.collisions.right)
            {
                _animator.SetBool("Fall", true);
            }

            if (velocity.y == 0 && controller.collisions.below)
            {
                _animator.SetBool("Fall", false);
                _animator.SetTrigger("Land");
            }

        }
    }

    void Flip()
    {
        mirror = !mirror;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    void meleeAttack(Attacks _usedSpell) {

        if (!_usedSpell.OnCooldown) {
            Vector3 fwd = new Vector3(0,0,0);

            // set atack into right direction
            if (mirror) { fwd = gameObject.transform.TransformDirection(Vector3.right); }
            else { fwd = gameObject.transform.TransformDirection(Vector3.left); }

            // send a visual debug ray
            Debug.DrawRay(gameObject.transform.position, fwd * abilityArray[0].Range, Color.green);

            // create a raycast
            RaycastHit2D objectHit = Physics2D.Raycast(gameObject.transform.position, fwd, _usedSpell.Range);
            // compares if raycast hits a player
            if (objectHit.transform.tag == "Player") {
                Debug.Log(name + " hit: " + objectHit.transform.gameObject.name);
                _usedSpell.Use(objectHit.transform.gameObject);
                StartCoroutine(OffCooldown(_usedSpell));
                Debug.Log(name + " Health: " + objectHit.transform.gameObject.GetComponent<Player>().playerVitals.CurrHealth);
            }
        }
    }

    IEnumerator OffCooldown(Attacks _spell) {
        yield return new WaitForSeconds(_spell.Cooldown);
        _spell.OnCooldown = false;
    }
}
