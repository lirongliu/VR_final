using UnityEngine;
using System.Collections;

public class DarkBackyardSceneController : SceneController {


	// Use this for initialization
	public DarkBackyardSceneController () {
		currInstructionIdx = 0;
		
		instructionList = new string[1];
		instructionList [0] = "Task 3:\nWork with your partner to find the way home. Tablet player can light up the environment with spotlight. Cardboard player can kill enemies with torchlight, but it will also decrease the light of tablet player if you look at him.";
	}

	override public bool checkIfToShowInstruction() {
		if (currInstructionIdx == 0) {
			return true;
		}
		return false;
	}

}
