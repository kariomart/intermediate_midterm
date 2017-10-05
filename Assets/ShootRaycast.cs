using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRaycast : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {

			Ray shootRay = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
			float maxRayDistance = 100f;
			Debug.DrawRay (shootRay.origin, shootRay.direction, Color.magenta);

			RaycastHit shootRayHit = new RaycastHit ();

			if (Physics.Raycast (shootRay, out shootRayHit, maxRayDistance)) {
				Destroy (shootRayHit.transform.gameObject);

			}


		}

		Camera.main.transform.Rotate (Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal"), 0f);
		
	}
}
