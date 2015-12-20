using UnityEngine;
using System.Collections;

public class BackyardSceneTbPlayer : TbNetworkedPlayer {
	
	void Start ()
	{
		DontDestroyOnLoad (this);
		
		avatar.transform.localPosition = new Vector3 (5, 1f, -1);

		if (photonView.isMine) {
			reset();
		}
		
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

		
		// disable spotlight
		GameObject spotlight = Utility.FindTransform (this.transform, "Spotlight").gameObject;
		spotlight.GetComponent<Light> ().intensity = 2;

		print ("GameController.play_audio" + GameController.play_audio);
		//Debug.Log ("GameController.play_audio" + GameController.play_audio);

		if (GameController.play_audio) {
			AudioSource audio=this.GetComponent<AudioSource>();
			audio.Play();
		}
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		}
		else {
			TbInstructionController t = Utility.getTbInstructionController();
			if (t != null && t.ShowingInstruction) {
				instructionHandler();
			} else {
				inputHandler();
			}

			if (NetworkController.iAmReady == false) {
				if (t.isInstructionFinished(Constants.tbPlayerID)) {
					NetworkController.iAmReady = true;
					photonView.RPC("setOtherPlayerReady", PhotonTargets.Others, true);
				}
			}
		}

	}
}
