using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class herodatashow_script : MonoBehaviour {

	//public Text [] Text_item;
	public int hero_id=Global_const.NONE_ID;

	public GameObject meetlist;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void sethero_ID(int _id)
	{
		hero_id = _id;

		if (hero_id==Global_const.NONE_ID) {

			clear ();
			return;
		}

		Global_HeroData global_herodata = Global_HeroData.getInstance ();

		try
		{
		this.transform.FindChild("Text (0)").GetComponent<Text>().text = global_herodata.List_hero[hero_id].GetAllName();

        }
		catch {

		}

		try
		{
            this.transform.FindChild("Text (1)").GetComponent<Text>().text = ""+ global_herodata.List_hero [hero_id].lead;
		}
		catch {

		}

		try
		{
            this.transform.FindChild("Text (2)").GetComponent<Text>().text = ""+  global_herodata.List_hero [hero_id].might;
		}
		catch {

		}

		try
		{
            this.transform.FindChild("Text (3)").GetComponent<Text>().text = ""+  global_herodata.List_hero [hero_id].wit;
		}
		catch {

		}

		try
		{
            this.transform.FindChild("Text (4)").GetComponent<Text>().text = ""+  global_herodata.List_hero [hero_id].polity;
		}
		catch {

		}

		try
		{
            this.transform.FindChild("Text (5)").GetComponent<Text>().text = ""+  global_herodata.List_hero [hero_id].charm;
		}
		catch {

		}

		try
		{
            this.transform.FindChild("Text (6)").GetComponent<Text>().text = ""+ global_herodata.List_hero [hero_id].reputation;
		}
		catch {
		}

		try
		{
	//	Text_item [7].text =""+  global_herodata.List_hero [hero_id].m_relationship.likelist [GlobalData.getInstance ().nowheroid];
		}
		catch {

		}
			
		try
		{
            this.transform.FindChild("Text (8)").GetComponent<Text>().text = ""+  global_herodata.List_hero [hero_id].bad_reputation;
		}
		catch {

		}
	}

	public void interactive()
	{
		if (hero_id == Global_const.NONE_ID) {
			return;
		}
		//meetlist.GetComponent<heromeet_select_script> ().moveout ();
		Global_events.getInstance ().traggerGlobalEvent (379,0,GlobalData.getInstance().nowheroid,hero_id);

		meetlist.GetComponent<UI_OBJ> ().moveout ();
	}

	void clear()
	{
		for (int i = 0; i < this.transform.childCount; i++) {
            //Debug.Log(i);

            this.transform.FindChild("Text ("+i+")").GetComponent<Text>().text = "-";

        }

	}
}
