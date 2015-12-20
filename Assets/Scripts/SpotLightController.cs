using UnityEngine;
using System.Collections;

public class SpotLightController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print ("SpotLightController");

		GameObject tbNetworkedPlayer = GameObject.FindGameObjectWithTag (Constants.tbNetworkedPlayerTag);
		GameObject cbNetworkedPlayer = GameObject.FindGameObjectWithTag (Constants.cbNetworkedPlayerTag);
		if (tbNetworkedPlayer != null && this.transform.parent == tbNetworkedPlayer.transform) {
			print ("tbPlayerID in SpotLightController");
			Transform avatarTransform = tbNetworkedPlayer.transform.Find ("Avatar");
			if (avatarTransform != null) {
				this.transform.SetParent (avatarTransform);
				this.transform.localPosition = new Vector3(0, Constants.tbSpotlightHight, 0);
			}
		} else if (cbNetworkedPlayer != null && this.transform.parent == cbNetworkedPlayer.transform) {
			print ("cbPlayerID in SpotLightController");

			Transform headTransform = Utility.FindTransform(cbNetworkedPlayer.transform, "AvatarHead");
			if (headTransform) {
				this.transform.SetParent (headTransform);
				this.transform.localPosition = Vector3.zero;
			}

			Light light = this.GetComponent<Light>();
			light.spotAngle = Constants.cbMaxSpotlightAngle;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
