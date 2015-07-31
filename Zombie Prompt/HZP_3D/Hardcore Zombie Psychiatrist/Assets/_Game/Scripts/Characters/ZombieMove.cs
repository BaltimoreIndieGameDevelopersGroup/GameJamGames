// AgentPatrol.cs
using UnityEngine;
using System.Collections;


public class ZombieMove : MonoBehaviour {
	
	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	public State state;
	private GameObject patient;

	private float wanderSpeed;

	public enum State
	{
		WANDER,
		CHASE,
		EAT,
		SLEEP
	}
	
	
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		
		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;

		wanderSpeed = agent.speed / 2;

		Wander();
	}
	
	
	void GotoNextPoint() 
	{
		// Returns if no points have been set up
		if (points.Length == 0)
			return;
		
		// Set the agent to go to the currently selected destination.
		agent.destination = points [destPoint].position;
		
		// Choose the next point in the array as the destination
		int nextPoint;
		do {
			float rand = Random.value;
			while (rand == 1.0) {
				rand = Random.value;
			}
			nextPoint = Mathf.FloorToInt (rand * points.Length);
		} while (nextPoint == destPoint);
		destPoint = nextPoint;
	
	}
	
	void Update () {

		if (agent == null) {
			return;
		}
		if (state == State.CHASE) {
			if (patient != null) {
				agent.destination = patient.transform.position;
			}
		} 
		else if (state == State.EAT || state == State.SLEEP) {
			agent.Stop();
		}
		else if (state == State.WANDER && agent.remainingDistance < 0.5f)
		{
			// Choose the next destination point when the agent gets
			// close to the current one.
			GotoNextPoint();
		}
	}

	// Pre-integration test code:
	//
	//void OnTriggerEnter(Collider other)
	//{
	//	if (other.CompareTag ("Patient")) {
	//		Chase(other.gameObject);
	//	}
	//}
	//
	//void OnTriggerExit(Collider other)
	//{
	//	Wander();
	//}

	void Wander()
	{
		state = State.WANDER;
		agent.speed = wanderSpeed;
		agent.Resume();
	}

	void Chase(GameObject other)
	{
		state = State.CHASE;
		patient = other;
		agent.speed = wanderSpeed * 2;
	}

	void Eat(GameObject other)
	{
		state = State.EAT;
		patient = other;
	}

	void Sleep()
	{
		state = State.SLEEP;
	}
}
