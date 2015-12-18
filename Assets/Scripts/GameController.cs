using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameController : Photon.MonoBehaviour {
	
	public static bool play_audio=true;
	public string currScene = Constants.backyardSceneName;

	public bool gameStarted = false;

	public GameObject cbPlayer, tbPlayer;


	void Start () {
		DontDestroyOnLoad (this);
	}


	// Update is called once per frame
	void Update () {

		if (PhotonNetwork.playerList.Length == 2) {
			gameStarted = true;
		}

		if (cbPlayer == null) cbPlayer = Utility.getCbPlayer();
		if (tbPlayer == null) tbPlayer = Utility.getTbPlayer();

		
		if (cbPlayer != null && NetworkController.whoAmI == Constants.cbPlayerID) {
			if (cbPlayer.GetComponent<CbInstructionController>().enabled == false) {
				cbPlayer.GetComponent<CbInstructionController>().enabled = true;
			}
		} else if (tbPlayer != null && NetworkController.whoAmI == Constants.tbPlayerID) {
			if (tbPlayer.GetComponent<TbInstructionController>().enabled == false) {
				tbPlayer.GetComponent<TbInstructionController>().enabled = true;
			}
		}

		if (gameStarted == false) {
			return;
		}

	}

	public string getNextScene() {
		print ("currScene " + currScene);
		string nextScene = "";
		if (currScene == Constants.backyardSceneName) {
			nextScene = Constants.bossSceneName;

		} else if (currScene == Constants.bossSceneName) {
			nextScene = Constants.darkBackyardSceneName;

		} else if (currScene == Constants.darkBackyardSceneName) {
			nextScene = Constants.backyardSceneName;		//	for testing only... not working now...
		}
		return nextScene;
	}

	public void setCurrScene(string currScene) {
		this.currScene = currScene;
	}
}
