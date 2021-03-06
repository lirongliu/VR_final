﻿using UnityEngine;
using System.Collections;

public class BossSceneTbPlayer : TbNetworkedPlayer {

	public AudioClip BossClip;

	GameObject cbAvatar;
	
	public override void reset(bool restart) {
		base.reset (restart);
//		avatar.transform.localPosition = new Vector3 (5, 1f, -1);
		GameObject cbAvatar = GameObject.FindWithTag (Constants.cbPlayerAvatarTag);
		if (cbAvatar != null) {
			cbAvatar.transform.localPosition = new Vector3 (-5, 1f, -2);
		}
		posSent = new Vector3 (-5, 1f, -2);
	}

	void Start ()
	{
		DontDestroyOnLoad (this);
		
		if (photonView.isMine) {
			reset(false);
		}

		cbAvatar = GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);


		avatar.SetActive (false);

		if (GameController.play_audio) {
			AudioSource audio=this.GetComponent<AudioSource>();
			audio.Stop();
			audio.clip=BossClip;
			audio.Play();
		}
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			cbAvatar.transform.localPosition = Vector3.Lerp (cbAvatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
//			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
//			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		}
		else {
			
			checkLife ();
			checkFallingOutsideTheScene();

			
			if (cbAvatar != null) {
				posSent = cbAvatar.transform.position;
			}

			TbInstructionController t = Utility.getTbInstructionController();
			if (t != null && t.ShowingInstruction) {
				instructionHandler();
				print ("instructionHandler.....");
			} else {
				inputHandler();
				print ("inputHandler.....");

			}	
			if (NetworkController.iAmReady == false) {
				if (t.isInstructionFinished(Constants.tbPlayerID)) {
					NetworkController.iAmReady = true;
					photonView.RPC("setOtherPlayerReady", PhotonTargets.Others, true);
				}
			}
		}
	}
	
	override protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting){
//			if (cbAvatar == null)
//				return;

			stream.SendNext(posSent);
//			stream.SendNext(cbAvatar.transform.position);
//			stream.SendNext(playerGlobal.rotation);
//			stream.SendNext(headTransform.localRotation);
		}
		else{
			correctAvatarPos = (Vector3)stream.ReceiveNext();
			print ("correctAvatarPos " + correctAvatarPos);
//			correctAvatarRot = (Quaternion)stream.ReceiveNext();
//			correctHeadRot = (Quaternion)stream.ReceiveNext();
		}
	}
	override protected void inputHandler() {

		if (Input.GetKey ("l")) {
			string nextScene = Utility.getGameController().getNextScene();
			photonView.RPC("loadScene", PhotonTargets.All, nextScene);
		}

		
		if (cbAvatar == null)
			return;
		
		AvatarController avatarController = cbAvatar.GetComponent<AvatarController> ();

		if (Input.GetKey ("d")) {
			avatarController.Move(Utility.movementAdjustedWithFPS(new Vector3(movingSpeed, 0, 0)));
		}
		
		if (Input.GetKey ("a")) {
			avatarController.Move(Utility.movementAdjustedWithFPS(new Vector3(-movingSpeed, 0, 0)));
		}
		
		if (Input.GetKey ("s")) {
			avatarController.Move(Utility.movementAdjustedWithFPS(new Vector3(0, 0, -movingSpeed)));
		}
		
		if (Input.GetKey ("w")) {
			avatarController.Move(Utility.movementAdjustedWithFPS(new Vector3(0, 0, movingSpeed)));
		}

	}

	
}
