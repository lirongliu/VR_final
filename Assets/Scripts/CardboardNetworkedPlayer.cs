using UnityEngine;
using System.Collections;

public class CardboardNetworkedPlayer : NetworkedPlayer {
	
	void Start () {

		if (photonView.isMine) {
			Debug.Log ("player is mine. I am the cardboard player.");
			
			//GameObject.Find ("CardboardMain").transform.Translate (Random.Range (-4.0F, 4.0F), 0, Random.Range (-4.0F, 4.0F));

			GameObject cardboardMain=GameObject.Find ("CardboardMain");

			playerGlobal = cardboardMain.transform;
			playerLocal = playerGlobal.Find ("Head");
			
			this.transform.SetParent (playerLocal);

			avatar.transform.localPosition = new Vector3(0, 1, 0);

			//avatar.SetActive(false)
		}
	}
	

	void Update () {
		//Debug.Log(avatar.transform.forward.x+"\t"+avatar.transform.forward.z);

		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			print ("correctAvatarPos: " + correctAvatarPos);
		}
		else {
			inputHandler();

		}

	}
}
