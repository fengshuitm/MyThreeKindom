using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Kindom_selectescript_item : ItemScript {

	public GameObject Flag;
	public GameObject fatherlist;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}


	public override bool Invalidate ()
	{

	/*	Texture2D tex=Resources.Load("spirite/flags/flag_"+id) as Texture2D;
		if (tex == null) {

			return;
		}
		Sprite Flagsprite = Flag.GetComponent<Image> ().sprite;
		*/
		//Flagsprite.r
		try
		{
		Flag.GetComponent<Image> ().sprite=Global_source_loader.getInstance().Flag_Sprite[id];// Sprite.Create(tex,new Rect(0,6*Flagsprite.rect.height,Flagsprite.rect.width,Flagsprite.rect.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值

		int kingID=Global_KindomData.getInstance().list_KindomData[id].KingID;

		if (Global_const.NONE_ID != kingID) {
			Text_item [0].text = "" + Global_HeroData.getInstance ().List_hero [kingID].GetAllName ();
		} else {
			Text_item [0].text = "";
		}

		Text_item [1].text =""+  Global_KindomData.getInstance().list_KindomData[id].id;
		
			return true;

		}
		catch {

			return false;
		}

	}

	public void interactive()
	{

		//Global_HeroData global_herodata = Global_HeroData.getInstance ();

		GlobalData.getInstance().nowkindomid=Global_KindomData.getInstance().list_KindomData[id].id;

		//Kindom_editor.GetComponent<UI_OBJ> ().move ();


		Messenger.Broadcast<int>("SelectKindom", id);

        //SendMessageUpwards ("Invalidate");
        Messenger.Broadcast("Invalidate");

        fatherlist.GetComponent<UI_OBJ> ().moveout ();

	}


}
