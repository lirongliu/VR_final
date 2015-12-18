using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class TbNetworkedPlayer : NetworkedPlayer
{

	void Start () {
	}
	
	void Update() {

	}

	protected virtual void instructionHandler() {
		if (Input.GetKeyUp ("space")) {
			Utility.getTbInstructionController ().requestToJumpToNext ();
		}
	}
	
	protected virtual void inputHandler() {
		
//		if (Input.GetKey ("r")) {
//			photonView.RPC("resetScene", PhotonTargets.All);
//		}

		if (Input.GetKey ("l")) {
			string nextScene = Utility.getGameController().getNextScene();
			photonView.RPC("loadScene", PhotonTargets.All, nextScene);
		}

		if (Input.GetKey ("q")) {	//	moving towards the viewing direction
			this.moveForward();
		}
		if (Input.GetKey ("d")) {
			this.move(new Vector3(1, 0, 0));
		}
		
		if (Input.GetKey ("a")) {
			this.move(new Vector3(-1, 0, 0));
		}
		
		if (Input.GetKey ("s")) {
			this.move(new Vector3(0, 0, -1));
		}
		
		if (Input.GetKey ("w")) {
			this.move(new Vector3(0, 0, 1));
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

		if (Input.GetKey ("]")) {
			movingSpeed *= 1.5f;

		}
		
		if (Input.GetKey ("[")) {
			movingSpeed /= 1.5f;
		}
	}

}
