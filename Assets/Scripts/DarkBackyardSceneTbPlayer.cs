using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DarkBackyardSceneTbPlayer : NetworkedPlayer {

	void Start ()
	{
		DontDestroyOnLoad (this);

		avatar.transform.localPosition = new Vector3 (5, 0.5f, -1);
		
		Transform avatarHeadTransform = this.transform.Find("AvatarHead");
		Transform avatarBodyTransform = this.transform.Find("AvatarBody");
		
		/* hierachy:
		 * avatar
		 * avatarBody
		 * head
		 */
		avatarBodyTransform.SetParent(avatar.transform);
		avatarBodyTransform.localPosition = Vector3.zero;
		
		avatarHeadTransform.SetParent(avatarBodyTransform);
		avatarHeadTransform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
		
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
