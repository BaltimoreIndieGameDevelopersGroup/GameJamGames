using UnityEngine;
using UnityEngine.EventSystems;

public class Targeter : MonoBehaviour 
{
		
	[Tooltip("The layer mask to use when targeting objects. Objects on others layers are ignored.")]
	public LayerMask layerMask = 1; // Default layer.
	
	[Tooltip("The max targeting distance. The selector won't check for targets farther than this.")]
	public float maxTargetingDistance = 30f;

	[Tooltip("The key that 'uses' the target.")]
	public KeyCode useKey = KeyCode.Space;
	
	[Tooltip("The button that 'uses' the target.")]
	public string useButton = "Fire1";

	private Targetable m_target = null;

	/// <summary>
	/// Runs a raycast to check for targets at the center of the screen and updates
	/// the target. If the player hits the use button, uses the target.
	/// </summary>
	void Update()
	{
		if (!enabled || (Time.timeScale <= 0) || (Camera.main == null)) return;

		// Update target:
		float distance = Mathf.Infinity;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray, maxTargetingDistance, layerMask);
		bool found = false;
		foreach (var hit in hits) {
			distance = hit.distance;
			if ((m_target == null) || (m_target.gameObject != hit.collider.gameObject))
			{
				if (m_target != null) m_target.Deselect();
				m_target = hit.collider.GetComponent<Targetable>();
			}
			if (m_target != null) 
			{
				m_target.Select(distance);
				found = true;
				break;
			}
		}
		if (!found) {
			if (m_target != null) m_target.Deselect();
			m_target = null;
		}

		// Check use button:
		if (IsUseButtonDown() && (m_target != null) && (distance <= m_target.maxUsableDistance)) 
		{
			ExecuteEvents.Execute<IUseEventHandler>(m_target.gameObject, null, (x,y)=>x.OnUse());
		}
	}
	
	private bool IsUseButtonDown()
	{
		return ((useKey != KeyCode.None) && Input.GetKeyDown(useKey)) ||
			(!string.IsNullOrEmpty(useButton) && Input.GetButtonUp(useButton));
	}
	
}
