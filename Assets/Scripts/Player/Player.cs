// -----| purpose |---------------------------------------------------------------
// this script is used for moveing the player and using spells

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerAbilities))]
public class Player : MonoBehaviour
{
    #region Variables
    [HideInInspector] public Controller2D controller;
    [HideInInspector] public Vector2 input;

    [Tooltip("Define Controller: P1, P2, P3, P4, KB")]
    public string playerAxis;

    #region Player Vitals
    [Header("Player Vitals:")]

    [HideInInspector] public LivingEntity playerVitals;
    
    public string name = "";
    public string type;
    public int maxHealth;
    public int lives = 3;
    public float moveSpeed = 6; 
    public float slowedSpeed = 3;

    [HideInInspector] public string[] playerControles = new string[5];
    #endregion

    #region Jumping
    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 velocity;

    [Header("Jumping:")]
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    [Tooltip("Time it takes to jump (Used to calculate gravity)")]
    public float timeToJumpApex = 0.4f;

    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;
    float maxJumpVelocity;
    float minJumpVelocity;
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
    [HideInInspector] public bool gamerulesDisabled = false;
    #endregion

    #region Animations 
    [HideInInspector] public Animator _animator;
    private bool mirror = false;
    private bool death = false;
    [HideInInspector] public bool flipEnable = true;
    private bool stunFlip = false;
    private bool dead = false;

    [Header("Getting Hit")]
    public Color hitColor = Color.red;
    [Tooltip("Time it takes to change back to the original color")]
    public float changebackTime = 0.2f;
    #endregion

    public PlayerAbilities playerAbilitiesScript;
    #endregion

    #region Getter&Setter
    public bool Mirror { get { return mirror; } }
    #endregion

    void Awake()
    {
        controller = GetComponent<Controller2D>();
        _animator = GetComponent<Animator>();

        playerAbilitiesScript = gameObject.GetComponent<PlayerAbilities>();

        if (GameManager._instance != null && GameManager._instance.playerMaxHealth != 0) { maxHealth = GameManager._instance.playerMaxHealth; }
        if (GameManager._instance != null && GameManager._instance.lifeLimit != 0) { lives = GameManager._instance.lifeLimit; }

        // create playerVitals
        playerVitals = new LivingEntity(gameObject, name, type, moveSpeed, slowedSpeed, maxHealth, lives);

    }

    void Start()
    {
        // --- [ set name and axis ] ---
        foreach (CharacterPicture player in GameManager._instance.charPics) {
            // check if object isn't null
            if (player != null) {
                // check player type
                if (player.Character == type) {
                    // set name and axis
                    name = player.Name;
                    playerAxis = player.Axis;
                }
            }
        }

        // check if axis is set correctly
        if (playerAxis == "KB" || playerAxis == "P1" || playerAxis == "P2" || playerAxis == "P3" || playerAxis == "P4") { }
        else { Debug.LogError("<b>" + name + "</b> uses a incorrect playerAxis"); }

        // --- [ set standart controles ] ---
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

        #region debug player Controles
        string debugText = name + " Controles:\n";

        // get all controles
        for (int i = 0; i < playerControles.Length; i++)
        {
            // set debug text
            switch (i)
            {
                case 0: debugText += "  jump: "; break;
                case 1: debugText += "  basic: "; break;
                case 2: debugText += "  spell_1: "; break;
                case 3: debugText += "  spell_2: "; break;
                case 4: debugText += "  spell_3: "; break;
            }

            // add value
            debugText += playerControles[i];
            // add colon
            if (i != 4) { debugText += ","; }
        }

        // print debug text and key
        CustomDebug.Log(debugText, "Controles");
        #endregion

        // --- [ calculate gravity ] ---
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        // --- [ Imobelised ] ---
        if (playerVitals.Stunned || playerVitals.KnockUped || playerVitals.KnockBacked || playerVitals.Dashing || gamerulesDisabled) { disabled = true; }
        else { disabled = false; }

        // --- [ Condition Animations ] ---
        _animator.SetBool("Stunned", playerVitals.Stunned);

        _animator.SetBool("KnockUp", playerVitals.KnockUped);

        _animator.SetBool("KnockBack", playerVitals.KnockBacked);

        if (playerVitals.KnockBacked) {
            if ((!mirror && velocity.x < 0) || (mirror && velocity.x > 0)) {
                ConditionFlip();
                flipEnable = false;
            }
        }
        
        if (!playerVitals.KnockBacked) { stunFlip = false; flipEnable = true; }

        // Get movement input ( controler / keyboard )
        input = new Vector2(Input.GetAxisRaw(playerAxis + "_Horizontal"), Input.GetAxisRaw(playerAxis + "_Vertical"));

        // check for movement
        Movement();

        // add gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        // Stop jumping / falling, when colliding top / bottom
        if (controller.collisions.above || controller.collisions.below) { velocity.y = 0; }

        #region Flipping
        if (flipEnable)
        {
            // check if player should be fliped
            if (Input.GetAxis(playerAxis + "_Horizontal") > 0 && !mirror) { Flip(); }
            else if (Input.GetAxis(playerAxis + "_Horizontal") < 0 && mirror) { Flip(); }
        }
        #endregion
    }

