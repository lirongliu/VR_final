using UnityEngine;
using System.Collections;

public class TbBossSceneCameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject cbAvatar = GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);
		if (cbAvatar != null) {
			Vector3 cbAvatarPos = cbAvatar.transform.position;
//			this.transform.position = new Vector3 (cbAvatarPos.x, 15, cbAvatarPos.z);
			this.transform.position = new Vector3 (0, 15, 0);

			Camera camera = this.GetComponent<Camera> ();
			camera.orthographicSize = 12;
		}
	}
}
