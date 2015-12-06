using UnityEngine;
using System.Collections;

public class SpotLightController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		GameObject iPadNetworkedPlayer = GameObject.FindGameObjectWithTag ("iPadNetworkedPlayer");
		GameObject cbNetworkedPlayer = GameObject.FindGameObjectWithTag ("cbNetworkedPlayer");
		if (iPadNetworkedPlayer != null && this.transform.parent == iPadNetworkedPlayer.transform) {
			print ("IS_IPAD_PLAYER");
//			if (iPadNetworkedPlayer != null) {
				Transform avatarTransform = iPadNetworkedPlayer.transform.Find ("Avatar");
				if (avatarTransform != null) {
					this.transform.SetParent (avatarTransform);
				}
//			}
		} else if (cbNetworkedPlayer != null && this.transform.parent == cbNetworkedPlayer.transform) {
			print ("IS_CB_PLAYER");
//			if (cbNetworkedPlayer != null) {
				Transform avatarTransform = cbNetworkedPlayer.transform.Find ("Avatar");
				if (avatarTransform != null) {
					this.transform.SetParent (avatarTransform);
					this.transform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
				}
//			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
