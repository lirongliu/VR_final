using UnityEngine;
using System.Collections;

public class DimLight : MonoBehaviour {

	private Light spotlight;

	void Start () {
		spotlight= GetComponent<Light>();

	}
	

	void Update () {
		if (spotlight.intensity > 0) {
			spotlight.intensity -= 0.01f;
			Debug.Log(spotlight.intensity);
			//Debug.Log (Input.GetMouseButton (0) + "\t" + Input.GetMouseButton (1) + "\t" + Input.GetMouseButton(2));
		} else {
			Destroy(this);
		
		}

	}
}