using UnityEngine;
using System.Collections;

public class DarkBackyardSceneController : SceneController {

	// Use this for initialization
	public DarkBackyardSceneController (): base()  {
		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;
		
		instructionList = new string[2][];
		instructionList [cbId] = new string[1];
		instructionList [cbId][0] = "Task 3:Work with your partner to find the way home. Use your torchlight to kill enemies. But your light will decrease the light of tablet player if you look at your partner.";

		instructionList [tbId] = new string[1];
		instructionList [tbId][0] = "Task 3:Work with your partner to find the way home. Use your light to guide your partner and warn them of threat! Avoid being stared by your partner whose torchlight can hurt you.";


		restartInstructionList = new string[2][];
		restartInstructionList [cbId] = new string[1];
		restartInstructionList [cbId][0] = "Restart";
		
		restartInstructionList [tbId] = new string[1];
		restartInstructionList [tbId] [0] = "Restart";
	}
}
