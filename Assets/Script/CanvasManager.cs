using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour {
    public Image mCar;
    public Image frontCar;
    public Image backCar;

    float front_change = 0;
    float back_change = 0;

    float destoryDistance = 100.0f;

    void Start () {
	    
	}
	
	void FixedUpdate () {
//        front_change = (float)2.5 * (GetComponent<CreateCars>().GetDistanceFront());
//       back_change = (float)2.5 * (GetComponent<CreateCars>().GetDistanceBack());
        CheckEdge();
        frontCar.rectTransform.position = new Vector3(frontCar.rectTransform.position.x, mCar.rectTransform.position.y + front_change, 0);
        backCar.rectTransform.position = new Vector3(backCar.rectTransform.position.x, mCar.rectTransform.position.y + back_change, 0);
    }

    void CheckEdge() {
        if(front_change > destoryDistance || front_change <  0 - destoryDistance) {
            frontCar.enabled = false;
        }
        else {
            frontCar.enabled = true;
        }
        if (back_change > destoryDistance || back_change < 0  - destoryDistance) {
            backCar.enabled = false;
        }
        else {
            backCar.enabled = true;
        }
    }

    public void SetFrontChange(float change) {
        front_change = change;
    }

    public void SetBackChange(float change) {
        back_change = change;
    }
}
