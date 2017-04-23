using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Kindomeditor : UI_OBJ {

	public GameObject nowflag;
	public GameObject nowid;
	public GameObject nowking;

	public GameObject messageBox;
	// Use this for initialization
	void Start () {
	}
	
    

	// Update is called once per frame
	void Update () {
	
	}

	void Awake()
	{
		Messenger.AddListener<int>("SelectKindom", SelectKindom);
		Messenger.AddListener<int>("Selecthero", Selecthero);

	}

	void SelectKindom(int _id)
	{

		this.GetComponent<UI_OBJ> ().move ();
	}

	void Selecthero(int _id)
	{
		int nowkindomid = GlobalData.getInstance ().nowkindomid;
		Global_KindomData.getInstance ().list_KindomData [nowkindomid].KingID = _id;

        Global_HeroData.getInstance().List_hero[_id].m_relationship.belong_kindom = nowkindomid;
        Invalidate ();
	}

	public override void Invalidate ()
	{
		try
		{
		int nowkindomid = GlobalData.getInstance ().nowkindomid;
		nowflag.GetComponent<Image> ().sprite = Global_source_loader.getInstance ().Flag_Sprite [nowkindomid];
		nowid.GetComponent<Text> ().text =""+ nowkindomid;

		int nowkingid = Global_KindomData.getInstance ().list_KindomData [nowkindomid].KingID;

			if(Global_const.NONE_ID!= nowkingid)
			{
				string nowkingname = Global_HeroData.getInstance ().List_hero [nowkingid].GetAllName();
				nowking.GetComponent<Text> ().text =""+ nowkingname;
			}
			else
			{
				nowking.GetComponent<Text> ().text ="-";
			}
		}
		catch {


		}
	}

	public void Reset()
	{
		int nowkindomid = GlobalData.getInstance ().nowkindomid;

		Global_KindomData.getInstance ().list_KindomData [nowkindomid].KingID=Global_const.NONE_ID;

		Invalidate ();
	}

	public void Save()
	{
        //int nowkindomid = GlobalData.getInstance ().nowkindomid;

        //Global_KindomData.getInstance ().list_KindomData [nowkindomid].KingID=Global_const.NONE_ID;

        //Invalidate ();

        int nowkindomid = GlobalData.getInstance().nowkindomid;

        StartCoroutine(Global_XML_IO.getInstance().Save_kindom_Xml());
        StartCoroutine(Global_XML_IO.getInstance().Save_hero_Xml());

        messageBox.GetComponent<UI_OBJ> ().move ();

		Messenger.Broadcast ("invalidatelist");
	}

}
