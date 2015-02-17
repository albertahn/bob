using UnityEngine;
using System.Collections;

public class PlayerCtrl_robot : MonoBehaviour {
	private float h = 0.0f;
	private float v = 0.0f;
	
	private Transform tr;
	
	public float normalSpeed;
	public float dashSpeed;
	public float shootSpeed;
	
	public float moveSpeed;
	public float rotSpeed;
	private Vector3 target = Vector3.zero;
	
	public int hp = 100;
	public int maxHp = 100;
	
	public delegate void PlayerDieHandler_robot();
	public static event PlayerDieHandler_robot OnPlayerDie;
	public Material trail;
	
	private GameMgr _gameMgr;
	
	private FireCtrl_robot _fireCtrl;
	
	//hitpublic AudioClip hitSound;
	public AudioClip hitSound;
	public Transform auidioPos;
	
	
	[System.Serializable]
	public class Anim{
		public AnimationClip idle;
		public AnimationClip runForward;
		public AnimationClip runBackward;
		public AnimationClip runRight;
		public AnimationClip runLeft;
		//new sword
		public AnimationClip sword_robo;
	}
	
	public Anim anim;
	public Animation _animation;
	private GameObject _mainCamera;
	
	// Use this for initialization
	void Start () {
		
		//init new prefs restart
		PlayerPrefs.SetInt ("mon1",0);
		PlayerPrefs.SetInt ("mon2",0);
		PlayerPrefs.SetInt ("level",0);
		
		PlayerPrefs.SetInt ("skill1_lv",0);
		PlayerPrefs.SetInt ("skill2_lv",0);
		PlayerPrefs.SetInt ("skill_point_left", 10);
		//end
		
		tr = GetComponent<Transform> ();
		_gameMgr = GameObject.Find ("GameManager").GetComponent<GameMgr> ();
		_fireCtrl = GetComponent<FireCtrl_robot> ();
		
		_animation = GetComponentInChildren<Animation> ();
		_animation.clip = anim.idle;
		_animation.Play ();
		_mainCamera = GameObject.Find ("Main Camera");
		
		normalSpeed = 7.0f;
		dashSpeed = 20.0f;
		shootSpeed = 3.0f;
		
		moveSpeed = 7.0f;
		rotSpeed = 100.0f;
	}
	//animate whorle sword
	public void animeSword(){
		
		_animation.clip = anim.sword_robo;
		_animation.Play ();
		
		//Debug.Log ("swords");
		
		//_animation.CrossFade (anim.sword_robo.name);
		
	}
	
	int showSetting =0;
	
	public void OnGUI(){
		
		int kills = PlayerPrefs.GetInt ("mon1")+ PlayerPrefs.GetInt ("mon2");
		
		GUILayout.Label("kills: " + kills);
		
		GUILayout.Label("Lv: " + PlayerPrefs.GetInt ("level"));
		
		
		
		if (GUI.Button (new Rect (Screen.width/22, Screen.height/9, Screen.width/22,Screen.height/22 ), "<color=white><size=30>X</size></color>")) {
			
			showSetting =1;
			
		}//
		
		if (showSetting == 1) {
			if (GUI.Button (new Rect (0, 0, Screen.width/5,Screen.height/5 ), "<color=white><size=30>Exit</size></color>")) {
				
				Application.Quit();
				
			}//
			
			if (GUI.Button (new Rect (0, Screen.height/2, Screen.width/5,Screen.height/5 ), "<color=white><size=30>cancel</size></color>")) {
				
				showSetting =0;
				
			}//
			
		}
		
		
	}//end gui
	
	
	//upskill
	public void upSkillPoint(){
		
		
		
		int skillpoint = PlayerPrefs.GetInt ("skill_point_left");
		
		PlayerPrefs.SetInt ("skill_point_left", skillpoint+1);
		
		
		
	}//upskill
	
	int beforeLevel = 0;
	
