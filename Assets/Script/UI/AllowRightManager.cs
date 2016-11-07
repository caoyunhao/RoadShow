using UnityEngine;
using System.Collections;

public class AllowRightManager : MonoBehaviour {

    public float distanceOfFront = 50.0f;
    public float distanceOfBack = 80.0f;

    private GameObject groundGreen;//地上显示绿色的GameObject
    private Renderer greenRenderer;
    private GameObject groundRed;//地上显示红色的GameObject
    private Renderer redRenderer;

    private GameObject SUVBan;//禁止右转标志的GameObject

    private Renderer rendererAllow;//允许右转的标志
    private Renderer rendererBan;//禁止右转的标志

    private GameObject suv;
    static public GameObject frontCar;//前车
    static public GameObject backCar;//后车
    static public bool creatPick = false;
    static public bool creatTruck = false;

    static bool wantChange = false;
    static bool isAllowed = false;//显示是否允许换道

    GameObject[] newCar = new GameObject[5];

    void Start() {
        groundGreen = GameObject.Find("AllowThrough");
        greenRenderer = groundGreen.GetComponent<Renderer>();

        groundRed = GameObject.Find("NoThrough");
        redRenderer = groundRed.GetComponent<Renderer>();

        SUVBan = GameObject.Find("Car/SUV/Ban");
        rendererBan = SUVBan.GetComponent<Renderer>();

        rendererAllow = GetComponent<Renderer>();

        suv = GameObject.Find("Car/SUV");
        //pick = GameObject.Find("MovePick" + "(Clone)");
        //truck = GameObject.Find("MovePick" + "(Clone)");
    }

    void FixedUpdate() {

        if (Input.GetKey(KeyCode.Alpha1))//想换道，请按1
            wantChange = true;
        if (Input.GetKey(KeyCode.Alpha2)) {//取消换道，请按2
            wantChange = false;
            GameObject.Find("Car/SUV/DashBoard/ColorPanel").GetComponent<gradual>().setColor(20, 240, 60, 220, 180);
        }
        //改变Canvas小车的位置
        UICarShow();
        if (wantChange) {//如果想换道
            isAllowed = CheckThrough();
            if (isAllowed) {//如果允许
                ShowGreen();
                CloseRed();
                GameObject.Find("Car/SUV/DashBoard").GetComponent<ChangStatus>().setbottomcolor(Color.white);
                GameObject.Find("Car/SUV/DashBoard").GetComponent<ChangStatus>().settopcolor(Color.white);
            }
            else {//不允许
                ShowRed();
                CloseGreen();
            }
        }
        else {
            CloseGreen();
            CloseRed();
        }
    }

    private void UICarShow() {
        float suvZ = suv.transform.position.z;
        float front_car_z = -100000.0f;
        float back_car_z = -1000000.0f;
        float y_top = 0f;
        float y_bottom = -50f;
        if (frontCar != null) {
            front_car_z = frontCar.transform.GetChild(1).position.z;
        }
        if (backCar != null) {
            back_car_z = backCar.transform.GetChild(1).position.z;
        }
        if (Mathf.Abs(front_car_z - suvZ) < 30) {
            y_top = -25 + (front_car_z - suvZ);
            GameObject.Find("Car/SUV/DashBoard").GetComponent<ChangStatus>().settoppostion(y_top);
        }

        if (Mathf.Abs(back_car_z - suvZ) < 30) {
            y_bottom = -25 + (back_car_z - suvZ);
            //	Debug.Log("y is "+y_bottom);
            GameObject.Find("Car/SUV/DashBoard").GetComponent<ChangStatus>().setbottompostion(y_bottom);
        }

    }

