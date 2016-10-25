using UnityEngine;
using System.Collections;
/*
 * @description:用于驾驶车辆的控制
 */ 
public class CarControl : MonoBehaviour {

    //碰撞体的声明
	public WheelCollider[] wheelcoilder = new WheelCollider[4];
	//实际的轮子的声明
	public GameObject[] wheel = new GameObject[4];
    public float MaxSpeed = 20;
    public float MaxMotor = 2500; //最大的动力
    public float MaxAngle = 50; //最大的旋转角度
    public float MaxBrokeTorque; //最大的刹车动力

    private float Max = 1200;

    public Vector3 centerofmass; //重心位置
    
	private bool isEnvironment1 = true;
	private int newZ = 350;//the end of the terrain
	public Transform carsteer;
	private Quaternion target;
	private int num = 0;//判断当前第几次触碰到第一个触发器  有可能当运行到第二段路的时候才开始变道，所以要把所有的自动引导标志点向前移一个地面的大小
	private float last_h = 0;
    // Use this for initialization

	private float speed;//当前的车的速度

    private GameObject allowThrough;

    void Start () {//初始化函数
        //Debug.Log(GetComponent<Rigidbody>().centerOfMass);//获得当前物体的刚体组件并且获得它的重心
        GetComponent<Rigidbody>().centerOfMass = centerofmass;
	    target = carsteer.localRotation;

        allowThrough = GameObject.Find("AllowThrough");
	}
	
	// Update is called once per frame

 	void FixedUpdate () {//每一帧的调用函数
        /*键盘控制
        float h = Input.GetAxis("Horizontal");//左右 输入范围(-1,1)
        float v = Input.GetAxis("Vertical"); //上下 输入范围(-1, 1)
        float handbroke = Input.GetAxis("Jump");//空格键是手刹*/

      //  方向盘控制
        float h = Input.GetAxis("Logisteer");//左右 输入范围(0,2)
		float v = 1-Input.GetAxis("Speed"); //上下 输入范围(0, 2)
        float handbroke = 1 - Input.GetAxis("Brake");//手刹
		print(h);
		//Debug.Log ("brake is "+handbroke);

        wheelcoilder[0].steerAngle = wheelcoilder[1].steerAngle = h* MaxAngle;    //车的转弯角度
		speed = GetComponent<Rigidbody> ().velocity.magnitude;
	    //Debug.Log ("speed is "+ speed);
		if (speed > MaxSpeed) {
			v = 0;
			wheelcoilder[2].brakeTorque = wheelcoilder[3].brakeTorque = 100;
			//GetComponent<Rigidbody> ().velocity=new Vector3(0,0,10*Time.deltaTime);
		} 
		if (handbroke > 0)//判断是否按下了空格（手刹）
        {
            float broketorque =  handbroke * MaxBrokeTorque;
			//Debug.Log("broketorque = "+ broketorque); 
            wheelcoilder[2].brakeTorque = wheelcoilder[3].brakeTorque = broketorque;//应用刹车动力
			wheelcoilder[2].motorTorque = wheelcoilder[3].motorTorque = 0;
        }
        else
        {
            wheelcoilder[2].brakeTorque = wheelcoilder[3].brakeTorque = 0;
			wheelcoilder[2].motorTorque = wheelcoilder[3].motorTorque = -v * MaxMotor;  //车的动力
        }
        changewheels();
		changcarsteer (h);
    }

	void changcarsteer(float h){
		/*if (h == 0) {//方向盘自动回滚到正常
			carsteer.localRotation = Quaternion.Slerp(carsteer.localRotation, target, Time.deltaTime);
		} else {
			float rotation = h * Max;
			carsteer.RotateAround (carsteer.position, carsteer.forward, rotation*Time.deltaTime);
		}*/
		if (last_h != h) {
			float rotation = (h - last_h) * Max;
			//Debug.Log ("rotation is " + rotation);
			carsteer.RotateAround (carsteer.position, carsteer.forward, rotation /** Time.deltaTime*/);
			//carsteer.RotateAround (carsteer.position, carsteer.forward, rotation);
			last_h = h;
		} else {
			//Debug.Log ("h max is "+h);
		}
	}

    void changewheels()
    {
        for (int i=0;i<4;i++)
        {
             Quaternion quat;// it usual used to represent the rotation in unity.这个通常在unity中是表示旋转角度，包括物体在游戏世界的角度和自身的旋转角度
             Vector3 position;//unity中通常表示一个三维向量
             wheelcoilder[i].GetWorldPose(out position,out quat);//get the collides' position and ratation in the world.得到碰撞体的位置和状态
             wheel[i].transform.position = position;//transform is position,roatation and scale of an object
             wheel[i].transform.rotation = quat;
             //Debug.Log(i);//在控制栏中打印出相应的信息
        }   
        
    }

	void OnTriggerEnter(Collider go){  
		//碰撞到point后转向下一个point  
		//Debug.Log("reach："+go.name); 
		if(go.transform.name=="SUV_Touch"){  
			go.transform.position = new Vector3(go.transform.position.x,go.transform.position.y,(go.transform.position.z+350));
			GameObject end =  GameObject.Find("End");
			if((end.transform.position.z == 698)&&isEnvironment1){//第一次不变化

			}else{
				end.transform.position = new Vector3(end.transform.position.x,end.transform.position.y,(end.transform.position.z+350));
			}
			CreateNewEnvironment();
		}
	}  

	//create a new environment
	private void CreateNewEnvironment(){
		if(isEnvironment1){//move the environment2 to be the next environment
			GameObject CurrentEnvironment = GameObject.Find("Environment1");
			GameObject NewEnvironment = GameObject.Find("Environment2");
			if((NewEnvironment.transform.position.z - CurrentEnvironment.transform.position.z)==350){
				//it means that the new terrain is in the correct position
			}else{
				NewEnvironment.transform.position = new Vector3(0,0,newZ);
			}
			newZ += 350;
			isEnvironment1 = false;
		}else{////move the environment1 to be the next environment
			GameObject CurrentEnvironment = GameObject.Find("Environment2");
			GameObject NewEnvironment = GameObject.Find("Environment1");
			if((NewEnvironment.transform.position.z - CurrentEnvironment.transform.position.z)==350){
				//it means that the new terrain is in the correct position
			}else{
				NewEnvironment.transform.position = new Vector3(0,0,newZ);
			}
			newZ += 350;
			isEnvironment1 = true;
		}
	}

	public float getSpeed(){
		return speed;
	}

}
