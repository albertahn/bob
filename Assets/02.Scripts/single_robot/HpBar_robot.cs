using UnityEngine;
using System.Collections;

public class HpBar_robot: MonoBehaviour {
	public GameObject player;
	private int maxHP;
	
	// Use this for initialization
	void Start () {
		maxHP = player.GetComponent<PlayerCtrl_robot> ().maxHp;
	}
	// Update is called once per frame
	void Update () {
		if (player != null) {
			int hp = player.GetComponent<PlayerCtrl_robot> ().hp;
			Vector3 temp = new Vector3 ((float)hp / maxHP, 1, 1);
			this.transform.localScale = temp;
		}
		
	}
}
