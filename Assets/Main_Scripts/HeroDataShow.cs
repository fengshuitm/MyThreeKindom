using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroDataShow : UI_OBJ {

	public GameObject [] Datas;

	// Use this for initialization
	void Start () {

        Datas[0] = transform.FindChild("lead").gameObject;
        Datas[1] = transform.FindChild("wit").gameObject;
        Datas[2] = transform.FindChild("charm").gameObject;
        Datas[3] = transform.FindChild("might").gameObject;
        Datas[4] = transform.FindChild("polity").gameObject;
        Datas[5] = transform.FindChild("armycount").gameObject;
        Datas[6] = transform.FindChild("Morale").gameObject;
        Datas[7] = transform.FindChild("style").gameObject;

        Datas[8] = transform.FindChild("reputation").gameObject;
        Datas[9] = transform.FindChild("bad_reputation").gameObject;
        Datas[10] = transform.FindChild("money").gameObject;
        Datas[11] = transform.FindChild("forage").gameObject;

    }

    void FixedUpdate () {

		if (Global_const.NONE_ID == GlobalData.getInstance ().nowheroid) {

			return;
		}
		try
		{
			herodata nowherotemp = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid];
			Datas [0].GetComponent<Text> ().text =""+ nowherotemp.lead;
			Datas [1].GetComponent<Text> ().text =""+ nowherotemp.wit;
			Datas [2].GetComponent<Text> ().text =""+ nowherotemp.charm;
			Datas [3].GetComponent<Text> ().text =""+ nowherotemp.might;
			Datas [4].GetComponent<Text> ().text =""+ nowherotemp.polity;
			Datas [5].GetComponent<Text> ().text =""+ Global_GuardunitData.getInstance().List_guardunit[nowherotemp.id].Armycount;
            Datas[6].GetComponent<Text>().text = "" + Global_GuardunitData.getInstance().List_guardunit[nowherotemp.id].Morale;
            Datas[7].GetComponent<Text>().text = "" + Global_GuardunitData.getInstance().List_guardunit[nowherotemp.id].style;

            Datas [8].GetComponent<Text> ().text =""+ nowherotemp.reputation;
			Datas [9].GetComponent<Text> ().text =""+ nowherotemp.bad_reputation;
            Datas[10].GetComponent<Text>().text = "" + nowherotemp.money;
            Datas[11].GetComponent<Text>().text = "" + nowherotemp.forage;

        }
        catch {

		}
	}
}
