using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackyardSceneController : SceneController {


	// Use this for initialization
	public BackyardSceneController () {

		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;

		instructionList = new string[2][];
		instructionList [cbId] = new string[3];
		instructionList [cbId][0] = "cb Task 1:\nWork with your partner to find the exit of the maze!\nPlease note: neither of you can see the whole scene of the maze.";
		instructionList [cbId][1] = "cb test msg 2";
		instructionList [cbId][2] = "cb test msg 3";
		
		instructionList [tbId] = new string[3];
		instructionList [tbId][0] = "tb Task 1:\nWork with your partner to find the exit of the maze!\nPlease note: neither of you can see the whole scene of the maze.";
		instructionList [tbId][1] = "tb test msg 2";
		instructionList [tbId][2] = "tb test msg 3";
	}
	
}
