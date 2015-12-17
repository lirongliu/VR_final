using UnityEngine;
using System.Collections;

public class BossSceneController : SceneController {

	// Use this for initialization
	public BossSceneController () {
		currInstructionIdx = 0;
		instructionList = new string[1];
		instructionList [0] = "Task 2:\nYou and your partner are controlling the same character. Tablet player controls the position, cardboard player controls the direction. Stare at the boss for 10 seconds and you will win.";
	}
	
	override public bool checkIfToShowInstruction() {
		if (currInstructionIdx == 0) {
			return true;
		}
		return false;
	}
}
