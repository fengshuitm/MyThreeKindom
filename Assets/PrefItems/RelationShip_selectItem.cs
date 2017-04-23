using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RelationShip_selectItem : ItemScript {

	public GameObject hero_editor;

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
			Text_item [2].text =""+  global_herodata.List_hero_sort [id].id;
			Text_item [3].text =""+  global_herodata.List_hero_sort [GlobalData.getInstance().nowheroid].m_relationship.friendsLikeHash[id];
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
        /*	Global_HeroData global_herodata = Global_HeroData.getInstance ();

            GlobalData.getInstance().nowheroid=global_herodata.List_hero_sort [id].id;

            hero_editor.GetComponent<creatorscript> ().move ();
            */
        Messenger.Broadcast<GameObject>("RelationShipItem",this.gameObject);

    }

	public void OnSelected()
	{


	}
		
}