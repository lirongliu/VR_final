using UnityEngine;
using System.Collections;

public class TopCameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {


		// 
		GameObject iPadAvatar = GameObject.FindGameObjectWithTag ("iPadNetworkedPlayerAvatar");
		GameObject cbAvatar = GameObject.FindGameObjectWithTag ("cbNetworkedPlayerAvatar");
		if (iPadAvatar != null && cbAvatar != null) {
			Vector3 iPadAvatarPos = iPadAvatar.transform.position;
			Vector3 cbAvatarPos = cbAvatar.transform.position;
			Vector3 center = (iPadAvatarPos + cbAvatarPos) / 2;
			this.transform.position = new Vector3 (center.x, 15, center.z);

			Camera camera = this.GetComponent<Camera> ();
			if (camera != null) {
				float orthographicSize;

				float yDiff = Mathf.Abs (iPadAvatarPos.z - cbAvatarPos.z);
				float xDiff = Mathf.Abs (iPadAvatarPos.x - cbAvatarPos.x);

				if (xDiff / yDiff > camera.aspect) {
					orthographicSize = xDiff / camera.aspect * 1.5f / 2f;
				} else {
					orthographicSize = yDiff * 1.5f / 2f;
				}

				camera.orthographicSize = Mathf.Max (Constants.minOrthographicSize, orthographicSize);
			}
		} else if (iPadAvatar != null) {
			Vector3 iPadAvatarPos = iPadAvatar.transform.position;
			this.transform.position = new Vector3 (iPadAvatarPos.x, 15, iPadAvatarPos.z);
		}
	}
}
