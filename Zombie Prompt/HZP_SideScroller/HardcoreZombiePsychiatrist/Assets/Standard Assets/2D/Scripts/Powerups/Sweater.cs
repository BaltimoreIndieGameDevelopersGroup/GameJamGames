using UnityEngine;
using System.Collections;

public class Sweater : PowerUp {

	private bool activated = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivatePowerUp() {
		activated = true;
	}

	public void DeactivatePowerUp() {
		activated = false;
	}

	public bool IsActive() {
		return activated;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.tag == "Player") {
			ActivatePowerUp();
			Destroy(gameObject);
		}
	}
}
