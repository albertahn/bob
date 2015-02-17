using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl_robot : MonoBehaviour {
	public GameObject arrow;
	public GameObject Sarrow;
	public GameObject wave;
	public GameObject boom;
	public Transform firePos;
	public Transform[] SFirePos= new Transform[12];
	public AudioClip fireSfx;
	//specialaudio
	public AudioClip spfireSfx;
	
	private Transform tr;
	
	public float time,time2,time3,time4;//time은 기본공격, time2는 특수공격.
	private float basicAttackCool=0.1f;
	public float specialAttackCool = 10;
	private float specialAttack2Cool = 1;
	public float quickCool=10.0f;
	
	private GameMgr _gameMgr;
	
	private float space = 0.0f;
	private PlayerCtrl_robot playerCtrl;
	
	public bool skillBool1;
	public bool skillBool2;
	private float dashPeriod=1;
	
	// Use this for initialization
	// Update is called once per frame
	
	
	void Start(){
		time = Time.time;
		_gameMgr = GameObject.Find ("GameManager").GetComponent<GameMgr>();
		tr = GetComponent<Transform> ();
		playerCtrl = GetComponent<PlayerCtrl_robot> ();
		skillBool1 = true;
		skillBool2 = true;
		
		
		//anime
		
		
	}
	
	void Update () {
		#if UNITY_ANDROID||UNITY_IPHONE
		#else
		space = Input.GetAxis ("Jump");
		
		if (Input.GetMouseButton (0)) {
			Fire ();
		} 
		else if (Input.GetMouseButton (1)) {
			
			
			SFire ();
		} 
		else if (Input.GetMouseButton (2)) {
			if(Time.time - time3>specialAttack2Cool){
				time3 = Time.time;
				S2Fire();
			}
		}else{
			fireBool=false;
		}
		
		if (space >= 0.1f) {
			Dash();
		}
		#endif
		//level dash perdiod
		dashPeriod = dashPeriod;
		
		if (Time.time - time4 > dashPeriod+PlayerPrefs.GetInt("skill1_lv")) {
			
			//Debug.Log (""+dashPeriod);
			if(playerCtrl.moveSpeed==playerCtrl.dashSpeed){
				playerCtrl.moveSpeed=playerCtrl.normalSpeed;
				TrailRenderer trailrender = null;
				trailrender = GetComponent<TrailRenderer>();
				if(trailrender!=null)
					Destroy(trailrender);
			}else if(playerCtrl.moveSpeed==playerCtrl.shootSpeed){
				if(!fireBool)		
					playerCtrl.moveSpeed=playerCtrl.normalSpeed;
			}
		}
		
		if (Time.time - time4 > quickCool) {
			skillBool1=true;
		}
		
		if (Time.time - time2 > specialAttackCool) {
			skillBool2=true;
		}
		
	}//end
	
	
	
	
	
	public void Dash(){
		
		
		if(skillBool1){
			time4 = Time.time;
			skillBool1=false;
			playerCtrl.moveSpeed = playerCtrl.dashSpeed;
			if(GetComponent<TrailRenderer>()==null)
				gameObject.AddComponent<TrailRenderer>().material=playerCtrl.trail;
		}
		
	}
	
	
	private bool fireBool;
	public void Fire(){
		
		
		
		fireBool = true;
		if(playerCtrl.moveSpeed!=playerCtrl.dashSpeed)
			playerCtrl.moveSpeed = playerCtrl.shootSpeed;
		if (Time.time - time > basicAttackCool) {
			time = Time.time;
			StartCoroutine(this.CreateArrow());
			StartCoroutine (this.PlaySfx(fireSfx));
		}
	}
	void No_Fire(){
		fireBool = false;
	}
	
	public void SFire(){
		if (skillBool2) {
			time2 = Time.time;
			skillBool2=false;
			StartCoroutine (this.CreateWave ());
			//specialsound 
			StartCoroutine (this.PlaySfx(spfireSfx));
		}
	}
	private	void S2Fire(){
		StartCoroutine (this.CreateBoom());
	}
	IEnumerator CreateArrow(){
		Instantiate (arrow, firePos.position, firePos.rotation);
		yield return null;
	}
	
	IEnumerator CreateWave(){
		
		
		playerCtrl.animeSword ();
		foreach (Transform pos in SFirePos) {
			Instantiate (Sarrow, pos.position, pos.rotation);
		}
		Instantiate (wave, tr.position+new Vector3(0,1,0), firePos.rotation);
		
		
		
		yield return null;
	}
	
	IEnumerator CreateBoom(){
		Instantiate (boom, firePos.position, firePos.rotation);
		yield return null;
	}
	
	IEnumerator PlaySfx(AudioClip _clip){
		//audio.PlayOneShot (_clip, 0.9f);
		_gameMgr.PlaySfx (firePos.position, _clip);
		yield return null;
	}
	
	void SetSpecial(){
		specialAttackCool = 1;
		quickCool = 1;
	}
	void SetFail(){
		specialAttackCool = 10.0f;
		quickCool = 10.0f;
	}
}
