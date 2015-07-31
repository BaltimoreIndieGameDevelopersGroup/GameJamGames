using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleMenu : MonoBehaviour {

	public Text hardcore;
	public Text zombie;
	public Text psychiatrist;
	public AudioSource audioSource;
	public AudioClip wordClip;
	public AudioClip finalClip;
	
	// Use this for initialization
	IEnumerator Start () {
		zombie.gameObject.SetActive(false);
		psychiatrist.gameObject.SetActive(false);
		audioSource.PlayOneShot(wordClip);
		yield return new WaitForSeconds(1);
		zombie.gameObject.SetActive(true);
		audioSource.PlayOneShot(wordClip);
		yield return new WaitForSeconds(1);
		psychiatrist.gameObject.SetActive(true);
		audioSource.PlayOneShot(finalClip);
	}

	public void OnClick() {
		Application.LoadLevel(Application.loadedLevel + 1);	
	}
}