    void Movement()
    {
        // checkif player can move
        if (!disabled)
        {
            _animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw(playerAxis + "_Horizontal")));
            // --- [ horizontal movement ] ---
            // get current movementspeed
            float targetVelocityX = input.x * playerVitals.MoveSpeed;
            // move player
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            // kock player away from wall
            if ((input.x < 0 && controller.collisions.left) || (input.x > 0 && controller.collisions.right)) { velocity.x = -input.x; }

            // find out on which side the player is walling
            int wallDirX = (controller.collisions.left) ? -1 : 1;

            bool wallSliding = false;

            // --- [ sticked to wall ] ---
            // check if player is faceing a wall
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                // active wallSliding
                wallSliding = true;

                // disable flipping
                flipEnable = true;

                // set animation
                _animator.SetBool("WallSlide", true);
                _animator.SetBool("Fall", false);

                // regulate sliding speed
                if (velocity.y < -wallSlideSpeedMax) { velocity.y = -wallSlideSpeedMax; }

                // check if player should stick to wall
                if (timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    // sub timeToWallUnstick if player is aiming away
                    if (input.x != wallDirX && input.x != 0) { timeToWallUnstick -= Time.deltaTime; }
                    // reset timeToWallUnstick if player is aiming towaerds wall
                    else { timeToWallUnstick = wallStickTime; }
                }

