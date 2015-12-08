using UnityEngine;
using System.Collections;

public class SpotLightController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		GameObject iPadNetworkedPlayer = GameObject.FindGameObjectWithTag ("iPadNetworkedPlayer");
		GameObject cbNetworkedPlayer = GameObject.FindGameObjectWithTag ("cbNetworkedPlayer");
		if (iPadNetworkedPlayer != null && this.transform.parent == iPadNetworkedPlayer.transform) {
			print ("IS_IPAD_PLAYER in SpotLightController");
			Transform avatarTransform = iPadNetworkedPlayer.transform.Find ("Avatar");
			if (avatarTransform != null) {
				this.transform.SetParent (avatarTransform);
				this.transform.localPosition = new Vector3(0, 10, 0);
			}
		} else if (cbNetworkedPlayer != null && this.transform.parent == cbNetworkedPlayer.transform) {
			print ("IS_CB_PLAYER in SpotLightController");

			Transform headTransform = Utility.FindTransform(cbNetworkedPlayer.transform, "AvatarHead");
			if (headTransform) {
				this.transform.SetParent (headTransform);
				this.transform.localPosition = Vector3.zero;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
