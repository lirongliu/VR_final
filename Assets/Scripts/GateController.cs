using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	

	void Update () {


		GameObject cbPlayer= GameObject.FindWithTag("cbNetworkedPlayer");
		GameObject tbPlayer= GameObject.FindWithTag("iPadNetworkedPlayer");
		Transform tb_avatar = tbPlayer.transform.Find ("Avatar");

		//Transform gete_right = this.transform.Find ("gate_right");
		//Transform gete_left=this.transform.Find ("gate_left");

		// if cb player or tablet player is close to the door
		float distance = Vector3.Distance (tb_avatar.transform.position, this.transform.position);

		if (distance < 5) {
	
			if (this.transform.Find ("gate_right").transform.localEulerAngles.y < 90){

				this.transform.Find ("gate_left").transform.Rotate(0,Time.deltaTime*(-10),0,Space.Self);
				this.transform.Find ("gate_right").transform.Rotate(0,Time.deltaTime*(10),0,Space.Self);

			//Debug.Log (this.transform.Find ("gate_left").transform.localEulerAngles.y);
			}
		}

	}
}
