using UnityEngine;
using System.Collections;

public class NetworkMgr2: Photon.MonoBehaviour {
	private int scene;
	public GameObject player;
	public static int playerWhoIsIt;
	private static PhotonView ScenePhotonView;

	// Use this for initialization
	void Start () {
		scene = 0;
		playerWhoIsIt = 0;
		PhotonNetwork.ConnectUsingSettings ("0.1");
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	void OnJoinedLobby()
	{
		scene = 1;
	}

	void OnJoinedRoom()
	{
		TagPlayer (PhotonNetwork.player.ID);
		//ScenePhotonView.RPC("TagPlayer", PhotonTargets.Others, PhotonNetwork.player.ID);
		StartCoroutine (this.CreatePlayer ());
		if (PhotonNetwork.isMasterClient == true) {
						scene = 2;
				} else {
						scene = 3;
				}
	}

	[RPC]
	void TagPlayer(int playerID)
	{
		playerWhoIsIt = playerID;
	}

	void OnGUI(){
		switch (scene) {
		case 1:
			if(GUI.Button(new Rect(20,20,200,25),"Start Server"))
			{
				PhotonNetwork.CreateRoom("test",true,true,4);
			}
			if(GUI.Button(new Rect(20,50,200,25),"Connect to Server"))
			{
				PhotonNetwork.JoinRandomRoom();
			}
			break;
		case 2:
			GUI.Label(new Rect(20,20,200,25),"Initialization Server...");
			GUI.Label (new Rect(20,50,200,25),"Client Count = "+PhotonNetwork.playerList.Length);
			GUI.Label(new Rect(20,70,200,25),"player who is it = "+playerWhoIsIt);
			break;
		case 3:
			GUI.Label(new Rect(20,20,200,25),"Connect to Server");
			GUI.Label(new Rect(20,50,200,25),"player who is it = "+playerWhoIsIt);
			break;
		}
	}

	IEnumerator CreatePlayer()
	{
		Vector3 pos = new Vector3 (Random.Range (-20.0f, 20.0f), 0.0f, Random.Range (-20.0f, 20.0f));
		PhotonNetwork.Instantiate("Player_net2",pos,Quaternion.identity,0);
		while (player==null) {
						player = GameObject.Find ("Player_net2(Clone)");
				}
		setPlayer ();
		yield return null;
	}

	
	public void setPlayer(){
		GameObject.Find ("Main Camera").SendMessage ("SetTarget",player,SendMessageOptions.DontRequireReceiver);
		GameObject.Find ("Main Camera").SendMessage ("SetPlayer",player,SendMessageOptions.DontRequireReceiver);
		GameObject.Find ("UI").SendMessage ("SetPlayer",player,SendMessageOptions.DontRequireReceiver);
	}


	//sound process
	public float sfxVolumn = 1.0f;
	public bool isSfxMute = false;
	public void PlaySfx(Vector3 pos,AudioClip sfx)
	{
		if(isSfxMute) return;
		
		GameObject soundObj = new GameObject ("Sfx");
		soundObj.transform.position = pos;
		
		AudioSource _audioSource = soundObj.AddComponent<AudioSource> ();
		
		_audioSource.clip = sfx;
		_audioSource.minDistance = 10.0f;
		_audioSource.maxDistance = 30.0f;
		_audioSource.volume = sfxVolumn;
		_audioSource.Play ();
		
		Destroy (soundObj, sfx.length);
	}
}
