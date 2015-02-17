using UnityEngine;
using System.Collections;

public class ryhtmeCtrl : MonoBehaviour {
	private Transform frontTr;
	private float maxFrontTr=1.0f;
	private float velocity;
	private float frontPos=0;

	// Use this for initialization
	void Start () {
		frontTr = GameObject.FindGameObjectWithTag ("RYTHME_FRONT").GetComponent<Transform> ();
		velocity = maxFrontTr / 3;
	}
	
	// Update is called once per frame
	void Update () {
		frontPos = frontPos + velocity * Time.deltaTime;
		if (frontPos > maxFrontTr)
						frontPos = 0;

		Vector3 temp = new Vector3 (frontPos, 1, 1);
		frontTr.localScale= temp;
	}
}
