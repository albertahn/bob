using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour {

	public Transform[] points;
	public GameObject monsterPrefab;

	public float createTime = 2.0f;

	public bool isGameOver = false;

	public float sfxVolumn = 1.0f;
	public bool isSfxMute = false;

	public float startTime;
	public float passedTime;
	// Use this for initialization
	void Start () {

	points = GameObject.Find ("SpawnPoint").GetComponentsInChildren<Transform> ();

		if (points.Length > 0) {
			StartCoroutine (this.CreateMonster ());
			StartCoroutine (this.CreateMonster2 ());
		}
		startTime = Time.time;


	}

	public int maxMonster=10;
	IEnumerator CreateMonster()
	{
		while (!isGameOver) {
			int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;

			if(monsterCount<maxMonster)
			{
				yield return new WaitForSeconds(createTime);
				int idx = Random.Range(1,points.Length);
				Instantiate(monsterPrefab,points[idx].position,points[idx].rotation);
			}else{
				yield return null;
			}
		}
	}
	
	public GameObject monster2Prefab;
	public int maxMonster2=10;
	IEnumerator CreateMonster2()
	{
		while (!isGameOver) {
			int monsterCount2 = (int)GameObject.FindGameObjectsWithTag("MONSTER2").Length;
			
			if(monsterCount2<maxMonster2)
			{
				yield return new WaitForSeconds(createTime);
				int idx = Random.Range(1,points.Length);
				Instantiate(monster2Prefab,points[idx].position,points[idx].rotation);
			}else{
				yield return null;
			}
		}
	}

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
	void Update () {
		passedTime = Time.time - startTime;
		LevelBalancing ();
	}
	
	
	void LevelBalancing(){
		if (passedTime < 10) {
			maxMonster=10;
			maxMonster2=10;
		} else if (passedTime < 20) {
			
			maxMonster=20;
			maxMonster2=20;
		} else if (passedTime < 30) {
			
			maxMonster=30;
			maxMonster2=30;
			
		} else if (passedTime < 50) {
			
			maxMonster=50;
			maxMonster2=50;
			
		} else if (passedTime < 80) {
			
			maxMonster=80;
			maxMonster2=80;
			
		} else if (passedTime < 130) {
			
			maxMonster=130;
			maxMonster2=130;
			
		} else if (passedTime < 210) {
			
			maxMonster=210;
			maxMonster2=210;
		}else if(passedTime<340){
			
			maxMonster=340;
			maxMonster2=340;
			
		}else{
			
			maxMonster=650;
			maxMonster2=650;
			
		}
	}
}
