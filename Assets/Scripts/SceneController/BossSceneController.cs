using UnityEngine;
using System.Collections;

public class BossSceneController : SceneController {

	// Use this for initialization
	public BossSceneController (): base() {
		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;
		
		instructionList = new string[2][];
		instructionList [cbId] = new string[3];
		instructionList [cbId] [0] = "Your partner is controlling the movement of your body!";
		instructionList [cbId] [1] = "Your can turn your head around to kill the ghost.";
		instructionList [cbId][2] = "Stare at the boss (biggest ghost) for 10 seconds and you will win";

		instructionList [tbId] = new string[3];
		instructionList [tbId][0] = "You’re controlling your partner’s body!";
		instructionList [tbId][1] = "Try to escape from the small ghosts or you will get killed.";
		instructionList [tbId][2] = "Your partner’s task is to stare at the biggest ghost and kill it!";

		restartInstructionList = new string[2][];
		restartInstructionList [cbId] = new string[1];
		restartInstructionList [cbId][0] = "Restart";
		
		restartInstructionList [tbId] = new string[1];
		restartInstructionList [tbId][0] = "Restart";

	}
}
