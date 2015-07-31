using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PLAYTHEGAMEEEEEE : MonoBehaviour {

	public Image splash;
	public Text showScore;
	public ScoreController score;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		showScore.text = score.GetScore().ToString();
	}

	public void playTHEGAAAAAME(){
		splash.enabled = false;
		Time.timeScale = 1;
	}

	public void playAgain(){
		Application.LoadLevel (Application.loadedLevelName);
	}
}
