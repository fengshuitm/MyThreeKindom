using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hero_datashow_script : UI_OBJ {

	public int hero_id=Global_const.NONE_ID;
	public Text lead_text;
	public Text might_text;
	public Text wit_text;
	public Text polity_text;
	public Text charm_text;
	public Text sex_text;
	public Text age_text;
	public Text reputation_text;
	public Text friend_text;
	public Text name_text;
	public Text leader_text;
	public Text belong_text;

	public Text [] Datas;


	public Image head_image;

	// Use this for initialization
	void Start () {
	
		//Global_UI.getInstance ().HerodataUI = this.gameObject;
		this.gameObject.SetActive(false);

	}

    private void Awake()
    {
        Messenger.AddListener("Invalidate", Invalidate);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener("Invalidate", Invalidate);

    }

    // Update is called once per frame
    void FixedUpdate () {
		

	}

	public void move()
	{
		//this.transform.position = new Vector3(0, 0, 0);
		this.gameObject.SetActive(true);
		this.transform.localPosition =new Vector3(0,0,0);

        Invalidate();

    }

    public void  shownowhero()
	{
		hero_id = GlobalData.getInstance ().nowheroid;
        move ();
	}

    public override void Invalidate()
    {
        try
        {
            head_image.sprite = Global_source_loader.getInstance().hero_L_face[hero_id];

            herodata tempherodata = Global_HeroData.getInstance().List_hero[hero_id];
            Datas[0].text = tempherodata.lead + "";
            Datas[1].text = tempherodata.might + "";
            Datas[2].text = tempherodata.wit + "";
            Datas[3].text = tempherodata.polity + "";
            Datas[4].text = tempherodata.charm + "";
            Datas[5].text = tempherodata.sex + "";
            Datas[6].text = (GlobalData.getInstance().year - tempherodata.born_year) + "";
            Datas[7].text = tempherodata.reputation + "";



            if (hero_id == GlobalData.getInstance().nowheroid)
            {
                Datas[8].text = "";
            }
            else
            {
                //	Datas[8].text=tempherodata.m_relationship.likelist[GlobalData.getInstance().nowheroid]+"";
            }

            Datas[9].text = tempherodata.GetAllName() + "";


            int leader_id = Global_const.NONE_ID;

            leader_id = Global_HeroData.getInstance().List_hero[hero_id].m_relationship.Factions_leader_ID;

            if (leader_id != Global_const.NONE_ID)
            {
                Datas[10].text = Global_HeroData.getInstance().List_hero[leader_id].GetAllName();
            }
            else
            {
                Datas[10].text = "";
            }

            int belongkindom = tempherodata.m_relationship.belong_kindom;
            if (belongkindom != Global_const.NONE_ID)
            {
                int kingidtemp = Global_KindomData.getInstance().list_KindomData[belongkindom].KingID;
                Datas[11].text = Global_HeroData.getInstance().List_hero[kingidtemp].heroname[0];
            }
            else
            {
                Datas[11].text = "在野";
            }

            Datas[12].text = "" + Global_GuardunitData.getInstance().List_guardunit[tempherodata.id].Armycount;
            Datas[13].text = "" + Global_GuardunitData.getInstance().List_guardunit[tempherodata.id].Morale;

            transform.FindChild("texts/nowimportance").GetComponent<Text>().text = ""+ tempherodata.NOWimportanceID;
        }
        catch
        {


        }
    }

}
