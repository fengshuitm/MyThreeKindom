using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hero_selectscript_pic : ItemScript {

	public GameObject hero_editor;
	public GameObject face;
	public GameObject name;

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

		face.GetComponent<Image> ().sprite = Global_source_loader.getInstance ().hero_L_face [id];
		name.GetComponent<Text> ().text=global_herodata.List_hero_sort[id].id+"-"+global_herodata.List_hero_sort[id].GetAllName();
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
		Global_HeroData global_herodata = Global_HeroData.getInstance ();

		GlobalData.getInstance().nowheroid=global_herodata.List_hero_sort [id].id;

		hero_editor.GetComponent<creatorscript> ().move ();

	}


}
