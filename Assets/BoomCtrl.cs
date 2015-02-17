using UnityEngine;
using System.Collections;

public class BoomCtrl : MonoBehaviour {
	public int damage = 20;
	public float speed = 1000.0f;
	private Transform tr;
	
	// Use this for initialization
	void Start () {
		rigidbody.AddForce (transform.forward * speed);
		tr = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll){
			StartCoroutine(this.Explosion());
	}

	IEnumerator Explosion(){
				Collider[] colls = Physics.OverlapSphere (tr.position, 10.0f);

				foreach (Collider coll in colls) {
						if (coll.rigidbody != null) {
								if(coll.tag=="MONSTER"){
									//coll.GetComponent<MonsterCtrl>().GetBoomHit(tr.position);
					coll.gameObject.SendMessage("GetBoomHit",tr.position, SendMessageOptions.DontRequireReceiver);
								}else if(coll.tag=="MONSTER2"){
									//coll.GetComponent<MonsterCtrl2>().nvAgent.Stop();
					coll.gameObject.SendMessage("GetBoomHit",tr.position, SendMessageOptions.DontRequireReceiver);
								}
								coll.rigidbody.mass = 1.0f;
								coll.rigidbody.AddExplosionForce (800.0f, tr.position, 10.0f, 300.0f);

						}

						Destroy (gameObject, 5.0f);
						yield return null;
				}
		}
}