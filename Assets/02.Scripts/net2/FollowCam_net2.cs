using UnityEngine;
using System.Collections;

public class FollowCam_net2 : MonoBehaviour {
	public Transform target;
	public float dist = 10.0f;
	public float height=10.0f;
	public AudioClip backSfx;
	private NetworkMgr2 _networkMgr2;
	
	private Transform tr;
	// Use this for initialization
	void Start () {
		_networkMgr2 = GameObject.Find ("NetworkManager").GetComponent<NetworkMgr2> ();
		tr = GetComponent<Transform> ();
		
		AudioSource _audioSource = gameObject.AddComponent<AudioSource> ();
		
		_audioSource.clip = backSfx;
		_audioSource.minDistance = 10.0f;
		_audioSource.maxDistance = 30.0f;
		_audioSource.volume = _networkMgr2.sfxVolumn;
		_audioSource.Play ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (target != null) {
			tr.position = target.position - Vector3.forward * dist + Vector3.up * height;
			tr.LookAt (target);
		}
	}
	
	public void SetTarget(){
		target = _networkMgr2.player.transform;
	}
}