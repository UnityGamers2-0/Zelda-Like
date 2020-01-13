using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimations : MonoBehaviour {

	public float lookRadius = 20f;
	private Animator a;

	[SerializeField]Transform target;
	[SerializeField]NavMeshAgent agent;

	void Start () 
	{
		//GameObject.FindGameObjectsWithTag ("Mage", "Knight", "Archer");
		target = PlayerManager.instance.player.transform;
		agent = GetComponent<NavMeshAgent>();

		a = GetComponent<Animator>();
	}

	//Allows enemy to move towards player
	void Update ()
	{
		float distance = Vector3.Distance (target.position, transform.position);
		//Debug.Log(target.position - transform.position);

		if (distance <= lookRadius) 
		{
			agent.SetDestination (target.position);

			if (distance <= agent.stoppingDistance + Mathf.Abs(target.position.y - transform.position.y)) 
			{
				//Attack and face the target
				a.SetTrigger("Attack01");
			}	
		}	
	}
}
