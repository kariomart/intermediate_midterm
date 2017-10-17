using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscController : MonoBehaviour {

	public GameObject player;
	public GameObject iMac;
	public GameObject bugdomIcon;
	public GameObject UIText;
	public GameObject perspectiveCamera;
	public GameObject orthoCamera;
	public BoxCollider boxCollider;
	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public bool discEntered;
	public bool pickedUp;


	// Use this for initialization
	void Start () {
		
		boxCollider = GetComponent<BoxCollider> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (discEntered) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (startMarker.position, endMarker.position, fracJourney);

			//Debug.Log (fracJourney);
			if (fracJourney >= 1) {
				perspectiveCamera.gameObject.SetActive (false);
				orthoCamera.gameObject.SetActive (true);
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				player.GetComponent<RigidbodyMove> ().macView = true;
				player.GetComponent<RigidbodyMove> ().discInserted = true;
				bugdomIcon.SetActive (true);
				//UIText.SetActive (true);

				Destroy (this.gameObject);
			}
		}


		
	}

	public void DiscEnter () {

		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
		transform.eulerAngles = new Vector3 (90, 0, 0);

	}
}
