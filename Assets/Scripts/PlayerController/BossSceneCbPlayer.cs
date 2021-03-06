﻿using UnityEngine;
using System.Collections;

public class BossSceneCbPlayer : CbNetworkedPlayer {
	
	private GameObject hitEnemy;
	private GameObject boss;

	void OnLevelWasLoaded(int level) {
		print ("level " + level);
		// FIXED: sometimes the boss doesn't show up in tbPlayer view.
		// TO FIX: sometimes there are two bosses.
		if (level == 1 && photonView.isMine) {

			photonView.RPC ("generateBoss", PhotonTargets.All, Random.Range (-30, 30), Random.Range (-30, 30), 1000f, "boss");
		}
	}
	
	public override void reset(bool restart) {
		base.reset (restart);
		
		avatar.transform.localPosition = new Vector3 (-5, 1f, -2);
//		GameObject tbAvatar = GameObject.FindWithTag (Constants.tbPlayerAvatarTag);
//		if (tbAvatar != null) {
//			tbAvatar.transform.localPosition = new Vector3 (-5, 1f, -1);
//		}
	}

	
	void Start ()
	{
		DontDestroyOnLoad (this);


		if (photonView.isMine) {
			reset(false);

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

		
		// disable spotlight
		GameObject spotlight = Utility.FindTransform (this.transform, "Spotlight").gameObject;
		spotlight.GetComponent<Light> ().intensity = Constants.cbSpotlightIntensity;
	}
	
	void Update(){

		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
//			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
//			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
//			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
			
			if (spotlight == null) {
				spotlight = GameObject.FindWithTag("cbSpotlight");
			}
			if (spotlight != null) {
				spotlight.transform.forward = Vector3.Lerp (spotlight.transform.forward, correctDir, Time.deltaTime * 5);
			}
		} else {
			
			checkLife ();
			checkFallingOutsideTheScene();

			
			CbInstructionController c = Utility.getCbInstructionController();
			if (c != null && c.ShowingInstruction) {
				instructionHandler();
			} else {
				inputHandler ();
			}			
			if (NetworkController.iAmReady == false) {
				if (c.isInstructionFinished(Constants.cbPlayerID)) {
					NetworkController.iAmReady = true;
					photonView.RPC("setOtherPlayerReady", PhotonTargets.Others, true);
				}
			}

//			print (NetworkController.iAmReady + " " + NetworkController.otherPlayerReady);
			if (Utility.playersAreReady()) {
				if (boss == null) {
					boss = GameObject.FindGameObjectWithTag (Constants.bossTag);
				}

				RaycastHit hit;
				Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit);
				this.checkHitEnemies (hit, ref hitEnemy);
				this.checkHitTbPlayer (hit);

				if (boss != null) {
					this.checkHitBoss();
				}

				// only cb is responsible for generating enemies
				if (Time.frameCount % 50 == 0) {
					float angle = Random.Range(0, 2 * Mathf.PI);
					float x = Mathf.Cos(angle) * 15;
					float z = Mathf.Sin(angle) * 15;
					photonView.RPC ("generateEnemy", PhotonTargets.All, x, z, 100f, "chaseCb", Random.Range(1, 1000000000));
				}
			}
		}
	}
	
	override protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
//		if (stream.isWriting){
//			if (headTransform == null)
//				return;
////			stream.SendNext(cbAvatar.transform.position);
//			//			stream.SendNext(playerGlobal.rotation);
//						stream.SendNext(headTransform.localRotation);
//		}
//		else{
////						correctAvatarPos Random.Range = (Vector3)stream.ReceiveNext();
//			//			correctAvatarRot = (Quaternion)stream.ReceiveNext();
//			correctHeadRot = (Quaternion)stream.ReceiveNext();
//		}

		if (stream.isWriting){
			stream.SendNext (Camera.main.transform.forward);
		} else {
			correctDir = (Vector3)stream.ReceiveNext ();
		}
	}
	
	protected void checkHitBoss() {
//		print ("Camera.main.transform.position  " + Camera.main.transform.position);
//		print ("Camera.main.transform.forward  " + Camera.main.transform.forward);
//		print ("boss.transform.position " + boss.transform.position);
		Vector3 realBossPos = boss.transform.position + new Vector3 (0, 7.5f, 0);
		float angle = Utility.getVectorAngle(Camera.main.transform.forward, realBossPos - Camera.main.transform.position);
//		print ("angle " + angle);
		if (angle < Constants.cbMaxSpotlightAngle / 2) {
			BossController ec = boss.GetComponent<BossController> ();
			ec.getHit (250);
//			print ("ec life " + ec.getLife());
			
			if (ec.shouldBeDead ()) {

				print ("boss should be dead!!!!!!");
				Destroy(ec);
				
				string nextScene = Utility.getGameController().getNextScene();
				photonView.RPC("loadScene", PhotonTargets.All, nextScene);
				
//				if (NetworkController.enemyList.IndexOf (hitEnemy) != null && NetworkController.enemyList.IndexOf (hitEnemy) != (-1)) {
//					//print("NetworkedPlayer enemyList:"+NetworkController.enemyList[0]+"\t"+NetworkController.enemyList[1]+"\t"+NetworkController.enemyList[2]);
//					//print("INDEX:"+NetworkController.enemyList.IndexOf (hit.collider.gameObject));
//					photonView.RPC ("destroyEnemy", PhotonTargets.All, NetworkController.enemyList.IndexOf (hitEnemy));
				//			headTransform	}
			}
		} else {
//			print ("revive!!!");
			BossController ec = boss.GetComponent<BossController> ();
			ec.revive();
		}
	}

}
