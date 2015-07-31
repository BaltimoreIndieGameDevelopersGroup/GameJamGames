using UnityEngine;
using System.Collections;

public class SensorTrigger : MonoBehaviour 
{

	public Transform reportTo;
	public string enterMethod = "OnTriggerEnter";
	public string exitMethod = "OnTriggerExit";
	public bool debug = false;

	public void Start()
	{
		if (reportTo == null) reportTo = transform.parent;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (debug) Debug.Log (reportTo.name + "." + enterMethod + ": " + other.name);
		if (reportTo != null) reportTo.SendMessage(enterMethod, other.transform, SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.transform.parent == null) return;
		if (debug) Debug.Log (reportTo.name + "." + exitMethod + ": " + other.name);
		if (reportTo != null) reportTo.SendMessage(exitMethod, other.transform, SendMessageOptions.DontRequireReceiver);
	}

/*
	void OnTriggerEnter(Collider other) 
	{
		if (other.transform.parent == null) return;
		if (debug) Debug.Log (reportTo.name + ".OnTriggerEnter: " + other.transform.parent.name);
		if (reportTo != null) reportTo.SendMessage(enterMethod, other.transform.parent);
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.transform.parent == null) return;
		if (debug) Debug.Log (reportTo.name + ".OnTriggerExit: " + other.transform.parent.name);
		if (reportTo != null) reportTo.SendMessage(exitMethod, other.transform.parent);
	}

*/
}
