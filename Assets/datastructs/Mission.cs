using UnityEngine;
using System.Collections;

public class Mission  {

	//当前任务
	int Now_mission;
	//当前行为
	int Now_action;
	//上个月任务成果
	int LastMonth_Achievements;
	
	public void ClearLastMonth()
	{
		LastMonth_Achievements=0;
	}

}
