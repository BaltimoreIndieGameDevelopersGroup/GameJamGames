using UnityEngine;
using System.Collections;

public class PowerUps : MonoBehaviour {

	public GameObject playerObject;
	private PlayerCollision playerCollision;

	public bool sweaterEnabled;
	public bool pillEnabled;

	private bool invulnCounting;
	private float invulnerableTimer;
	public float invulnerableTimerDefault;

	// Use this for initialization
	void Start () {
		sweaterEnabled = false;
		invulnerableTimer = invulnerableTimerDefault;
		playerCollision = playerObject.GetComponent<PlayerCollision>();
	}
	
	// Update is called once per frame
	void Update () {
	if (invulnCounting == true){
			invulnerableTimer -= Time.deltaTime;
			Debug.Log (invulnerableTimer);
		}

	if (invulnerableTimer <= 0){
			invulnCounting = false;
			invulnerableTimer = invulnerableTimerDefault;
			playerCollision.invulnerable = false;
		}
	}



	public void InvulCountDown(){
		invulnCounting = true;
	}	
}
