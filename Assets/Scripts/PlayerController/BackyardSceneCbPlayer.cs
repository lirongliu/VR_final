using UnityEngine;
using System.Collections;

public class BackyardSceneCbPlayer : CbNetworkedPlayer {

	public override void reset(bool restart) {
		base.reset (restart);

		avatar.transform.localPosition = Constants.backyardStartCoord + new Vector3 (0, 0, -1);
		GameObject tbAvatar = GameObject.FindWithTag (Constants.tbPlayerAvatarTag);
		if (tbAvatar != null) {
			tbAvatar.transform.localPosition = Constants.backyardStartCoord + new Vector3 (0, 0, 1);
		}
	}

	void Start ()
	{
		DontDestroyOnLoad (this);

		if (photonView.isMine) {
			reset(false);

			GameObject cb = GameObject.Find ("CardboardMain");
			
			Transform avatarHeadTransform = this.transform.Find("AvatarHead");
			Transform avatarBodyTransform = this.transform.Find("AvatarBody");

			/* hierachy:
			 * avatar
			 * avatarBody
			 * CardboardMain
			 * head
			 */
			avatarBodyTransform.SetParent(avatar.transform);
			avatarBodyTransform.localPosition = Vector3.zero;
			
			cb.transform.SetParent(avatarBodyTransform);

			Transform cbHead = cb.transform.Find("Head");
			Transform mainCamera = Utility.FindTransform(cb.transform, "Main Camera");

			avatarHeadTransform.SetParent(mainCamera);
//			avatarHeadTransform.SetParent(cb.transform);
			cb.transform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
			avatarHeadTransform.localPosition = Vector3.zero;
			
			// set head transform
			this.headTransform = mainCamera;
		}   else {
			
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
		}
		
		playerLocal = headTransform;
		playerGlobal = avatar.transform;

		// disable spotlight
		GameObject spotlight = Utility.FindTransform (this.transform, "Spotlight").gameObject;
		spotlight.GetComponent<Light> ().intensity = 0;
	}
	
	void Update(){
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);

			if (spotlight == null) {
				spotlight = GameObject.FindWithTag("cbSpotlight");
			}
			if (spotlight != null) {
				spotlight.transform.forward = Vector3.Lerp (spotlight.transform.forward, correctDir, Time.deltaTime * 5);
			}

//			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		} else {
			
			checkLife ();
			checkFallingOutsideTheScene();
			
			if (playerGlobal != null) {
				posSent = playerGlobal.transform.position;
				rotationSent = playerGlobal.rotation;
			}

			CbInstructionController c = Utility.getCbInstructionController();
			if (c != null && c.ShowingInstruction) {
				instructionHandler();
			} else {
				inputHandler ();
				cbInputHandler();
			}
			
			if (NetworkController.iAmReady == false) {
				if (c.isInstructionFinished(Constants.cbPlayerID)) {
					NetworkController.iAmReady = true;
					photonView.RPC("setOtherPlayerReady", PhotonTargets.Others, true);
				}
			}
			
			if (shouldBeMoving) {
				moveForward();
			}
		}

		if (arriveInDest(Constants.backyardDestinationCoord, 10)) {
			string nextScene = Utility.getGameController().getNextScene();
			photonView.RPC("loadScene", PhotonTargets.All, nextScene);
		}
	}
}
