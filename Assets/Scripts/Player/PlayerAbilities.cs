﻿using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour {

    private Player playerScrpt;
    public Attacks[] abilityArray = new Attacks[4];
    private bool isAttacking;
    public Attacks castedMeeleSpell;
    public Attacks startedAbility;

    private int spellToSwitch = 1;

    public bool attackEnable = true;

    void Awake()
    {
        playerScrpt = gameObject.GetComponent<Player>();
    }

    void Start()
    {
        // --- [ set starter abilities ] ---
        abilityArray[0] = AbilityManager.Instance.CreateBasic(gameObject);          // basic
        /*
            abilityArray[1] = AbilityManager.Instance.CreateSaggitarius(gameObject);    // spell_1
            abilityArray[2] = AbilityManager.Instance.CreateCapricorn(gameObject);      // spell_2
            abilityArray[3] = AbilityManager.Instance.CreateVirgo(gameObject);          // spell_3
        */
    }

    void Update()
    {
        // --- [ update Cooldowns / timeBetweenCasts ] ---
        foreach (Attacks _spell in abilityArray)
        {
            // check if spell isn't null
            if (_spell != null)
            {
                // update cooldown
                _spell.UpdateCooldowns();
                // update timeBetweenCasts
                if (_spell.IsCast) { _spell.TimeBeteewnCasts += Time.deltaTime; }
            }
        }

        // --- [ use ability ] ---
        // start spell (on button pressed)
        if (attackEnable)
        {
            if (abilityArray[0] != null) { if (Input.GetKeyDown(playerScrpt.playerControles[1])) { startedAbility = abilityArray[0]; abilityArray[0].StartSpell(); } } // basic
            if (abilityArray[1] != null) { if (Input.GetKeyDown(playerScrpt.playerControles[2])) { startedAbility = abilityArray[1]; abilityArray[1].StartSpell(); } }// spell_1
            if (abilityArray[2] != null) { if (Input.GetKeyDown(playerScrpt.playerControles[3])) { startedAbility = abilityArray[2]; abilityArray[2].StartSpell(); } } // spell_2
            if (abilityArray[3] != null) { if (Input.GetKeyDown(playerScrpt.playerControles[4])) { startedAbility = abilityArray[3]; abilityArray[3].StartSpell(); } } // spell_3
        }

        // use aftercast (on button released)
        if (abilityArray[0] != null) { if (Input.GetKeyUp(playerScrpt.playerControles[1])) { abilityArray[0].AfterCast(); } } // basic
        if (abilityArray[1] != null) { if (Input.GetKeyUp(playerScrpt.playerControles[2])) { abilityArray[1].AfterCast(); } } // spell_1
        if (abilityArray[2] != null) { if (Input.GetKeyUp(playerScrpt.playerControles[3])) { abilityArray[2].AfterCast(); } } // spell_2
        if (abilityArray[3] != null) { if (Input.GetKeyUp(playerScrpt.playerControles[4])) { abilityArray[3].AfterCast(); } } // spell_3

        // --- [ virgo stun ] ---
        // check if player is faceing a wall
        if (playerScrpt.controller.collisions.left || playerScrpt.controller.collisions.right)
        {
            // ckeck if player is knockBacked from a virgo spell
            if (playerScrpt.playerVitals.KnockBacked && playerScrpt.playerVitals.KnockBackSpell.ID == 6)
            {
                // transfer knockBackSpell to virgoSpell
                Virgo virgoSpell = (Virgo)playerScrpt.playerVitals.KnockBackSpell;
                // stun target
                playerScrpt.playerVitals.ApplyStun(virgoSpell.StunTime, playerScrpt.playerVitals.KnockBackSpell);
            }
        }

        // --- [ virgo animation stop ] ---
        // get all of the players abilities
        foreach (Attacks ability in abilityArray)
        {
            // check if entry is empty
            if (ability != null)
            {
                // check if the ability is a virgo spell
                if (ability.ID == 6)
                {
                    if (!ability.IsStarted && !playerScrpt.playerVitals.VirgoDash)
                    {
                        gameObject.GetComponent<Animator>().SetBool("Virgo", false);
                    }
                }

                // --- [ sagittarius cast animation ] ---
                // check if the ability is a sagittarius spell
                if (ability.ID == 9)
                {
                    // check if sagittarius spell is casting
                    if (/*ability.IsCast &&*/ gameObject.GetComponent<Animator>().GetBool("SagittariusActive"))
                    {
                        // get aim
                        switch (Util.Aim8Direction(new Vector2(Input.GetAxis(playerScrpt.playerAxis + "_Vertical"), Input.GetAxis(playerScrpt.playerAxis + "_Horizontal"))))
                        {
                            case "up":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 1);
                                break;
                            case "upRight":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 2);
                                break;
                            case "right":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 3);
                                break;
                            case "downRight":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 4);
                                break;
                            case "down":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 5);
                                break;
                            case "downLeft":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 4);
                                break;
                            case "left":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 3);
                                break;
                            case "upLeft":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 2);
                                break;
                            case "noAim":
                                gameObject.GetComponent<Animator>().SetInteger("SagittariusDir", 3);
                                break;
                        }
                    }
                }

                // --- [ capricorn type ] ---
                // check if the ability is a capricorn spell
                if (ability.ID == 10)
                {
                    if (GetCapricorn2Targets() != null)
                    {
                        if (ability.SpellCount != 1) { ability.SpellCount = 1; }
                    }

                    else
                    {
                        if (ability.SpellCount != 0) { ability.SpellCount = 0; }
                    }
                }
            }
        }

        #region use meele abilities
        // check if a meelespell was used
        if (castedMeeleSpell != null)
        {
            // disable attacking and flipping
            isAttacking = true;
            playerScrpt.flipEnable = false;

            // check in how many directions the spell can be aimed
            castedMeeleSpell.SpellDirection = Util.GetAimDirection(castedMeeleSpell, playerScrpt);

            // cast spell
            UseMeleeAttack();
        }

        else
        {
            // enable attacking and flipping
            if (isAttacking) { isAttacking = false; }
            if (!playerScrpt.flipEnable) { playerScrpt.flipEnable = true; }
        }
        #endregion
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
                                castedMeeleSpell.Use(objectHit.transform.gameObject);
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

    public void SwitchSpell(string _spellName)
    {
        bool isInInventory = false;
        foreach (Attacks spell in abilityArray) { if (spell != null) { if (_spellName == spell.Name) { isInInventory = true; } } }

        if (!isInInventory)
        {
            switch (_spellName)
            {
                case "Aries": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateAries(gameObject); break;
                case "Taurus": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateTaurus(gameObject); break;
                case "Gemini": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateGemini(gameObject); break;
                case "Cancer": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateCancer(gameObject); break;
                case "Leo": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateLeo(gameObject); break;
                case "Virgo": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateVirgo(gameObject); break;
                case "Libra": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateLibra(gameObject); break;
                case "Scorpio": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateScorpio(gameObject); break;
                case "Sagittarius": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateSaggitarius(gameObject); break;
                case "Capricorn": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateCapricorn(gameObject); break;
                case "Aquarius": abilityArray[spellToSwitch] = AbilityManager.Instance.CreateAquarius(gameObject); break;
                case "Pisces": abilityArray[spellToSwitch] = AbilityManager.Instance.CreatePisces(gameObject); break;
                default: Debug.LogError(_spellName + " is not a accepted value for 'SwitchSpell'"); break;
            }
        }

        spellToSwitch++;
        if (spellToSwitch > 3) { spellToSwitch = 1; }
    }

    public void FireSkillShot(Attacks _spell, GameObject _bullet)
    {
        // check if bullet is flound
        if (_bullet != null)
        {
            // instantiate bullet
            GameObject bulletInstance = Instantiate(_bullet, transform.position, transform.rotation) as GameObject;

            // check in how many directions the bullet can be shot
            bulletInstance.GetComponent<SpellBullet>().spellDir = Util.GetAimDirection(_spell, playerScrpt);

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

    public void CastSpell() { startedAbility.Cast(); startedAbility = null; }

    public void EnaleAttack() { attackEnable = true; }
}
