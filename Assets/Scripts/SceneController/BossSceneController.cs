using UnityEngine;
using System.Collections;

public class BossSceneController : SceneController {

	// Use this for initialization
	public BossSceneController (): base() {
		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;
		
		instructionList = new string[2][];
		instructionList [cbId] = new string[1];
		instructionList [cbId][0] = "Task 2:\nYou and your partner are controlling the same character! Stare at the boss for 10 seconds and to pass this puzzle.";
		
		instructionList [tbId] = new string[1];
		instructionList [tbId][0] = "Task 2:\nYou and your partner are controlling the same character. Control the movement of the character to avoid the enemies!";
		
		restartInstructionList = new string[2][];
		restartInstructionList [cbId] = new string[1];
		restartInstructionList [cbId][0] = "restart msg: Task 2:\nYou and your partner are controlling the same character! Stare at the boss for 10 seconds and to pass this puzzle.";
		
		restartInstructionList [tbId] = new string[1];
		restartInstructionList [tbId][0] = "restart msg: Task 2:\nYou and your partner are controlling the same character. Control the movement of the character to avoid the enemies!";

	}
}
