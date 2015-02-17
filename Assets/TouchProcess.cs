using UnityEngine;
using System.Collections;

public class TouchProcess : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		#if UNITY_EDITOR
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 100.0f, Color.green);
	
		RaycastHit hit;

		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hit, 100.0f)) {
				if (hit.collider.tag == "JOYSTICK_LEFT") {
							Destroy (hit.collider.gameObject);
					}
			}
		}

		#elif UNITY_ANDROID
//		Joystick joystick = FindObjectOfType(Joystick);
		/*if(Input.touchCount>0&&Input.GetTouch(0).phase==TouchPhase.Began)
		{
			Ray ray=Camera.main.ScreenPointToRay(Input.touches[0].position);

			RaycastHit hit;

			if(Physics.Raycast(ray,out hit,100.0f))
			{
				if(hit.collider.tag=="JOYSTICK_LEFT")
				{
					Destroy(hit.collider.gameObject);
				}
			}
		}*/

	#endif
	}
}
