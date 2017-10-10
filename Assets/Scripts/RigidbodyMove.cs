using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// put this script on your player Cube
public class RigidbodyMove : MonoBehaviour {

	Rigidbody myRigidbody;
	public GameObject mainCamera;
	GameObject carriedObject;
	Vector3 inputVector;

	float speed = 2f;
	bool carrying;
	public float distance;
	public float smooth;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody>(); // assign reference to RB in Start
		mainCamera = GameObject.FindWithTag("MainCamera"); 
	}

	void Carry(GameObject o) {
		o.transform.position = Vector3.Lerp(o.transform.position,  mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime  * smooth) ;
		//o.transform.position = mainCamera.transform.position + mainCamera.transform.forward;
	}



	void Pickup() {

		if (Input.GetKeyDown (KeyCode.E)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera> ().ScreenPointToRay (new Vector3 (x, y));
			Debug.DrawRay (ray.origin, ray.direction, Color.magenta);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				Pickupable p = hit.collider.GetComponent<Pickupable> ();

				if (p != null) {
					Debug.Log ("PICKED UP " + p);
					carrying = true;
					carriedObject = p.gameObject;
					p.GetComponent<Rigidbody> ().isKinematic = true;

				}


			}


		}

	}

	void checkDrop () {
		if (Input.GetKeyDown(KeyCode.E)) {
			DropObject ();

		}

	}

	void DropObject() {
		carrying = false;
		carriedObject.GetComponent<Rigidbody> ().isKinematic = false;
		carriedObject = null;


	}

	// regular Update is called once per frame, where we do rendering + input
	void Update () {
		// horizontal input is A/D, LeftArrow/RightArrow
		float inputHorizontal = Input.GetAxis( "Horizontal" );
		// vertical input is W/S, UpArrow/DownArrow
		float inputVertical = Input.GetAxis( "Vertical" );

		// rotate the cube
		transform.Rotate( 0f, inputHorizontal * Time.deltaTime * 90f, 0f );

		// put our input values into an "input vector"
		inputVector = new Vector3( 0f, 0f, inputVertical );


		if (Input.GetMouseButtonDown (0) && carriedObject != null && carriedObject.gameObject.name == "disc") {

			// STEP 1: define the Ray; based on Camera perspective
			Ray shootRay = new Ray( Camera.main.transform.position, Camera.main.transform.forward);

			// STEP 2: define a maximum distance for the Raycast
			float maxRayDistance = 100f;

			// STEP 3: visualize the Ray
			Debug.DrawRay( shootRay.origin, shootRay.direction, Color.yellow);

			// STEP 3.5: define a RaycastHit variable to remember what we hit
			RaycastHit shootRayHit = new RaycastHit();

			// STEP 4: let's shoot the Raycast!!!
			if(Physics.Raycast(shootRay, out shootRayHit, maxRayDistance ) ) {
				if (shootRayHit.transform.gameObject.name == "iMac") {
					carriedObject.GetComponent<DiscController> ().DiscEnter ();
					carriedObject.GetComponent<DiscController> ().discEntered = true;
					carriedObject = null;
					carrying = false;
					//shootRayHit.transform.gameObject.GetComponent<DiscController> ().DiscEnter ();
					//shootRayHit.transform.gameObject.GetComponent<DiscController> ().discEntered = true;

				}
				
			}


		}
			

		// normalize inputVector if magnitude > 1f, to avoid diagonal movement exploit
		if( inputVector.magnitude > 1f ) {
			inputVector = Vector3.Normalize( inputVector );
		}

		if (carrying) {
			Carry (carriedObject);
			checkDrop ();

		} else {
			Pickup  ();
		}


	}

	// FixedUpdate runs at a "Fixed" framerate, which is when physics run
	void FixedUpdate () {
		// both of these lines of code do basically the same thing
		myRigidbody.AddForce( transform.TransformDirection(inputVector) * speed);
		myRigidbody.AddRelativeForce( inputVector * 25f );
	}

}