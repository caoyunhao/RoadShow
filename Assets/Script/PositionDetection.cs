using UnityEngine;
using System.Collections;
/*
 * @description:用于判断1号和2号的生成车的位置上是否有其他车
 */
public class PositionDetection : MonoBehaviour {
	// Use this for initialization
	private string name;
	void Start () {
		name = transform.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider go){
	//	Debug.Log ("name is "+go.transform.name);
		if (name == "1") {
			GameObject.Find ("SceneControl").GetComponent<AICarCreate> ().JudgeCreatePositionOne (false);
		} else {
			GameObject.Find ("SceneControl").GetComponent<AICarCreate> ().JudgeCreatePositionTwo(false);
		}
	}
	void OnTriggerStay(Collider go){
		if (name == "1") {
			GameObject.Find ("SceneControl").GetComponent<AICarCreate> ().JudgeCreatePositionOne (false);
		} else {
			GameObject.Find ("SceneControl").GetComponent<AICarCreate> ().JudgeCreatePositionTwo (false);
		}
	}
	void OnTriggerExit(Collider go){
	//	Debug.Log ("name is "+go.transform.name);
		if (name == "1") {
			GameObject.Find ("SceneControl").GetComponent<AICarCreate> ().JudgeCreatePositionOne (true);
		} else {
			GameObject.Find ("SceneControl").GetComponent<AICarCreate> ().JudgeCreatePositionTwo (true);
		}
	}

}
