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