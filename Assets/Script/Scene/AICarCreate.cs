using UnityEngine;
using System.Collections;

/*
 * @description:用于自动生成车到上面的车
 *              同时为了避免生成车的地点重复，发生碰撞
 */
public class AICarCreate : MonoBehaviour {

	public GameObject[] car = new GameObject[3];//四种车型
	public Transform[] positions = new Transform[5];//五个起始位置
	static public GameObject[] newcar = new GameObject[5];
	private GameObject createcar;
	private int create_number = 0;
	private int show_number = 0;
	private string pathname;
	private Transform space;
	private bool none_backcar = true;
	private bool none_Rfrontcar = true;
	private bool none_frontcar = true;
	private bool none_Rcome  = true;
	private bool none_Lcome  = true;
	private bool isCreating = false;
	private bool positionOneOkay = true;
	private bool positionTwoOkay = true;
	public Transform end;
	private int car_num = 0;
	GameObject frontCar;
	GameObject backCar;
	// Use this for initialization
	void Start () {
		car_num = car.Length;
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (!isCreating) {
			if (none_frontcar) {
				none_frontcar = false;
				CreateCar("go1");
			}else if (none_Rfrontcar) {
				none_Rfrontcar = false;
				CreateCar("go2");
			} else if (none_backcar) {
				none_backcar = false;
				CreateCar("go3");
			} else if (none_Rcome) {//当前没有来向右车
				none_Rcome = false;
				CreateCar("go4");
			} else if (none_Lcome) {//当前没有来向左车
				none_Lcome = false;
				CreateCar("go5");
			}
		}
	}
	
	//选择生成的车型
	private void chooseCar(){
		int num = Random.Range (0, car_num);
		createcar = car[num];
	}
	
	//选择汽车的生成点
	private void choosePoistion(string name){

		switch (name) {
		case "go1":
			//Debug.Log("create front");
			space = positions[0];
			pathname = "go1";
			break;
		case "go2":
			//Debug.Log("create Rfront");
			space = positions[1];
			pathname = "go2";
			break;
		case "go3":
			//Debug.Log("create back");
			space = positions[2];
			pathname = "go3";
			break;
		case "go4":
			//Debug.Log("create Rcome");
			space = positions[3];
			pathname = "go4";
			break;
		case "go5":
			//Debug.Log("create Lcome");
			space = positions[4];
			pathname = "go5";
			break;
		}
	}
	
