using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;


public class Global_XML_IO 
{

	public XmlDocument xml_herolist = new XmlDocument();
	public XmlDocument xml_kindomlist = new XmlDocument();

 

    //势力数据池
    public XmlNode xNode_kindom;
    public XmlElement[] nowkindom_Elem_List = new XmlElement[Global_const.getInstance().MAXKINDOM_COUNT];

    //武将数据池
    public XmlNode xNode_hero;// = xml_herolist.SelectSingleNode("//herolist");
    public XmlElement [] nowhero_Elem_List = new XmlElement[Global_const.getInstance().MAXHEROS];


    private static  Global_XML_IO instance = new Global_XML_IO(); 

	private Global_XML_IO(){




    }

	public static Global_XML_IO  getInstance() 
	{ 

		return instance; 
	}

	// Update is called once per frame
	void Update () {

	}




	void addXMLData()
	{
		string path = Application.dataPath + "/data2.xml";
		if(File.Exists(path))
		{
			XmlDocument xml = new XmlDocument();
			xml.Load(path);
			XmlNode root = xml.SelectSingleNode("objects");
			//下面的东西就跟上面创建xml元素是一样的。我们把他复制过来就行了
			XmlElement element = xml.CreateElement("messages");
			//设置节点的属性
			element.SetAttribute("id", "2");
			XmlElement elementChild1 = xml.CreateElement("contents");

			elementChild1.SetAttribute("name", "b");
			//设置节点内面的内容
			elementChild1.InnerText = "天狼，你的梦想就是。。。。。";
			XmlElement elementChild2 = xml.CreateElement("mission");
			elementChild2.SetAttribute("map", "def");
			elementChild2.InnerText = "我要妹子。。。。。。。。。。";
			//把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
			element.AppendChild(elementChild1);
			element.AppendChild(elementChild2);

			root.AppendChild(element);

			xml.AppendChild(root);
			//最后保存文件
			xml.Save(path);
		}
	}

	void CreateXML()
	{
		/*string path = Application.dataPath + "/datayearlist.xml";
		if(!File.Exists(path))
		{
			//创建最上一层的节点。
			XmlDocument xml = new XmlDocument();
			//创建最上一层的节点。
			XmlElement root = xml.CreateElement("objects");
			//创建子节点
			XmlElement element = xml.CreateElement("messages");
			//设置节点的属性
			element.SetAttribute("id", "1");
			XmlElement elementChild1 = xml.CreateElement("contents");

			elementChild1.SetAttribute("name", "a");
			//设置节点内面的内容
			elementChild1.InnerText = "这就是你，你就是天狼";
			XmlElement elementChild2 = xml.CreateElement("mission");
			elementChild2.SetAttribute("map", "abc");
			elementChild2.InnerText = "去吧，少年，去实现你的梦想";
			//把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
			element.AppendChild(elementChild1);
			element.AppendChild(elementChild2);

			root.AppendChild(element);

			xml.AppendChild(root);
			//最后保存文件
			xml.Save(path);
		}
		*/
	}

	public void updateXML()
	{
		/*string path = Application.dataPath + "/data.xml";
		if(File.Exists(path))
		{
			XmlDocument xml = new XmlDocument();
			xml.Load(path);
			XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
			foreach(XmlElement xl1 in xmlNodeList)
			{
				if(xl1.GetAttribute("id")=="1")
				{
					//把messages里id为1的属性改为5
					xl1.SetAttribute("id", "5");
				}

				if (xl1.GetAttribute("id") =="2")
				{
					foreach(XmlElement xl2 in xl1.ChildNodes)
					{
						if(xl2.GetAttribute("map")=="abc")
						{
							//把mission里map为abc的属性改为df，并修改其里面的内容
							xl2.SetAttribute("map", "df");
							xl2.InnerText = "我成功改变了你";
						}

					}
				}
			}
			xml.Save(path);
		}
		*/

		//	emptyTest ();

		//xml_herolist.Save ((Application.dataPath+"/xml_herolist.xml"));

		//xml_kindomlist.Save ((Application.dataPath+"/xml_kindomlist.xml"));

		//xml_importancelist.Save ((Application.dataPath+"/xml_importancelist.xml"));

		//Load_Xml();

	}