    bool CheckThrough()//检测能否变道
    {
        float suvZ = suv.transform.position.z;
        float front_car_z = -100000.0f;
        float back_car_z = -1000000.0f;
        float suvVz = suv.transform.GetComponent<Rigidbody>().velocity.z;
        float front_car_Vz = 0.0f;
        float back_car_Vz = 0.0f;
        bool front_car_Ok = false;
        bool back_car_Ok = false;
        //Debug.Log("suvVz = " + suvVz);
        //Debug.Log("pickVz = " + pickVz);
        //Debug.Log("truckVz = " + truckVz);
        if (frontCar != null) {
            front_car_z = frontCar.transform.GetChild(1).position.z;
            front_car_Vz = frontCar.transform.GetChild(1).GetComponent<Rigidbody>().velocity.z;
            //          Debug.Log(front_car_Vz);
        }
        if (backCar != null) {
            back_car_z = backCar.transform.GetChild(1).position.z;
            back_car_Vz = backCar.transform.GetChild(1).GetComponent<Rigidbody>().velocity.z;
            //            Debug.Log(back_car_Vz);
        }
        int red1_end = 0;
        int red2_end = 240;
        int yellow1_end = 0;
        int yellow2_end = 0;
        int eyan_end = 0;
        int whole_mesh = 260;
        int max_speed = 180;
        front_car_Vz *= 9;
        suvVz *= 9;
        back_car_Vz *= 9;
        //总的mesh数是246个，从右到左,显示最小的速度在220处
        if (suvZ - front_car_z > distanceOfBack) {//在前车前面
            front_car_Ok = true;
            red1_end = 0;
            red2_end = whole_mesh;
            yellow2_end = whole_mesh;
            yellow1_end = 0;
            eyan_end = whole_mesh;
        }
        else if (front_car_z - suvZ > distanceOfFront) {//在前车后面
            front_car_Ok = true;
            red1_end = 0;
            yellow1_end = 0;
        }
        else {
            if (front_car_z - suvZ > 30) {//在前车的后面
                if (front_car_Vz > suvVz) {//速度小于前车
                    front_car_Ok = true;
                }
                else {//速度大于前车
                    front_car_Ok = false;
                    GameObject.Find("Car/SUV/DashBoard").GetComponent<ChangStatus>().settopcolor(Color.red);
                }
                red1_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * front_car_Vz);
                yellow1_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * front_car_Vz * 0.7);
            }
            else if (suvZ - front_car_z > 30) {//在前车的前面
                if (front_car_Vz < suvVz) {//速度大于前车
                    front_car_Ok = true;
                }
                else {//速度小于前车
                    front_car_Ok = false;
                    GameObject.Find("Car/SUV/DashBoard").GetComponent<ChangStatus>().settopcolor(Color.red);
                }
                red2_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * front_car_Vz);//最小警戒速度是前车的速度
                yellow2_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * front_car_Vz * 1.2);
                red1_end = 0; //最大速度是限速
                yellow1_end = 0;
                eyan_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * front_car_Vz * 1.4);
            }
        }

        if (suvZ - back_car_z > distanceOfBack || back_car_z - suvZ > distanceOfFront) {//在后车前面
            back_car_Ok = true;
            red2_end = whole_mesh;
            yellow2_end = whole_mesh;
            eyan_end = whole_mesh;
        }
        else if (back_car_z - suvZ > distanceOfFront) {//在后车后面
            back_car_Ok = true;
            red1_end = 0;
            red2_end = whole_mesh;
            yellow2_end = whole_mesh;
            yellow1_end = 0;
            eyan_end = whole_mesh;
        }
        else {
            if (back_car_z - suvZ > 30) {//在后车的后面
                if (back_car_Vz > suvVz) {
                    back_car_Ok = true;
                }
                else {
                    back_car_Ok = false;
                    GameObject.Find("Car/SUV/DashBoard").GetComponent<ChangStatus>().setbottomcolor(Color.red);
                }
                red1_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * back_car_Vz);//最大警戒速度是后车的速度
                yellow1_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * back_car_Vz * 0.7);
                red2_end = whole_mesh; //最小速度是0
                yellow2_end = whole_mesh;
                eyan_end = whole_mesh;
            }
            else {//在后车的前面
                if (back_car_Vz < suvVz) {
                    back_car_Ok = true;
                }
                else {
                    back_car_Ok = false;
                    GameObject.Find("Car/SUV/DashBoard").GetComponent<ChangStatus>().setbottomcolor(Color.red);
                }
                red2_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * back_car_Vz); //最小速度是后车的速度
                yellow2_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * back_car_Vz * 1.2);
                eyan_end = whole_mesh - (int)((whole_mesh / max_speed) * 1.0 * back_car_Vz * 1.4);
            }
        }
        GameObject.Find("Car/SUV/DashBoard/ColorPanel").GetComponent<gradual>().setColor(red1_end, red2_end, yellow1_end, yellow2_end, eyan_end);
        return back_car_Ok && front_car_Ok;
    }

    bool ShowGreen() {
        greenRenderer.enabled = true;
        rendererAllow.enabled = true;
        return true;
    }

    bool ShowRed() {
        redRenderer.enabled = true;
        rendererBan.enabled = true;
        return true;
    }

    bool CloseGreen() {
        greenRenderer.enabled = false;
        rendererAllow.enabled = false;
        return true;
    }

    bool CloseRed() {
        redRenderer.enabled = false;
        rendererBan.enabled = false;
        return true;
    }
}