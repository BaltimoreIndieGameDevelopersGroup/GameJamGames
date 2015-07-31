using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	public int numMeds = 0;
	public int maxMeds = 5;

	public void AddMeds(int amount) {
		numMeds = Mathf.Min(maxMeds, numMeds + amount);
		HUD.SetMedsText(numMeds);
	}

	public void RemoveMeds(int amount) {
		numMeds -= amount;
		HUD.SetMedsText(numMeds);
	}
}
