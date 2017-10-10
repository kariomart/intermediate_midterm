using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscController : MonoBehaviour {

	public GameObject iMac;
	public GameObject UIText;
	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public bool discEntered;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {

		if (discEntered) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (startMarker.position, endMarker.position, fracJourney);

			//Debug.Log (fracJourney);
			if (fracJourney >= 1) {
				UIText.SetActive (true);
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
