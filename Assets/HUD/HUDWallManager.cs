using UnityEngine;
using System.Collections;

public class HUDWallManager : MonoBehaviour {

    public GameObject m_Wall;
    public GameObject m_TestCar;
    [SerializeField, Range(-90.0f, 90.0f)]
    private float m_Angle;
    [SerializeField]
    private Color m_WallColor;

    private float m_TestCarHUDWallPos;

    // Use this for initialization
    void Start() {
        m_Angle = 0.0f;
        m_WallColor = Color.red;
        m_TestCarHUDWallPos = 20.0f;
    }

    // Update is called once per frame
    void Update() {
        HUDRotateWall();
        HUDWallColor();
        HUDWallPosition();
    }

    private void HUDWallPosition() {
        var tmp = m_Wall.transform.position;
        m_Wall.transform.position = new Vector3(tmp.x, tmp.y, m_TestCar.transform.position.z + m_TestCarHUDWallPos);
    }

    private void HUDRotateWall() {
        Vector3 tmp = m_Wall.transform.rotation.eulerAngles;
        m_Wall.transform.rotation = Quaternion.Euler(m_Angle, tmp.y, tmp.z);
    }

    private void HUDWallColor() {
        GameObject wall_l = m_Wall.transform.GetChild(0).gameObject;
        GameObject wall_r = m_Wall.transform.GetChild(1).gameObject;
        wall_l.GetComponent<MeshRenderer>().material.color = m_WallColor;
        wall_r.GetComponent<MeshRenderer>().material.color = m_WallColor;
    }

    public void SetHUDWallRotation(float _angle) {
        m_Angle = _angle;
    }

    public void SetHUDWallColor(Color _color) {
        m_WallColor = _color;
    }
}
