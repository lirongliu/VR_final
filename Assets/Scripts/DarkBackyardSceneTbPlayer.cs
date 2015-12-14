using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DarkBackyardSceneTbPlayer : NetworkedPlayer {

	void Start ()
	{
		DontDestroyOnLoad (this);
		
		avatar.SetActive (true);
		
		avatar.transform.localPosition = Constants.darkBackyardStartCoord + new Vector3(2, 0, 0);
		print ("avatar.transform.localPosition" + avatar.transform.localPosition);
		// set head transform
		this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");

		playerLocal = headTransform;
		playerGlobal = avatar.transform;

		
		// enable spotlight
		GameObject spotlight = Utility.FindTransform (this.transform, "Spotlight").gameObject;
		spotlight.GetComponent<Light> ().enabled = true;

		this.movingSpeed = Constants.defaultMovingSpeed / 1.5f;	//	dark environment, slow the speed...
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
