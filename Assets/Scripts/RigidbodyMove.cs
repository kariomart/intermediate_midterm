using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// put this script on your player Cube
public class RigidbodyMove : MonoBehaviour {

	Rigidbody myRigidbody;
	public GameObject mainCamera;
	public GameObject carriedObject;
	Vector3 inputVector;

	public GameObject escapeText;

	public GameObject perspectiveCam;
	public GameObject orthoCam;
	public GameObject room;
	public GameObject bugdom;
	public GameObject screen_on;
	public GameObject screen_off;
	public GameObject bugdomIcon;
	public GameObject appleMenu;

	float speed = 2f;
	public bool carrying;
	public bool macView = false;
	public bool discInserted;
	public float distance;
	public float smooth;
	public bool jumping;
	public float collisionCount;

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
					//carriedObject.GetComponent<DiscController> ().pickedUp = true;
					carriedObject.GetComponent<Collider> ().enabled = false;

				}


			}


		}

	}

	void checkDrop () {
		if (Input.GetKeyDown(KeyCode.E) && carrying) {
			DropObject ();

		}

	}

	public void DropObject() {
		Debug.Log ("Dropped " + carriedObject);
		carrying = false;
		carriedObject.GetComponent<Rigidbody> ().isKinematic = false;
		carriedObject.GetComponent<Collider> ().enabled = true;
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


		if (Input.GetMouseButtonDown(0)) {
			LeftClickRay ();
		}

		if (Input.GetKeyDown (KeyCode.Escape) && macView) {

			macView = false;
			perspectiveCam.SetActive (true);
			orthoCam.SetActive (false);


		}

		if (macView) { 
		escapeText.SetActive (true); 
		Cursor.visible = true; 
		Cursor.lockState = CursorLockMode.None; 
		} else { escapeText.SetActive (false); }
	
			

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


		if (collisionCount == 0) {

			jumping = true;
		} else {
			jumping = false;
		}

	}

	// FixedUpdate runs at a "Fixed" framerate, which is when physics run
	void FixedUpdate () {
//		Debug.Log (jumping);
		// both of these lines of code do basically the same thing
		if (!jumping) {
			myRigidbody.AddForce (transform.TransformDirection (inputVector) * speed);
			myRigidbody.AddRelativeForce (inputVector * 25f);
		}


		if (Input.GetKeyDown (KeyCode.Space)) {

			Debug.Log (transform.up);
			//myRigidbody.AddForce (transform.up * 10f, ForceMode.Impulse);
			myRigidbody.velocity = (transform.up * 10f);
			jumping = true;

		}


	}

	void OnCollisionEnter(Collision coll) {

		collisionCount++;

	}

	void OnCollisionExit(Collision coll) {

		collisionCount--;
	}


	void LeftClickRay () {
		
			Ray shootRay;

			// STEP 1: define the Ray; based on Camera perspective
			if (macView) {
				shootRay = new Ray (orthoCam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), orthoCam.transform.forward);

			} else {
				shootRay = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
			}
				

			float maxRayDistance = 15f;
			Debug.DrawRay(shootRay.origin, shootRay.direction, Color.yellow);
			RaycastHit shootRayHit = new RaycastHit();


		if (Physics.Raycast (shootRay, out shootRayHit, maxRayDistance)) {
			string name = shootRayHit.transform.gameObject.name;

				
			if (name == "iMac" && carrying) {

				EnterDisc ();
				
			}

			if (name == "iMac") {
					
				checkImac ();

			}
					

			if (name == "bugdom_icon" && macView) {

				launchBugdom ();

			}

			if (name == "switch") {
				Debug.Log ("switch pressed!!");
				restartMac ();

			}

			if (name == "apple_menu") {

				bugdom.SetActive (false);
				room.SetActive (true);
					

			}
		}

	}


	void EnterDisc() {

		carriedObject.GetComponent<DiscController> ().DiscEnter ();
		carriedObject.GetComponent<DiscController> ().discEntered = true;
		carriedObject = null;
		carrying = false;
		//macView = true;
		//discInserted = true;
		//shootRayHit.transform.gameObject.GetComponent<DiscController> ().DiscEnter ();
		//shootRayHit.transform.gameObject.GetComponent<DiscController> ().discEntered = true;

	}

	void checkImac() {

		perspectiveCam.gameObject.SetActive (false);
		orthoCam.gameObject.SetActive (true);
		macView = true;

	}

	void launchBugdom() {

		screen_on.SetActive(false);
		bugdomIcon.SetActive (false);
		screen_off.SetActive (true);
		bugdomIcon.SetActive (false);
		room.SetActive (false);
		bugdom.SetActive (true);

	}

	void restartMac() {
		screen_off.SetActive (false);
		screen_on.SetActive(true);
		appleMenu.SetActive (true);
	}

}