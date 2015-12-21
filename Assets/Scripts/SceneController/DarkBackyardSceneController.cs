using UnityEngine;
using System.Collections;

public class DarkBackyardSceneController : SceneController {

	// Use this for initialization
	public DarkBackyardSceneController () {
		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;
		
		instructionList = new string[2][];
		instructionList [cbId] = new string[1];
		instructionList [cbId][0] = "Task 3:\nWork with your partner to find the way home. \n Use your torchlight to kill enemies. \n But your light will decrease the light of \n tablet player if you look at your partner.";

		instructionList [tbId] = new string[1];
		instructionList [tbId][0] = "Task 3:\nWork with your partner to find the way home. \n Use your light to guide your partner and warn them of threat! \n Avoid being stared by your partner whose torchlight can hurt you.";
	}
}
