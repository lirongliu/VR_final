using UnityEngine;
using System.Collections;

public class BossSceneController : SceneController {

	// Use this for initialization
	public BossSceneController () {
		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;
		
		instructionList = new string[2][];
		instructionList [cbId] = new string[3];
		instructionList [cbId][0] = "Task 2:\nYou and your partner are controlling the same character. Tablet player controls the position, cardboard player controls the direction. Stare at the boss for 10 seconds and you will win.";
		instructionList [cbId][1] = "cb test msg 2";
		instructionList [cbId][2] = "cb test msg 3";
		
		instructionList [tbId] = new string[3];
		instructionList [tbId][0] = "Task 2:\nYou and your partner are controlling the same character. Tablet player controls the position, cardboard player controls the direction. Stare at the boss for 10 seconds and you will win.";
		instructionList [tbId][1] = "tb test msg 2";
		instructionList [tbId][2] = "tb test msg 3";
	}
}
