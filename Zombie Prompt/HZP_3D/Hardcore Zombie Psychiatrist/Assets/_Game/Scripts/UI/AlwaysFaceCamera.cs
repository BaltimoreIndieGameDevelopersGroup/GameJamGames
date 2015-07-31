using UnityEngine;

/// <summary>
/// Component that keeps its game object always facing the main camera.
/// </summary>
public class AlwaysFaceCamera : MonoBehaviour 
{
		
	public bool yAxisOnly = false;
	
	void Update() {
		if ((transform != null) && (UnityEngine.Camera.main != null)) 
		{
			if (yAxisOnly) 
			{
				transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
			}
			else 
			{
				transform.LookAt(Camera.main.transform);
			}
		}
	}
	
}
