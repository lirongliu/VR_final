using UnityEngine;
using System.Collections;

public class SceneController {

	protected string[] instructionList;

	public int currInstructionIdx;

	public SceneController() {

	}

	public virtual bool checkIfToShowInstruction() {
		return false;
	}

	public virtual string getCurrentInstruction() {
		if (currInstructionIdx < instructionList.Length) {
			return instructionList [currInstructionIdx];
		} else {
			return null;
		}
	}

}
