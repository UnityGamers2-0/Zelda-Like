using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public Player player;
	Rigidbody rb;
	Collider c;
	bool flying;
	private Transform anchor;
	public float power;

	float flyTime = 10f;
	float stopTime;

	// Use this for initialization
	void Start () {
		//So the base arrow is not affected by any code
		flying = name == "Arrow" ? false : true;
		//Kills the arrow if in the air for too long
		stopTime = Time.time + flyTime;
		rb = GetComponent<Rigidbody>();
		c = GetComponentInChildren<MeshCollider>();
		if (flying)
		{
			rb.isKinematic = false;
			//makes the arrow face the direction the player is
			Vector3 camRot = player.cam.transform.localEulerAngles;
			rb.transform.localEulerAngles = new Vector3(camRot.x, camRot.y, camRot.z);
			//Moves the arrow one unit in front of player
			Vector3 playerPos = player.cam.transform.InverseTransformDirection(player.cam.transform.position);
			rb.transform.position = player.cam.transform.TransformDirection(new Vector3(playerPos.x, playerPos.y, playerPos.z + 1));
			//Enables collider
			c.enabled = true;
			//Adds forward momentum to arrow
			Vector3 localVel = rb.transform.InverseTransformDirection(rb.velocity);
			//max 65; min 5
			rb.velocity = rb.transform.TransformDirection(new Vector3(localVel.x, localVel.y, localVel.z + power));
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		//Kill arrow if in air too long
		if (stopTime <= Time.time && flying && name == "Arrow(Clone)")
		{
			Destroy(gameObject);
		}
		//make arrow face direction of travel
		if (flying)
		{
			rb.transform.LookAt(transform.position + rb.velocity);
		}
		//Make arrow stick to what it hits
		else if (anchor != null)
		{
            if (anchor.parent.gameObject.activeSelf)
            {
                transform.position = anchor.transform.position;
                transform.rotation = anchor.transform.rotation;
            }
            else
            {
                flying = true;
				rb = gameObject.AddComponent<Rigidbody>();
				rb.angularDrag = 2f;
				c.isTrigger = false;
            }
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (flying && !collision.collider.name.Contains("Arrow"))
		{
            float speed = rb.velocity.magnitude;
			flying = false;
			rb.transform.position = collision.contacts[0].point;
			c.isTrigger = true;

			GameObject anchor = new GameObject("ARROW_ANCHOR");
			anchor.transform.position = transform.position;
			anchor.transform.rotation = transform.rotation;
			anchor.transform.parent = collision.transform;
			this.anchor = anchor.transform;
            object[] @params = new object[] { speed, player };
			Destroy(rb);
			collision.gameObject.SendMessage("arrowHit", @params, SendMessageOptions.DontRequireReceiver);
		}
	}
}
