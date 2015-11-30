using UnityEngine;
using System.Collections;

public class NetworkedPlayer : Photon.MonoBehaviour
{
	public GameObject avatar;
	
	public Transform playerGlobal;
	public Transform playerLocal;
	
	protected Vector3 correctAvatarPos = Vector3.zero; //We lerp towards this
	protected Quaternion correctAvatarRot = Quaternion.identity; //We lerp towards this
	
	void Update()
	{
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
		}
		else {
			inputHandler();
		}

	}
	
	void Start ()
	{
		Debug.Log("i'm instantiated");

		if (photonView.isMine && NetworkController.whoAmI == Constants.IS_IPAD_PLAYER) {
			Debug.Log ("player is mine. I am the iPad player.");

			this.transform.localPosition = new Vector3(2, 1, 2);
			Transform spotlight = this.transform.Find("Spotlight");
			spotlight.SetParent(avatar.transform);

		} else if (photonView.isMine && NetworkController.whoAmI == Constants.IS_CB_PLAYER) {
			Debug.Log ("player is mine. I am the cardboard player.");

			GameObject cb = GameObject.Find ("CardboardMain");
			cb.transform.SetParent(avatar.transform);
//			cb.transform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
			this.transform.localPosition = new Vector3(-2,1,-2);
			
			//avatar.SetActive(false);
		}
		playerGlobal = avatar.transform;
	}
	
	protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(playerGlobal.position);
			stream.SendNext(playerGlobal.rotation);
//			stream.SendNext(playerLocal.localPosition);
//			stream.SendNext(playerLocal.localRotation);
//			print ("sent correctAvatarPos: " + playerGlobal.position);

		}
		else
		{
//			this.transform.position = (Vector3)stream.ReceiveNext();
//			this.transform.rotation = (Quaternion)stream.ReceiveNext();
			correctAvatarPos = (Vector3)stream.ReceiveNext();
			correctAvatarRot = (Quaternion)stream.ReceiveNext();
//			print ("received correctAvatarPos: " + correctAvatarPos);

		}
	}
	
	protected void translate(Vector3 dir) {
		
		Vector3 oldPos = playerGlobal.localPosition;
		Vector3 newPos = oldPos + dir * 0.1f;
		playerGlobal.localPosition = newPos;
	}

	protected void inputHandler() {
		if (Input.GetKey ("q")) {	//	moving towards the viewing direction
			this.translate (playerGlobal.forward);
		}
		
		if (Input.GetKey ("d")) {
			this.translate (new Vector3(1, 0, 0));
		}
		
		if (Input.GetKey ("a")) {
			this.translate (new Vector3(-1, 0, 0));
		}
		
		if (Input.GetKey ("s")) {
			this.translate (new Vector3(0, 0, -1));
		}
		
		if (Input.GetKey ("w")) {
			this.translate (new Vector3(0, 0, 1));
		}

		
		if (Input.GetKey ("right")) {
			playerGlobal.Rotate (new Vector3 (0, 2, 0));
		}
		if (Input.GetKey ("left")) {
			playerGlobal.Rotate (new Vector3 (0, -2, 0));
		}
		
		if (Input.GetKey ("down")) {
			playerGlobal.Rotate (new Vector3 (2, 0, 0));
		}
		
		if (Input.GetKey ("up")) {
			playerGlobal.Rotate (new Vector3 (-2, 0, 0));
		}
		
	}
}