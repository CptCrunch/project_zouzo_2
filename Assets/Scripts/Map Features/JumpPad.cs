using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour {

    Controller2D controler;

	// Use this for initialization
	void Start () {
        controler = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controler.collisions.above) {
            Debug.Log("hey");
        }
	}
}
