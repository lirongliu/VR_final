using UnityEngine;
using System.Collections;

public class BackyardSceneController : SceneController {

	// Use this for initialization
	public BackyardSceneController () {

		currInstructionIdx = 0;

		instructionList = new string[1];
		instructionList [0] = "Task 1:\nWork with your partner to find the exit of the maze!\nPlease note: neither of you can see the whole scene of the maze.";
	
	}
	
	public string getCurrentInstruction() {
		if (currInstructionIdx < instructionList.Length) {
			return instructionList [currInstructionIdx];
		} else {
			return null;
		}
	}
	
	public override bool checkIfToShowInstruction() {
		if (currInstructionIdx == 0) {
			return true;
		}
		return false;
	}

}
