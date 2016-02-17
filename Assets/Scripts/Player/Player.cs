// -----| purpose |---------------------------------------------------------------
// this script is used for moveing the player and using spells

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2D))]
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
    public float maxHealth;
    public int lives = 3;
    public float moveSpeed = 6;
    public float slowedSpeed = 3;

    private string[] playerControles = new string[5];
    #endregion

    #region Jumping
    [Header("Jumping:")]

    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 velocity;

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

    #region Ability Variables
    public Attacks[] abilityArray = new Attacks[4];
    private bool isAttacking;
    public Attacks castedMeeleSpell;
    #endregion

    #region Animations 
    private Animator _animator;
    private bool mirror = false;
    private bool death = false;
    [HideInInspector] public bool flipEnable = true;
    #endregion

    #endregion

    #region Getter&Setter
    public bool Mirror { get { return mirror; } }
    #endregion

    void Awake()
    {
        // --- [ set name and axis ] ---
        foreach (CharacterPicture player in Gamerules._instance.charPics)
        {
            // check if object isn't null
            if (player != null)
            {
                // check player type
                if (player.Character == type)
                {
                    // set name and axis
                    name = player.Name;
                    playerAxis = player.Axis;
                }
            }
        }
    }

    void Start()
    {
        controller = GetComponent<Controller2D>();
        _animator = GetComponent<Animator>();

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

        // --- [ set starter abilities ] ---
        abilityArray[0] = AbilityManager.Instance.CreateBasic(gameObject);           // basic
        abilityArray[1] = AbilityManager.Instance.CreateLeo(gameObject);             // spell_1
        abilityArray[2] = AbilityManager.Instance.CreateCapricorn(gameObject);       // spell_2
        abilityArray[3] = AbilityManager.Instance.CreateSaggitarius(gameObject);     // spell_3

        // create playerVitals
        playerVitals = new LivingEntity(gameObject, name, moveSpeed, slowedSpeed, maxHealth, lives);

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

        /*if (gamerulesDisabled) { Debug.Log(name + " is disabled"); }*/

        // --- [ update Cooldowns / timeBetweenCasts ] ---
        foreach (Attacks _spell in abilityArray)
        {
            // update cooldown
            _spell.UpdateCooldowns();
            // update timeBetweenCasts
            if (_spell.IsCasted) { _spell.TimeBeteewnCasts += Time.deltaTime; }
        }

        // --- [ use ability ] ---
        // use cast (on button pressed)
        if (Input.GetKeyDown(playerControles[1])) { abilityArray[0].Cast(gameObject); } // basic
        if (Input.GetKeyDown(playerControles[2])) { abilityArray[1].Cast(gameObject); } // spell_1
        if (Input.GetKeyDown(playerControles[3])) { abilityArray[2].Cast(gameObject); } // spell_2
        if (Input.GetKeyDown(playerControles[4])) { abilityArray[3].Cast(gameObject); } // spell_3

        // use aftercast (on button released)
        if (Input.GetKeyUp(playerControles[1])) { abilityArray[0].AfterCast(); } // basic
        if (Input.GetKeyUp(playerControles[2])) { abilityArray[1].AfterCast(); } // spell_1
        if (Input.GetKeyUp(playerControles[3])) { abilityArray[2].AfterCast(); } // spell_2
        if (Input.GetKeyUp(playerControles[4])) { abilityArray[3].AfterCast(); } // spell_3

        // --- [ virgo stun ] ---
        // check if player is faceing a wall
        if (controller.collisions.left || controller.collisions.right)
        {
            // ckeck if player is knockBacked from a virgo spell
            if (playerVitals.KnockBacked && playerVitals.KnockBackSpell.ID == 6)
            {
                // transfer knockBackSpell to virgoSpell
                Virgo virgoSpell = (Virgo)playerVitals.KnockBackSpell;
                // stun target
                playerVitals.ApplyStun(virgoSpell.StunTime, playerVitals.KnockBackSpell);
            }
        }

        // Get movement input ( controler / keyboard )
        input = new Vector2(Input.GetAxisRaw(playerAxis + "_Horizontal"), Input.GetAxisRaw(playerAxis + "_Vertical"));
        _animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw(playerAxis + "_Horizontal")));

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

        #region use meele abilities
        // check if a meelespell was used
        if (castedMeeleSpell != null)
        {
            // disable attacking and flipping
            isAttacking = true;
            flipEnable = false;

            // check in how many directions the spell can be aimed
            castedMeeleSpell.SpellDirection = Util.GetAimDirection(castedMeeleSpell, this);

            // cast spell
            UseMeleeAttack();
        }

        else
        {
            // enable attacking and flipping
            if (isAttacking) { isAttacking = false; }
            if (!flipEnable) { flipEnable = true; }
        }
        #endregion
    }

    void Movement()
    {
        // checkif player can move
        if (!disabled)
        {
            // --- [ horizontal movement ] ---
            // - get current movementspeed
            float targetVelocityX = input.x * playerVitals.MoveSpeed;
            // - move player
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

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
    
    void Flip()
    {
        mirror = !mirror;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    void UseMeleeAttack()
    {
        // only continue if spell is still active (spell needs time to reach full range)
        if (castedMeeleSpell.ShallTravel)
        {
            // get travel distance
            float travelDistance = castedMeeleSpell.TravelDistance();

            // create a Debug ray
            Debug.DrawRay(gameObject.transform.position, castedMeeleSpell.SpellDirection * travelDistance, Color.green);

            // create a raycast
            foreach (RaycastHit2D objectHit in Physics2D.RaycastAll(gameObject.transform.position, castedMeeleSpell.SpellDirection, travelDistance))
            {
                // check if hitted object is a Player
                if (objectHit.transform.tag == "Player")
                {
                    // only continue if spell didn't hit the spell caster
                    if (objectHit.transform.gameObject != gameObject)
                    {
                        // only continue if enemy isn't registered previorsly (one anemy shall only be hitted once)
                        if (!Util.IsGameObjectIncluded(castedMeeleSpell.HitTargets, objectHit.transform.gameObject))
                        {
                            // register enemy
                            Util.IncludeGameObject(castedMeeleSpell.HitTargets, objectHit.transform.gameObject);

                            // checks if the spell already hit its max of players
                            if (!castedMeeleSpell.MaxTargetsReached())
                            {
                                CustomDebug.Log("<b><color=white>" + castedMeeleSpell.Name + "</color></b> hit <b>" + objectHit.transform.gameObject.GetComponent<Player>().playerVitals.Name + "</b>", "Spells");

                                // add player hit
                                castedMeeleSpell.PlayersHit++;

                                // use spell
                                castedMeeleSpell.Use(objectHit.transform.gameObject, gameObject);
                                break;
                            }
                        }
                    }
                }

            }
        }

        // stop attack
        else
        {
            castedMeeleSpell.HitTargets = new GameObject[4];
            castedMeeleSpell.ResetPlayersHit();
            castedMeeleSpell.ShallTravel = true;
            castedMeeleSpell = null;
        }
    }

    public void PickupOrb(Attacks attack)
    {
        if (attack != null)
        {
            Attacks firstSpell = abilityArray[1];
            Attacks secondSpell = abilityArray[2];

            for (int i = 0; i < abilityArray.Length; i++)
            {
                if (abilityArray[i].Name == attack.Name)
                {
                    print("Same");
                    abilityArray[i] = attack;
                    CustomDebug.LogArray(abilityArray);
                    return;
                }
            }

            for (int i = 0; i < abilityArray.Length; i++)
            {
                if (abilityArray[i].Name != attack.Name)
                {
                    abilityArray[1] = attack;
                    abilityArray[2] = firstSpell;
                    abilityArray[3] = secondSpell;
                    CustomDebug.LogArray(abilityArray);
                    return;
                }
            }

            
        }
    }

    public void FireSkillShot(Attacks _spell, GameObject _bullet)
    {
        // check if bullet is flound
        if (_bullet != null)
        {
            // instantiate bullet
            GameObject bulletInstance = Instantiate(_bullet, transform.position, transform.rotation) as GameObject;

            // check in how many directions the bullet can be shot
            bulletInstance.GetComponent<SpellBullet>().spellDir = Util.GetAimDirection(_spell, this);

            // pass over bullet speed
            bulletInstance.GetComponent<SpellBullet>().usedSpell = _spell;

            // pass over caster
            bulletInstance.GetComponent<SpellBullet>().caster = gameObject;

            // destroy bullet, if still existing, after secounds 
            try { Destroy(bulletInstance, 4); }
            catch (System.NullReferenceException) { }
        }

        // send error log if bullet is not found
        else { Debug.LogError(_spell.Name + "bullet not found"); }
    }

    public GameObject GetCapricorn2Targets()
    {
        float range = AbilityManager.Instance.spells[10].knockBackRange;
        GameObject nearestPlayer = null;
        float distanceToPlayer = range;

        // get all player objects
        foreach (GameObject targetObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            // get kocked up players
            if (targetObject.GetComponent<Player>().playerVitals.KnockUped)
            {
                // get the distance to the knock uped player
                float distance = Mathf.Abs(Vector2.Distance(transform.position, targetObject.transform.position));

                // compair if player is in range & get nearest player
                if (distanceToPlayer >= distance)
                {
                    nearestPlayer = targetObject;
                    distanceToPlayer = distance;
                }
            }
        }
        return nearestPlayer;
    }

    public void Die()
    {

    }
}
