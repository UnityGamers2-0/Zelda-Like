using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Entity {
    
	[Space]
	public float lookRadius = 20f;
    private Animator a;

	[SerializeField]Transform target;
	[SerializeField]NavMeshAgent agent;
	[Space]
	[SerializeField] AudioSource hit;
	[SerializeField] AudioSource miss;

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
				FaceTarget();
                a.SetTrigger("Attack");
			}	
		}	
	}

	//Rotates enemy (if necessary) to follow target
	void FaceTarget()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation (new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, lookRadius);
	}

	void TriggerAttack()
	{
		if(Attack(target))
		{
			hit.Play();
		}
		else
		{
			miss.Play();
		}
		
	}
}
