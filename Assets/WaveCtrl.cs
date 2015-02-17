using UnityEngine;
using System.Collections;

public class WaveCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Destroy(gameObject,1.0f);
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "ARROW_ENEMY") {
			Destroy(coll.gameObject);


		}
	}
}
