﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class King_selectscript_item : ItemScript {

	public GameObject fatherlist;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}


	public override bool Invalidate ()
	{
		try
		{
		Global_HeroData global_herodata = Global_HeroData.getInstance ();

		Text_item [0].text = global_herodata.List_hero_sort[id].GetAllName();
		Text_item [1].text =""+ global_herodata.List_hero_sort [id].herozi;

		Text_item [2].text =""+  global_herodata.List_hero_sort [id].lead;
		Text_item [3].text =""+  global_herodata.List_hero_sort [id].might;
		Text_item [4].text =""+  global_herodata.List_hero_sort [id].wit;
		Text_item [5].text =""+  global_herodata.List_hero_sort [id].polity;
		Text_item [6].text = ""+ global_herodata.List_hero_sort [id].charm;
		Text_item [7].text =""+  global_herodata.List_hero_sort [id].id;
			return true;

		}
		catch {
			return false;


		}

	}

	public void interactive()
	{
		if (Global_HeroData.getInstance ().sortcount <= 0) {
			return;
		}
		//meetlist.GetComponent<heromeet_select_script> ().moveout ();
		//Global_events.getInstance ().traggerGlobalEvent (379,0,GlobalData.getInstance().nowheroid,hero_id);

		KindomData global_kindomdata=Global_KindomData.getInstance().list_KindomData[GlobalData.getInstance().nowkindomid];

		global_kindomdata.KingID = id;
        /*Global_HeroData global_herodata = Global_HeroData.getInstance ();

		GlobalData.getInstance().nowheroid=global_herodata.List_hero_sort [id].id;
		*/

        //SendMessageUpwards ("Invalidate");
        Messenger.Broadcast("Invalidate");

        fatherlist.GetComponent<UI_OBJ> ().moveout ();

	}


}
