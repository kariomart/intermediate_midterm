using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquirrelController : MonoBehaviour {

	public RigidbodyMove player;
	public GameObject acorn;
	public GameObject key;
	public Text NPCtext;
	bool droppedKey;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool inRange = (player.transform.position - transform.position).magnitude < 10f;

		//Debug.Log ((player.transform.position - transform.position).magnitude);

		if (inRange && player.carrying && player.carriedObject.name == "acorn" && !droppedKey) {
				DropKey ();
				droppedKey = true;
				player.DropObject ();
				Destroy (acorn);

		}

		else if(inRange && droppedKey) {
			NPCtext.text = "Thank you! Here's your key!";
		}

		else if(inRange) {
			NPCtext.text = "Could you find me my acorn? I'll give you a key for it!!";
		}
		else{
			NPCtext.text = "";
		}
		
	}

	void DropKey() {

		GameObject temp = Instantiate (key, new Vector3(transform.position.x + 3f, transform.position.y + 3f, transform.position.z), Quaternion.identity);
		temp.gameObject.name = "key";


	}

}
