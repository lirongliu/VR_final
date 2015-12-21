using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController {

	protected string[][] instructionList;
	protected string[][] restartInstructionList;

	public int[] currInstructionIdx;

	protected int tbId = Constants.tbPlayerID;
	protected int cbId = Constants.cbPlayerID;

	public bool restarting;

	public SceneController() {
		// first time is defintely not restarting
		restarting = false;
	}
	
	public void reset() {
		// At this point, it has already run through all the instruction. therefore "restarting" is set to true
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;
		restarting = true;
	}

	public virtual bool checkIfToShowInstruction(int playerId) {
		if (restarting) {
			return currInstructionIdx[playerId] < restartInstructionList[playerId].Length;
		} else {
			return currInstructionIdx[playerId] < instructionList[playerId].Length;
		}
	}
	
	public virtual string getCurrentInstruction(int playerId) {
//		Debug.Log("restarting" +  restarting);
		
		if (restarting) {
			if (currInstructionIdx [playerId] < restartInstructionList [playerId].Length) {
				return restartInstructionList [playerId] [currInstructionIdx [playerId]];
			} 
		} else {
			if (currInstructionIdx [playerId] < instructionList [playerId].Length) {
				return instructionList [playerId] [currInstructionIdx [playerId]];
			} 
		}
		return null;

	}

}
