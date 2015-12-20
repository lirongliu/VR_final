using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CbInstructionController : InstructionController {

	// Use this for initialization
	void Start () {
		gc = Utility.getGameController ();

		instructionObj = GameObject.FindWithTag("cbInstruction");
		instructionObj.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
		SceneController sc = null;
		if (ShowingInstruction == false) {
			
			if (gc.currScene == Constants.backyardSceneName) {
				//				sc = backyardSceneController;
				string currInstruction = backyardSceneController.getCurrentInstruction(Constants.cbPlayerID);
				if (backyardSceneController.checkIfToShowInstruction(Constants.cbPlayerID) && currInstruction != null) {
//					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					showInstruction(currInstruction);
					print (currInstruction);
					
					//					StartCoroutine(showInstruction(currInstruction, 5));
					backyardSceneController.currInstructionIdx[Constants.cbPlayerID]++;
				}
			} else if (gc.currScene == Constants.bossSceneName) {
				//				sc = bossSceneController;
				string currInstruction = bossSceneController.getCurrentInstruction(Constants.cbPlayerID);
				if (bossSceneController.checkIfToShowInstruction(Constants.cbPlayerID) && currInstruction != null) {
					print (currInstruction);
//					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					showInstruction(currInstruction);

					
					//					StartCoroutine(showInstructionRoutine(currInstruction, 5));
					bossSceneController.currInstructionIdx[Constants.cbPlayerID]++;
				}
			} else if (gc.currScene == Constants.darkBackyardSceneName) {
				//				sc = darkBackyardSceneController;
				string currInstruction = darkBackyardSceneController.getCurrentInstruction(Constants.cbPlayerID);
				if (darkBackyardSceneController.checkIfToShowInstruction(Constants.cbPlayerID) && currInstruction != null) {
					print (currInstruction);
					
//					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					showInstruction(currInstruction);

					//					StartCoroutine(showInstructionRoutine(currInstruction, 5));
					darkBackyardSceneController.currInstructionIdx[Constants.cbPlayerID]++;
				}
			}
		}
	}

}
