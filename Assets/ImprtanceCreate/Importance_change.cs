using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Importance_change : MonoBehaviour {

	public GameObject [] itemList;
	public GameObject messageBox;


	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{


		itemList [0] = this.transform.FindChild("id").gameObject;
		itemList [1] = this.transform.FindChild("M_name").gameObject;
		itemList [2] = this.transform.FindChild ("incomeNOW").gameObject;
		itemList [3] = this.transform.FindChild("agricultureNOW").gameObject;
		itemList [4] = this.transform.FindChild("PeoplecountNOW").gameObject;
		itemList [5] = this.transform.FindChild("defenseNOW").gameObject;
		itemList [6] = this.transform.FindChild("money").gameObject;
		itemList [7] = this.transform.FindChild("forage").gameObject;
		itemList [8] = this.transform.FindChild("LeaderID_button/LeaderID").gameObject;
		itemList [9] = this.transform.FindChild("KindomID_button/KindomID").gameObject;
		itemList [10] = this.transform.FindChild("Point_button/Point").gameObject;

		//(Text_item[0] as GameObject).GetComponent<Text> () = "fssf";
		//importancechangeUI = GameObject.Find ("Canvas/importancechange");

		//Messenger.Broadcast<float>("Selecthero", id);

		Messenger.AddListener<int>("Selecthero", Selecthero);
		//Messenger.AddListener<int>("SelectKindom", SelectKindom);
		Messenger.AddListener<int>("SelectImportance", SelectImportance);

		Messenger.AddListener ("Invalidate",Invalidate);


		Invalidate ();
	}


	void OnDestroy()
	{
		//Messenger.RemoveListener("Invalidate", Invalidate);


	}

/*	void SelectKindom(int _id)
	{

		Global_ImportanceData.getInstance ().List_importance [GlobalData.getInstance ().nowimportanceID].KindomID = _id;
		Invalidate ();
	}
    */

	void Selecthero(int _id)
	{
		Global_ImportanceData.getInstance ().List_importance [GlobalData.getInstance ().nowimportanceID].LeaderID = _id;
		Invalidate ();
	}

	void SelectImportance(int _id)
	{
		GlobalData.getInstance ().nowimportanceID = _id;
		Invalidate ();
	}

	void Invalidate()
	{
		/*
		itemList [0] = this.transform.FindChild("id").gameObject;
		itemList [1] = this.transform.FindChild("M_name").gameObject;
		itemList [2] = this.transform.FindChild ("incomeNOW").gameObject;
		itemList [3] = this.transform.FindChild("agricultureNOW").gameObject;
		itemList [4] = this.transform.FindChild("PeoplecountNOW").gameObject;
		itemList [5] = this.transform.FindChild("defenseNOW").gameObject;
		itemList [6] = this.transform.FindChild("money").gameObject;
		itemList [7] = this.transform.FindChild("forage").gameObject;
		itemList [8] = this.transform.FindChild("LeaderID").gameObject;
		*/

		if (Global_const.NONE_ID == GlobalData.getInstance ().nowimportanceID) {
			return;

		}

		Importance tempimportance = Global_ImportanceData.getInstance ().List_importance [GlobalData.getInstance ().nowimportanceID];
		itemList [0].GetComponent<Text> ().text = ""+tempimportance.id;
		itemList [1].GetComponent<InputField>().text = ""+tempimportance.M_name;
		itemList [2].GetComponent<InputField>().text = ""+tempimportance.incomeNOW;
		itemList [3].GetComponent<InputField>().text = ""+tempimportance.agricultureNOW;
		itemList [4].GetComponent<InputField>().text = ""+tempimportance.PeoplecountNOW;
		itemList [5].GetComponent<InputField>().text = ""+tempimportance.defenseNOW;
		itemList [6].GetComponent<InputField>().text = ""+tempimportance.money;
		itemList [7].GetComponent<InputField>().text = ""+tempimportance.forage;
		int leaderidtemp = tempimportance.LeaderID;

		if (leaderidtemp != Global_const.NONE_ID) {
			itemList [8].GetComponent<Text> ().text = Global_HeroData.getInstance ().List_hero [leaderidtemp].GetAllName ();
		} else {
			itemList [8].GetComponent<Text> ().text = "无";
		}

		//int kindomidtemp = tempimportance.KindomID;
/*
		if (kindomidtemp != Global_const.NONE_ID) {
			itemList [9].GetComponent<Text> ().text =""+ kindomidtemp;
		} else {
			itemList [9].GetComponent<Text> ().text = "无";
		}
*/
		itemList [10].GetComponent<Text> ().text = "x:" + tempimportance.GetPosition().x +"\n" + "y:" + tempimportance.GetPosition().y;

	}
	// Update is called once per frame
	void Update () {
	
	}

	public void save()
	{
		try
		{
		Importance tempimportance = Global_ImportanceData.getInstance ().List_importance [GlobalData.getInstance ().nowimportanceID];

		tempimportance.M_name = itemList [1].GetComponent<InputField> ().text;
		tempimportance.incomeNOW=int.Parse(itemList [2].GetComponent<InputField>().text);
		tempimportance.agricultureNOW=int.Parse(itemList [3].GetComponent<InputField>().text);
		tempimportance.PeoplecountNOW=int.Parse(itemList [4].GetComponent<InputField>().text);
		tempimportance.defenseNOW=int.Parse(itemList [5].GetComponent<InputField>().text);
		tempimportance.money=int.Parse(itemList [6].GetComponent<InputField>().text);
		tempimportance.forage=int.Parse(itemList [7].GetComponent<InputField>().text);
            //leader已载入
            //kindom已载入

            StartCoroutine(Global_ImportanceData.getInstance().Save_importance_Xml());
	    
            //Global_XML_IO.getInstance().Save_importance_Xml();
		
			Messenger.Broadcast ("invalidatelist");

			messageBox.GetComponent<UI_OBJ>().move();


		}
		catch {

		}

	}
}