                // reset timeToWallUnstick
                else { timeToWallUnstick = wallStickTime; }
            }

            else
            {
                // set animation
                _animator.SetBool("WallSlide", false);
                // enable flipping
                flipEnable = true;
            }

            // --- [ jumps ] ---
            // check if jump button is pressed
            if (Input.GetKeyDown(playerControles[0]))
            {
                // check if player is wallsliding
                if (wallSliding)
                {
                    // set animation
                    _animator.SetBool("WallSlide", true);
                    _animator.SetBool("Falling", false);

                    // --- [ wall climb ] ---
                    // check if player is aiming towards wall
                    if ((wallDirX < 0 && input.x < 0) || (wallDirX > 0 && input.x > 0))
                    {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }

                    // --- [ wall jump ] ---
                    // check if player has no aim
                    else if (input.x == 0)
                    {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }

                    // --- [ wall leap ] ---
                    // check if player is aiming away from wall
                    else
                    {
                        // check if player should leap diagonal
                        if (Mathf.Abs(input.y) > 0.2)
                        {
                            // set velocity
                            velocity.x = -wallDirX * wallLeap / 2;
                            velocity.y = wallLeap;
                        }

                        // check if player should leap strait forward
                        else
                        {
                            // set velocity
                            velocity.x = -wallDirX * wallLeap;
                            velocity.y = wallLeap / 4;
                        }
                    }
                }

                // set wallslide animation off
                else { _animator.SetBool("WallSlide", false); }

                // --- [ jump on floor ] ---
                // check if player is on floor
                if (controller.collisions.below)
                {
                    // set animation
                    _animator.SetTrigger("Jump");

                    // jump (set velocity)
                    velocity.y = maxJumpVelocity;
                }
            }

            if (Input.GetKeyUp(playerControles[0]))
            {
                if (velocity.y > minJumpVelocity) { velocity.y = minJumpVelocity; }
            }

            // check if player is falling (and isn't facing a wall)
            if (velocity.y < 0.1 || !controller.collisions.below || !controller.collisions.left || !controller.collisions.right)
            {
                // set animations
                _animator.SetBool("Fall", true);
            }

            // set player standing
            if (velocity.y == 0 && controller.collisions.below)
            {
                // set animation
                _animator.SetBool("Fall", false);
                _animator.SetTrigger("Land");
            }

        }
    }
    
    public void Flip()
    {
        if (!disabled)
        {
            mirror = !mirror;
            Vector2 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void ConditionFlip()
    {
        mirror = !mirror;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void PickupOrb(Attacks attack)
    {
        if (attack != null)
        {
            Attacks firstSpell = playerAbilitiesScript.abilityArray[1];
            Attacks secondSpell = playerAbilitiesScript.abilityArray[2];

            for (int i = 0; i < playerAbilitiesScript.abilityArray.Length; i++)
            {
                if (playerAbilitiesScript.abilityArray[i].Name == attack.Name)
                {
                    print("Same");
                    playerAbilitiesScript.abilityArray[i] = attack;
                    //CustomDebug.LogArray(playerAbilitiesScript.abilityArray);
                    return;
                }
            }

            for (int i = 0; i < playerAbilitiesScript.abilityArray.Length; i++)
            {
                if (playerAbilitiesScript.abilityArray[i].Name != attack.Name)
                {
                    playerAbilitiesScript.abilityArray[1] = attack;
                    playerAbilitiesScript.abilityArray[2] = firstSpell;
                    playerAbilitiesScript.abilityArray[3] = secondSpell;
                    //CustomDebug.LogArray(playerAbilitiesScript.abilityArray);
                    return;
                }
            }

            
        }
    }

    #region Die
    public void Die()
    {
        if (!dead) {
            CustomDebug.Log("Died", "Testing");
            _animator.SetTrigger("Death");
            dead = true;
        }
    }

    private IEnumerator IDie() {
        GetComponent<Animator>().enabled = false;
        gamerulesDisabled = true;
        flipEnable = false;
        yield return new WaitForSeconds(GameManager._instance.timeDeathSpawn);
        switch(type) {
            case "Earth":
                if(GameManager._instance.lifeLimitPlayer[0] > 0) {
                    GameManager._instance.SpawnNewPlayer(gameObject);
                }
                break;
            case "Sun":
                if (GameManager._instance.lifeLimitPlayer[1] > 0) {
                    GameManager._instance.SpawnNewPlayer(gameObject);
                }
                break;
        }
       
        Destroy(gameObject);
        yield return null;
    }
    #endregion

    #region Getting Hit
    /// <summary>
    /// Change Color when hit
    /// </summary>
    /// <param name="t">Time between changes</param>
    /// <returns></returns>
    public void ChangeToHitColor(float t) {
        StartCoroutine(ChangeToColor(t));
    }

    private IEnumerator ChangeToColor(float t) {
        Color tmpColor = GetComponent<SpriteRenderer>().color;

        gameObject.GetComponent<SpriteRenderer>().color = hitColor;
        yield return new WaitForSeconds(t);
        gameObject.GetComponent<SpriteRenderer>().color = tmpColor;
        yield return null;
    }
    #endregion
}
