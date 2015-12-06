using UnityEngine;
using System.Collections;

public class EnemyController : Photon.MonoBehaviour {

	private GameObject cbPlayer;
	private Transform avatar;

	void Start () {

	}
	

	void Update () {

	}

	void FixedUpdate(){
		if (cbPlayer == null) {
			cbPlayer = GameObject.FindWithTag("cbNetworkedPlayer");
			avatar=cbPlayer.transform.Find("Avatar");
		}

		this.transform.position = Vector3.Lerp (this.transform.position,avatar.position,Time.deltaTime * 0.1f);

	}




}
