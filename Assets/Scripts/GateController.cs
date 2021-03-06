﻿using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void Update () {

		GameObject cbNetworkedPlayer = GameObject.FindWithTag (Constants.cbNetworkedPlayerTag);
		GameObject tbNetworkedPlayer = GameObject.FindWithTag (Constants.tbNetworkedPlayerTag);

		if (cbNetworkedPlayer == null || tbNetworkedPlayer == null) {
			return;
		}

		Transform cb_avatar= cbNetworkedPlayer.transform.Find("Avatar");
		Transform tb_avatar= tbNetworkedPlayer.transform.Find("Avatar");

		// if cb player or tablet player is close to the door
		float distance1 = Vector3.Distance (tb_avatar.transform.position, this.transform.position);
		float distance2 = Vector3.Distance (cb_avatar.transform.position, this.transform.position);


		if ( distance1 < 5 && distance2 < 5) {
	
			if (this.transform.Find ("gate_right").transform.localEulerAngles.y < 90){

				this.transform.Find ("gate_left").transform.Rotate(0,Time.deltaTime*(-10),0,Space.Self);
				this.transform.Find ("gate_right").transform.Rotate(0,Time.deltaTime*(10),0,Space.Self);

			//Debug.Log (this.transform.Find ("gate_left").transform.localEulerAngles.y);
			}
		}

	}
}
