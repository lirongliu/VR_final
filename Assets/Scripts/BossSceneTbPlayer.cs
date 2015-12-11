﻿using UnityEngine;
using System.Collections;

public class BossSceneTbPlayer : NetworkedPlayer {
	GameObject cbAvatar;
	void Start ()
	{
		DontDestroyOnLoad (this);

		cbAvatar = GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);


//		avatar.transform.localPosition = new Vector3 (5, 0.5f, -1);

		// set head transform
//		this.headTransform = Utility.FindTransform (cbAvatar.transform, "AvatarHead");
		
//		playerLocal = headTransform;
//		playerGlobal = cbAvatar.transform;
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			cbAvatar.transform.localPosition = Vector3.Lerp (cbAvatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
//			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
//			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		}
		else {
			inputHandler();
		}
	}
	
	override protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting){
			stream.SendNext(cbAvatar.transform.position);
//			stream.SendNext(playerGlobal.rotation);
//			stream.SendNext(headTransform.localRotation);
		}
		else{
			correctAvatarPos = (Vector3)stream.ReceiveNext();
//			correctAvatarRot = (Quaternion)stream.ReceiveNext();
//			correctHeadRot = (Quaternion)stream.ReceiveNext();
		}
	}

	override protected void inputHandler() {
		float movingSpeed = 0.2f;
		
		AvatarController avatarController = cbAvatar.GetComponent<AvatarController> ();
		
		if (Input.GetKey ("d")) {
			avatarController.Move (new Vector3 (movingSpeed, 0, 0));
		}
		
		if (Input.GetKey ("a")) {
			avatarController.Move (new Vector3 (-movingSpeed, 0, 0));
		}
		
		if (Input.GetKey ("s")) {
			avatarController.Move (new Vector3 (0, 0, -movingSpeed));
		}
		
		if (Input.GetKey ("w")) {
			avatarController.Move (new Vector3 (0, 0, movingSpeed));
		}

	}

	
}
