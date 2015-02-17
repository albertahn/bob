using UnityEngine;
using System.Collections;

public class preArrowCtrl : MonoBehaviour {
	public int damage = 20;
	public float speed = 1000.0f;


	// Use this for initialization
	void Start () {
		rigidbody.AddForce (transform.forward * speed);
		Destroy (gameObject, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		damage = 20 * PlayerPrefs.GetInt ("skill2_lv");
		//Debug.Log ("damage:"+damage);

	}
}
