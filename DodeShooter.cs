using UnityEngine;
using System.Collections;

public class DodeShooter : MonoBehaviour {

	public enum PlayerState
	{
		Oustside,
		Inside
	};

	public Boomerang boomerang;

	public GameObject stunBlast;
	
	public bool boomerangReady = true;

	public bool pleseboPillTook = false;

	public bool bossStunned = false;

	[HideInInspector]
	public PlayerState playerState = PlayerState.Oustside;

	void Awake()
	{

	}

	void Update () {

		if(playerState == PlayerState.Oustside)
		{
			DetectDodecahedron();
		}
		else
		{
			BoomerangAttack();
		}
	}
	
	void DetectDodecahedron()
	{
		if (pleseboPillTook && !bossStunned && Input.GetMouseButtonDown(0))
		{
			ThrowStunShockBall();
		}

		if (bossStunned)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.collider.CompareTag("Pentagon") && Input.GetKey(KeyCode.Q))
			{
				GameObject.FindGameObjectWithTag("Player").transform.position = hit.collider.transform.parent.parent.transform.position + Vector3.up;
				playerState = PlayerState.Inside;
			}
		}
	}

	void ThrowStunShockBall()
	{
		GameObject stunBlastObj = Instantiate(stunBlast, transform.parent.localPosition + transform.parent.forward * 50, Quaternion.identity) as GameObject;

		stunBlastObj.GetComponent<Rotate>().enabled = false;

		stunBlastObj.tag = "SendedPill";

		stunBlastObj.GetComponent<Rigidbody>().AddForce(transform.forward * 600f, ForceMode.VelocityChange);
		
		pleseboPillTook = false;
	}

	void BoomerangAttack()
	{
		if (boomerangReady && Input.GetKey(KeyCode.X))
		{
			Boomerang boom = Instantiate(boomerang, transform.localScale, Quaternion.identity) as Boomerang;

			boom.duration = 0.45f;

			boom.StartCoroutine("Throw");

			boomerangReady = false;

			//Time.timeScale = 0.25f;

			Invoke("ReloadBoomerang", boom.duration);

			Destroy(boom.gameObject, boom.duration);
		}
	}

	void ReloadBoomerang()
	{
		//Time.timeScale = 1f;
		boomerangReady = true;
	}
	

}
