using UnityEngine;
using System.Collections;

public class BossSceneCbPlayer : NetworkedPlayer {
	
	private GameObject hitEnemy;
	private GameObject boss;

	void OnLevelWasLoaded(int level) {
		print ("level " + level);
		// FIXED: sometimes the boss doesn't show up in tbPlayer view.
		if (level == 1 && photonView.isMine) {

			photonView.RPC ("generateBoss", PhotonTargets.All, Random.Range (-30, 30), Random.Range (-30, 30), 1000f, "boss");
		}
	}
	
	void Start ()
	{
		DontDestroyOnLoad (this);
		
		avatar.transform.localPosition = new Vector3 (-5, 1f, -2);

		if (photonView.isMine) {
			GameObject cb = GameObject.Find ("CardboardMain");
			
			Transform cbHead = cb.transform.Find("Head");
			Transform mainCamera = Utility.FindTransform(cb.transform, "Main Camera");
			this.headTransform = mainCamera;
			// set head transform
//			this.headTransform = cb.transform;
		} else {

			// set head transform
			this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");
		}
		
		playerLocal = headTransform;
		playerGlobal = avatar.transform;

		
		// disable spotlight
		GameObject spotlight = Utility.FindTransform (this.transform, "Spotlight").gameObject;
		spotlight.GetComponent<Light> ().enabled = true;
	}
	
	void Update(){

		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
//			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
//			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		} else {
			inputHandler ();

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
				photonView.RPC ("generateEnemy", PhotonTargets.All, x, z, 100f, "chaseCb");
			}
		}
	}
	
	override protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting){
//			stream.SendNext(cbAvatar.transform.position);
			//			stream.SendNext(playerGlobal.rotation);
						stream.SendNext(headTransform.localRotation);
		}
		else{
//						correctAvatarPos Random.Range = (Vector3)stream.ReceiveNext();
			//			correctAvatarRot = (Quaternion)stream.ReceiveNext();
			correctHeadRot = (Quaternion)stream.ReceiveNext();
		}
	}
	
 	override protected void inputHandler() {
		
		AvatarController avatarController = avatar.GetComponent<AvatarController> ();
		
		if (Input.GetKey ("l")) {
			string nextScene = Utility.getGameController().getNextScene();
			photonView.RPC("loadScene", PhotonTargets.All, nextScene);
		}

		if (Input.GetKey ("right")) {
			playerLocal.Rotate (new Vector3 (0, 2, 0));
		}
		if (Input.GetKey ("left")) {
			playerLocal.Rotate (new Vector3 (0, -2, 0));
		}
		
		if (Input.GetKey ("down")) {
			playerLocal.Rotate (new Vector3 (2, 0, 0));
		}
		
		if (Input.GetKey ("up")) {
			playerLocal.Rotate (new Vector3 (-2, 0, 0));
		}
		
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

	
	protected void checkHitBoss() {
//		print ("Camera.main.transform.position  " + Camera.main.transform.position);
//		print ("Camera.main.transform.forward  " + Camera.main.transform.forward);
//		print ("boss.transform.position " + boss.transform.position);
		Vector3 realBossPos = boss.transform.position + new Vector3 (0, 7.5f, 0);
		float angle = Utility.getVectorAngle(Camera.main.transform.forward, realBossPos - Camera.main.transform.position);
//		print ("angle " + angle);
		if (angle < Constants.cbMaxSpotlightAngle / 2) {
			EnemyController ec = boss.GetComponent<EnemyController> ();
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
			EnemyController ec = boss.GetComponent<EnemyController> ();
			ec.revive();
		}
	}

}
