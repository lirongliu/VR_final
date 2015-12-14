using UnityEngine;
using System.Collections;

public class TbBackyardCameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject tbAvatar = GameObject.FindGameObjectWithTag (Constants.tbPlayerAvatarTag);
		if (tbAvatar != null) {
			Vector3 tbAvatarPos = tbAvatar.transform.position;
			this.transform.position = new Vector3 (tbAvatarPos.x, 15, tbAvatarPos.z);
			
			Camera camera = this.GetComponent<Camera> ();
			camera.orthographicSize = 18;
		}
	}
}
