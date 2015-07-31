using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	public Text messageText;
	public float defaultMessageDuration = 5;
	public Text medsText;
	public Text numPatientsText;

	private static HUD m_instance = null;

	private void Awake()
	{
		m_instance = this;
	}

	private void Start()
	{
		messageText.gameObject.SetActive(false);
	}

	public static void ShowMessage(string message)
	{
		ShowMessage(message, m_instance.defaultMessageDuration);
	}

	public static void ShowMessage(string message, float duration)
	{
		m_instance.StopAllCoroutines();
		m_instance.StartCoroutine(m_instance.ShowMessageCoroutine(message, duration));
	}

	private IEnumerator ShowMessageCoroutine(string message, float duration)
	{
		messageText.text = message;
		messageText.gameObject.SetActive(true);
		yield return new WaitForSeconds(duration);
		messageText.gameObject.SetActive(false);
	}

	public static void SetMedsText(int amount)
	{
		m_instance.medsText.text = amount.ToString();
	}
	
	public static void SetPatientsText(int amount)
	{
		m_instance.numPatientsText.text = amount.ToString();
	}
}
