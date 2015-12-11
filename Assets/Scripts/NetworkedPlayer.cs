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

	void Start () {
	}
	
	void Update() {

	}
	
	protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting){
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
	
	protected void inputHandler() {
		float movingSpeed = 0.2f;

		AvatarController avatarController = avatar.GetComponent<AvatarController> ();
		if (Input.GetKey ("q")) {	//	moving towards the viewing direction
			avatarController.Move(Camera.main.transform.forward * 0.1f);
		}

		if (Input.GetKey ("l")) {
			photonView.RPC("loadScene", PhotonTargets.All, "BossScene");
		}

		if (Input.GetKey ("d")) {
			avatarController.Move(new Vector3(movingSpeed, 0, 0));
		}
		
		if (Input.GetKey ("a")) {
			avatarController.Move(new Vector3(-movingSpeed, 0, 0));
		}
		
		if (Input.GetKey ("s")) {
			avatarController.Move(new Vector3(0, 0, -movingSpeed));
		}
		
		if (Input.GetKey ("w")) {
			avatarController.Move(new Vector3(0, 0, movingSpeed));
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

}