	public void checkLevelUp(int currentLV){ //
		
		if (beforeLevel < currentLV) {
			
			upSkillPoint();
			
			beforeLevel = currentLV;
		}
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		int lv1 = 1;
		int lv2 = 3;
		int lv3 = 15;
		int lv4 = 20;
		int lv5 = 25;
		int lv6 = 30;
		int lv7 = 35;
		
		
		
		int kills = PlayerPrefs.GetInt ("mon1") + PlayerPrefs.GetInt ("mon2");
		
		if (lv1 < kills && kills < lv2) {
			
			PlayerPrefs.SetInt ("level", 1);
			checkLevelUp (1);
			
			
		} else if (lv2 < kills && kills < lv3) {
			
			PlayerPrefs.SetInt ("level", 2);
			checkLevelUp (2);
			
			
		} else if (lv3 < kills && kills < lv4) {
			
			PlayerPrefs.SetInt ("level", 3);
			checkLevelUp (3);
			
		} else if (lv4 < kills && kills < lv5) {
			
			PlayerPrefs.SetInt ("level", 4);
			checkLevelUp (4);
			
		} else if (lv5 < kills && kills < lv6) {
			
			PlayerPrefs.SetInt ("level", 5);
			checkLevelUp (5);
			
		} else if (lv6 < kills && kills < lv7) {
			
			PlayerPrefs.SetInt ("level", 6);
			checkLevelUp (6);
			
		} else if (lv7 < kills) {
			
			PlayerPrefs.SetInt ("level", 7);
			checkLevelUp (7);
			
		}
		//end levels
		
		
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");
		
		
		Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
		tr.Translate (moveDir * Time.deltaTime * moveSpeed, Space.World);
		
		
		#if UNITY_ANDROID||UNITY_IPHONE
		if (Input.touchCount > 0) {
			if (Input.touchCount == 1&&Input.GetTouch(0).phase==TouchPhase.Ended) {
				
				Ray ray3 = Camera.main.ScreenPointToRay (Input.touches [0].position);
				RaycastHit hit3;
				
				if (Physics.Raycast (ray3, out hit3, 100.0f)) {
					if (hit3.collider.tag == "SKILL1") {
						_fireCtrl.Dash ();
					} else if (hit3.collider.tag == "SKILL2") {
						_fireCtrl.SFire ();
					} else if (hit3.collider.tag == "SKILL1_UP") {
						
						int skillpoint = PlayerPrefs.GetInt ("skill_point_left");
						if (skillpoint > 0) { //has points
							int skill1lv = PlayerPrefs.GetInt ("skill1_lv");
							PlayerPrefs.SetInt ("skill1_lv", skill1lv + 1);
							PlayerPrefs.SetInt ("skill_point_left", skillpoint - 1);
							
						}
						
						
					} else if (hit3.collider.tag == "SKILL2_UP") {
						
						
						int skillpoint = PlayerPrefs.GetInt ("skill_point_left");
						if (skillpoint > 0) {
							int skill2lv = PlayerPrefs.GetInt ("skill2_lv");
							PlayerPrefs.SetInt ("skill2_lv", skill2lv + 1);
							PlayerPrefs.SetInt ("skill_point_left", skillpoint - 1);
						}
						
						
					}
				}
				
			} else { //in2
				Ray ray = Camera.main.ScreenPointToRay (Input.touches [0].position);
				Ray ray2 = Camera.main.ScreenPointToRay (Input.touches [1].position);
				
				RaycastHit hit;
				
				if (Physics.Raycast (ray, out hit, 100.0f)&&Input.GetTouch(0).phase==TouchPhase.Ended) {
					if (hit.collider.tag == "SKILL1") {
						_fireCtrl.Dash ();
					} else if (hit.collider.tag == "SKILL2") {
						_fireCtrl.SFire ();
					} else if (hit.collider.tag == "SKILL1_UP") {
						
						int skillpoint = PlayerPrefs.GetInt ("skill_point_left");
						if (skillpoint > 0) { //has points
							int skill1lv = PlayerPrefs.GetInt ("skill1_lv");
							PlayerPrefs.SetInt ("skill1_lv", skill1lv + 1);
							PlayerPrefs.SetInt ("skill_point_left", skillpoint - 1);
							
						}
						
						
					} else if (hit.collider.tag == "SKILL2_UP") {
						
						
						int skillpoint = PlayerPrefs.GetInt ("skill_point_left");
						if (skillpoint > 0) {
							int skill2lv = PlayerPrefs.GetInt ("skill2_lv");
							PlayerPrefs.SetInt ("skill2_lv", skill2lv + 1);
							PlayerPrefs.SetInt ("skill_point_left", skillpoint - 1);
						}
						
						
					}
				}//[jy
				
				if (Physics.Raycast (ray2, out hit, 100.0f)&&Input.GetTouch(1).phase==TouchPhase.Ended) {
					if (hit.collider.tag == "SKILL1") {
						_fireCtrl.Dash ();
					} else if (hit.collider.tag == "SKILL2") {
						_fireCtrl.SFire ();
					} else if (hit.collider.tag == "SKILL1_UP") {
						
						int skillpoint = PlayerPrefs.GetInt ("skill_point_left");
						if (skillpoint > 0) { //has points
							int skill1lv = PlayerPrefs.GetInt ("skill1_lv");
							PlayerPrefs.SetInt ("skill1_lv", skill1lv + 1);
							PlayerPrefs.SetInt ("skill_point_left", skillpoint - 1);
							
						}
						
						
					} else if (hit.collider.tag == "SKILL2_UP") {
						
						
						int skillpoint = PlayerPrefs.GetInt ("skill_point_left");
						if (skillpoint > 0) {
							int skill2lv = PlayerPrefs.GetInt ("skill2_lv");
							PlayerPrefs.SetInt ("skill2_lv", skill2lv + 1);
							PlayerPrefs.SetInt ("skill_point_left", skillpoint - 1);
						}
						
						
					}
				}//if
			}//click2
			
			
			
			
			
			
			
		}//if touch end
		#else
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 100.0f, Color.green);
		
