using UnityEngine;
using System.Collections;
/*
 * @description:1.每一个车前面的小球是引导方向的作用，保障ai车在道路上行驶并且保持方向
 *              2.这个小球也用于判断车的碰撞情况，用于检测触发
 */
public class BallMove : MonoBehaviour
{
	private Transform goal;
	public GameObject car;
	public GameObject whole;
	private float move_speed;
	private Rigidbody carbody;
	private string way_name;
	// Use this for initialization
	void Start ()
	{
		carbody = car.GetComponent<Rigidbody> ();
		//find the way which the car will follow
		if (transform.position.x == GameObject.Find ("0").transform.position.x) {
			goal = GameObject.Find ("AIStop1").transform;
			way_name = "go1";
		} else if (transform.position.x == GameObject.Find ("1").transform.position.x
		           &&transform.position.z > GameObject.Find ("1").transform.position.z) {
			goal = GameObject.Find ("AIStop2").transform;
			way_name = "go2";
		} else if (transform.position.x == GameObject.Find ("2").transform.position.x
		           &&transform.position.z < GameObject.Find ("1").transform.position.z) {
			goal = GameObject.Find ("AIStop2").transform;
			way_name = "go3";
		} else if (transform.position.x == GameObject.Find ("3").transform.position.x) {
			goal = GameObject.Find ("AIStop3").transform;
			way_name = "go4";
		} else if (transform.position.x == GameObject.Find ("4").transform.position.x) {
			goal = GameObject.Find ("AIStop4").transform;
			way_name = "go5";
		}
//		Debug.Log ("target is "+goal.name);
		//设置前引导球的位置
		if (transform.position.z > transform.parent.GetChild (1).transform.position.z) {
			transform.position = new Vector3 (transform.parent.GetChild (1).transform.position.x, 
			                                  transform.parent.GetChild (1).transform.position.y,
			                                  transform.parent.GetChild (1).transform.position.z + 30);
		} else {
			transform.position = new Vector3 (transform.parent.GetChild (1).transform.position.x, 
			                                  transform.parent.GetChild (1).transform.position.y,
			                                  transform.parent.GetChild (1).transform.position.z - 30);
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		move_speed = carbody.velocity.magnitude;  
		//Debug.Log("ball："+move_speed);  
		//改变小球的移动方向  
		transform.forward = goal.position - transform.position;  
		transform.Translate (0, 0, move_speed * Time.deltaTime);
		//判断车辆是否掉到地形以下
		if (carbody.position.y < -5) {
			GameObject.Find ("SceneControl").GetComponent<AICarCreate> ().reCreate (way_name);
			Destroy (whole);
		}
	}

	void OnTriggerEnter (Collider go)
	{  
		//碰撞检测，碰到前车的话要刹车
		if (go.transform.name == "Car") {
			transform.parent.GetChild (1).GetComponent<AutoCarControl> ().SlowDown (50000000);
		} else if (go.transform.name == goal.name || go.transform.name == "End") {//到达终点,消灭车辆实例
	//		Debug.Log("destory");
			GameObject.Find ("SceneControl").GetComponent<AICarCreate> ().reCreate (way_name);
			Destroy (whole);
		}
	}

	void OnTriggerExit (Collider go)
	{
		//碰撞检测，碰到前车的话要刹车
		if (go.transform.name == "Car") {
			if (go.transform.name == "Car") {
				transform.parent.GetChild (1).GetComponent<AutoCarControl> ().Normal ();
			}

		}
	}
	//控制车辆的类会调用这个函数获得当前的位置
	public string getpath(){
		return way_name;
	}
}