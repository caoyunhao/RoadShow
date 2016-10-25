using UnityEngine;
using System.Collections;

public class AllowThroughManager : MonoBehaviour {

    private GameObject suv;

	void Start () {
        suv = GameObject.Find("SUV");
	}
	
	void FixedUpdate () {
        float toZ = suv.transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, toZ + 9.48f);
	}
}
