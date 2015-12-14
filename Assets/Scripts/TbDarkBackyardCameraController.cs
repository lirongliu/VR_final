using UnityEngine;
using System.Collections;

public class TbDarkBackyardCameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {

		GameObject tbAvatar = GameObject.FindGameObjectWithTag (Constants.tbPlayerAvatarTag);
		GameObject cbAvatar = GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);
		if (tbAvatar != null && cbAvatar != null) {
			Vector3 tbAvatarPos = tbAvatar.transform.position;
			Vector3 cbAvatarPos = cbAvatar.transform.position;
			Vector3 center = (tbAvatarPos + cbAvatarPos) / 2;
			this.transform.position = new Vector3 (center.x, 15, center.z);

			Camera camera = this.GetComponent<Camera> ();
			if (camera != null) {
				float orthographicSize;

				float yDiff = Mathf.Abs (tbAvatarPos.z - cbAvatarPos.z);
				float xDiff = Mathf.Abs (tbAvatarPos.x - cbAvatarPos.x);

				if (xDiff / yDiff > camera.aspect) {
					orthographicSize = xDiff / camera.aspect * 1.5f / 2f;
				} else {
					orthographicSize = yDiff * 1.5f / 2f;
				}

				camera.orthographicSize = Mathf.Max (Constants.minOrthographicSize, orthographicSize);
			}
		} else if (tbAvatar != null) {
			Vector3 iPadAvatarPos = tbAvatar.transform.position;
			this.transform.position = new Vector3 (iPadAvatarPos.x, 15, iPadAvatarPos.z);
		}
	}
}
