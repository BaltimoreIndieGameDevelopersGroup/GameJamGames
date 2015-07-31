using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Patient : MonoBehaviour, IUseEventHandler
{

	public Slider brainsSlider;
	public int brainsLeft = 5;
	public AudioClip screamAudioClip;
	public AudioClip eatAudioClip;
	public string useMessage = "CHOMP!";
	public GameObject zombiePrefab;
	public AudioClip turnIntoZombieAudioClip;
	
	private int m_initialBrains;
	private AudioSource m_audioSource = null;

	public bool IsAlive { get { return brainsLeft > 0; } }
	public bool IsDead { get { return !IsAlive; } }
	
	void Awake() {
		m_audioSource = GetComponent<AudioSource>();
		m_initialBrains = brainsLeft;
	}
	
	private void Start() 
	{
		LevelManager.AddPatient();
		SendMessage("Wander", SendMessageOptions.DontRequireReceiver);
	}

	public void OnUse() 
	{
		HUD.ShowMessage(useMessage);
		GetEaten(true);
	}
	
	public void GetEaten(bool byPlayer) 
	{
		if ((m_audioSource != null) && (eatAudioClip != null)) 
		{
			m_audioSource.PlayOneShot (eatAudioClip);
			m_audioSource.PlayOneShot (screamAudioClip);
		}
			
		brainsLeft--;
		brainsSlider.value = Mathf.Clamp((float) brainsLeft / (float) m_initialBrains, 0, 1);
		if (brainsLeft <= 0) TurnIntoZombie(byPlayer);
	}
	
	public void TurnIntoZombie(bool byPlayer) 
	{
		LevelManager.RemovePatient(byPlayer);
		if ((m_audioSource != null) && (turnIntoZombieAudioClip != null)) AudioSource.PlayClipAtPoint(turnIntoZombieAudioClip, transform.position);
		var zombie = Instantiate(zombiePrefab, transform.position, transform.rotation) as GameObject;
		zombie.name = "Zombie";
		zombie.SetActive(true);
		Destroy(gameObject);
	}
	
	void OnSeeEnter(Transform other) 
	{
		if (other.CompareTag("Zombie"))
		{
			//Debug.Log (name + ".see: " + other.name);
			SendMessage("Flee", other.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
	
}
