using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController {

	protected string[][] instructionList;

	public int[] currInstructionIdx;

	protected int tbId = Constants.tbPlayerID;
	protected int cbId = Constants.cbPlayerID;

	public SceneController() {

	}

	public virtual bool checkIfToShowInstruction(int playerId) {
		if (currInstructionIdx[playerId] < instructionList[playerId].Length) {
			return true;
		}
		return false;
	}
	
	public virtual string getCurrentInstruction(int playerId) {
		if (currInstructionIdx[playerId] < instructionList[playerId].Length) {
			return instructionList[playerId][currInstructionIdx[playerId]];
		} else {
			return null;
		}
	}

}
