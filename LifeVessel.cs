using UnityEngine;
using System.Collections;

public class LifeVessel : MonoBehaviour {

	public Transform stonePos;
	public Transform bossPos;
	float speed = 1f;
	
	
	void Update() {
		transform.position = Vector3.Lerp(stonePos.localPosition, bossPos.localPosition, Mathf.PingPong(Time.time * speed, 1.0f));
		transform.rotation = Quaternion.LookRotation(transform.forward, stonePos.localPosition - bossPos.localPosition);
	}
}
