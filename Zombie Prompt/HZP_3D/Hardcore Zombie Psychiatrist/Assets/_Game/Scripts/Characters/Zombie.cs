using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Zombie : MonoBehaviour, IUseEventHandler
{

	public Slider rechargeSlider;
	public float sedateDuration = 10;
	public float useDistance = 2;
	public AudioClip useAudioClip;
	public string useMessage = "Zzzzz...";
	public float timeBetweenBites = 2;
	public AudioClip eatBrainsAudioClip;

	private Animator m_animator	= null;
	private AudioSource m_audioSource = null;
	private float m_timeElapsed = Mathf.Infinity;
	private float m_maxUseDistance = 0;
	private Targetable m_targetable = null;
	private Player m_player = null;
	private float m_nextBiteTime = 0;
	public Patient m_patient = null;
	private bool m_canEat = false;
	
	public bool IsSedate { get { return m_timeElapsed < sedateDuration; } }
	public bool IsAwake { get { return !IsSedate; } }
	
	void Awake()
	{
		m_animator = GetComponentInChildren<Animator>();
		m_audioSource = GetComponent<AudioSource>();
		m_player = FindObjectOfType<Player>();
		m_targetable = GetComponent<Targetable>();
		m_maxUseDistance = (m_targetable == null) ? 0 : m_targetable.maxUsableDistance;
		if (m_player == null || m_targetable == null) enabled = false;
	}
	
	void Update ()
	{
		if ((m_patient != null) && m_canEat && (Time.time > m_nextBiteTime)) BitePatient();
		m_targetable.maxUsableDistance = (m_player.numMeds <= 0) ? 0 : m_maxUseDistance;
		if (IsSedate) 
		{
			m_timeElapsed += Time.deltaTime;
			rechargeSlider.value = 1 - Mathf.Clamp (m_timeElapsed / sedateDuration, 0, 1);
			if (IsAwake) 
			{
				SendMessage("Wander", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public void OnUse() {
		if (IsAwake)
		{
			m_timeElapsed = 0;
			HUD.ShowMessage(useMessage);
			m_player.RemoveMeds(1);
			if ((m_audioSource != null) && (useAudioClip != null)) m_audioSource.PlayOneShot(useAudioClip);
			SendMessage("Sleep", SendMessageOptions.DontRequireReceiver);
        }
	}
	
	void BitePatient() 
	{
		transform.LookAt(m_patient.transform.position);
		m_animator.SetTrigger("Bite");
		if ((m_audioSource != null) && (eatBrainsAudioClip != null)) m_audioSource.PlayOneShot(eatBrainsAudioClip);
		if (m_patient.brainsLeft <= 1) LevelManager.ZombieAtePatient();
		m_patient.GetEaten(false);
		m_nextBiteTime = Time.time + timeBetweenBites;
	}
	
	void OnSeeEnter(Transform other) 
	{
		if (IsAwake && other.CompareTag("Patient") && m_patient == null)
		{
			//Debug.Log (name + ".see: " + other.name);
			m_patient = other.GetComponent<Patient>();
			SendMessage("Chase", m_patient.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnSeeExit(Transform other)
	{
		if (IsAwake && m_patient != null && other.gameObject == m_patient.gameObject) 
		{
			//Debug.Log (name + ".DON'T see: " + other.name);
			m_patient = null;
			SendMessage("Wander", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnBiteRangeEnter(Transform other)
	{
		if (other.GetComponent<Collider>().CompareTag("Patient"))
		{
			//Debug.Log (name + ".IN BITE RANGE: " + other.name);
			m_patient = other.GetComponent<Collider>().GetComponent<Patient> ();
			m_canEat = true;
		}
	}
	
	void OnBiteRangeExit(Transform other)
	{
		if ((m_patient != null) && (m_patient.gameObject == other.gameObject || m_patient.transform == other.transform.parent)) 
		{
			//Debug.Log ("Exiting bite range: " + other.name);
			m_patient = null;
			m_canEat = false;
		}
	}
	
}
