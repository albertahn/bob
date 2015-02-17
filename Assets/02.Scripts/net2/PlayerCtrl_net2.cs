using UnityEngine;
using System.Collections;

public class PlayerCtrl_net2 : MonoBehaviour {
	private Transform tr;
	private PhotonView photonView;

	private Vector3 currPos = Vector3.zero;
	private Quaternion currRot = Quaternion.identity;

	public GameObject arrow;
	public Transform firePos;
		
	public enum AnimState{idle=0,runForward,runBackward,runRight,runLeft}
	public AnimState animState = AnimState.idle;
	public AnimationClip[] animClips;
	private Animation _animation;

	public GameObject bloodEffect;
	public GameObject bloodDecal;

	public bool fireBool;

	private bool isDie = false;
	public int maxHp = 100;
	public int hp = 100;
	private float respawnTime = 3.0f;

	private MoveCtrl_net2 _moveCtrl;
	private SkillCtrl_net2 _skillCtrl;

	public NetworkMgr2 _networkMgr;
	public AudioClip fireSfx;

	void Awake(){
		tr = GetComponent<Transform> ();
		photonView =GetComponent<PhotonView>();
		_animation = GetComponentInChildren<Animation>();
	}
	void Start(){
		basicClock = Time.time;
		basicAttackCool=0.1f;
		fireBool = false;

		isDie = false;
		maxHp = 100;
		hp = 100;
		respawnTime = 3.0f;
		_moveCtrl = GetComponent<MoveCtrl_net2> ();
		_skillCtrl = GetComponent<SkillCtrl_net2> ();
		_networkMgr = GameObject.Find ("NetworkManager").GetComponent<NetworkMgr2>();
	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			if(isDie)return;
			#if UNITY_ANDROID||UNITY_IPHONE
			#else
			if(Input.GetMouseButton(0))
			{
				preFire();
			}else{
				fireBool=false;
			}

			#endif

			_animation.CrossFade(animClips[(int)animState].name,0.2f);
		} else {
			if(Vector3.Distance(tr.position,currPos)>=2.0f)
			{
				tr.position=currPos;
				tr.rotation=currRot;
			}else{
			tr.position = Vector3.Lerp (tr.position, currPos, Time.deltaTime * 10.0f);
			tr.rotation = Quaternion.Slerp (tr.rotation, currRot, Time.deltaTime * 10.0f);
			_animation.CrossFade(animClips[(int)animState].name,0.2f);
			}
		}

	}

	private float basicClock;
	private float basicAttackCool;
	public void preFire(){
		fireBool = true;
		if (_moveCtrl.moveSpeed != _skillCtrl.dashSpeed) {
						_moveCtrl.moveSpeed = _skillCtrl.shootSpeed;
				}
		if (Time.time - basicClock > basicAttackCool) {
			basicClock = Time.time;
			Fire ();
			photonView.RPC("Fire", PhotonTargets.Others);
		}
	}
	[RPC]
	void Fire()
	{
		StartCoroutine (this.PlaySfx(fireSfx));
		StartCoroutine (this.CreateBullet ());
	}

	IEnumerator CreateBullet()
	{
		GameObject.Instantiate (arrow, firePos.position, firePos.rotation);
		yield return null;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 pos = tr.position;
			Quaternion rot = tr.rotation;
			int _animState = (int)animState;
			
			stream.SendNext(pos);
			stream.SendNext(rot);
			stream.SendNext(_animState);
		}
		else
		{
			Vector3 revPos = Vector3.zero;
			Quaternion revRot = Quaternion.identity;
			int _animState=0;
			
			revPos = (Vector3)stream.ReceiveNext();
			revRot = (Quaternion)stream.ReceiveNext();
			_animState = (int)stream.ReceiveNext();
			
			currPos = revPos;
			currRot = revRot;
			animState = (AnimState)_animState;
		}
	}	


	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "ARROW") {
			StartCoroutine(this.CreateBloodEffect(coll.transform.position));
						Destroy (coll.gameObject);
			hp-=20;

			if(hp<=0){
				hp=0;
				StartCoroutine(this.RespawnPlayer(respawnTime));
			}
		}
	}
	
	IEnumerator RespawnPlayer(float waitTime){
		isDie = true;
		StartCoroutine (this.PlayerVisible (false, 0.0f));
		yield return new WaitForSeconds (waitTime);
		tr.position = new Vector3 (Random.Range (-20.0f, 20.0f), 0.0f, Random.Range (-20.0f, 20.0f));
		hp = 100;
		isDie = false;
		StartCoroutine (this.PlayerVisible (true, 0.5f));
	}

	IEnumerator PlayerVisible(bool visibled,float delayTime){
		yield return new WaitForSeconds(delayTime);
		
		foreach(SkinnedMeshRenderer temp in GetComponentsInChildren<SkinnedMeshRenderer>()) {
			temp.enabled = visibled;
		}
		
		foreach(MeshRenderer temp in GetComponentsInChildren<MeshRenderer>()) {
			temp.enabled = visibled;		
		}
		
		if(photonView.isMine)
		{
			GetComponent<MoveCtrl_net2>().enabled=visibled;
		}
	}

	IEnumerator CreateBloodEffect(Vector3 pos)
	{
		GameObject _blood1 = (GameObject)Instantiate (bloodEffect, pos, Quaternion.identity);
		Destroy (_blood1, 2.0f);
		
		Vector3 decalPos = tr.position + (Vector3.up * 0.1f);
		Quaternion decalRot = Quaternion.Euler (0, Random.Range (0, 360), 0);
		
		GameObject _blood2 = (GameObject)Instantiate (bloodDecal, decalPos, decalRot);
		float _scale = Random.Range (1.5f, 3.5f);
		_blood2.transform.localScale = new Vector3 (_scale, 1, _scale);
		
		Destroy (_blood2, 5.0f);
				
		yield return null;
	}
	
	void joystick_x_procss(float x){
		if (x >= 0.1f) {
			animState = AnimState.runRight;
		} else if (x <= -0.1f) {
			animState = AnimState.runLeft;
		}
	}	
	void joystick_y_procss(float y){
		if (y >= 0.1f) {
			animState = AnimState.runForward;
		} else if (y <= -0.1f) {
			animState = AnimState.runBackward;
		}
	}

	void joystick_idle(){
		animState = AnimState.idle;
	}
	void Fire_process(){
		fireBool = true;
		preFire();
	}
	
	void No_Fire_process(){
		fireBool = false;
	}

	IEnumerator PlaySfx(AudioClip _clip){
		if(_networkMgr!=null)
			_networkMgr.PlaySfx (firePos.position, _clip);
		yield return null;
	}
}