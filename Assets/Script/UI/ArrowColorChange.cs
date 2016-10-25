using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArrowColorChange : MonoBehaviour {

	private GameObject check; 
	// Use this for initialization
	void Start () {
		check = GameObject.Find ("check").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		Color changecolor = GameObject.Find ("ColorPanel").GetComponent<gradual> ().GetMeshColor (check.transform.position);
		transform.GetComponent<Image> ().color = changecolor;
	}
}
