using UnityEngine;
using System.Collections;

public class DarkBackyardSceneCbPlayer : CbNetworkedPlayer {

	private GameObject hitEnemy;

	void Start ()
	{
		DontDestroyOnLoad (this);

		GameObject boss = GameObject.FindGameObjectWithTag (Constants.bossTag);
		if (boss != null) {
			Destroy(boss);
		}

		// reset the positions of both players to avoid lerp effect
		avatar.transform.localPosition = Constants.darkBackyardStartCoord + new Vector3(-2, 0, 0);
		GameObject tbAvatar = GameObject.FindWithTag (Constants.tbPlayerAvatarTag);
		if (tbAvatar != null) {
			tbAvatar.transform.localPosition = Constants.darkBackyardStartCoord + new Vector3 (2, 0, 0);
		}

		if (photonView.isMine) {
			GameObject cb = GameObject.Find ("CardboardMain");
			
			Transform cbHead = cb.transform.Find("Head");
			Transform mainCamera = Utility.FindTransform(cb.transform, "Main Camera");
			// set head transform
			this.headTransform = mainCamera;

		} else {
			
			// set head transform
			this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");
		}
		
		playerLocal = headTransform;
		playerGlobal = avatar.transform;

		
		// enable spotlight
		GameObject spotlight = Utility.FindTransform (this.transform, "Spotlight").gameObject;
		spotlight.GetComponent<Light> ().intensity = Constants.cbSpotlightIntensity;

		this.movingSpeed = Constants.defaultMovingSpeed / 1.5f;	//	dark environment, slow the speed...
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
//			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
			
			if (spotlight == null) {
				spotlight = GameObject.FindWithTag("cbSpotlight");
			}
			if (spotlight != null) {
				spotlight.transform.forward = Vector3.Lerp (spotlight.transform.forward, correctDir, Time.deltaTime * 5);
			}
		} else {
			
			CbInstructionController c = Utility.getCbInstructionController();
			if (c != null && c.ShowingInstruction) {
				instructionHandler();
			} else {
				inputHandler ();
				cbInputHandler();
			}

			if (shouldBeMoving) {
				moveForward();
			}

			RaycastHit hit;
			Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit);
			this.checkHitEnemies (hit, ref hitEnemy);
			this.checkHitTbPlayer (hit);
			
			// only cb is responsible for generating enemies
			if (Time.frameCount % 100 == 0) {
				photonView.RPC ("generateEnemy", PhotonTargets.All, Random.Range (-30f, 30f), Random.Range (-30f, 30f), 100f, "chaseBoth");
			}
		}
		
		if (arriveInDest(Constants.darkBackyardDestinationCoord, 4)) {
			print ("finished!!!");
		}
	}

}
