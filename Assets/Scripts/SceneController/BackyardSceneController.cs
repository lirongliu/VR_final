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
		instructionList [cbId] = new string[3];
		instructionList [cbId] [0] = "Work with your partner to find the exit of the maze!";
		instructionList [cbId][1] = "Press the button to move or stay still.";
		instructionList [cbId][2] = "Please note: neither of you can see the whole scene, please communicate to change information.";
		
		instructionList [tbId] = new string[3];
		instructionList [tbId][0] = "Work with your partner to find the exit of the maze! Press \"space\" to continue reading the instruction.";
		instructionList [tbId][1] = "Press ASDW to move";
		instructionList [tbId][2] = "Please note: neither of you can see the whole scene, please communicate to shared information.";

		
		restartInstructionList = new string[2][];
		restartInstructionList [cbId] = new string[1];
		restartInstructionList [cbId][0] = "Restart";
		
		restartInstructionList [tbId] = new string[1];
		restartInstructionList [tbId][0] = "Restart!";
	}
	
}
