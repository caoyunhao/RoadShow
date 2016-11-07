using UnityEngine;
using System.Collections;

public class HUDRoadManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            GameObject hud = GameObject.Find("HUD");
            hud.GetComponent<HUDManager>().SetInRoadPosition(transform.position);
            Debug.Log("enter");
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            GameObject hud = GameObject.Find("HUD");
            hud.GetComponent<HUDManager>().SetInRoadPosition(Vector3.zero);
            Debug.Log("exit");
        }
    }
}
