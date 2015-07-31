using UnityEngine;
using UnityEngine.UI;

public class Targetable : MonoBehaviour 
{

	public float maxUsableDistance = 2;

	public GameObject usablePanel;

	public GameObject tooFarPanel;

	public void Start()
	{
		Deselect();
	}

	public void Select(float distance)
	{
		SetPanels(distance <= maxUsableDistance);
	}

	public void Deselect()
	{
		SetPanels(false);
	}

	private void SetPanels(bool usable)
	{
		usablePanel.SetActive(usable);
		tooFarPanel.SetActive(!usable);
	}

}
