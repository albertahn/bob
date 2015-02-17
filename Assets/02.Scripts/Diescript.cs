using UnityEngine;
using System.Collections;

using Boomlagoon.JSON;

[RequireComponent(typeof(AudioSource))]
public class Diescript : MonoBehaviour {

	//public AudioClip diesound;

	public AudioClip backgroundMusic;
	private GameMgr_die _gameMgr;
	public Transform auidioPos;
	// Use this for initialization]

	private Server LoginServer;
	
	public GUIStyle style = new GUIStyle() ;

	public int leftRetry, rightRetry, topRetry, bottomRetry;
	string email;
	string userindex ;
	int kills;
	void Start () {
		
		//Debug.Log ("dead");

		_gameMgr = GameObject.Find ("GameManager").GetComponent<GameMgr_die>();

	//	StartCoroutine (this.PlayClip(diesound));

		StartCoroutine (this.PlayClip (backgroundMusic));

		leftRetry = (int) (Screen.width/15);
		rightRetry = (int) (Screen.width/4);
		topRetry = (int) (Screen.height/5);
		bottomRetry= (int) (Screen.height/4);

		style.normal.textColor = Color.red;
		style.fontSize = leftRetry;
//get server
		LoginServer = GetComponent<Server>();
		//snd to server the data

		 kills = PlayerPrefs.GetInt ("mon1")+PlayerPrefs.GetInt ("mon2");

		userindex = PlayerPrefs.GetString("user_index");
         email = PlayerPrefs.GetString("email");


		
		string emailman = LoginServer.fuckdata.GetString ("password");

		StartCoroutine (sendData(userindex, email, ""+kills));
		
	}


	private IEnumerator sendData (string userindex, string email, string kills){

		yield return StartCoroutine (LoginServer.SaveBestScore(userindex, email, ""+kills ));


		}

	//gui stuff
	public void OnGUI(){




		if (GUI.Button (new Rect (leftRetry , topRetry, rightRetry,bottomRetry), "Retry")) {
			
			Application.LoadLevel ("scPlay");
			
		}//endifelsegui

		if (GUI.Button (new Rect (leftRetry*5 , topRetry, rightRetry,bottomRetry), "Home")) {
			
			Application.LoadLevel ("LoginScene");
			
		}//endifelsegui


		if (GUI.Button (new Rect (leftRetry*9 , topRetry, rightRetry,bottomRetry), "Ranking")) {
			
			Application.LoadLevel ("ranking");
			
		}//endifelsegui

		int kills = PlayerPrefs.GetInt ("mon1")+PlayerPrefs.GetInt ("mon2");

		GUI.Label (new Rect (leftRetry, topRetry*3,rightRetry,bottomRetry), "Your Kills:"+kills, style);


		//GUILayout.Label("kills: " + kills);
	}//gui



	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator PlayClip(AudioClip _clip){
		//audio.PlayOneShot (_clip, 0.9f);
		_gameMgr.PlaySfx (auidioPos.position, _clip);
		yield return null;
	}//end play



}
