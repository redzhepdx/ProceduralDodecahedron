using UnityEngine;
using System.Collections;

public class Shockwave : MonoBehaviour {

	
	void Update () {
		if(GetComponent<SphereCollider>().radius <= 2000)
		{
			GetComponent<SphereCollider>().radius += 8.5f;
		}
	}
}