	/*
	 * 欧拉角到四元数：
      给定一个欧拉旋转(X, Y, Z)（即分别绕x轴、y轴和z轴旋转X、Y、Z度），则对应的四元数为：
      x = sin(Y/2)sin(Z/2)cos(X/2)+cos(Y/2)cos(Z/2)sin(X/2)
      y = sin(Y/2)cos(Z/2)cos(X/2)+cos(Y/2)sin(Z/2)sin(X/2)
      z = cos(Y/2)sin(Z/2)cos(X/2)-sin(Y/2)cos(Z/2)sin(X/2)
      w = cos(Y/2)cos(Z/2)cos(X/2)-sin(Y/2)sin(Z/2)sin(X/2)
      q = ((x, y, z), w)
	 */
	private void CreateCar(string path_name){
		isCreating = true;
		//Debug.Log ("should create " +path_name);
		chooseCar ();
		choosePoistion (path_name);
	    //这表明当前是生成在我们驾驶的方向
		if (pathname == "go4" || pathname == "go5") {
			if(space.transform.position.z > end.position.z){//说明已经超出边界
				if(path_name == "go4"){
					StartCoroutine(redo (path_name));
					//Debug.Log("redo go4");
				}else{
					StartCoroutine(redo (path_name));
					//Debug.Log("redo go5");
				}
				isCreating = false;
				//Debug.Log ("reopen");
				return;
			}else{
				newcar [create_number] = (GameObject)Instantiate (createcar, new Vector3 (space.transform.position.x, space.transform.position.y, 
				                                                                          space.transform.position.z), new Quaternion (0, 1, 0, 0));
	//			Debug.Log(path_name);
			}
		} else if (pathname == "go2") {
			if(positionOneOkay){
                if (AllowRightManager.frontCar != null) Destroy(AllowRightManager.frontCar);
				newcar[create_number] = (GameObject)Instantiate(createcar, new Vector3(space.transform.position.x, space.transform.position.y, 
				                                                                       space.transform.position.z), new Quaternion(0, 0, 0, 0));
				AllowRightManager.frontCar = newcar[create_number];
//                Debug.Log("create "+path_name);
            }
            else{
				//Debug.Log("redo go2");
				isCreating = false;
				//Debug.Log ("reopen");
				StartCoroutine(redo (path_name));
				return;
			}

		}else if (pathname == "go3") {
			if(positionTwoOkay){
                if (AllowRightManager.backCar != null) Destroy(AllowRightManager.backCar);
                newcar[create_number] = (GameObject)Instantiate(createcar, new Vector3(space.transform.position.x, space.transform.position.y, 
				                                                                       space.transform.position.z), new Quaternion(0, 0, 0, 0));
                
				AllowRightManager.backCar = newcar[create_number];
              //  Debug.Log(path_name);
            }
            else{
				//Debug.Log("redo go3");
				isCreating = false;
				//Debug.Log ("reopen");
				StartCoroutine(redo (path_name));
				return;
			}
			
		}else {
			newcar[create_number] = (GameObject)Instantiate(createcar, new Vector3(space.transform.position.x, space.transform.position.y, 
			                                                                       space.transform.position.z), new Quaternion(0, 0, 0, 0));
			//Debug.Log(path_name);
		}
		create_number++;
	    StartCoroutine(wait(2));
	}
	private IEnumerator redo(string path){
		yield return new WaitForSeconds (3);
		if (path == "go2") {
			none_Rfrontcar = true;
		} else if (path == "go3") {
			none_backcar = true;
		} else if (path == "go4") {
			none_Rcome = true;
		} else if (path == "go5"){
			none_Lcome = true;
		}
	}

	//make the clonecar be seen
	private void show(){
		//ArrayList-->集合的一种，其中可以放任何类型，不受限制，长度可变，自增加长度
		ArrayList list = new ArrayList ();
		getList (list, newcar[show_number].transform);
		//Debug.Log ("num is "+list.Count);
		for (int i = 0; i< list.Count; i++) {
			Transform trans = (Transform)list[i];
			trans.gameObject.layer = LayerMask.NameToLayer("See");
		}
		show_number ++;
		create_number --;
		if (create_number == 0) {
			show_number = 0;
		}
	}

	//find all child objects in the objects
	private void getList(ArrayList list,Transform car){
		//find all waypoints 
		for (int i=0; i<car.childCount; i++) {
			//Debug.Log(path.transform.GetChild(i).name); 
			Transform trans = car.transform.GetChild(i);
			list.Add(trans);
			if(trans.childCount>0){
				getList(list,trans);
			}
		}
	}
	
	IEnumerator wait(float waitTime){
		yield return new WaitForSeconds(waitTime);
		show ();
		isCreating = false;
		//Debug.Log ("reopen");
	}

	public void reCreate(string name){
	//	Debug.Log("newcar in "+ name);
		StartCoroutine (changestatus (name));
	}

	private IEnumerator changestatus(string name){
		yield return new WaitForSeconds(3);
		if (name == "go1") {
			none_frontcar = true;
		} else if (name == "go2") {
			none_Rfrontcar = true;
		} else if (name == "go3") {
			none_backcar = true;
		} else if (name == "go4") {
			none_Rcome = true;
		} else if (name == "go5") {
			none_Lcome = true;
		}
	}

	public void JudgeCreatePositionOne(bool isok){
		positionOneOkay = isok;
	}

	public void JudgeCreatePositionTwo(bool isok){
		positionTwoOkay = isok;
	}

    public GameObject getOneOfFive(int creatNumber)
    {
        return newcar[creatNumber].transform.GetChild(1).gameObject;
    }

}
