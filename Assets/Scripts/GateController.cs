using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

	public RigidbodyMove player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		bool inRange = (player.transform.position - transform.position).magnitude < 10f;

		if (inRange && player.carrying && player.carriedObject.name == "key") {
			Destroy (this.gameObject);
			player.DropObject ();

		}
		
	}
}
