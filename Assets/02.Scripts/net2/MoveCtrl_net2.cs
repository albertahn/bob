using UnityEngine;
using System.Collections;

public class MoveCtrl_net2 : MonoBehaviour {
	
	private float h = 0.0f;
	private float v = 0.0f;
	public float moveSpeed;
	private GameObject _mainCamera;

	private Transform tr;

	// Use this for initialization

	void Start () {
		_mainCamera = GameObject.Find ("Main Camera");
		tr = GetComponent<Transform> ();
		moveSpeed = 7.0f;
		if(!GetComponent<PhotonView>().isMine)
			this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
						h = Input.GetAxis ("Horizontal");
						v = Input.GetAxis ("Vertical");
				
						Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
						tr.Translate (moveDir * Time.deltaTime * moveSpeed, Space.World);
		
		#if UNITY_ANDROID||UNITY_IPHONE
		#else
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 100.0f, Color.green);
		
		RaycastHit hit;

		Vector3 target=Vector3.zero;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
			target = hit.point;
			target.y = 0;//something new
			tr.LookAt(target);
		}

		if(h!=0||v!=0){
			this.SendMessage("joystick_x_procss",h,SendMessageOptions.DontRequireReceiver);
			this.SendMessage("joystick_y_procss",v,SendMessageOptions.DontRequireReceiver);
		}else{
			this.SendMessage("joystick_idle",SendMessageOptions.DontRequireReceiver);
		}
		#endif

	}

	void GetSpeed(){
		_mainCamera.SendMessage ("GetSpeed",moveSpeed,SendMessageOptions.DontRequireReceiver);
	}

}
