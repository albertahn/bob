using UnityEngine;
using System.Collections;

public class uiCtrl_net2 : MonoBehaviour {
	private SkillCtrl_net2 skillCtrl;
	private float startTime;
	
	public Material skill1;
	public Material skill2;
	
	public Texture skill1_on;
	public Texture skill1_off;
	public Texture skill2_on;
	public Texture skill2_off;
	
	public TextMesh skill1_text;
	public TextMesh skill2_text;
	
	public TextMesh clock_text;

	private GameObject player;
	private NetworkMgr2 _networkMgr;
	
	// Use this for initialization
	void Start () {
		_networkMgr = GameObject.Find ("NetworkManager").GetComponent<NetworkMgr2>();
		GameObject.Find ("hp_front_parent").GetComponent<HpBar_net2>().setPlayer();
		//skill1_text.text = "0";
	}

	public void SetPlayer(){
		player = _networkMgr.player;
		skillCtrl = player.GetComponent<SkillCtrl_net2> ();
		GameObject.Find ("hp_front_parent").GetComponent<HpBar_net2> ().setPlayer ();
		startTime = Time.time;
//		this.GetComponent<HpBar> ().setPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (skillCtrl != null) {
						if (skillCtrl.skill1Bool == true) {
								skill1.mainTexture = skill1_on;
								skill1_text.text = "";
						} else {
								skill1_text.text = (skillCtrl.skill1Cool - Time.time + skillCtrl.skill1Clock).ToString ("N0");
								skill1.mainTexture = skill1_off;
						}
		
						if (skillCtrl.skill2Bool == true) {
								skill2.mainTexture = skill2_on;
								skill2_text.text = "";
						} else {
								skill2_text.text = (skillCtrl.skill2Cool - Time.time + skillCtrl.skill2Clock).ToString ("N0");
								skill2.mainTexture = skill2_off;
						}
				}
		clock_text.text = (Time.time - startTime).ToString ("N0");
	}
}
