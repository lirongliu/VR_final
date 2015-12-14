using UnityEngine;
using System.Collections;

public class BossSceneCbPlayer : NetworkedPlayer {
	
	private GameObject hitEnemy;
	private GameObject boss;

	void OnLevelWasLoaded(int level) {
		print ("level " + level);
		if (level == 1) {
			photonView.RPC ("generateBoss", PhotonTargets.All, Random.Range (-30, 30), Random.Range (-30, 30), 1000f, "boss");
		}
	}
	
	void Start ()
	{
		DontDestroyOnLoad (this);
		
		avatar.transform.localPosition = new Vector3 (-5, 0.5f, -2);

		if (photonView.isMine) {
			GameObject cb = GameObject.Find ("CardboardMain");

			// set head transform
			this.headTransform = cb.transform;
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
				photonView.RPC ("generateEnemy", PhotonTargets.All, Random.Range (-30, 30), Random.Range (-30, 30), 100f, "chaseCb");
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
//						correctAvatarPos = (Vector3)stream.ReceiveNext();
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
	}

	
	protected void checkHitBoss() {
		
		float angle = Utility.getVectorAngle(Camera.main.transform.forward, boss.transform.position - Camera.main.transform.position);
		if (angle < Constants.cbSpotlightAngle / 2) {
			EnemyController ec = boss.GetComponent<EnemyController> ();
			ec.getHit (300);
			
			if (ec.shouldBeDead ()) {

				print ("boss should be dead");
				
//				if (NetworkController.enemyList.IndexOf (hitEnemy) != null && NetworkController.enemyList.IndexOf (hitEnemy) != (-1)) {
//					//print("NetworkedPlayer enemyList:"+NetworkController.enemyList[0]+"\t"+NetworkController.enemyList[1]+"\t"+NetworkController.enemyList[2]);
//					//print("INDEX:"+NetworkController.enemyList.IndexOf (hit.collider.gameObject));
//					photonView.RPC ("destroyEnemy", PhotonTargets.All, NetworkController.enemyList.IndexOf (hitEnemy));
//				}
			}
		} else {
			
			EnemyController ec = boss.GetComponent<EnemyController> ();
			ec.revive();
		}
	}

}
