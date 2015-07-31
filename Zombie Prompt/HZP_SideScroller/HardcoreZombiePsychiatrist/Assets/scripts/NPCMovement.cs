using UnityEngine;
using System.Collections;

public class NPCMovement: MonoBehaviour {
	private Rigidbody2D m_RigidBody2D;
	public float speed = 1.0f;
	public float direction = -1.0f;

	public AudioSource munchSound;
	

	public GameObject scoreObject;
	public ScoreController scoreController;

	private void Start() {
		m_RigidBody2D = GetComponent<Rigidbody2D>();
		scoreObject = GameObject.Find ("Score");
		scoreController = scoreObject.GetComponent<ScoreController> ();
	}
	
	private void FixedUpdate() {
		m_RigidBody2D.velocity = new Vector2 (speed * direction, 0f);
	}
	
	private void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Ledge"){
		Flip ();
		}
	}
	
	private void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "Player") {
			Flip ();
		}
	}
	
	private void Flip() {
		direction *= -1.0f;
	}

	private void OnDestroy() {
		var points = scoreController.pointsPerEnemy;
		scoreController.UpdateScore(points);
		munchSound.Play ();
	}
}


