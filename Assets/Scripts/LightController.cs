using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

	public GameObject spotlight;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			//Debug.Log(Input.mousePosition.x+"\t"+Input.mousePosition.y);

			Vector3 lightPosition=new Vector3(Input.mousePosition.x,10, Input.mousePosition.y);
			Quaternion lightRotation = Quaternion.identity;

			//Instantiate(spotlight,lightPosition,lightRotation);
		}
	}

}
