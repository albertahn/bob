using UnityEngine;
using System.Collections;

public class SkillCtrl_net2 : MonoBehaviour {
	private MoveCtrl_net2 _moveCtrl;
	private PlayerCtrl_net2 _playerCtrl;
	
	public float normalSpeed,dashSpeed,shootSpeed;

	public float skill1Clock, skill2Clock;
	public bool skill1Bool=true, skill2Bool=true;
	public float skill1Cool=10, skill2Cool=10;

	private float spaceButton = 0.0f;

	public Transform[] SFirePos= new Transform[12];

	public GameObject Sarrow;
	public GameObject wave;

	private Transform tr;
	private PhotonView photonView;

	private TrailRenderer trailrender;
	public AudioClip skill1Sfx;

	public NetworkMgr2 _networkMgr;


	// Use this for initialization
	void Start () {
		_moveCtrl = GetComponent<MoveCtrl_net2> ();
		_playerCtrl = GetComponent<PlayerCtrl_net2> ();
		
		normalSpeed = 7.0f;
		dashSpeed = 20.0f;
		shootSpeed = 3.0f;
		
		tr = GetComponent<Transform> ();
		photonView =GetComponent<PhotonView>();
		_networkMgr = GameObject.Find ("NetworkManager").GetComponent<NetworkMgr2>();

		if(!GetComponent<PhotonView>().isMine)
			this.enabled = false;

		trailrender = GetComponent<TrailRenderer> ();
		trailrender.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID||UNITY_IPHONE
		if(Input.touchCount>0)
		{
			if(Input.touchCount==1){
				Ray ray3 = Camera.main.ScreenPointToRay(Input.touches[0].position);
				RaycastHit hit3;
				
				if(Physics.Raycast(ray3,out hit3,100.0f))
				{
					if(hit3.collider.tag =="SKILL1"){
						preDash();
					}else if(hit3.collider.tag =="SKILL2"){
						preSkill1();
					}
				}
				
			}
			else{
				Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
				Ray ray2 = Camera.main.ScreenPointToRay(Input.touches[1].position);
				
				RaycastHit hit;
				
				if(Physics.Raycast(ray,out hit,100.0f))
				{
					if(hit.collider.tag =="SKILL1"){
						preDash();
					}else if(hit.collider.tag =="SKILL2"){
						preSkill1();
					}
				}
				
				if(Physics.Raycast(ray2,out hit,100.0f))
				{
					if(hit.collider.tag =="SKILL1"){
						preDash();
					}else if(hit.collider.tag =="SKILL2"){
						preSkill1();
					}
				}
			}
		}
		#else
		spaceButton = Input.GetAxis ("Jump");
		
		if (Input.GetMouseButton (1)) {
			preSkill1 ();
		}
		
		if (spaceButton >= 0.1f) {
			preDash();
		}
		#endif

		if (Time.time - skill1Clock > skill1Cool) {
			skill1Bool=true;
		}

		if (Time.time - skill2Clock > skill2Cool) {
			skill2Bool=true;
		}
		if (Time.time - skill2Clock > dashPeriod) {
			if(_moveCtrl.moveSpeed==dashSpeed){
				_moveCtrl.moveSpeed=normalSpeed;
				DeleteDash();
				photonView.RPC("DeleteDash",PhotonTargets.Others);
			}else if(_moveCtrl.moveSpeed==shootSpeed){
				if(!_playerCtrl.fireBool)
					_moveCtrl.moveSpeed=normalSpeed;
			}
		}

	}

	public void preSkill1(){
		if (skill1Bool) {
			skill1Clock = Time.time;
			skill1Bool=false;
			Skill1 ();
			photonView.RPC("Skill1",PhotonTargets.Others);

			/*
			if(_markerCtrl.markerState==true)
			{
				SetSpecial();
			}
			else{
				SetFail();
			}*/
		}
	}
	[RPC]
	public void Skill1(){
		StartCoroutine (this.PlaySfx(skill1Sfx));
		StartCoroutine (this.CreateWave ());
	}
	IEnumerator CreateWave(){		
		foreach (Transform pos in SFirePos) {
			Instantiate (Sarrow, pos.position, pos.rotation);
		}
		Instantiate (wave, tr.position+new Vector3(0,1,0), tr.rotation);
		
		yield return null;
	}


	
	private float dashPeriod=1;
	void preDash(){
		if (skill2Bool) {
			Dash();
			photonView.RPC("Dash",PhotonTargets.Others);
			skill2Clock = Time.time;
			skill2Bool = false;
			_moveCtrl.moveSpeed = dashSpeed;
			
			/*if(_markerCtrl.markerState==true)
			{
				SetSpecial();
			}
			else{
				SetFail();
			}*/
		}
	}
	[RPC]
	public void Dash(){
		trailrender.enabled=true;
	}
	[RPC]
	public void DeleteDash(){
		trailrender.enabled = false;
	}
	IEnumerator PlaySfx(AudioClip _clip){
		if(_networkMgr!=null)
			_networkMgr.PlaySfx (tr.position, _clip);
		yield return null;
	}

}
