using UnityEngine;
using System.Collections;

public class InstructionController : Photon.MonoBehaviour {

	GameController gc;

	private BackyardSceneController backyardSceneController = new BackyardSceneController();
	private BossSceneController bossSceneController = new BossSceneController();
	private DarkBackyardSceneController darkBackyardSceneController = new DarkBackyardSceneController();

	// Use this for initialization
	void Start () {
		gc = Utility.getGameController ();
	}
	
	// Update is called once per frame
	void Update () {
		
		SceneController sc = null;
		if (NetworkController.whoAmI == Constants.IS_IPAD_PLAYER) {
			
			if (gc.currScene == Constants.backyardSceneName) {
				//				sc = backyardSceneController;
				string currInstruction = backyardSceneController.getCurrentInstruction();
				if (currInstruction != null) {
					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					print (currInstruction);
					
					//					StartCoroutine(showInstruction(currInstruction, 5));
					backyardSceneController.currInstructionIdx++;
				}
			} else if (gc.currScene == Constants.bossSceneName) {
				//				sc = bossSceneController;
				string currInstruction = bossSceneController.getCurrentInstruction();
				if (currInstruction != null) {
					print (currInstruction);
					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					
					//					StartCoroutine(showInstructionRoutine(currInstruction, 5));
					bossSceneController.currInstructionIdx++;
				}
			} else if (gc.currScene == Constants.darkBackyardSceneName) {
				//				sc = darkBackyardSceneController;
				string currInstruction = darkBackyardSceneController.getCurrentInstruction();
				if (currInstruction != null) {
					print (currInstruction);
					
					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					//					StartCoroutine(showInstructionRoutine(currInstruction, 5));
					darkBackyardSceneController.currInstructionIdx++;
				}
			}
		}
	}
}
