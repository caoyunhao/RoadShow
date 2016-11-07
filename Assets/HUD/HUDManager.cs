using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {
    [SerializeField]
    private Vector3 m_RoadPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInRoadPosition(Vector3 _road_pos) {
        m_RoadPosition = _road_pos;
    }
}
