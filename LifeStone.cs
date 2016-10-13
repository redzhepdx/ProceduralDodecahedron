using UnityEngine;
using System.Collections;

public class LifeStone : MonoBehaviour {

	public int Health = 100;

	public LifeVessel lifeVessel;

	
	void Start()
	{   
			Invoke("SpawnLifeVessel",0.1f);	
	}

	void Update()
	{
		CheckForDestroy();
	}

	void SpawnLifeVessel()
	{
		LifeVessel lf = Instantiate(lifeVessel, transform.localPosition, Quaternion.identity) as LifeVessel;

		lf.stonePos = transform;

		lf.bossPos = GameObject.FindGameObjectWithTag("Boss").transform;

		lf.transform.parent = transform;

	}

	void CheckForDestroy()
	{
		if(Health <= 0)
		{
			GameObject.FindGameObjectWithTag("Boss").GetComponent<ThunderBoss>().TakeDamage();

			CancelInvoke("SpawnLifeVessel");

			Destroy(gameObject);

			GameObject.FindGameObjectWithTag("Player").transform.position += Vector3.up * 140;

			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DodeShooter>().playerState = DodeShooter.PlayerState.Oustside;

			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DodeShooter>().bossStunned = false;
		}
	}

	void SpawnBugs()
	{

	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("Boomerang"))
		{
			Destroy(col.gameObject);

			Health -= 15;
		}

	}
}