	void emptyTest()
	{

		foreach (XmlElement xlyear in xml_herolist.ChildNodes) {
			bool deletenowyearnode = true;

			if (xlyear.HasChildNodes) {
				deletenowyearnode = false;

				foreach (XmlElement xlmonth in xlyear.ChildNodes) {
					bool deletenowmonthnode = true;

					if (xlmonth.HasChildNodes) {
						deletenowmonthnode = false;
					}

					if (deletenowmonthnode == true) {
						xlyear.RemoveChild (xlmonth);
					}
				}
			}
			if (deletenowyearnode == true) {
				xml_herolist.SelectSingleNode("xml_herolist").RemoveChild (xlyear);
			}

		}

	}

    

   

	public void Load_kindom_Xml(string _xmlname="/xml_kindomlist.xml")
	{
		//xml_kindomlist.Load ((Application.dataPath + _xmlname));

        xml_kindomlist.Load((Application.dataPath + _xmlname));

        xNode_kindom= xml_kindomlist.SelectSingleNode("kindomlist");

        XmlNodeList xmlNodeList = xNode_kindom.ChildNodes;

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
            nowkindom_Elem_List[id] = xl1;
        }
    }


	public void Load_hero_Xml(string _xmlname="/xml_herolist.xml")
	{

		xml_herolist.Load ((Application.dataPath + _xmlname));

        xNode_hero= xml_herolist.SelectSingleNode("//herolist");

        XmlNodeList xmlNodeList = xNode_hero.ChildNodes;


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

            nowhero_Elem_List[id] = xl1;//这一行最消耗效率
        }




        //创建xml文档
        //XmlDocument xml = new XmlDocument();
        /*	XmlReaderSettings set = new XmlReaderSettings();
		set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
		xml_yearlist.Load(XmlReader.Create((Application.dataPath+"/yearlist.xml"),set));
	*/
        //载入herolist


        //herolist_load ();
        //得到objects节点下的所有子节点
        /*XmlNodeList xmlNodeList = xml_yearlist.SelectSingleNode("yearlist").ChildNodes;
		//遍历所有子节点
		foreach(XmlElement xl1 in xmlNodeList)
		{

			if(xl1.GetAttribute("id")=="1")
			{
				//继续遍历id为1的节点下的子节点
				foreach(XmlElement xl2 in xl1.ChildNodes)
				{
					//放到一个textlist文本里
					//textList.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
					//得到name为a的节点里的内容。放到TextList里
					if (xl2.GetAttribute("name") == "a")
					{
						Adialogue.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
					}
					//得到name为b的节点里的内容。放到TextList里
					else if (xl2.GetAttribute("name") == "b")
					{
						Bdialogue.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
					}
				}
			}
		}
		//print(xml.OuterXml);
		*/
    }

    public IEnumerator Save_hero_Xml(string _xmlname = "/xml_herolist.xml")
	{

		for(int i=0;i<Global_const.getInstance().MAXHEROS;i++)
		{
            
			//Save_hero_Xml (i,_xmlname);

          /*  if(true== _onlyactive)
            {
                if((int)herodata.BORNSTATUE.BORN!=Global_HeroData.getInstance().List_hero[i].now_statue)
                {
                    Save_hero_Xml(i, _xmlname);
                }
            }
            else
            {
            */
                Save_hero_Xml(i, _xmlname);

            //}

        }

        xml_herolist.Save((Application.dataPath + _xmlname));

        yield return 1;

        //Load_hero_Xml();

    }

    public void Save_hero_Xml(int heroid,string _xmlname="/xml_herolist.xml")
	{


        herodata temp_hero =Global_HeroData.getInstance().List_hero[heroid];

        //XmlNode xNode =xml_herolist.SelectSingleNode("//herolist");

        XmlElement nowhero_Elem = nowhero_Elem_List[heroid];// (XmlElement)xNode.SelectSingleNode("//hero[@id='" + heroid + "']");//这一行最消耗效率

        
        if (nowhero_Elem != null)
		{

			//xNode.AppendChild (nowhero_Elem);
			//xml_herolist.AppendChild (xNode);
			//string firstname = nowhero_Elem.GetAttribute("firstname");
			//firstname_text.text = firstname;
		}
		else
		{
			nowhero_Elem = xml_herolist.CreateElement("hero");
            //设置节点的属性
            //nowhero_Elem.SetAttribute("id",""+i);
            //nowhero_Elem.SetAttribute("firstname", List_hero [i].heroname [0]);

            xNode_hero.AppendChild (nowhero_Elem);

		}


        //part1
        /*
		temphero.heroname[0]=part1_inputfields[0].GetComponent<InputField>().text;
		temphero.heroname[1]=part1_inputfields[1].GetComponent<InputField>().text;
		temphero.herozi=part1_inputfields[2].GetComponent<InputField>().text;
		temphero.herotitle=part1_inputfields[3].GetComponent<InputField>().text;
		temphero.herooldname=part1_inputfields[4].GetComponent<InputField>().text;
		temphero.sex=part1_inputfields [5].GetComponent<Dropdown> ().value;
		*/
        nowhero_Elem.SetAttribute("id",""+heroid);
		nowhero_Elem.SetAttribute ("firstname", temp_hero.heroname [0]);
		nowhero_Elem.SetAttribute ("lastname", temp_hero.heroname [1]);
		nowhero_Elem.SetAttribute ("herozi", temp_hero.herozi);
		nowhero_Elem.SetAttribute ("herotitle", temp_hero.herotitle);
		nowhero_Elem.SetAttribute ("herooldname", temp_hero.herooldname);
		nowhero_Elem.SetAttribute ("sex", ""+ temp_hero.sex);


		//part2
		/*
			temphero.m_junwang_zhou=part2_inputfields[1].GetComponent<InputField> ().text;
			temphero.m_junwang_jun=part2_inputfields[2].GetComponent<InputField> ().text;
			temphero.m_junwang_xian=part2_inputfields[3].GetComponent<InputField> ().text;
			temphero.m_relationship.m_family.fatherID=part2_inputfields[4].GetComponent<Dropdown> ().value;//=""+Global_HeroData.getInstance().List_hero[fatherID].GetAllName();
			temphero.m_relationship.m_family.motherID=part2_inputfields[5].GetComponent<Dropdown> ().value;
		*/
		nowhero_Elem.SetAttribute("m_junwang_zhou",""+ temp_hero.m_junwang_zhou);
		nowhero_Elem.SetAttribute("m_junwang_jun",""+ temp_hero.m_junwang_jun);
		nowhero_Elem.SetAttribute("m_junwang_xian",""+ temp_hero.m_junwang_xian);
		nowhero_Elem.SetAttribute("fatherID",""+ temp_hero.m_relationship.m_family.fatherID);
		nowhero_Elem.SetAttribute("motherID",""+ temp_hero.m_relationship.m_family.motherID);
		nowhero_Elem.SetAttribute("NOWimportanceID",""+ temp_hero.NOWimportanceID);

		//part3
		/*	
			temphero.born_year=int.Parse( part3_inputfields[0].GetComponent<InputField> ().text);
			temphero.die_year=int.Parse( part3_inputfields[1].GetComponent<InputField> ().text);
			temphero.born_year_fantasy=int.Parse( part3_inputfields[2].GetComponent<InputField> ().text);
			temphero.die_year_fantasy=int.Parse( part3_inputfields[3].GetComponent<InputField> ().text);
			temphero.die_reason=part3_inputfields[4].GetComponent<InputField> ().text;
			temphero.biography=part3_inputfields[5].GetComponent<InputField> ().text;
			temphero.born=part3_inputfields[6].GetComponent<InputField> ().text;
			temphero.occupation=part3_inputfields[7].GetComponent<InputField> ().text;
			temphero.idea=part3_inputfields[8].GetComponent<InputField> ().text;
			temphero.officeposition=part3_inputfields[9].GetComponent<InputField> ().text;
			temphero.militaryposition=part3_inputfields[10].GetComponent<InputField> ().text;
			temphero.title=part3_inputfields[11].GetComponent<InputField> ().text;
		*/

		nowhero_Elem.SetAttribute("born_year",""+ temp_hero.born_year);
		nowhero_Elem.SetAttribute("die_year",""+ temp_hero.die_year);
		nowhero_Elem.SetAttribute("born_year_fantasy",""+ temp_hero.born_year_fantasy);
		nowhero_Elem.SetAttribute("die_year_fantasy",""+ temp_hero.die_year_fantasy);
		nowhero_Elem.SetAttribute("die_reason",""+ temp_hero.die_reason);
		nowhero_Elem.SetAttribute("biography",""+ temp_hero.biography);
		nowhero_Elem.SetAttribute("born",""+ temp_hero.born);
		nowhero_Elem.SetAttribute("occupation",""+ temp_hero.occupation);
		nowhero_Elem.SetAttribute("idea",""+ temp_hero.idea);
		nowhero_Elem.SetAttribute("officeposition",""+ temp_hero.officeposition);
		nowhero_Elem.SetAttribute("militaryposition",""+ temp_hero.militaryposition);
		nowhero_Elem.SetAttribute("title",""+ temp_hero.title);


		/*
		//part4
		temphero.lead=int.Parse( part4_inputfields [0].text);
		temphero.might =int.Parse( part4_inputfields [1].text);
		temphero.wit =int.Parse( part4_inputfields [2].text);
		temphero.polity =int.Parse( part4_inputfields [3].text);
		temphero.charm =int.Parse( part4_inputfields [4].text);
		temphero.luck =int.Parse( part4_inputfields [5].text);
		temphero.reputation =int.Parse( part4_inputfields [6].text);
		temphero.bad_reputation =int.Parse( part4_inputfields [7].text);

		temphero.skill[0] = part4_inputfields [8].text;
		temphero.skill[1] = part4_inputfields [9].text;
		temphero.skill[2] = part4_inputfields [10].text;
		temphero.skill[3] = part4_inputfields [11].text;
		temphero.skill[4] = part4_inputfields [12].text;
		temphero.skill[5] = part4_inputfields [13].text;
		temphero.S_skill[0] = part4_inputfields [14].text;
		temphero.S_skill[1] = part4_inputfields [15].text;

		temphero.money =int.Parse( part4_inputfields [16].text);
		temphero.forage =int.Parse( part4_inputfields [17].text);
		temphero.guardunit.style =part4_inputfields [18].text;
		temphero.guardunit.Armycount =int.Parse( part4_inputfields [19].text);
		*/

		nowhero_Elem.SetAttribute ("lead", ""+ temp_hero.lead);
		nowhero_Elem.SetAttribute ("might", ""+ temp_hero.might);
		nowhero_Elem.SetAttribute ("wit", ""+ temp_hero.wit);
		nowhero_Elem.SetAttribute ("polity", ""+ temp_hero.polity);
		nowhero_Elem.SetAttribute ("charm", ""+ temp_hero.charm);
		nowhero_Elem.SetAttribute ("luck", ""+ temp_hero.luck);
		nowhero_Elem.SetAttribute ("reputation", ""+ temp_hero.reputation);
		nowhero_Elem.SetAttribute ("bad_reputation", ""+ temp_hero.bad_reputation);


        for (int i = 0; i < Global_const.getInstance ().MAXSKILL_COUNT; i++) {
			string tempskillname = "skill" + i;
			nowhero_Elem.SetAttribute ("tempskillname", "" + temp_hero.skill [i]);
		}

		for (int i = 0; i < Global_const.getInstance ().MAX_S_SKILL_COUNT; i++) {
			string tempSskillname = "Sskill" + i;
			nowhero_Elem.SetAttribute ("tempSskillname", "" + temp_hero.S_skill [i]);
		}

		nowhero_Elem.SetAttribute ("money", ""+ temp_hero.money);
		nowhero_Elem.SetAttribute ("forage", ""+ temp_hero.forage);
		nowhero_Elem.SetAttribute ("style",""+ Global_GuardunitData.getInstance().List_guardunit[heroid].style);
		nowhero_Elem.SetAttribute ("Armycount",""+ Global_GuardunitData.getInstance().List_guardunit[heroid].Armycount);

		nowhero_Elem.SetAttribute ("phase", ""+ temp_hero.phase);
		nowhero_Elem.SetAttribute ("character",""+ temp_hero.character);
		nowhero_Elem.SetAttribute ("Morale",""+ Global_GuardunitData.getInstance().List_guardunit[heroid].Morale);

		//初始所属势力

		//List_hero[i].m_relationship.belong_kindom=Global_const.NONE_ID;
		//坐标
		//int x,y;
		//List_hero[i].x=tempRandom.getRandX();	   			
		//List_hero[i].z=tempRandom.getRandZ();	   			

		//技能ID槽
		//int [] skill=new int[Global_const.getInstance().MAXSKILL_COUNT];
		//List_hero[i].skill=tempRandom.getRandSkill();	   			

		nowhero_Elem.SetAttribute ("Born_importanceID", ""+ temp_hero.Born_importanceID);

		/////////////mate
		/// 
		for (int i = 0; i < Global_const.getInstance ().MAXMATS; i++) {
			if (Global_const.NONE_ID != temp_hero.m_relationship.mates [i]) {
				
				nowhero_Elem.SetAttribute ("Mate" + i, "" + temp_hero.m_relationship.mates [i]);

            }
        }
		//friends
		nowhero_Elem.SetAttribute ("Friendscount", ""+ temp_hero.m_relationship.friendscount);


		for (int i = 0; i < temp_hero.m_relationship.friendscount ; i++) {

			nowhero_Elem.SetAttribute ("Friend"+i, ""+ temp_hero.m_relationship.friends[i]);
			int IDtemp = temp_hero.m_relationship.friends [i];

			nowhero_Elem.SetAttribute ("friendsLikeHash"+IDtemp, ""+ temp_hero.m_relationship.friendsLikeHash[IDtemp]);

        }
        //righteousbrothers
        for (int i = 0; i <Global_const.getInstance ().MAXRIGHTEOUSBROTHERS ; i++) {
			if (Global_const.NONE_ID != temp_hero.m_relationship.righteousbrothers [i]) {
				nowhero_Elem.SetAttribute ("Rightousbrothers" + i, "" + temp_hero.m_relationship.righteousbrothers [i]);

            }
        }
		//subordinateid
		nowhero_Elem.SetAttribute ("Subordinatecount", ""+ temp_hero.m_relationship.subordinatecount);

		for (int i = 0; i < temp_hero.m_relationship.subordinatecount ; i++) {

			nowhero_Elem.SetAttribute ("Subordinateid"+i, ""+ temp_hero.m_relationship.subordinateid[i]);

        }
        nowhero_Elem.SetAttribute ("Factions_leader_ID", ""+ temp_hero.m_relationship.Factions_leader_ID);

        nowhero_Elem.SetAttribute("belong_kindom", "" + temp_hero.m_relationship.belong_kindom);

        nowhero_Elem.SetAttribute("nowstatue", "" + temp_hero.now_statue);


        //Global_HeroData.getInstance ().herolist_load ();
    }




    public IEnumerator Save_kindom_Xml(string _xmlname = "/xml_kindomlist.xml")
    {

        for (int i = 0; i < Global_const.getInstance().MAXKINDOM_COUNT; i++)
        {
            Save_kindom_Xml(i, _xmlname);
        }

        xml_kindomlist.Save((Application.dataPath + _xmlname));

        yield return 1;

        //Load_hero_Xml();

    }

    public void Save_kindom_Xml(int kindomid,string _xmlname="/xml_kindomlist.xml")
	{

        KindomData temp_kindom = Global_KindomData.getInstance().list_KindomData[kindomid];

        //XmlNode xNode =xml_herolist.SelectSingleNode("//herolist");

        XmlElement nowkindom_Elem = nowkindom_Elem_List[kindomid];// (XmlElement)xNode.SelectSingleNode("//hero[@id='" + heroid + "']");//这一行最消耗效率


        //KindomData[] List_kindom=Global_KindomData.getInstance().list_KindomData;

		//XmlNode xNode =xml_kindomlist.SelectSingleNode("//kindomlist");

		//XmlElement nowkindom_Elem = (XmlElement)xNode.SelectSingleNode("//kindom[@id='" + kindomid + "']");
		if (nowkindom_Elem != null)
		{

			//xNode.AppendChild (nowhero_Elem);
			//xml_herolist.AppendChild (xNode);
			//string firstname = nowhero_Elem.GetAttribute("firstname");
			//firstname_text.text = firstname;
		}
		else
		{
			nowkindom_Elem = xml_kindomlist.CreateElement("kindom");
			//设置节点的属性
			//nowhero_Elem.SetAttribute("id",""+i);
			//nowhero_Elem.SetAttribute("firstname", List_hero [i].heroname [0]);

			xNode_kindom.AppendChild (nowkindom_Elem);

		}



        nowkindom_Elem.SetAttribute("id","" +temp_kindom.id);
		nowkindom_Elem.SetAttribute("KingID",""+ temp_kindom.KingID);

        for(int i=0;i<Global_const.getInstance().MAXKINDOM_COUNT;i++)
        {

            nowkindom_Elem.SetAttribute("Kindomrelation" + i, "" + temp_kindom.Kindomrelation[i]);

        }


	}

    



   
}
