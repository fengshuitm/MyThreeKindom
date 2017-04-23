using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Importance_selectscript_item : ItemScript {

	public GameObject fatherlist;
	// Use this for initialization
	public GameObject importancechangeUI;


	void Awake()
	{
		Text_item [0] = this.transform.FindChild("id").GetComponent<Text>();
		Text_item [1] = this.transform.FindChild("M_name").GetComponent<Text>();
		Text_item [2] = this.transform.FindChild("incomeNOW").GetComponent<Text>();
		Text_item [3] = this.transform.FindChild("agricultureNOW").GetComponent<Text>();
		Text_item [4] = this.transform.FindChild("PeoplecountNOW").GetComponent<Text>();
		Text_item [5] = this.transform.FindChild("defenseNOW").GetComponent<Text>();
		Text_item [6] = this.transform.FindChild("money").GetComponent<Text>();
		Text_item [7] = this.transform.FindChild("forage").GetComponent<Text>();

		Text_item [8] = this.transform.FindChild("LeaderID").GetComponent<Text>();

		importancechangeUI = GameObject.Find ("Canvas/importancechange");
	}

	void Start () {



	}

	// Update is called once per frame
	void Update () {

	}



	public override bool Invalidate ()
	{
		try
		{
			Global_ImportanceData global_Importancedata = Global_ImportanceData.getInstance ();

			if(global_Importancedata.List_importance[id].id==Global_const.NONE_ID)
			{
				clear();
				return false;

			}
			else
			{
			/*
			 * 
			Text_item [0] = this.transform.FindChild("id").GetComponent<Text>();
			Text_item [1] = this.transform.FindChild("M_name").GetComponent<Text>();
			Text_item [2] = this.transform.FindChild("incomeNOW").GetComponent<Text>();
			Text_item [3] = this.transform.FindChild("agricultureNOW").GetComponent<Text>();
			Text_item [4] = this.transform.FindChild("PeoplecountNOW").GetComponent<Text>();
			Text_item [5] = this.transform.FindChild("defenseNOW").GetComponent<Text>();
			Text_item [6] = this.transform.FindChild("money").GetComponent<Text>();
			Text_item [7] = this.transform.FindChild("forage").GetComponent<Text>();
			Text_item [8] = this.transform.FindChild("LeaderID").GetComponent<Text>();
			*/
			Text_item [0].text =""+ global_Importancedata.List_importance[id].id;
			Text_item [1].text =""+ global_Importancedata.List_importance[id].M_name;
			Text_item [2].text =""+ global_Importancedata.List_importance[id].incomeNOW;
			Text_item [3].text =""+ global_Importancedata.List_importance[id].agricultureNOW;
			Text_item [4].text =""+ global_Importancedata.List_importance[id].PeoplecountNOW;
			Text_item [5].text =""+ global_Importancedata.List_importance[id].defenseNOW;
			Text_item [6].text =""+ global_Importancedata.List_importance[id].money;
			Text_item [7].text =""+ global_Importancedata.List_importance[id].forage;

				int leaderidtemp = global_Importancedata.List_importance[id].LeaderID;

				if (leaderidtemp != Global_const.NONE_ID) {
					Text_item [8].text =""+ Global_HeroData.getInstance ().List_hero [leaderidtemp].GetAllName ();

				} else {
					Text_item [8].text = "无";
				}

			return true;
			}
		}
		catch {
			return false;

		}

	}

	public void interactive()
		{

		Messenger.Broadcast<int>("SelectImportance",id);
		importancechangeUI.GetComponent<UI_OBJ> ().move ();
		fatherlist.GetComponent<UI_OBJ> ().moveout();
		Messenger.Broadcast ("Invalidate");

	}


}