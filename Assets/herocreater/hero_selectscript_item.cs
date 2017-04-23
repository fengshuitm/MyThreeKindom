using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hero_selectscript_item : ItemScript {

	//public GameObject hero_editor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public override bool Invalidate ()
	{

		if(this.id==Global_const.NONE_ID)
        {
            this.transform.FindChild("Text (0)").GetComponent<Text>().text = "";
            return true;
        }

        Global_HeroData global_herodata = Global_HeroData.getInstance();

        if (this.id> global_herodata.sortcount)
        {
            this.transform.FindChild("Text (0)").GetComponent<Text>().text = "";
            return true;
        }

        if (global_herodata.List_hero_sort[id].id == GlobalData.getInstance().nowheroid)
        {
            this.transform.FindChild("Text (0)").GetComponent<Text>().color = Color.yellow;
        }
        else if(global_herodata.List_hero_sort[id].m_relationship.belong_kindom== global_herodata.List_hero[GlobalData.getInstance().nowheroid].m_relationship.belong_kindom)
        {
            if(global_herodata.List_hero_sort[GlobalData.getInstance().nowheroid].m_relationship.belong_kindom!=Global_const.NONE_ID)
            {
                this.transform.FindChild("Text (0)").GetComponent<Text>().color = Color.green;
            }
        }
        else
        {
            this.transform.FindChild("Text (0)").GetComponent<Text>().color = Color.white;
        }



        string strtemp = "";
            int strlength = 7;
            string strspace = "            ";
            strtemp= "姓名:" + global_herodata.List_hero_sort[id].GetAllName()+ strspace;
            this.transform.FindChild("Text (0)").GetComponent<Text>().text = strtemp.Substring(0, strlength);

            strtemp = "字:" + global_herodata.List_hero_sort[id].herozi+ strspace;
            this.transform.FindChild("Text (0)").GetComponent<Text>().text += strtemp.Substring(0, strlength);

            strtemp = "统御:" + global_herodata.List_hero_sort[id].lead+ strspace;
            this.transform.FindChild("Text (0)").GetComponent<Text>().text += strtemp.Substring(0, strlength);

            strtemp = "力量:" + global_herodata.List_hero_sort[id].might+ strspace;
            this.transform.FindChild("Text (0)").GetComponent<Text>().text += strtemp.Substring(0, strlength);

            strtemp = "智力:" + global_herodata.List_hero_sort[id].wit + strspace;
            this.transform.FindChild("Text (0)").GetComponent<Text>().text += strtemp.Substring(0, strlength);

            strtemp = "政治:" + global_herodata.List_hero_sort[id].polity + strspace;
            this.transform.FindChild("Text (0)").GetComponent<Text>().text += strtemp.Substring(0, strlength);

            strtemp = "魅力:" + global_herodata.List_hero_sort[id].charm + strspace;
            this.transform.FindChild("Text (0)").GetComponent<Text>().text += strtemp.Substring(0, strlength);

            strtemp = "ID:" + global_herodata.List_hero_sort[id].id + strspace;
            this.transform.FindChild("Text (0)").GetComponent<Text>().text += strtemp.Substring(0, strlength);
			return true;

	}

	public void interactive()
	{
		/*if (Global_HeroData.getInstance ().sortcount <= 0) {
			return;
		}
		//meetlist.GetComponent<heromeet_select_script> ().moveout ();
		//Global_events.getInstance ().traggerGlobalEvent (379,0,GlobalData.getInstance().nowheroid,hero_id);
		Global_HeroData global_herodata = Global_HeroData.getInstance ();

		GlobalData.getInstance().nowheroid=global_herodata.List_hero_sort [id].id;

		hero_editor.GetComponent<creatorscript> ().move ();
        */
        //////////////////////////////////////////////////////////


        if (Global_HeroData.getInstance().sortcount <= 0)
        {
            return;
        }
        //meetlist.GetComponent<heromeet_select_script> ().moveout ();
        //Global_events.getInstance ().traggerGlobalEvent (379,0,GlobalData.getInstance().nowheroid,hero_id);

        //KindomData global_kindomdata=Global_KindomData.getInstance().list_KindomData[GlobalData.getInstance().nowkindomid];

        //global_kindomdata.KingID = id;
        /*Global_HeroData global_herodata = Global_HeroData.getInstance ();

		GlobalData.getInstance().nowheroid=global_herodata.List_hero_sort [id].id;
		*/
        Global_HeroData global_herodata = Global_HeroData.getInstance();

        if(id!=Global_const.NONE_ID)
        {
            Messenger.Broadcast<int>("Selecthero", global_herodata.List_hero_sort[id].id);
        }
        

    }


}