		RaycastHit hit;
		
		if(Physics.Raycast (ray, out hit, Mathf.Infinity)){
			target = hit.point;
			target.y = 0;
			tr.LookAt(target);
			
			//Debug.Log("UPSKILL1");
			
			if(hit.collider.tag=="SKILL1_UP"){
				
				Debug.Log("SKILL1_UP");
				
				if (Input.GetMouseButtonUp (0)) {
					
					
					if(PlayerPrefs.GetInt("skill_point_left")>0){
						
						int skill1lv = PlayerPrefs.GetInt("skill1_lv");
						PlayerPrefs.SetInt("skill1_lv", skill1lv+1);
						
						int skillpoint = PlayerPrefs.GetInt("skill_point_left");
						
						PlayerPrefs.SetInt("skill_point_left", skillpoint-1);
						
					}//if skill left
					
					
				}//mouse 
				
			}//endskill1
			//2
			if(hit.collider.tag=="SKILL2_UP"){ 
				
				Debug.Log("skill2");
				
				if (Input.GetMouseButtonUp (0)) {
					if(PlayerPrefs.GetInt("skill_point_left")>0){
						
						int skill1lv = PlayerPrefs.GetInt("skill2_lv");
						
						PlayerPrefs.SetInt("skill2_lv", skill1lv+1);
						
						int skillpoint = PlayerPrefs.GetInt("skill_point_left");
						
						PlayerPrefs.SetInt("skill_point_left", skillpoint-1);
					}
					
				} 
				
			}//end sk2
			
			
		}
		
		
		if (v >= 0.1f) {
			_animation.CrossFade (anim.runForward.name);
			
			//sword
			
			//_animation.CrossFade (anim.sword_robo.name);
			
		} else if (v <= -0.1f) {
			_animation.CrossFade (anim.runBackward.name);
		} else if (h >= 0.1f) {
			_animation.CrossFade (anim.runRight.name);
		} else if (h <= -0.1f) {
			_animation.CrossFade (anim.runLeft.name);
		} else {
			_animation.CrossFade (anim.idle.name);
		}//
		#endif
	}//end update
	
	
	
	void joystick_x_procss(float x){
		if (x >= 0.1f) {
			_animation.CrossFade (anim.runRight.name);
		} else if (x <= -0.1f) {
			_animation.CrossFade (anim.runLeft.name);
		}
	}
	
	void joystick_y_procss(float y){
		if (y >= 0.1f) {
			_animation.CrossFade (anim.runForward.name);
		} else if (y <= -0.1f) {
			_animation.CrossFade (anim.runBackward.name);
		}
	}
	void joystick_idle(){
		_animation.CrossFade (anim.idle.name);
	}
	
	
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "PUNCH") {
			hp -= 10;
			
			StartCoroutine (this.PlayClip(hitSound));
		} else if (coll.gameObject.tag == "ARROW_ENEMY") {
			hp -= 10;
			Destroy (coll.gameObject);
			
			StartCoroutine (this.PlayClip(hitSound));
		}
		
		if (hp <= 0) {
			hp = 0;
			//OnPlayerDie ();
			PlayerDie();
			_gameMgr.isGameOver = true;
			
			
			Application.LoadLevel ("DieScene");
			
		}
	}
	
	void PlayerDie(){
		
		GameObject[] monsters = GameObject.FindGameObjectsWithTag ("MONSTER");
		
		GameObject[] monsters2 = GameObject.FindGameObjectsWithTag ("MONSTER2");
		
		foreach (GameObject monster in monsters){
			monster.SendMessage ("OnPlayerDie",SendMessageOptions.DontRequireReceiver);
		}
		
		foreach (GameObject monster in monsters2){
			monster.SendMessage ("OnPlayerDie",SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void GetSpeed(){
		_mainCamera.SendMessage ("GetSpeed",moveSpeed,SendMessageOptions.DontRequireReceiver);
	}
	
	
	IEnumerator PlayClip(AudioClip _clip){
		//audio.PlayOneShot (_clip, 0.9f);
		_gameMgr.PlaySfx (auidioPos.position, _clip);
		yield return null;
	}//end play
	
	
}
