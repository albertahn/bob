using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	public Transform target;
	public float dist = 10.0f;
	public float height=10.0f;
	public AudioClip backSfx;
	private GameMgr _gameMgr;

	private Transform tr;


	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform> ();

		AudioSource _audioSource = gameObject.AddComponent<AudioSource> ();
		_gameMgr = GameObject.Find ("GameManager").GetComponent<GameMgr>();
		
		_audioSource.clip = backSfx;
		_audioSource.minDistance = 10.0f;
		_audioSource.maxDistance = 30.0f;
		_audioSource.volume = _gameMgr.sfxVolumn;
		_audioSource.Play ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		tr.position = target.position -Vector3.forward*dist+Vector3.up * height;
		tr.LookAt (target);
	}
}
