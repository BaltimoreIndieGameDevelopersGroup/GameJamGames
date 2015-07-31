using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{

	public string startMessage = "Start!";
	public string winLevelMessage = "LEVEL FINISHED!";
	public float winLevelMessageDuration = 3;
	public string loseLevelMessage = "Oh No! A zombie ate a patient!";
	public float loseLevelMessageDuration = 5;
	public int numPatients = 0;
	
	private static LevelManager m_instance = null;
	
	private void Awake()
	{
		m_instance = this;
	}
	
	private void Start()
	{
		HUD.ShowMessage(startMessage);
	}

	public static void AddPatient()
	{
		m_instance.numPatients++;
		HUD.SetPatientsText(m_instance.numPatients);
	}
	
	public static void RemovePatient(bool byPlayer)
	{
		m_instance.numPatients--;
		HUD.SetPatientsText(m_instance.numPatients);
		if (byPlayer && m_instance.numPatients <= 0) 
		{
			m_instance.StartCoroutine(m_instance.WinLevel());
		}
	}
	
	public static void ZombieAtePatient()
	{
		m_instance.StartCoroutine(m_instance.LoseLevel());
	}
	
	public IEnumerator WinLevel()
	{
		HUD.ShowMessage(winLevelMessage);
		yield return new WaitForSeconds(winLevelMessageDuration);
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	public IEnumerator LoseLevel()
	{
		HUD.ShowMessage(loseLevelMessage);
		yield return new WaitForSeconds(loseLevelMessageDuration);
		Application.LoadLevel(Application.loadedLevel);
	}
	
}
