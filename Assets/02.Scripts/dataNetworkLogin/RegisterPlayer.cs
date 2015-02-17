using UnityEngine;
using System.Collections;

using Boomlagoon.JSON;


public class RegisterPlayer : MonoBehaviour {

	//GUISkin mySkin = GUI.skin;
	
	public bool UsernameCORRECT = false;
	public	bool PasswordCORRECT = false;


	string GameOwner;
	string GameOwnerPassword;

	
	public string PlayerUsername, Email ;
	public	string PlayerPassword;
	public	string RePlayerPassword;




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

//player username logged afteer
	public bool loggedin;
	private string username;
	private string hintPart;

	public int login_btn_height;
	public GUIStyle style ;
	private Server LoginServer; //= GetComponent<Server>();

	//single player;
	
	public int singleLeft, singleRight, singleTop, singleBottom;

	// Use this for initialization
	void Start () {

		LoginServer = GetComponent<Server>();

	
		PlayerPassword = ""; PlayerPassword = "";

		//Single player ints
		
		singleLeft  = (int)(Screen.width / 12);
		singleRight = (int)(Screen.width / 2);
		singleTop = (int)(Screen.width / 9);
		singleBottom = (int)(Screen.width / 5);

		LoaderHeight = 10;
	   LoaderWidth = 10;
		GeneralWidth = (int) (Screen.width/2-Screen.height*.2);

		GenHeight  = (int) (Screen.width/2-Screen.height*.5);
		
		
		UserHeight = (int) (Screen.width/2-Screen.height*.25)/4 ;
		UserWidth = 1; 
		feedbackWidth =170;
		feedHeight =170;

		login_btn_height= (int) (Screen.width/2-Screen.height*.02) ;
		
			PassHeight = (int) (Screen.width/2-Screen.height*.25) ;
	 PassWidth = 1;

		LogHeight = (int) (Screen.width/2-Screen.height*.5)+ 101;
		LogWidth = (int) (Screen.width/2-Screen.height*.25)+ 163;
		//notloggedin
		loggedin = false;



		style.normal.textColor = Color.cyan;
		style.fontSize = login_btn_height/10;
	


	}
	


void Update () {

		//Debug.Log("gameown"+PlayerUsername+":GameOwnerPassword:"+PlayerPassword);

	}

	public void OnGUI(){

		if(loggedin){

			GUI.backgroundColor=Color.cyan;


			GUILayout.Label("Welcome "+ username);

			//show single play button
			if (GUI.Button (new Rect (singleLeft, singleTop, singleRight,singleBottom), "<color=white><size=30>Play Single</size></color>")) {
				
				Application.LoadLevel ("scPlay");
				
				Debug.Log ("singleman");
				
			}//endifelse
			
			
			//show multiplay button
			if (GUI.Button (new Rect (singleLeft, singleTop*3, singleRight,singleBottom), "<color=white><size=30>Multiplayer</size></color>")) {
				
				Application.LoadLevel ("scNet");
				
				Debug.Log ("smeman");
				
			}//endifelse



		}else{ // not loggedin
			//email label
			GUI.Label (new Rect (GeneralWidth/2 - UserHeight, UserHeight,GeneralWidth,GeneralWidth/4), "Email:", style);
			//pass label
			GUI.Label (new Rect (GeneralWidth/2 - UserHeight, UserHeight+UserHeight, GeneralWidth,GeneralWidth/4), "Password:", style);
			//re pass
			GUI.Label (new Rect (GeneralWidth/2 - UserHeight, UserHeight+UserHeight*2, GeneralWidth,GeneralWidth/4), "Re-Password:", style);
			//username
			GUI.Label (new Rect (GeneralWidth/2 - UserHeight, UserHeight+UserHeight*3, GeneralWidth,GeneralWidth/4), "username", style);


			GUILayout.Label(""+hintPart);
			GUI.backgroundColor = Color.white;

		//Text area for the username
			Email = GUI.TextArea (new Rect(GeneralWidth, UserHeight,GeneralWidth,GeneralWidth/5),Email,  100);

		//Text area for the password
			PlayerPassword = GUI.PasswordField(new Rect (GeneralWidth, PassHeight/2,GeneralWidth, GeneralWidth/5), PlayerPassword, "*"[0], 25); //(new Rect(GeneralWidth,PassHeight,100,20),PlayerPassword);
			//repassword
			RePlayerPassword = GUI.PasswordField(new Rect (GeneralWidth, PassHeight-UserHeight,GeneralWidth, GeneralWidth/5), RePlayerPassword, "*"[0], 25);
         //player username 
			PlayerUsername = GUI.TextArea (new Rect(GeneralWidth, UserHeight*4,GeneralWidth,GeneralWidth/5),PlayerUsername,  100);

		// login 
			if (GUI.Button (new Rect (GeneralWidth, login_btn_height, GeneralWidth/2,GeneralWidth/4), "Log-In")) {


            //login button
				StartCoroutine (RegLoginData(Email, PlayerPassword,RePlayerPassword, PlayerUsername ));
				 }//endifelsegui


			// Loads level (0). This map can be set in the build settings
			if (GUI.Button (new Rect (login_btn_height/5,4,GeneralWidth/4,GeneralWidth/5), "Back")) {
				
				Application.LoadLevel ("LoginScene");


				//login button
				//StartCoroutine (GetLoginData(PlayerUsername, PlayerPassword));
			}//endifelsegui


		}//end else notlogged in
		
	}//end gui

//getlogin data

	private IEnumerator RegLoginData (string email, string password, string password2, string username)
	{

		yield return StartCoroutine (LoginServer.RegUser(Email, PlayerPassword,RePlayerPassword, PlayerUsername ));

		string emailman = LoginServer.fuckdata.GetString ("password");


		Debug.Log("emailman : "+ LoginServer.fuckdata.GetString("email")) ;

		username = LoginServer.fuckdata.GetString("username");

		if(username !=""){

			loggedin= true;

			Debug.Log("loggedin fucker : "+LoginServer.fuckdata.GetString("email")) ;

		}else{

			loggedin= false;
			hintPart ="Username or Password was Incorrect";

			Debug.Log("not logged in : "+LoginServer.fuckdata.GetString("email")) ;
		}

		
		yield return null;
		
		
	}

}
