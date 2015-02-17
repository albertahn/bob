using UnityEngine;
using System.Collections;

public class uiCtrl : MonoBehaviour {
	private FireCtrl fireCtrl;
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


	//skill level
	public TextMesh skill1_lv;
	public TextMesh skill2_lv;
	//skill point up
	
	public TextMesh skill1_up;
	public TextMesh skill2_up;
	//skillpoints leftmesh
	public TextMesh skill_point_mesh;

	// Use this for initialization
	void Start () {
		fireCtrl = GameObject.Find ("Player").GetComponent<FireCtrl> ();
		startTime = GameObject.Find("GameManager").GetComponent<GameMgr>().startTime;
		//skill1_text.text = "0";
	}
	
	// Update is called once per frame
	void Update () {

		skill1_lv.text = "lv: "+PlayerPrefs.GetInt ("skill1_lv");
		
		skill2_lv.text =  "lv: "+PlayerPrefs.GetInt ("skill2_lv");
		
		//skill1_lv.renderer.enabled = false;
		
		if (PlayerPrefs.GetInt ("skill_point_left") < 1) {
			
			skill1_up.renderer.enabled = false;
			skill2_up.renderer.enabled = false;
			skill_point_mesh.renderer.enabled = false;
			
		} else {
			skill1_up.renderer.enabled = true;
			skill2_up.renderer.enabled = true;
			skill_point_mesh.renderer.enabled = true;
			skill_point_mesh.text = "skill points:" + PlayerPrefs.GetInt ("skill_point_left");
		}//end


		if (fireCtrl.skillBool1 == true) {
			skill1.mainTexture = skill1_on;
			skill1_text.text = "";
		} else {
			skill1_text.text = (fireCtrl.quickCool- Time.time + fireCtrl.time4).ToString ("N0");
			skill1.mainTexture = skill1_off;
		}
		
		if (fireCtrl.skillBool2 == true) {
			skill2.mainTexture = skill2_on;
			skill2_text.text = "";
		} else {
			skill2_text.text = (fireCtrl.specialAttackCool- Time.time + fireCtrl.time2).ToString ("N0");
			skill2.mainTexture = skill2_off;
		}

		clock_text.text = (Time.time - startTime).ToString ("N0");
	}
}