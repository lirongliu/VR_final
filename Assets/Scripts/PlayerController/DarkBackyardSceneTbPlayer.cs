using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DarkBackyardSceneTbPlayer : TbNetworkedPlayer {

	public AudioClip Dark_Clip;
	
	public override void reset ()
	{
		base.reset ();
		// reset the positions of both players to avoid lerp effect
		avatar.transform.localPosition = Constants.darkBackyardStartCoord + new Vector3(2, 0, 0);
		GameObject tbAvatar = GameObject.FindWithTag (Constants.tbPlayerAvatarTag);
		if (tbAvatar != null) {
			tbAvatar.transform.localPosition = Constants.darkBackyardStartCoord + new Vector3 (-2, 0, 0);
		}
	}

	void Start ()
	{
		DontDestroyOnLoad (this);

		GameObject boss = GameObject.FindGameObjectWithTag (Constants.bossTag);
		if (boss != null) {
			Destroy(boss);
		}
		
		avatar.SetActive (true);

		// reset the positions of both players to avoid lerp effect
		avatar.transform.localPosition = Constants.darkBackyardStartCoord + new Vector3(2, 0, 0);
		GameObject cbAvatar = GameObject.FindWithTag (Constants.tbPlayerAvatarTag);
		if (cbAvatar != null) {
			cbAvatar.transform.localPosition = Constants.darkBackyardStartCoord + new Vector3 (-2, 0, 0);
		}
		
		if (photonView.isMine) {
			reset ();
		}

		// set head transform
		this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");

		playerLocal = headTransform;
		playerGlobal = avatar.transform;

		
		// enable spotlight
		GameObject spotlight = Utility.FindTransform (this.transform, "Spotlight").gameObject;
		spotlight.GetComponent<Light> ().intensity = Constants.tbMaxSpotlightIntensity;


		if (GameController.play_audio) {
			AudioSource audio=this.GetComponent<AudioSource>();
			audio.Stop();
			audio.clip=Dark_Clip;
			audio.Play();
		}
		this.movingSpeed = Constants.defaultMovingSpeed / 1.5f;	//	dark environment, slow the speed...
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);

//			print ("real: " + avatar.transform.localPosition + ", " + "correct: " + correctAvatarPos);
		}
		else {
			
			checkLife ();
			checkFallingOutsideTheScene();

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
