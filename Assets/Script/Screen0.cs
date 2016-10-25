using UnityEngine;
using System.Collections;

public class Screen0 : MonoBehaviour {

    public GameObject car;
    // Use this for initialization
    void Start() {

    }
	// Update is called once per frame
	void Update () {
        transform.position = new UnityEngine.Vector3(transform.position.x, transform.position.y, car.transform.position.z);
    }
}
