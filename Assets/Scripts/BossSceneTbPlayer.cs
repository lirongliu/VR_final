using UnityEngine;
using System.Collections;

public class BossSceneTbPlayer : NetworkedPlayer {
	
	void Start ()
	{
		DontDestroyOnLoad (this);
		
		avatar.transform.localPosition = new Vector3 (5, 0.5f, -1);

		// set head transform
		this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");
		
		playerLocal = headTransform;
		playerGlobal = avatar.transform;		
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		}
		else {
			inputHandler();
		}
	}

	
}
