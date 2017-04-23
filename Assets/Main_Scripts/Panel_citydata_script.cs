using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Panel_citydata_script : MonoBehaviour {
	public Text city_name;

	public Text city_peoplecount;

	public Text city_herocount;

	public Text city_leadername;

	int tempcount=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		//if (10 == tempcount) {
		//	tempcount=0;
		   int nowimportanceID = GlobalData.getInstance().nowselectedimportance;

		   city_name.text = "" + nowimportanceID;

		   city_peoplecount.text = "" + Global_ImportanceData.getInstance ().List_importance [nowimportanceID].PeoplecountNOW;

		   city_herocount.text = "" + Global_ImportanceData.getInstance ().List_importance [nowimportanceID].heroCount;

    		int leaderidtemp = Global_ImportanceData.getInstance ().List_importance [nowimportanceID].LeaderID;
	    	
		if (leaderidtemp != Global_const.NONE_ID) {
			city_leadername.text = "";

			for (int i = 0; i < Global_HeroData.getInstance ().List_hero [leaderidtemp].heroname.Length; i++) {
				city_leadername.text += Global_HeroData.getInstance ().List_hero [leaderidtemp].heroname[i];
			}
		} else {

			city_leadername.text = "-";
		}
		//}
	}
}
