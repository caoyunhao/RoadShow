using UnityEngine;
using System.Collections;

public class CreateCar : MonoBehaviour {

	public GameObject targetbackball;
	public GameObject targetfrontball;
	public GameObject Truck;
	public GameObject Pick;
	//private string exeEnvieonment = "Terrain1";//judge which terrain will execute the script
	//public GameObject Bus;
	private string path_name;
	public enum Carstatus {nocar,frontcar,behindcar,both};
		// Use this for initialization
	public Carstatus status;
	private int numberstatus;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.C)) {
			//when turn a road
			if(status == Carstatus.nocar){//there is no car on the road
				numberstatus = 0;
			}else if (status == Carstatus.frontcar){//there is a front car
				chooseway (0);
				numberstatus = 1;
			}else if (status == Carstatus.behindcar){//there is a behind car
				chooseway (1);
				numberstatus = 2;
			}else if (status == Carstatus.both){//there are both cars
				chooseway (0);
				chooseway (1); 
				numberstatus = 3;
			}
		}
	}

	public int getcarstatus(){
		return numberstatus;
	}

    void chooseway(int n)
    {
        switch (n)
        {
            case 1://goto 
                /*AllowRightManager.pick = (GameObject)*/Instantiate(Pick, new Vector3(targetfrontball.transform.position.x, targetfrontball.transform.position.y, targetfrontball.transform.position.z), new Quaternion(0, 0, 0, 0));//最后那个是一个四元数用于控制车的朝向  
                //AllowRightManager.creatPick = true;
                //wait for 1s to execute the function wait(float ,string);
                StartCoroutine(wait(1, "MovePick"));
                break;
            case 0:  //come
                /*AllowRightManager.truck = (GameObject)*/Instantiate(Truck, new Vector3(targetbackball.transform.position.x, targetbackball.transform.position.y, targetbackball.transform.position.z), new Quaternion(0, 0, 0, 0));
                //AllowRightManager.creatTruck = true;
                StartCoroutine(wait(1, "MoveTruck"));
                break;
        }
    }
	//make the clonecar be seen
	private void show(string name){
		//Debug.Log ("Show Car"); 
		GameObject car;
	    car = GameObject.Find(name+"(Clone)");
		//ArrayList-->集合的一种，其中可以放任何类型，不受限制，长度可变，自增加长度
		ArrayList list = new ArrayList ();
		getList (list, car.transform);
		//Debug.Log ("num is "+list.Count);
		for (int i = 0; i< list.Count; i++) {
			Transform trans = (Transform)list[i];
			trans.gameObject.layer = LayerMask.NameToLayer("See");
		}
		//Pick.layer = LayerMask.NameToLayer("Default");
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

	IEnumerator wait(float waitTime,string name){
		yield return new WaitForSeconds(waitTime);
		show (name);
	}
}