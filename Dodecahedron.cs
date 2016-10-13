using UnityEngine;
using System.Collections;

public class Dodecahedron : MonoBehaviour
{

    public SemiDodecahedron semiDode;
    [HideInInspector]
    public SemiDodecahedron upPart, downPart;
    [HideInInspector]
    public bool isContainsLifeStone = false;

    private float y_rot_angle = 108;
    private float offset = 7.8f;
    private float height = Pentagon.radius / 3;

    void Awake()
    {
        upPart = Instantiate<SemiDodecahedron>(semiDode);
        upPart.transform.name = "DownPart";
        upPart.transform.parent = transform;

        downPart = Instantiate<SemiDodecahedron>(semiDode);
        downPart.transform.name = "UpPart";
        downPart.transform.position += upPart.transform.up * height * offset;
        downPart.transform.Rotate(0, 180 - y_rot_angle, 180);
        downPart.transform.parent = transform;
    }

    void Update()
    {
        if(isContainsLifeStone){
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        foreach(Pentagon pentagon in upPart.pentagons)
        {
            pentagon.GetComponent<Renderer>().material.color = Color.red;
        }

        foreach (Pentagon pentagon in downPart.pentagons)
        {
            pentagon.GetComponent<Renderer>().material.color = Color.red;
        }

        upPart.root.GetComponent < Renderer>().material.color = Color.red;

        downPart.root.GetComponent < Renderer>().material.color = Color.red;

        isContainsLifeStone = false;
    }

}
