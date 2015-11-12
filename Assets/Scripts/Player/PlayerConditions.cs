using UnityEngine;
using System.Collections;

public class PlayerConditions : MonoBehaviour {

    private Player _instance;

    private float slowTime = 0;
    private float slowStrenght = 0;
    private float stunTime = 0;

    //Testing Conditions
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) { StartCoroutine(PlayerSlow(3, 2)); }
        if (Input.GetKeyDown(KeyCode.M)) { StartCoroutine(PlayerStun(4));  }
        if (Input.GetKeyDown(KeyCode.P)) { PlayerKnockUp(5.5f, 4.5f); }

        slowTime -= Time.deltaTime;
        stunTime -= Time.deltaTime;
    }

    void Awake() {
        _instance = gameObject.GetComponent<Player>();
    }

    public void PlayerKnockUp(float _yHeight, float _xRange) {
        if (_instance.controller.collisions.below) { // # would not do that
            //_instance.knockUp = true;
            //_instance.velocity.y += _yHeight * _instance.maxJumpHeight;
            //_instance.velocity.x = _xRange;
        }
        
        else {
            //_instance.knockUp = false;
        }
    }

    public IEnumerator PlayerSlow(int _ammount, float _time) {
        if (_ammount >= slowStrenght) {
            slowTime = _time;
            slowStrenght = _ammount;
        }
        float oldSpeed = _instance.moveSpeed;
        if (slowStrenght < oldSpeed) { _instance.moveSpeed -= slowStrenght; } else { _instance.moveSpeed = 1; }
        yield return new WaitForSeconds(slowTime);
        _instance.moveSpeed = oldSpeed;
    }

    public IEnumerator PlayerStun(float _time) {
        //_instance.stunned = true;
        yield return new WaitForSeconds(_time);
        //_instance.stunned = false;
    }

}
