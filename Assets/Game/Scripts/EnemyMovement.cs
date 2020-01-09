using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	[SerializeField]Transform target;
	[SerializeField]float movementSpeed = 10f;
	[SerializeField]float rotationalDamp = .5f;
	[SerializeField]public float lookRadius = 10f;
	[SerializeField]NavMeshAgent agent;

	void Update()
	{
		try
		{
			float distance = Vector3.Distance(target.position, transform.position);

			if (distance <= lookRadius)
			{
				agent.SetDestination(target.position);

				if (distance <= agent.stoppingDistance)
				{
					//Attack and face the target
					FaceTarget();
				}
			}
		}
		catch (UnassignedReferenceException) { }

		Turn ();
		//Move ();
	}

	void Turn()
	{
		try
		{
			Vector3 pos = target.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation(pos);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
		}
		catch (UnassignedReferenceException) { }
	}

	/*void Move()
	{
		transform.position += transform.forward * movementSpeed * Time.deltaTime;
	}		*/

	void FaceTarget()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation (new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
}
