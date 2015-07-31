using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour {

	public PowerUps powerUps;

	public bool invulnerable;

	public Button playAgain;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D coll) {

			if(coll.gameObject.tag == "crazyEnemy") {
			//Destroy (gameObject);

				if (powerUps.sweaterEnabled == true){
				Debug.Log ("Sweater is ded, rip");
				powerUps.sweaterEnabled = false;
				invulnerable = true;
				powerUps.InvulCountDown ();
				}
				
				if (invulnerable == false){
				Destroy (gameObject);
			}
		}

			if(coll.gameObject.tag == "HealthyEnemy"){
					Debug.Log ("touched healthy enemy's butt");
					Destroy(coll.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D trig){
		if(trig.gameObject.tag == "sweater"){
			powerUps.sweaterEnabled = true;
			Destroy (trig.gameObject);
			Debug.Log ("farts");
		}
	}

	void OnDestroy() {
		playAgain.enabled = true;
	}

}