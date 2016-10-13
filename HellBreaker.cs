using UnityEngine;
using System.Collections;

public class HellBreaker : MonoBehaviour {

	public float distance = 300f;
	bool inMotion = false;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			transform.GetChild(0).gameObject.SetActive(false);
		}
	}
}
