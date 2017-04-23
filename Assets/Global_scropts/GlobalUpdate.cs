using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CreativeSpore.SuperTilemapEditor;

public class GlobalUpdate :MonoBehaviour
{

	public Text data_panel_nowdata;
	RandomValue tempRandom=new RandomValue();

	int tempcount=0;
	int hero_update_count=0;

	Ray ray;

	int hero_update_flag=0;//上次武将更新所到flag
	int hero_UPDATE_COUNT=1;//Global_const.getInstance().MAXHEROS/100;//每次更新的武将数

	Hashtable timeHash=new Hashtable();

    bool b_first = true;
    // Use this for initialization
    void Start () {
	

		timeHash.Add (0, "子");
		timeHash.Add (1, "丑");
		timeHash.Add (2, "寅");
		timeHash.Add (3, "卯");
		timeHash.Add (4, "辰");
		timeHash.Add (5, "巳");
		timeHash.Add (6, "午");
		timeHash.Add (7, "未");
		timeHash.Add (8, "申");
		timeHash.Add (9, "酉");
		timeHash.Add (10, "戌");
		timeHash.Add (11, "亥");

		//首次更新武将列表
		//for (int i = 0; i < Global_const.getInstance ().MAXHEROS; i++) {

		//}

		LoadScenes ();

        GridManager.getInstance().GenerateMap();

        GridManager.getInstance().tiledmap_Mask.GetComponent<Tilemap>().OrderInLayer = 5;

    }

    void Awake()
	{
		Messenger.AddListener("UPDATEALL",UpdateAll);

	}

    private void OnDestroy()
    {
        Messenger.RemoveListener("UPDATEALL", UpdateAll);

    }


	void Update () {
		if (true == Global_events.getInstance ().re_init) {
			Global_events.getInstance ().init ();
			Global_events.getInstance ().re_init = false;
		}
			
        if(true==b_first)
        {
            this.UpdateAll();
            b_first = false;
        }

	}

	public void LoadScenes ()
	{
		int nowheroidtemp = GlobalData.getInstance ().nowheroid;
		GlobalData.getInstance ().nowimportanceID = Global_HeroData.getInstance ().List_hero [nowheroidtemp].NOWimportanceID;
	}

	void UpdateAll() {

        UpdateData();

        Global_ImportanceData.getInstance().Update();

        Global_events.getInstance ().Update();

        Global_HeroData.getInstance().Update();

        Global_Armydata.getInstance().Update();

    }

    public void UpdateData()
	{

		GlobalData.getInstance ().time+=4;

		if (GlobalData.getInstance ().time == 12) {

			GlobalData.getInstance ().day++;
			GlobalData.getInstance ().time = 0;
			
			if (GlobalData.getInstance ().day == 31) {

				GlobalData.getInstance ().month++;
				GlobalData.getInstance ().day = 1;


                //AutoSave();
                //herolist_script listscripttemp = herolist.GetComponent<herolist_script>();
                //listscripttemp.invalidatelist();

                if (GlobalData.getInstance ().month == 13) {
					GlobalData.getInstance ().year++;
					GlobalData.getInstance ().month = 1;
				}

			}

		}




		if (data_panel_nowdata == null) {
			data_panel_nowdata = GameObject.Find ("Canvas/Image_face/Image/DataText").GetComponent<Text> ();
		}

	    data_panel_nowdata.text = "" + GlobalData.getInstance().year + " " + GlobalData.getInstance().month + "月" + GlobalData.getInstance().day + "日\n"+(string)timeHash[GlobalData.getInstance().time]+"时";
        
	}

 

}
