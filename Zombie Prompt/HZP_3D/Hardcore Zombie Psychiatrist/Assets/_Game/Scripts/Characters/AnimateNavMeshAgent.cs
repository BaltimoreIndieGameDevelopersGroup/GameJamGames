using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimateNavMeshAgent : MonoBehaviour
{

	private NavMeshAgent m_navMeshAgent = null;
	private Animator m_animator = null;
	public bool useRootMotion = false;

	private ZombieMove zombieMove = null;
	
	void Awake() {
		m_navMeshAgent = GetComponent<NavMeshAgent>();
		m_animator = GetComponentInChildren<Animator>();
		if (m_navMeshAgent == null || m_animator == null) enabled = false;
		zombieMove = GetComponent<ZombieMove>();
	}
	
	void Start()
	{
		if (useRootMotion) m_navMeshAgent.updatePosition = false;
	}
	
	private void Update() 
	{
		if (zombieMove != null && zombieMove.state == ZombieMove.State.SLEEP) {
			m_animator.SetFloat("Speed", 0);
		} else {
			m_animator.SetFloat("Speed", m_navMeshAgent.speed);
		}
	}
	
}
