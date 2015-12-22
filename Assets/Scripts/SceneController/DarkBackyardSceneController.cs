using UnityEngine;
using System.Collections;

public class DarkBackyardSceneController : SceneController {

	// Use this for initialization
	public DarkBackyardSceneController (): base()  {
		currInstructionIdx = new int[2];
		currInstructionIdx [cbId] = 0;
		currInstructionIdx [tbId] = 0;
		
		instructionList = new string[2][];
		instructionList [cbId] = new string[3];
		instructionList [cbId][0] = "Find your way home!";
		instructionList [cbId][1] = "There will be enemies coming from different direction and you have a torchlight to kill them. Gaze at enemies and they will disappear.";
		instructionList [cbId][2] = "Remember: your torchlight will also hurt your partner, it will dim the intensity and reduce the range of his light. So do avoid that!";

		instructionList [tbId] = new string[3];
		instructionList [tbId][0] = "Find your way home! You have a spotlight that can light up the area and help you read the map.";
		instructionList [tbId][1] = "There will be enemies coming from different direction. It’s your job to tell your partner where are they coming from.";
		instructionList [tbId][2] = "Your partner has the torchlight to kill enemies, but the torchlight also has the power to dim the intensity and reduce the range of your light. So run away when your partner is turning towards you!";

		restartInstructionList = new string[2][];
		restartInstructionList [cbId] = new string[1];
		restartInstructionList [cbId][0] = "Restart";
		
		restartInstructionList [tbId] = new string[1];
		restartInstructionList [tbId] [0] = "Restart";
	}
}
