using UnityEngine;
using System.Collections;
/*
 * @description:用于控制AI车辆随机的状态,随机的变化车的加速度
 * 
 */ 
public class AutoCarControl : MonoBehaviour {

	public WheelCollider[] wheelcoilder = new WheelCollider[4];
	public GameObject[] wheel = new GameObject[4];
	private float motor;
	private float brakemotor = 0;
	private float maxangle = 25;
	public Transform target;
	//初始化速度
	private float minspeed = 0f;
	private float accelerate = 0f;
	private float handbrakemotor = 0;
	private string way_path;
	private bool freedom = true;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().centerOfMass = new Vector3 (0, -2, 0);
		motor = Random.Range(1000,1500);
		minspeed = GameObject.Find("SUV").GetComponent<Rigidbody>().velocity.magnitude;
	//	Debug.Log ("minspeed is "+minspeed);
		way_path = transform.parent.GetChild (0).GetComponent<BallMove> ().getpath ();
		InvokeRepeating ("changeState", 2, 3);
	//	Debug.Log ("current way " + way_path);
	}

	void FixedUpdate(){
		//用于更新当前前轮的状态  
		Vector3 offsetTargetPos= target.position;//小球的位置  
		Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);  
		float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z)*Mathf.Rad2Deg;  
		float steer = Mathf.Clamp(targetAngle*0.05f, -1, 1)*Mathf.Sign(GetComponent<Rigidbody>().velocity.magnitude);  
		wheelcoilder[0].steerAngle=wheelcoilder[1].steerAngle=steer*maxangle;  
		//刹车
		if (handbrakemotor != 0) {
			motor = 0;
			Debug.Log("stop!!!");
		} else {//正常行驶
			float speed = GetComponent<Rigidbody> ().velocity.magnitude;
			//Debug.Log ("speed is "+ speed);
			if (speed < minspeed) {
				if(way_path=="go4"||way_path=="go5"){
				    //对象车道的车
			//		Debug.Log("不改变速度");
				}else{
					motor = motor+50*Time.deltaTime;
					GetComponent<Rigidbody> ().velocity=Vector3.forward*minspeed;
				}
			} else {
				//避免需要刹车的时候这个最小速度的存在导致不能减速
				minspeed = 0;
				brakemotor = 0;
				motor = motor+accelerate*Time.deltaTime;
				if(motor<0){
					motor = 0;
				}
			//	Debug.Log("current motor is "+motor);
			}
		}
		transform.GetComponent<Rigidbody> ().AddForce (Vector3.down*1000);
		wheelcoilder[2].motorTorque=
			wheelcoilder[3].motorTorque=motor;  //车的动力
		wheelcoilder[2].brakeTorque = wheelcoilder[3].brakeTorque=handbrakemotor;//应用刹车动力
		//Debug.Log("go");
		changewheels ();
	}

	void changewheels()
	{
		for (int i=0;i<4;i++)
		{
			Quaternion quat;// it usual used to represent the rotation in unity.这个通常在unity中是表示旋转角度，包括物体在游戏世界的角度和自身的旋转角度
			Vector3 position;//unity中通常表示一个三维向量
			wheelcoilder[i].GetWorldPose(out position,out quat);//get the collides' position and ratation in the world.得到碰撞体的位置和状态
//			Debug.Log(i+" position is "+position);
			wheel[i].transform.position = position;//transform is position,roatation and scale of an object
//			Debug.Log(i+" wheel position is "+wheel[i].transform.position);
			wheel[i].transform.rotation = quat;
//			Debug.Log(i);//在控制栏中打印出相应的信息
		}
		
		
		
	}
	public void SlowDown(int bm){
		Debug.Log ("Slow down");
		handbrakemotor = bm;
		freedom = false;
	}
	public void Normal(){
		//Debug.Log ("Normal");
		handbrakemotor = 0;
		freedom = true;
	}
	public void SpeedUp(){
		handbrakemotor = 0;
		accelerate = 300;
		freedom = false;
	}

	private void changeState(){
		if (freedom) {
			accelerate = Random.Range (-300,200);
		}
	}
}
