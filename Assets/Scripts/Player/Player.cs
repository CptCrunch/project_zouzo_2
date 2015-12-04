using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private Vector2[] debugRayHitpointArray = new Vector2[4];

    #region Player Vitals
    [HideInInspector]
    public LivingEntity playerVitals;

    [Header("Player Vitals:")]
    public string name = "";
    public float moveSpeed = 6;
    public float slowedSpeed = 3;
    public float maxHealth;
    private string[] playerControles = new string[5];
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
        if (playerAxis == "KB" || playerAxis == "P1" || playerAxis == "P2" || playerAxis == "P3" || playerAxis == "P4") { }
        else { Debug.LogError("incorrect playerAxis"); }

        // set controles

        // - keybord
        if (playerAxis == "KB")
        {
            playerControles[0] = "space";       // jump
            playerControles[1] = "1";           // basic
            playerControles[2] = "2";           // spell_1
            playerControles[3] = "3";           // spell_2
            playerControles[4] = "4";           // spell_3
        }

        // - joysick
        if (playerAxis == "P1" || playerAxis == "P2" || playerAxis == "P3" || playerAxis == "P4")
        {
            playerControles[0] = "joystick " + playerAxis.Substring(1, 1) + " button 0";        // jump
            playerControles[1] = "joystick " + playerAxis.Substring(1, 1) + " button 5";        // basic
            playerControles[2] = "joystick " + playerAxis.Substring(1, 1) + " button 1";        // spell_1
            playerControles[3] = "joystick " + playerAxis.Substring(1, 1) + " button 2";        // spell_2
            playerControles[4] = "joystick " + playerAxis.Substring(1, 1) + " button 3";        // spell_3
        }

        // debug player Controles
        /*string debugText = name + " Controles:\n";

        for (int i = 0; i < playerControles.Length; i++) {
            
            // set debug text
            switch (i)
            {
                case 0: debugText += "  jump: "; break;
                case 1: debugText += "  basic: "; break;
                case 2: debugText += "  spell_1: "; break;
                case 3: debugText += "  spell_2: "; break;
                case 4: debugText += "  spell_3: "; break;
            }

            // print debug text and key
            debugText += playerControles[i] + ",";
        }

        Debug.Log(debugText);*/

        // set starter abilities
        abilityArray[0] = AbilityManager._instance.CreateBasic();
        abilityArray[1] = AbilityManager._instance.CreateCapricorn();
        abilityArray[2] = AbilityManager._instance.CreateBasic();
        abilityArray[3] = AbilityManager._instance.CreateBasic();

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
        #region Testing Conditions
        if (Input.GetKeyDown(KeyCode.P)) { playerVitals.ApplyStun(300); }
        if (Input.GetKeyDown(KeyCode.O)) { playerVitals.ApplySlowOverTime(300); }
        if (Input.GetKeyDown(KeyCode.L)) { playerVitals.ApplySlow(true); }
        if (Input.GetKeyDown(KeyCode.K)) { playerVitals.ApplySlow(false); }
        if (Input.GetKeyDown(KeyCode.I)) { playerVitals.ApplyPlayerKnockUp(5f, 300); }
        if (Input.GetKeyDown(KeyCode.U)) { playerVitals.ApplyPlayerKnockBack(5f, 0f, 300); }
        #endregion

        // Imobelised
        if (playerVitals.Stunned || playerVitals.KnockUped || playerVitals.KnockBacked) { disabled = true; }
        else { disabled = false; }

        // use ability
        if (!disabled)
        {
            // - basic
            if (Input.GetKeyDown(playerControles[1]))
            {
                if (abilityArray[0].IsMeele)
                {
                    StartCoroutine(meleeAttack(abilityArray[0]));
                    /*Debug.Log(name + " used basic");*/
                }
            }

            // - ability 1
            if (Input.GetKeyDown(playerControles[2]))
            {
                if (abilityArray[1].IsMeele)
                {
                    StartCoroutine(meleeAttack(abilityArray[1]));
                    /*Debug.Log(name + " used spell_1");*/
                }
            }

            // - ability 2
            if (Input.GetKeyDown(playerControles[3]))
            {
                if (abilityArray[2].IsMeele)
                {
                    StartCoroutine(meleeAttack(abilityArray[2]));
                    /*Debug.Log(name + " used spell_2");*/
                }
            }

            // - ability 3
            if (Input.GetKeyDown(playerControles[4]))
            {
                if (abilityArray[3].IsMeele)
                {
                    StartCoroutine(meleeAttack(abilityArray[3]));
                    /*Debug.Log(name + " used spell_3");*/
                }
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
            if (Input.GetKeyDown(playerControles[0]))
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
                }
                else
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

            if (Input.GetKeyUp(playerControles[0]))
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

    IEnumerator meleeAttack(Attacks _usedSpell)
    {

        if (!_usedSpell.OnCooldown)
        {
            Vector3 fwd = new Vector3(0, 0, 0);

            // set atack into right direction
            if (mirror) { fwd = gameObject.transform.TransformDirection(Vector3.right); }
            else { fwd = gameObject.transform.TransformDirection(Vector3.left); }

            Debug.DrawRay(gameObject.transform.position, fwd, Color.white);

            yield return new WaitForSeconds(_usedSpell.CastTime);

            // send a visual debug ray
            Debug.DrawRay(gameObject.transform.position, fwd * abilityArray[0].Range, Color.green);

            // reset the debugRayHitpointArray
            for (int i = 0; debugRayHitpointArray.Length > i; i++)
            {
                debugRayHitpointArray[i] = new Vector2(0, 0);
            }

            // create a raycast
            /*RaycastHit2D objectHit = Physics2D.RaycastAll(gameObject.transform.position, fwd, _usedSpell.Range);*/
            int loopindex = 0;
            foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(gameObject.transform.position, fwd, _usedSpell.Range))
            {

                // add objectHitpoint to debugRayHitpoint to show the hitten point in editor
                debugRayHitpointArray[loopindex] = objectHit.transform.position;

                // compares if raycast hits a player
                if (objectHit.transform.tag == "Player" && loopindex > 0)
                {
                    if (loopindex > 1)
                    {
                        if (_usedSpell.IsAOE)
                        {
                            Debug.Log(name + " hit: " + objectHit.transform.gameObject.name);

                            // use spell and set it on cooldown
                            _usedSpell.Use(objectHit.transform.gameObject);
                            StartCoroutine(OffCooldown(_usedSpell));

                            Debug.Log(name + " Health: " + objectHit.transform.gameObject.GetComponent<Player>().playerVitals.CurrHealth);
                        }
                    }

                    else
                    {
                        Debug.Log(name + " hit: " + objectHit.transform.gameObject.name);

                        // use spell and set it on cooldown
                        _usedSpell.Use(objectHit.transform.gameObject);
                        StartCoroutine(OffCooldown(_usedSpell));

                        Debug.Log(name + " Health: " + objectHit.transform.gameObject.GetComponent<Player>().playerVitals.CurrHealth);
                    }
                }
                loopindex++;
            }
        }
    }

    IEnumerator OffCooldown(Attacks _spell)
    {
        yield return new WaitForSeconds(_spell.Cooldown);
        _spell.OnCooldown = false;
    }

    private int hitpointDebugLoopindex;

    void OnDrawGizmos()
    {

        // reset loopindex
        hitpointDebugLoopindex = 0;

        // visualize hitpoints
        foreach (Vector2 raycastHitpoint in debugRayHitpointArray)
        {

            // only draw hitpoint if it hit a player
            if (raycastHitpoint != new Vector2(0, 0))
            {

                // set hitpoint color
                switch (hitpointDebugLoopindex)
                {
                    case 0:
                        Gizmos.color = Color.white;
                        break;

                    case 1:
                        Gizmos.color = Color.yellow;
                        break;

                    case 2:
                        Gizmos.color = Color.red;
                        break;

                    case 3:
                        Gizmos.color = Color.black;
                        break;
                }

                // draw hitpoint
                Gizmos.DrawLine(raycastHitpoint - new Vector2(0.2f, 0), raycastHitpoint + new Vector2(0.2f, 0));
                Gizmos.DrawLine(raycastHitpoint - new Vector2(0, 0.2f), raycastHitpoint + new Vector2(0, 0.2f));

                Gizmos.DrawLine(raycastHitpoint + new Vector2(0, 0.1f), raycastHitpoint + new Vector2(0.1f, 0));
                Gizmos.DrawLine(raycastHitpoint + new Vector2(0.1f, 0), raycastHitpoint + new Vector2(0, -0.1f));
                Gizmos.DrawLine(raycastHitpoint + new Vector2(0, -0.1f), raycastHitpoint + new Vector2(-0.1f, 0));
                Gizmos.DrawLine(raycastHitpoint + new Vector2(-0.1f, 0), raycastHitpoint + new Vector2(0, 0.1f));
            }
            hitpointDebugLoopindex++;
        }
    }
}
