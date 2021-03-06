﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionController : Photon.MonoBehaviour {

	public GameController gc;
	public GameObject instructionObj;	//	could be cb or tb depending on who the player is

	protected BackyardSceneController backyardSceneController = new BackyardSceneController();
	protected BossSceneController bossSceneController = new BossSceneController();
	protected DarkBackyardSceneController darkBackyardSceneController = new DarkBackyardSceneController();

	protected bool showingInstruction = false;

	public bool ShowingInstruction { get; set; }


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	public bool isInstructionFinished(int playerId) {
		if (ShowingInstruction)
			return false;

		if (gc != null) {
			if (gc.currScene == Constants.backyardSceneName) {
				return backyardSceneController.checkIfToShowInstruction (playerId) == false;
			} else if (gc.currScene == Constants.bossSceneName) {
				return bossSceneController.checkIfToShowInstruction (playerId) == false;
			} else if (gc.currScene == Constants.darkBackyardSceneName) {
				return darkBackyardSceneController.checkIfToShowInstruction (playerId) == false;
			}
		}
		return false;
	}

	public void resetInstruction() {
		
		if (gc != null) {
			if (gc.currScene == Constants.backyardSceneName) {
				backyardSceneController.reset ();
			} else if (gc.currScene == Constants.bossSceneName) {
				bossSceneController.reset ();
			} else if (gc.currScene == Constants.darkBackyardSceneName) {
				darkBackyardSceneController.reset ();
			}
		}
	}

	
	public virtual void showInstruction(string instructionText) {
		instructionObj.SetActive (true);
		
		Text text = Utility.FindTransform(instructionObj.transform, "Text").GetComponent<Text>();
		text.text = instructionText;
		
		ShowingInstruction = true;
		//		StartCoroutine (showInstructionRoutine(instructionText, seconds, instructionObj));
	}

	// return whether or not the request is accepted
	public virtual bool requestToJumpToNext() {
		if (ShowingInstruction) {
			stopInstruction();
			return true;
		}
		return false;
	}

	protected virtual void stopInstruction() {
		
		Text text = Utility.FindTransform(instructionObj.transform, "Text").GetComponent<Text>();
		text.text = "";
		instructionObj.SetActive (false);
		
		ShowingInstruction = false;
	}
}
