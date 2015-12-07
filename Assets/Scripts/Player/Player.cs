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

    #region Ability Variables
    private Attacks[] abilityArray = new Attacks[4];
    private bool isAttacking;

    private GameObject[] registeredEnemies = new GameObject[4];
    Vector3 spellDirection = new Vector3(0, 90, 0);
    private Attacks castedSpell;
    private int abilityLoopindex = 0;
    #endregion

    #region Animations 
    private Animator _animator;
    private bool mirror = false;
    private bool death = false;
    private bool flipEnable = true;
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
        abilityArray[2] = AbilityManager._instance.CreateLeo();
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

        // update Cooldowns
        foreach (Attacks _spell in abilityArray)
        {
            if (_spell.CurrCooldown > 0) { _spell.UpdateCooldowns(); }
        }

        // find out if capricorn_2 can be used
        float minDistance = AbilityManager._instance.abilities[9].maxKockBackDistance;
        GameObject neartestObject = null;

        // get all player objects
        foreach (GameObject targetObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            // get kocked up players
            if (targetObject.GetComponent<Player>().playerVitals.KnockUped)
            {
                // get the distance to the player
                float distance = Mathf.Abs(Vector2.Distance(transform.position, targetObject.transform.position));

                // compair if plyer is in range
                if (distance <= AbilityManager._instance.abilities[9].knockBackDistance)
                {
                    // get nearest player
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        neartestObject = targetObject;
                    }
                }
            }
        }

        // use ability
        if (!disabled && !isAttacking)
        {
            // get input
            int spellused = -1;
            if (Input.GetKeyDown(playerControles[1])) { spellused = 0; } // basic
            if (Input.GetKeyDown(playerControles[2])) { spellused = 1; } // spell_1
            if (Input.GetKeyDown(playerControles[3])) { spellused = 2; } // spell_2
            if (Input.GetKeyDown(playerControles[4])) { spellused = 3; } // spell_3

            // use spell if casted
            if (spellused > -1 && spellused > abilityArray.Length)
            {
                // cast spell only if it's not on cooldown
                if (!abilityArray[spellused].IsDisabled)
                {
                    // use Capricorn_2 spell
                    if (abilityArray[spellused].Name == "Capricorn" && !controller.collisions.below)
                    {
                        if (neartestObject != null)
                        {
                            // use spell
                            abilityArray[spellused].Use(neartestObject, gameObject);
                            castedSpell.SetCooldowne();
                        }
                    }

                    // use meele funktion if it is a meele attack
                    else if (abilityArray[spellused].IsMeele)
                    {
                        // set casted spell (also triggerst meele funktion)
                        castedSpell = abilityArray[spellused];
                        castedSpell.SetCooldowne();
                    }
                }

                else
                {
                    Debug.Log(abilityArray[spellused].Name + " on cooldown for " + abilityArray[spellused].CurrCooldown + "/" + abilityArray[spellused].MaxCooldown);
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
        if (flipEnable)
        {
            if (Input.GetAxis(playerAxis + "_Horizontal") > 0 && !mirror)
            {
                Flip();
            }
            else if (Input.GetAxis(playerAxis + "_Horizontal") < 0 && mirror)
            {
                Flip();
            }
        }
        #endregion

        
        // use meele abilities
        if (castedSpell != null)
        {
            // disable and enable spellcasting if a spell is active
            isAttacking = true;

            // disable flipping while attacking
            flipEnable = false;

            // get the spellDirection
            if (mirror) { spellDirection = gameObject.transform.TransformDirection(Vector3.right); }
            else { spellDirection = gameObject.transform.TransformDirection(Vector3.left); }

            // use the meele attack funktion
            UseMeleeAttack();
        }

        else
        {
            if (isAttacking) { isAttacking = false; }
            if (!flipEnable) { flipEnable = true; }
        }
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

    void UseMeleeAttack()
    {
        // only continue if spell is still active (spell needs time to get to full range)
        if (castedSpell.ShallTravel)
        {
            // get travel distance
            float travelDistance = castedSpell.TravelDistance();

            // create a Debug ray
            Debug.DrawRay(gameObject.transform.position, spellDirection * travelDistance, Color.green);

            // create a raycast
            foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(gameObject.transform.position, spellDirection, travelDistance))
            {
                // check if hitted object is a Player
                if (objectHit.transform.tag == "Player")
                {
                    // only continue if spell didn't hit the spell caster
                    if (objectHit.transform.gameObject != gameObject)
                    {
                        // get every already registered enemy seperat
                        bool isEnemyRegistered = false;
                        int firstFreeEntry = 0;
                        int entryIndex = 0;

                        foreach (GameObject enemy in registeredEnemies)
                        {
                            // only compare filled index
                            if (enemy != null)
                            {
                                // find out if enemy is already hited
                                if (enemy == objectHit.transform.gameObject)
                                {
                                    isEnemyRegistered = true;
                                }
                            }

                            // get free entry
                            else
                            {
                                firstFreeEntry = entryIndex;
                            }
                            entryIndex++;
                        }

                        // only continue if enemy isn't registered previorsly (one anemy shall only be hitted once)
                        if (!isEnemyRegistered)
                        {
                            // register enemy
                            registeredEnemies[firstFreeEntry] = objectHit.transform.gameObject;

                            // checks if the spell already hit its max of players
                            if (castedSpell.MaxTargetsReached())
                            {
                                // use spell
                                castedSpell.Use(objectHit.transform.gameObject, gameObject);
                                break;
                            }
                        }
                    }

                    abilityLoopindex++;
                }
            }
        }

        // stop attack
        else
        {
            registeredEnemies = new GameObject[4];
            castedSpell.ResetPlayersHit();
            castedSpell.ShallTravel = true;
            castedSpell = null;
        }
    }
}