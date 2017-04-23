using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading;

public class Global_ImportanceData {

	public Importance  [] List_importance = new Importance[Global_const.MAXIMPORTANCE];//= new herodata();
    public XmlDocument xml_importancelist = new XmlDocument();


    Random ran = new Random();
	  
	  private static  Global_ImportanceData instance = new Global_ImportanceData();

    //城池数据池
    public XmlNode xNode_importance;
    public XmlElement[] nowimportance_Elem_List = new XmlElement[Global_const.MAXIMPORTANCE];

    public GameObject importance_pref;
    public GameObject cityobj;

    private Global_ImportanceData(){
		  
		  for(int i=0;i<Global_const.MAXIMPORTANCE;i++)
		  {
			  List_importance[i]=new Importance();
		  }
		  
		  Init_List_importance();

          importance_pref = Resources.Load("prefs/cityprefab") as GameObject;

          Messenger.AddListener ("RESETIMORTANCE", Init);

    }


    public static Global_ImportanceData  getInstance() 
	  { 
	   
	   return instance; 
	  }
	  
  	//据点资料的更新
   	public  void Update()
   	{
		for (int i = 0; i < Global_const.MAXIMPORTANCE; i++) {
			List_importance [i].DataUpdate ();

		}

   	}


    public void Init()
    {
        
        cityobj = GameObject.Find("citys");

        if(cityobj==null)
        {
            return;
        }

        for (int i = 0; i < List_importance.Length; i++)
            {
                Importance tempimportance = List_importance[i];

                if (int.Parse(tempimportance.this_Elem.GetAttribute("Active")) == 0)
                {
                    continue;
                }
                else
                {
                    GameObject tempimportanceObj = GameObject.Find("citys/" + i);

                    XmlElement xl1 = nowimportance_Elem_List[i];

                    int templv = 0;
                  
                    templv = int.Parse(xl1.GetAttribute("级别"));

                    if (tempimportanceObj == null)
                    {
                       tempimportanceObj = GameObject.Instantiate(importance_pref, importance_pref.transform.position, importance_pref.transform.rotation) as GameObject;

                        tempimportanceObj.name = "" + i;
                        tempimportanceObj.transform.parent = cityobj.gameObject.transform;


                        tempimportanceObj.GetComponent<SpriteRenderer>().sprite = Global_source_loader.getInstance().ImportanceList[templv];
                        
                    }
                    else
                    {

                    }

                    tempimportanceObj.transform.localPosition = GridManager.getInstance().FromTiledVec2WorldVec(tempimportance.GetPosition());

                //设置map数据
                GridManager.getInstance().ImportanceMap[(int)tempimportance.GetPosition().x, (int)tempimportance.GetPosition().y] = tempimportance.id;

                }

            }


        /*
         * 测试据点位置MAP
         for (int i=0;i<GridManager.getInstance().maxwidth;i++)
        {
            string tempstr = "";
            for (int j = 0; j < GridManager.getInstance().maxheight; j++)
            {
                if (GridManager.getInstance().ImportanceMap[i, j]!=Global_const.NONE_ID)
                {
                    Debug.Log("i:" + i + " j:" + j + "  " + GridManager.getInstance().ImportanceMap[i, j]);
                
                }

            }


        }
        */
    }


  

    public void Load_importance_Xml(string _xmlname = "/xml_importancelist.xml")
    {
        xml_importancelist.Load((Application.dataPath + _xmlname));

        xNode_importance = xml_importancelist.SelectSingleNode("importancelist");

        XmlNodeList xmlNodeList = xNode_importance.ChildNodes;

        foreach (XmlElement xl1 in xmlNodeList)
        {
            int id = 0;
            try
            {
                id = int.Parse(xl1.GetAttribute("id"));
            }
            catch
            {

            }

            nowimportance_Elem_List[id] = xl1;//这一行最消耗效率

            this.List_importance[id].this_Elem = nowimportance_Elem_List[id];
        }
    }

    public IEnumerator Save_importance_Xml(string _xmlname = "/xml_importancelist.xml")
    {
        for (int i = 0; i < Global_const.MAXIMPORTANCE; i++)
        {
            Save_importance_Xml(i, _xmlname);

        }

        xml_importancelist.Save((Application.dataPath + _xmlname));

        yield return 1;
    }

    public void Save_importance_Xml(int id, string _xmlname = "/xml_importancelist.xml")
    {

        Importance now_importance = Global_ImportanceData.getInstance().List_importance[id];

        //XmlNode xNode =xml_importancelist.SelectSingleNode("//importancelist");

        XmlElement now_Elem = nowimportance_Elem_List[id];// (XmlElement)xNode.SelectSingleNode("//importance[@id='" + id + "']");

        if (null == now_Elem)
        {
            now_Elem = xml_importancelist.CreateElement("importance");
            now_Elem.SetAttribute("id", "" + id);

            xNode_importance.AppendChild(now_Elem);

            nowimportance_Elem_List[id] = now_Elem;
        }

        now_Elem.SetAttribute("id", "" + id);
        now_Elem.SetAttribute("M_name", "" + now_importance.M_name);
        now_Elem.SetAttribute("incomeNOW", "" + now_importance.incomeNOW);
        now_Elem.SetAttribute("agricultureNOW", "" + now_importance.agricultureNOW);
        now_Elem.SetAttribute("PeoplecountNOW", "" + now_importance.PeoplecountNOW);
        now_Elem.SetAttribute("defenseNOW", "" + now_importance.defenseNOW);
        now_Elem.SetAttribute("money", "" + now_importance.money);
        now_Elem.SetAttribute("forage", "" + now_importance.forage);
        now_Elem.SetAttribute("LeaderID", "" + now_importance.LeaderID);


    }

    public void  Init_List_importance()
	{
		for(int i=0;i<Global_const.MAXIMPORTANCE;i++)
		{
			
			List_importance[i].id=i;
		}

		importancelist_load();

	}



    public void importancelist_load ()
	{
		Load_importance_Xml ();
		//XmlNodeList xmlNodeList =tempIO.xml_importancelist.SelectSingleNode("importancelist").ChildNodes;

		//foreach (XmlElement xl1 in Global_XML_IO.getInstance().xNode_importance.ChildNodes) {

            for(int i = 0; i < Global_const.MAXIMPORTANCE;i++)
            {
            XmlElement xl1 =nowimportance_Elem_List[i];

            int id = 0;
			try {
				id = int.Parse (xl1.GetAttribute ("id"));

			} catch {

			}
				
			try {
				List_importance [id].M_name = xl1.GetAttribute ("M_name");

			} catch {

			}

			try {
				List_importance [id].incomeNOW = int.Parse (xl1.GetAttribute ("incomeNOW"));

			} catch {

			}

			try {
				List_importance [id].agricultureNOW =int.Parse ( xl1.GetAttribute ("agricultureNOW"));

			} catch {

			}

			try {
				List_importance [id].PeoplecountNOW= int.Parse (xl1.GetAttribute ("PeoplecountNOW"));

			} catch {

			}

			try {
				List_importance [id].defenseNOW = int.Parse (xl1.GetAttribute ("defenseNOW"));

			} catch {

			}

			try {
				List_importance [id].money= int.Parse (xl1.GetAttribute ("money"));

			} catch {

			}
				

			try {
				List_importance [id].forage= int.Parse (xl1.GetAttribute ("forage"));

			} catch {

			}

			try {
				List_importance [id].LeaderID= int.Parse (xl1.GetAttribute ("LeaderID"));

			} catch {

			}


        }

	}

}
