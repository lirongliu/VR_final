using UnityEngine;
using System.Collections;

public class iPadNetworkedPlayer : NetworkedPlayer {

	// Use this for initialization
	void Start () {
		
		Debug.Log("i'm instantiated");
		
		if (photonView.isMine) {
			Debug.Log ("player is mine. I am the iPad player.");
			
			this.transform.localPosition = new Vector3(0, 1, 0);
			playerGlobal = this.transform;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			print ("correctAvatarPos: " + correctAvatarPos);
		}
		else {
			inputHandler();
		}
	}
}
