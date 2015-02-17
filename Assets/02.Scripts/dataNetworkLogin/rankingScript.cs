using UnityEngine;
using System.Collections;

using Boomlagoon.JSON;


public class rankingScript : MonoBehaviour {

	//GUISkin mySkin = GUI.skin;
	
	public bool UsernameCORRECT = false;
	public	bool PasswordCORRECT = false;



	string GameOwner;
	string GameOwnerPassword;

//audio
	private GameMgr _gameMgr;
	public Transform auidioPos; public AudioClip loginsound;


	public string PlayerUsername ;
	public	string PlayerPassword;


	public int LoaderHeight;

	public int LoaderWidth ;
	public int GeneralWidth, GenHeight  ; 

	
	public int UserHeight ;
	public int UserWidth ;
	
	public int feedbackWidth ;
	public int feedHeight ;

	
	public int PassHeight;
	public int PassWidth ;

	
	public int LogHeight ;
	public int LogWidth ;

	//single player;

	public int singleLeft, singleRight, singleTop, singleBottom;


//player username logged afteer
	public bool loggedin;
	private string username;
	private string hintPart;

	public int login_btn_height;
	public GUIStyle style ;
	private Server LoginServer; //= GetComponent<Server>();

	// Use this for initialization
	void Start () {

		_gameMgr = GameObject.Find ("GameManager").GetComponent<GameMgr> ();

		StartCoroutine (this.PlayClip (loginsound));
		LoginServer = GetComponent<Server>();

	
		PlayerPassword = ""; //PlayerPassword = "";

		LoaderHeight = 10;
	    LoaderWidth = 10;
		GeneralWidth = (int) (Screen.width/2-Screen.height*.25);

		GenHeight  = (int) (Screen.width/2-Screen.height*.5);
		
		
		UserHeight = (int) (Screen.width/2-Screen.height*.25)/2 ;
		UserWidth = 1; 
		feedbackWidth =170;
		feedHeight =170;

		login_btn_height= (int) (Screen.width/2-Screen.height*.02) ;
		
			PassHeight = (int) (Screen.width/2-Screen.height*.25) ;
	    PassWidth = 1;

		LogHeight = (int) (Screen.width/2-Screen.height*.5)+ 101;
		LogWidth = (int) (Screen.width/2-Screen.height*.25)+ 163;
		//notloggedin

		if (PlayerPrefs.GetString ("user_index")=="") {
			loggedin = false;
				} else {

			loggedin = true;

			Debug.Log("index: " +PlayerPrefs.GetString ("user_index"));
				}



		//Single player ints

		singleLeft  = (int)(Screen.width / 12);
		singleRight = (int)(Screen.width / 2);
		singleTop = (int)(Screen.width / 9);
		singleBottom = (int)(Screen.width / 5);


	

		style.normal.textColor = Color.black;
		style.fontSize = login_btn_height/10;


	

	}
	


void Update () {

		//Debug.Log("gameown"+PlayerUsername+":GameOwnerPassword:"+PlayerPassword);

	}

	public void OnGUI(){

	

				GUI.backgroundColor = Color.cyan;


				GUILayout.Label ("Welcome " + PlayerPrefs.GetString ("username"));


		if (GUI.Button (new Rect (GeneralWidth / 2 - UserHeight / 2, UserHeight/2, GeneralWidth/2, GeneralWidth / 4), "Home:")) {
			
			Application.LoadLevel ("LoginScene");
			
		}//endifelsegui

				// not loggedin

				

				//Text area for the username
				PlayerUsername = GUI.TextArea (new Rect (GeneralWidth, UserHeight, GeneralWidth, GeneralWidth / 4), PlayerUsername, 100);

				// Debug.Log("widthscreenman :"+ Screen.width);

				//Text area for the password
				PlayerPassword = GUI.PasswordField (new Rect (GeneralWidth, PassHeight, GeneralWidth, GeneralWidth / 4), PlayerPassword, "*" [0], 25); //(new Rect(GeneralWidth,PassHeight,100,20),PlayerPassword);


				// login 
				if (GUI.Button (new Rect (GeneralWidth, login_btn_height, GeneralWidth / 2, GeneralWidth / 4), "Log-In")) {


						//login button
						StartCoroutine (GetLoginData (PlayerUsername, PlayerPassword));

				}//endifelsegui


				// Loads level (0). This map can be set in the build settings
				if (GUI.Button (new Rect (GeneralWidth + login_btn_height / 2, login_btn_height, GeneralWidth / 2, GeneralWidth / 4), "Register")) {
				
						Application.LoadLevel ("RegScene");
						//login button
						//StartCoroutine (GetLoginData(PlayerUsername, PlayerPassword));
				}//endifelsegui


		}

//getlogin data

	private IEnumerator GetLoginData (string email, string password)
	{

		yield return StartCoroutine (LoginServer.LoginUser(PlayerUsername, PlayerPassword));

		string emailman = LoginServer.fuckdata.GetString ("password");

		Debug.Log("mailman:  "+ password);

		// LoginServer.hello ();
		//if (_server.data.ContainsKey ("character")) {
		
		Debug.Log("emailman : "+ LoginServer.fuckdata.GetString("email")) ;

		username = LoginServer.fuckdata.GetString("username");

		if(username !=""){

			loggedin= true;

			Debug.Log("loggedin fucker : "+LoginServer.fuckdata.GetString("email")) ;

			PlayerPrefs.SetString("email", LoginServer.fuckdata.GetString("email"));

			PlayerPrefs.SetString("username", LoginServer.fuckdata.GetString("username"));

			PlayerPrefs.SetString("user_index", LoginServer.fuckdata.GetString("index"));

		}else{

			loggedin= false;

			hintPart ="Username or Password was Incorrect";

			Debug.Log("not logged in : "+LoginServer.fuckdata.GetString("email")) ;
		}

		
		yield return null;
		
		
	}

//music
	IEnumerator PlayClip(AudioClip _clip){
		//audio.PlayOneShot (_clip, 0.9f);
		_gameMgr.PlaySfx (auidioPos.position, _clip);
		yield return null;
	}//end play

}
