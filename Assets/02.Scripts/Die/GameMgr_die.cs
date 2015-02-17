using UnityEngine;
using System.Collections;

public class GameMgr_die: MonoBehaviour {
	
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
	
	
	// Update is called once per frame
	
}
