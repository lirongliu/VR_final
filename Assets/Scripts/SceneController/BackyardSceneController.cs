using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackyardSceneController : SceneController {


	// Use this for initialization
	public BackyardSceneController (): base() {

		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;

		instructionList = new string[2][];
		instructionList [cbId] = new string[2];
		instructionList [cbId][0] = "Task 1:\nWork with your partner to find the exit of the maze!\n Press the button to see the next instruction!";
		instructionList [cbId][1] = "Press the button to move or stay still.";
		
		instructionList [tbId] = new string[1];
		instructionList [tbId][0] = "Task 1:\nWork with your partner to find the exit of the maze!";

		
		restartInstructionList = new string[2][];
		restartInstructionList [cbId] = new string[2];
		restartInstructionList [cbId][0] = "restart msg: Task 1:\nWork with your partner to find the exit of the maze!\n Press the button to see the next instruction!";
		restartInstructionList [cbId][1] = "restart msg:  Press the button to move or stay still.";
		
		restartInstructionList [tbId] = new string[1];
		restartInstructionList [tbId][0] = "restart msg:  Task 1:\nWork with your partner to find the exit of the maze!";
	}
	
}
