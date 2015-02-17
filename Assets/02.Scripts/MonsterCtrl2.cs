using UnityEngine;
using System.Collections;

public class MonsterCtrl2 : MonoBehaviour {
	private Transform monsterTr;
	private Transform playerTr;
	public NavMeshAgent nvAgent;
	private Animator _animator;
	
	public enum MonsterState {idle,trace,attack,die};
	public MonsterState monsterState = MonsterState.idle;
	public float traceDist = 50.0f;
	public float attackDist=20.0f;
	private bool isDie = false;
	private bool isPlayerDie=false;
	
	public GameObject bloodEffect;
	public GameObject bloodDecal;
	
	private int hp = 100;

	
	public GameObject arrowByEnemy;
	public Transform firePos;
	
	// Use this for initialization
	void Start () {
		monsterTr = this.gameObject.GetComponent<Transform> ();
		playerTr = GameObject.FindWithTag ("Player").GetComponent<Transform> ();
		nvAgent = this.gameObject.GetComponent<NavMeshAgent> ();
		_animator = this.gameObject.GetComponent<Animator> ();
		
		//	nvAgent.destination = playerTr.position;
		
		StartCoroutine (this.CheckMonsterState ());
		StartCoroutine (this.MonsterAction ());
	}
	
	IEnumerator CheckMonsterState(){
		while (!isDie) {
			yield return new WaitForSeconds (0.2f);
			float dist = Vector3.Distance (playerTr.position, monsterTr.position);
			if (dist <= attackDist) {
				monsterState = MonsterState.attack;
			} else if (dist <= traceDist) {
				monsterState = MonsterState.trace;
			}
			else {
				if(monsterState==MonsterState.idle)
					monsterState = MonsterState.idle;
			}
		}
	}
	
	IEnumerator MonsterAction(){
		while (!isDie) {
			switch (monsterState) {
			case MonsterState.idle:
				nvAgent.Stop ();
				_animator.SetBool("IsTrace",false);
				break;
			case MonsterState.trace:
				nvAgent.destination = playerTr.position;
				_animator.SetBool("IsTrace",true);
				break;
			case MonsterState.attack:
				nvAgent.Stop ();
				_animator.SetBool("IsTrace",false);
				break;
			}
			yield return null;
		}
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "ARROW"||coll.gameObject.tag=="SARROW") {
			StartCoroutine (this.CreateBloodEffect(coll.transform.position));
			
			if(coll.gameObject.tag == "ARROW")
				hp -= coll.gameObject.GetComponent<ArrowCtrl>().damage;
			else 
				hp -= coll.gameObject.GetComponent<preArrowCtrl>().damage;
			if(hp<=0)
			{
				MonsterDie();
			}
			
			//Destroy (coll.gameObject);
			_animator.SetBool ("IsHit", true);
		}
	}
	
	private float time;
	private float basicAttackCool=1.0f;

	void Update()
	{
		if (!isPlayerDie) {
						if (Time.time - time > basicAttackCool && monsterState == MonsterState.attack) {
								monsterTr.LookAt (playerTr.position);

								time = Time.time;
								StartCoroutine (this.CreateArrow ());
						}
				}

		if (_animator.GetCurrentAnimatorStateInfo (0).nameHash == Animator.StringToHash ("Base Layer.gothit")) {
			_animator.SetBool ("IsHit", false);
		}
		if (_animator.GetCurrentAnimatorStateInfo (0).nameHash == Animator.StringToHash ("Base Layer.fall")) {
			_animator.SetBool("IsPlayerDie",false);
		}
		if (_animator.GetCurrentAnimatorStateInfo (0).nameHash == Animator.StringToHash ("Base Layer.die")) {
			_animator.SetBool("IsDie",false);
		}
	}
	
	void MonsterDie()
	{

		int myscore = PlayerPrefs.GetInt ("mon1");
		PlayerPrefs.SetInt ("mon2",myscore+1 );

		gameObject.tag = "Untagged";
		StopAllCoroutines ();
		
		isDie = true;
		monsterState = MonsterState.die;
		//nvAgent.Stop ();
		_animator.SetBool ("IsDie", true);
		
		gameObject.GetComponentInChildren<CapsuleCollider> ().enabled = false;
		
		foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>()) {
			coll.enabled = false;
		}
		
		Destroy (gameObject);
	}
	
	
	void OnPlayerDie()
	{
		StopAllCoroutines ();
		nvAgent.Stop ();
		_animator.SetBool ("IsPlayerDie", true);
		isPlayerDie = true;
	}
	
	IEnumerator CreateBloodEffect(Vector3 pos)
	{
		GameObject _blood1 = (GameObject)Instantiate (bloodEffect, pos, Quaternion.identity);
		Destroy (_blood1, 2.0f);
		
		Vector3 decalPos = monsterTr.position+(Vector3.up*0.01f);
		Quaternion decalRot = Quaternion.Euler(0,Random.Range(0,360),0);
		
		GameObject _blood2 = (GameObject)Instantiate (bloodDecal, decalPos, decalRot);
		float _scale = Random.Range (1.5f, 3.5f);
		_blood2.transform.localScale = new Vector3 (_scale, 1, _scale);
		Destroy (_blood2, 5.0f);
		
		yield return null;
	}
	
	void OnEnable(){
		PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
	}
	void OnDisable(){
		PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
	}

	IEnumerator CreateArrow(){
		Instantiate (arrowByEnemy, firePos.position, firePos.rotation);
		yield return null;
	}

	void GetBoomHit(Vector3 position){
		nvAgent.Stop ();
		rigidbody.AddExplosionForce (800.0f, position, 10.0f, 300.0f);
	}
}