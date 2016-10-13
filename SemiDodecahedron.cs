using UnityEngine;
using System.Collections;

public class SemiDodecahedron : MonoBehaviour {

	public Pentagon pentagon;
	[HideInInspector]
	public Pentagon root;
	[HideInInspector]
	public Pentagon[] pentagons = new Pentagon[5];
	private int faceNumber = 20;

	float dihedral_angle = 116.5650512f;

	//Making Semi Dodecahedron for given root pentagon
	void Awake() {

		root = Instantiate(pentagon, transform.position, transform.rotation) as Pentagon;

		root.transform.name = "Root";
		root.transform.parent = transform;
		for (int i = 0; i < root.vertices.Length; i++)
		{
			pentagons[i] = Instantiate<Pentagon>(pentagon);
			pentagons[i].transform.RotateAround(root.vertices[i], root.directions[i], -dihedral_angle);
			pentagons[i].GetComponent<Renderer>().material.color = (i % 2 == 1) ? Color.white : Color.black;
			pentagons[i].transform.parent = transform;
		}
	
	}
	
	void Update () {

	}

}
