using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TbInstructionController : InstructionController {


	// Use this for initialization
	void Start () {
		gc = Utility.getGameController ();
		instructionObj = GameObject.FindWithTag("tbInstruction");

		instructionObj.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if (ShowingInstruction == false) {
			if (gc.currScene == Constants.backyardSceneName) {
				string currInstruction = backyardSceneController.getCurrentInstruction(Constants.tbPlayerID);
				if (backyardSceneController.checkIfToShowInstruction(Constants.tbPlayerID) && currInstruction != null) {
//					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					showInstruction(currInstruction);

					print (currInstruction);

					backyardSceneController.currInstructionIdx[Constants.tbPlayerID]++;
				}
			} else if (gc.currScene == Constants.bossSceneName) {
				//				sc = bossSceneController;
				string currInstruction = bossSceneController.getCurrentInstruction(Constants.tbPlayerID);
				if (bossSceneController.checkIfToShowInstruction(Constants.tbPlayerID) && currInstruction != null) {
					print (currInstruction);
//					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					showInstruction(currInstruction);
					//					StartCoroutine(showInstructionRoutine(currInstruction, 5));
					bossSceneController.currInstructionIdx[Constants.tbPlayerID]++;
				}
			} else if (gc.currScene == Constants.darkBackyardSceneName) {
				//				sc = darkBackyardSceneController;
				string currInstruction = darkBackyardSceneController.getCurrentInstruction(Constants.tbPlayerID);
				if (darkBackyardSceneController.checkIfToShowInstruction(Constants.tbPlayerID) && currInstruction != null) {

//					photonView.RPC ("showInstruction", PhotonTargets.All, currInstruction, 5);
					showInstruction(currInstruction);
					print (currInstruction);

					//					StartCoroutine(showInstructionRoutine(currInstruction, 5));
					darkBackyardSceneController.currInstructionIdx[Constants.tbPlayerID]++;
				}
			}

		}
	}

}
