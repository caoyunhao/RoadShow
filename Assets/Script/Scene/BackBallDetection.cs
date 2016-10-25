using UnityEngine;
using System.Collections;
/*
 *@description:主要用于检测后车的情况，若有后车检测到，则加速 
 */
public class BackBallDetection : MonoBehaviour {


	private float move_speed;
	private Rigidbody carbody;
	public GameObject car;
	// Use this for initialization
	void Start () {
		carbody = car.GetComponent<Rigidbody> ();
		//设定后触发器的位置
		if (transform.position.z < transform.parent.GetChild (1).transform.position.z) {
			transform.position = new Vector3 (transform.parent.GetChild (1).transform.position.x, 
			                                  transform.parent.GetChild (1).transform.position.y,
			                                  transform.parent.GetChild (1).transform.position.z - 30);
		} else {
			transform.position = new Vector3 (transform.parent.GetChild (1).transform.position.x, 
			                                  transform.parent.GetChild (1).transform.position.y,
			                                  transform.parent.GetChild (1).transform.position.z + 30);
		}

	}
	
	// Update is called once per frame
	void Update () {
		move_speed = carbody.velocity.magnitude;  
		//Debug.Log("ball："+move_speed);  
		//改变小球的移动方向  
		transform.forward=carbody.position-transform.position;  
		transform.Translate(0,0,move_speed*Time.deltaTime);
	}

	void OnTriggerEnter(Collider go){
	//	Debug.Log ("name is "+go.transform.name);
		if (go.transform.name == "Car") {//表示后面有车追上来了
				transform.parent.GetChild(1).GetComponent<AutoCarControl>().SpeedUp();
		}
	}
}
