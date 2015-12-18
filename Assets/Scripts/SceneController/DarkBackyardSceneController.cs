using UnityEngine;
using System.Collections;

public class DarkBackyardSceneController : SceneController {

	// Use this for initialization
	public DarkBackyardSceneController () {
		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;
		
		instructionList = new string[2][];
		instructionList [cbId] = new string[3];
		instructionList [cbId][0] = "Task 3:\nWork with your partner to find the way home. Tablet player can light up the environment with spotlight. Cardboard player can kill enemies with torchlight, but it will also decrease the light of tablet player if you look at him.";
		instructionList [cbId][1] = "cb test msg 2";
		instructionList [cbId][2] = "cb test msg 3";
		
		instructionList [tbId] = new string[3];
		instructionList [tbId][0] = "Task 2:\nYou and your partner are controlling the same character. Tablet player controls the position, cardboard player controls the direction. Stare at the boss for 10 seconds and you will win.";
		instructionList [tbId][1] = "tb test msg 2";
		instructionList [tbId][2] = "tb test msg 3";
	}
}
