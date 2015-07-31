using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

	private int score;
	public int pointsPerEnemy;
	public float comboTimerSeconds;
	public int comboBonusPoints;
	private float comboTimerCountdown;
	private bool isComboBonusActive;

	// Use this for initialization
	void Start () {
		score = 0;
		isComboBonusActive = false;
		comboTimerCountdown = comboTimerSeconds;
	}
	
	// Update is called once per frame
	void Update () {
		if (isComboBonusActive){
			comboTimerCountdown -= Time.deltaTime;
		}
		
		if (comboTimerCountdown <= 0){
			isComboBonusActive = false;
			comboTimerCountdown = comboTimerSeconds;
		}
	}

	// Pass in positive score to increase, negative to decrease.
	public void UpdateScore(int points) {

		if (isComboBonusActive) {
			points += comboBonusPoints;
		} else {
			ActivateComboTimer();
		}

		score += points;
		Debug.Log (score);
	}

	public int GetScore() {
		return score;	
	}

	private void ActivateComboTimer() {
		isComboBonusActive = true;
	}

	

}
