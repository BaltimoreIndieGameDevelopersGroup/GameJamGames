// AgentPatrol.cs
using UnityEngine;
using System.Collections;


public class PatientMove : MonoBehaviour {
	
	public Transform[] points;
	public float fleeBurstTime; //time the patient should be in a speed burst

	private int destPoint = 0;
	private NavMeshAgent agent;
	private State state;
	private GameObject zombie;
	public float timeSinceFlee;

	
	private enum State
	{
		WANDER,
		FLEE
	}
	
	
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		
		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;

		destPoint = 0;
		GotoNextPoint();
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
		//do {
			float rand = Random.value;
			while (rand == 1.0) {
				rand = Random.value;
			}
		    nextPoint = Mathf.FloorToInt (rand * points.Length);
		//} while (nextPoint == destPoint);
		destPoint = nextPoint;

		
	}
	
	void Update () {
		
		if (state == State.FLEE) {


			timeSinceFlee += Time.deltaTime;
			if (timeSinceFlee > fleeBurstTime)
			{
				agent.speed /= 2.0f;
			}
			Wander();
		} 
		else if (state == State.WANDER)
		{
			if (agent.remainingDistance < 0.5f)
			{
				GotoNextPoint();
			}


		}
	}

	// Pre-integration test code:
	//
	//void OnTriggerEnter(Collider other)
	//{
	//	if (other.CompareTag ("Zombie")) {
	//		Flee(other.gameObject);
	//	}
	//}
	
	//void OnTriggerExit(Collider other)
	//{
	//	Wander();
	//}
	
	void Wander()
	{
		state = State.WANDER;
	}
	
	void Flee(GameObject other)
	{
		state = State.FLEE;
		zombie = other;
		timeSinceFlee = 0.0f;
		agent.speed *= 2.0f;

		double min = 2.0; //anything greater than 1 is fine here
		int minIndex = -1; //index of the point
		
		//figure out the waypoint which is in the "most" opposite direction from the zombie
		// To do this, find waypoint with the minimum dot product with the vector to the zombie
		//since we don't care about up and down (y-axis), use 2-D vectors
		Vector3 temp1 = zombie.transform.position - agent.transform.position;
		Vector2 towardZombie = new Vector2(temp1.x, temp1.z);
		towardZombie.Normalize();

		for (int i = 0; i < points.Length; i++)
		{
			
			Vector3 temp2 = points[i].position - agent.transform.position;
			Vector2 towardWaypoint = new Vector2(temp2.x, temp2.z);
			towardWaypoint.Normalize();
			
			double dotProduct = Vector2.Dot(towardZombie, towardWaypoint);
			if (dotProduct < min)
			{
				min = dotProduct;
				minIndex = i;
			}
		}
		agent.destination = points[minIndex].position;
		
	}
}
