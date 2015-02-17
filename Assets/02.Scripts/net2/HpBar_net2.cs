using UnityEngine;
using System.Collections;

public class HpBar_net2: MonoBehaviour {
	private GameObject player;
	private int maxHP;
	private NetworkMgr2 _networkMgr;
	
	// Use this for initialization
	void Start () {
		_networkMgr = GameObject.Find ("NetworkManager").GetComponent<NetworkMgr2>();
	}
	public void setPlayer(){
		player = _networkMgr.player;
		if(player!=null)
		maxHP = player.GetComponent<PlayerCtrl_net2>().maxHp;
	}
	// Update is called once per frame
	void Update () {
		if (player != null) {
			int hp = player.GetComponent<PlayerCtrl_net2> ().hp;
			Vector3 temp = new Vector3 ((float)hp / maxHP, 1, 1);
			this.transform.localScale = temp;
		}
		
	}
}
