using UnityEngine;
using System.Collections;

public class ChangCamera : MonoBehaviour {

	public GameObject CameraMain;
	public GameObject CameraSecond;
	private int number =1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.F5)) {
			if(number==1){
				CameraMain.SetActive(false);
				CameraSecond.SetActive(true);
				number = 2;
			}else if(number==2){
				CameraSecond.SetActive(false);
				CameraMain.SetActive(true);
                number = 1;
			}
			
		}
	}
}
