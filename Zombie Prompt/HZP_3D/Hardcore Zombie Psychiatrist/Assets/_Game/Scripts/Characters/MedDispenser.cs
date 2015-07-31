using UnityEngine;
using UnityEngine.UI;

public class MedDispenser : MonoBehaviour, IUseEventHandler 
{

	public Slider rechargeSlider;
	public float timeToRecharge = 3;
	public int medsToDispense = 1;
	public float useDistance = 2;
	public AudioClip useAudioClip;
	public string useMessage = "Got Meds!";

	private AudioSource m_audioSource = null;
	private Player m_player = null;
	private Targetable m_targetable = null;
	private float m_maxUsableDistance = 0;
	private float m_timeElapsed = 0;

	public bool IsRecharged { get { return m_timeElapsed >= timeToRecharge; } }
	public bool IsRecharging { get { return !IsRecharged; } }
	
	private void Awake()
	{
		m_audioSource = GetComponent<AudioSource>();
		m_player = FindObjectOfType<Player>();
		m_targetable = GetComponent<Targetable>();
		m_maxUsableDistance = (m_targetable == null) ? 0 : m_targetable.maxUsableDistance;
		if (m_targetable == null) enabled = false;
	}
	
	private void Update()
	{
		m_targetable.maxUsableDistance = IsRecharged ? m_maxUsableDistance : 0;
		if (IsRecharging) 
		{
			m_timeElapsed += Time.deltaTime;
			rechargeSlider.value = Mathf.Clamp (m_timeElapsed / timeToRecharge, 0, 1);
		}
	}
	
	public void OnUse() {
		if (IsRecharged)
		{
			m_timeElapsed = 0;
			HUD.ShowMessage(useMessage);
			if (m_player != null) m_player.AddMeds(medsToDispense);
			if ((m_audioSource != null) && (useAudioClip != null)) m_audioSource.PlayOneShot (useAudioClip);
		}
	}
}
