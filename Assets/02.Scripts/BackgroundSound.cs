using UnityEngine;
using System.Collections;

public class BackgroundSound : MonoBehaviour {
	
	private GameMgr _gameMgr;
	public AudioClip backsoundSfx;

	// Use this for initialization
	void Start () {
		_gameMgr = GameObject.Find ("GameManager").GetComponent<GameMgr>();
		_gameMgr.PlaySfx (this.transform.position, backsoundSfx);
	//	_gameMgr.PlaySfx (firePos.position, _clip);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
