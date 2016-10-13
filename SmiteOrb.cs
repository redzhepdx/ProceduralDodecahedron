using UnityEngine;
using System.Collections;

public class SmiteOrb : MonoBehaviour {

	public GameObject SmiteProjectile;

	Transform player;

	Vector3 StartPos;

	bool attackWithProjectile = true;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;

		StartPos = transform.position;
	}


	void Update () {
		AttackForDistance();
	}


	void AttackForDistance()
	{
		float distance = (transform.position - player.position).magnitude;

		Vector3 direction = (player.position - transform.position);

		if(distance < 300)
		{
			transform.position = Vector3.Lerp(StartPos, player.localPosition, Mathf.PingPong(Time.time , 1.0f));
		}
		else if(distance > 300 && distance < 500 && attackWithProjectile ){

			attackWithProjectile = false;

			GameObject projectile = Instantiate(SmiteProjectile, transform.position, Quaternion.identity) as GameObject;
			
			projectile.GetComponent<Rigidbody>().AddForce(direction, ForceMode.VelocityChange);

			Invoke("ReleaseProjectile", 2f);
		}
	}

	void ReleaseProjectile()
	{
		attackWithProjectile = true;
	}

}
