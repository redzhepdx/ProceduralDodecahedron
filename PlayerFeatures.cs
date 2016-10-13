using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerFeatures : MonoBehaviour {

	public int Health = 100;
	public string damageableTag = "";
	public string collectObjectTag = "";
	public string boundaryName = "";
	
	void Update () {
		CheckForDeath();
		print(Health);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag(damageableTag))
		{
			DecreaseHealth();
		}
		else if (collider.gameObject.CompareTag(collectObjectTag))
		{
			transform.GetChild(0).GetComponent<DodeShooter>().pleseboPillTook = true;
			Destroy(collider.gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag(damageableTag))
		{
			DecreaseHealth();
		}
		if (collision.gameObject.CompareTag(boundaryName))
		{
			SceneManager.LoadScene("Level9(Dodecahedron)");
		}
	}

	void DecreaseHealth()
	{
		int randomDecrease = Random.Range(5, 15);
		Health -= randomDecrease;
	}

	void CheckForDeath()
	{
		if(Health <= 0)
		{
			//Restart Game
			SceneManager.LoadScene("Level9(Dodecahedron)");
		}
	}

}
