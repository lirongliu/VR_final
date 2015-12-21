using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class NetworkedPlayer : Photon.MonoBehaviour
{
	
	public GameObject avatar;
	public Transform playerGlobal;
	public Transform playerLocal;
	public GameObject Enemy;

	protected Vector3 correctAvatarPos = Vector3.zero; //We lerp towards this
	protected Quaternion correctAvatarRot = Quaternion.identity; //We lerp towards this
	protected Quaternion correctHeadRot = Quaternion.identity; //We lerp towards this

	protected Transform headTransform;

	protected float movingSpeed = Constants.defaultMovingSpeed;

//	private int count = 0;

	protected int life = 100;

	void Start () {
	}
	
	void Update() {
	}

	public virtual void reset(bool restartScene) {
		life = 100;
		NetworkController.iAmReady = false;
		NetworkController.otherPlayerReady = false;

		if (restartScene) {
			CbInstructionController c = Utility.getCbInstructionController ();
			if (c != null) {
				c.resetInstruction ();
			}
		
			TbInstructionController t = Utility.getTbInstructionController ();
			if (t != null) {
				t.resetInstruction ();
			}
		}

	}
	
	protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

		if (stream.isWriting){
			if (playerGlobal == null || headTransform == null)
				return;

			stream.SendNext(playerGlobal.position);
			stream.SendNext(playerGlobal.rotation);
			stream.SendNext(headTransform.localRotation);
		}
		else{
			correctAvatarPos = (Vector3)stream.ReceiveNext();
			correctAvatarRot = (Quaternion)stream.ReceiveNext();
			correctHeadRot = (Quaternion)stream.ReceiveNext();
		}
	}
	
	protected virtual void instructionHandler() {
	}

	public void damage(int d) {
//		print ("damage");
		life -= d;
	}
	
	protected void checkLife() {
//		print ("checkLife " + life);

		if (life <= 0) {
			print ("checkLife " + life);
			photonView.RPC("resetCurrentScene", PhotonTargets.All);
		}
	}

	public void move(Vector3 dir) {
		
		AvatarController avatarController = avatar.GetComponent<AvatarController> ();
		avatarController.Move(Utility.movementAdjustedWithFPS(movingSpeed * dir));
	}

	public void moveForward() {
		this.move(Camera.main.transform.forward);
	}

	protected void checkFallingOutsideTheScene() {
		if (avatar.transform.position.y < -10) {
			print ("checkFallingOutsideTheScene");
			photonView.RPC("resetCurrentScene", PhotonTargets.All);
		}
	}

	protected bool arriveInDest(Vector3 dest, float allowance) {
		GameObject cbNetworkedPlayerAvatar = GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);
		GameObject tbNetworkedPlayerAvatar = GameObject.FindGameObjectWithTag (Constants.tbPlayerAvatarTag);
		if (cbNetworkedPlayerAvatar != null && tbNetworkedPlayerAvatar != null) {
			float dist1 = Vector3.Distance(cbNetworkedPlayerAvatar.transform.position, dest);
			float dist2 = Vector3.Distance(tbNetworkedPlayerAvatar.transform.position, dest);
//			print ("dist1 " + dist1);
//			print ("dist2 " + dist2);
			if (dist1 + dist2 < allowance) {
				return true;
			}
		}
		return false;
	}

	// only called by enemy controller
	public void killEnemy(int id) {
		photonView.RPC ("destroyEnemy", PhotonTargets.All, id);
	}
	public void assignEnemyChasingDir(Vector3 dir, int enemyId) {
		photonView.RPC ("setEnemyChasingDir", PhotonTargets.Others, dir, enemyId);
	}
	public void assignEnemyChasingTarget(int playId, int enemyId) {
		photonView.RPC ("setEnemyChasingTarget", PhotonTargets.Others, playId, enemyId);
	}
}
