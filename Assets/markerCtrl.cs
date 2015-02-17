using UnityEngine;
using System.Collections;

public class markerCtrl : MonoBehaviour {
	public bool markerState;

	// Use this for initialization
	void Start () {
		markerState = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll){
		markerState = true;
	}
	void OnTriggerExit(Collider coll){
		markerState = false;
	}
}
