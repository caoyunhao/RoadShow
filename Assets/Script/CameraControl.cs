using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Transform target;//要跟踪的车
	private Rigidbody car;
	private float move_speed = 3.0f;//跟踪的速度
	private Transform Camera;
	//private Transform m_target;
	private float minspeed = 4f;//摄像机跟踪车辆时车运行的最小速度
	private float m_SpinTurnLimit=90f;
	private float m_CurrentTurnAmount;
	private float m_TurnSpeedVelocityChange;
	private float m_LastFlatAngle;
	private float m_TurnSpeed = 1f;        
	private Vector3 m_RollUp = Vector3.up;
	private float m_RollSpeed = 0.2f;
	// Use this for initialization
	void Start () {
		//蝴蝶摄像机
		Camera = GetComponentInChildren<Camera> ().transform;
		//获得中间跟踪物体
		//m_target = Camera.parent;
		car = target.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		FollowTarget ();
	}

	void FollowTarget(){
		   // initialise some vars, we'll be modifying these in a moment
		   var targetForward = target.forward;
		   var targetUp = target.up;//绿色轴的方向
			// we're in 'follow rotation' mode, where the camera rig's rotation follows the object's rotation.
			
			// This section allows the camera to stop following the target's rotation when the target is spinning too fast.
			// eg when a car has been knocked into a spin. The camera will resume following the rotation
			// of the target when the target's angular velocity slows below the threshold.
			//从当前位置到目标位置的旋转角度
			var currentFlatAngle = Mathf.Atan2(target.forward.x, target.forward.z)*Mathf.Rad2Deg;
			if (m_SpinTurnLimit > 0)
			{
				//Mathf.DeltaAngle 计算当前角度和之前的角度的最小的差异值
				var targetSpinSpeed = Mathf.Abs(Mathf.DeltaAngle(m_LastFlatAngle, currentFlatAngle))/Time.deltaTime;
				var desiredTurnAmount = Mathf.InverseLerp(m_SpinTurnLimit, m_SpinTurnLimit*0.75f, targetSpinSpeed);
				var turnReactSpeed = (m_CurrentTurnAmount > desiredTurnAmount ? 0.1f : 1f);
				if (Application.isPlaying)
				{
					m_CurrentTurnAmount = Mathf.SmoothDamp(m_CurrentTurnAmount, desiredTurnAmount,
					                                       ref m_TurnSpeedVelocityChange, turnReactSpeed);
				}
				else
				{
					// for editor mode, smoothdamp won't work because it uses deltaTime internally
					m_CurrentTurnAmount = desiredTurnAmount;
				}
			}
			else
			{
				m_CurrentTurnAmount = 1;
			}
			m_LastFlatAngle = currentFlatAngle;
		
		// camera position moves towards target position:
		transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime*move_speed);
		
		// camera's rotation is split into two parts, which can have independend speed settings:
		// rotating towards the target's forward direction (which encompasses its 'yaw' and 'pitch')
		/*if (!m_FollowTilt)
		{
			targetForward.y = 0;
			if (targetForward.sqrMagnitude < float.Epsilon)
			{
				targetForward = transform.forward;
			}
		}*/
		var rollRotation = Quaternion.LookRotation(target.forward, m_RollUp);
		
		// and aligning with the target object's up direction (i.e. its 'roll')
		m_RollUp = m_RollSpeed > 0 ? Vector3.Slerp(m_RollUp, targetUp, m_RollSpeed*Time.deltaTime) : Vector3.up;
		transform.rotation = Quaternion.Lerp(transform.rotation, rollRotation, m_TurnSpeed*m_CurrentTurnAmount*Time.deltaTime);
	}
}
