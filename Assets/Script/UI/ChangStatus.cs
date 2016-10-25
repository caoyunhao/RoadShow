using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ChangStatus : MonoBehaviour {

	
	private int sd ;
	public Text speed;
	public GameObject arrow;
	private int angle;
	private Vector3 rotation;
	private GameObject car_top;
	private GameObject car_bottom;
	private float top_y;
	private float last_top_y;
	private float bottom_y;
	private float last_bottom_y;
	//public int r;
	// Use this for initialization
	void Start () {
		car_top = GameObject.Find ("Car/SUV/DashBoard/CarRighttop");
		car_bottom = GameObject.Find ("Car/SUV/DashBoard/CarRightbottom");
		rotation = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		float curspeed = GameObject.Find ("SUV").GetComponent<CarControl> ().getSpeed ();
		float percentage = (1 - curspeed / 20);
		rotation.z = -233 * percentage;
		speed.text = (int)curspeed * 9 + " KM/H";
		arrow.transform.localEulerAngles = rotation;
		car_top.GetComponent<RectTransform> ().localPosition = new Vector3 (car_top.transform.localPosition.x, top_y, car_top.transform.localPosition.z);
		car_bottom.GetComponent<RectTransform> ().localPosition = new Vector3 (car_top.transform.localPosition.x, bottom_y, car_top.transform.localPosition.z);
		//后车在下面刚出来
		if (bottom_y > -47 && bottom_y < -16) {
			car_bottom.GetComponent<Image> ().fillOrigin = (int)Image.OriginVertical.Top;
			float p = Mathf.Abs (((bottom_y + 47) * 1.0f) / (-47 + 29));
		//	Debug.Log("p is "+p);
			car_bottom.GetComponent<Image> ().fillAmount = p;
		} else if (bottom_y >= -16 && bottom_y < 0) {//从上面消失
			car_bottom.GetComponent<Image> ().fillOrigin = (int)Image.OriginVertical.Bottom;
			float p = 1 - Mathf.Abs (((bottom_y + 16) * 1.0f) / 16);
			//			Debug.Log("p is "+p);
			car_bottom.GetComponent<Image> ().fillAmount = p;
		} else {
			car_bottom.GetComponent<Image> ().fillAmount = 0.0f;
		}

		//前车在上面刚出来
		if (top_y > -29 && top_y < 0) {
			car_top.GetComponent<Image> ().fillOrigin = (int)Image.OriginVertical.Bottom;
			float p = 1 - Mathf.Abs (((top_y + 16) * 1.0f) / 16);
			//			Debug.Log("p is "+p);
			car_top.GetComponent<Image> ().fillAmount = p;
		} else if (top_y > -47 && top_y <= -29) {//从下面消失
			car_bottom.GetComponent<Image> ().fillOrigin = (int)Image.OriginVertical.Top;
			float p = Mathf.Abs (((top_y + 47) * 1.0f) / (-47 + 29));
			//			Debug.Log("p is "+p);
			car_top.GetComponent<Image> ().fillAmount = p;
		} else {
			car_top.GetComponent<Image> ().fillAmount = 0;
		}
		last_top_y = top_y;
		last_bottom_y = bottom_y;
	}

	public void settoppostion(float y){
		top_y = y;
	}

	public void setbottompostion(float y){
		bottom_y = y;
	}

	public void settopcolor(Color changecolor){
		car_top.GetComponent<Image>().color = changecolor;
	}

	public void setbottomcolor(Color changecolor){
		car_bottom.GetComponent<Image>().color = changecolor;
	}
}
