using UnityEngine;
using System.Collections;

public class WallCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onCollisionEnter(Collision coll)
	{
		if (coll.collider.tag == "ARROW"||coll.collider.tag =="ARROW_ENEMY") {
						Destroy (coll.gameObject);
				}
	}
	void OnTriggerEnter(Collider coll)
	{
		if (coll.collider.tag == "ARROW"||coll.collider.tag =="ARROW_ENEMY") {
			Destroy (coll.gameObject);
		}

	}
}