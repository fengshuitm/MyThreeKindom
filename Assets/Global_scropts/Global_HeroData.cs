using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading;

public class Global_HeroData {

	  public herodata  [] List_hero = new herodata[Global_const.getInstance().MAXHEROS];//= new herodata();
	  public herodata  [] List_hero_sort = new herodata[Global_const.getInstance().MAXHEROS];//= new herodata();
	  public int sortcount=0;

	  public int MAN=0,GIRL=1;
      public int ADULT_AGE=15;//成年时间

	  RandomValue tempRandom=new RandomValue();

	  borndata tempborndata = new borndata ();
	  Workdata tempworkdata = new Workdata ();


	  private static  Global_HeroData instance = new Global_HeroData(); 
	  private Global_HeroData(){
		  
		  for(int i=0;i<Global_const.getInstance().MAXHEROS;i++)
		  {
			  List_hero[i]=new herodata();
			  //List_hero[i].herodata();
		  }
		  
		  Init_List_hero();
    	  listTotemp ();

          

          Messenger.AddListener<int>("DEAD", DeadTest);

    }

    

    public void Init()
    {
        for (int i = 0; i < Global_const.getInstance().MAXHEROS; i++)
        {
            List_hero[i].BfirstUpdate = true;
            List_hero[i].Update();
        }
    }
    

    public void DeadTest(int _id)
    {

    }

    public void InitElem()
    {
        for (int i = 0; i < Global_const.getInstance().MAXHEROS; i++)
        {

            XmlElement elemtemp = Global_XML_IO.getInstance().nowhero_Elem_List[i];

            if (null == elemtemp)
            {
                elemtemp = Global_XML_IO.getInstance().xml_herolist.CreateElement("hero");

                Global_XML_IO.getInstance().xNode_hero.AppendChild(elemtemp);

                Global_XML_IO.getInstance().nowhero_Elem_List[i] = elemtemp;

            }

            List_hero[i].this_Elem = elemtemp;
        }
        
    }

    public static Global_HeroData  getInstance() 
	  { 
	   
	   return instance; 
	  } 

 
	  	//武将资料的更新
	   	public  void Update()
	   	{
	   		//遍历武将列表update武将之间关系变化
	   	   for(int i=0;i<Global_const.getInstance().MAXHEROS;i++)
            {
                herodata nowhero = this.List_hero[i];
                nowhero.Update();

            }	

	   	}

        public void TestBorn()
        {
            //遍历武将列表update武将之间关系变化
            for (int i = 0; i < Global_const.getInstance().MAXHEROS; i++)
            {
                herodata nowhero = this.List_hero[i];
                nowhero.Update();// TestBorn();

            }

        }

	   	//生成武将列表信息
	   	public void Init_List_hero()
	   	{
			
	   		for(int i=0;i<Global_const.getInstance().MAXHEROS;i++)
	   		{
	   			//ID
	   			//int id;
	   			List_hero[i].id=i;
	   			//性别
	   			//int sex;
	   			int sextemp=tempRandom.getSex();
	   			List_hero[i].sex=sextemp;
	   			//姓名
	   			
	   			//String [] name=new String[2];
	   			List_hero[i].heroname=tempRandom.getChineseName(sextemp);// .getChineseName();	   		
	   			//统帅
	   			//int lead;
	   			List_hero[i].lead=tempRandom.getRandLead();// int) (Math.random()*10)+1;
	   			//武力
	   			//int might;
	   			List_hero[i].might=tempRandom.getRandMight();
	   			//智力
	   			//int wit;
	   			List_hero[i].wit=tempRandom.getRandWit();
	   			//政治
	   			//int polity;
	   			List_hero[i].polity=tempRandom.getRandPolity();	   			
	   			//魅力
	   			//int charm;
	   			List_hero[i].charm=tempRandom.getRandCharm();	   			
	   			//相性
	   			//int phase;
	   			List_hero[i].phase=tempRandom.getRandPhase();	   			
	   			//性格
	   			//int character;
	   			List_hero[i].character=tempRandom.getRandPhase();	   			
	   			//官阶
	   			//int rank;
	   			List_hero[i].rank=tempRandom.getRandRank();	 
				
				List_hero [i].konwpoeple = Random.Range (0, 10);

				List_hero [i].bold = Random.Range (0, 10);
				List_hero [i].calm = Random.Range (0, 10);
				List_hero [i].luck = Random.Range (0, 10);
	
				List_hero [i].bad_reputation=Random.Range (0, 500);

	   			//出生年
	   			//int born_year;
	   			List_hero[i].born_year=tempRandom.getRandBorn_year();	
			    
				//死亡年
				List_hero[i].die_year=List_hero[i].born_year+Random.Range(45,100);	

    			//野心
				List_hero[i].ambition =tempRandom.getAmbition();

				//名声
				List_hero[i].reputation=tempRandom.getReputation();

	    		//初始所属势力
				List_hero[i].m_relationship.belong_kindom=Global_const.NONE_ID;
	   			//坐标
	   			//int x,y;

	   			//技能ID槽
	   			//int [] skill=new int[Global_const.getInstance().MAXSKILL_COUNT];
	   			//List_hero[i].skill=tempRandom.getRandSkill();	   			
	
	   			List_hero[i].Born_importanceID=tempRandom.getBornimportance();

			//出身
				//List_hero [i].born = Random.Range (0, tempborndata.born_listName.Length);
			//工作
				//List_hero [i].work = Random.Range (0, tempworkdata.work_listName.Length);

                List_hero[i].now_statue = 0;

				
	   		}
			
			herolist_load ();

            InitElem();
    //	int nowheroidtemp = GlobalData.getInstance ().nowheroid;
    //	GlobalData.getInstance ().nowimportanceID = Global_HeroData.getInstance ().List_hero [nowheroidtemp].NOWimportanceID;
    //InitHerosubordinateidLists ();
    }
	   	

	public void listTotemp()
	{
		sortcount = 0;

		for (int i = 0; i < Global_const.getInstance ().MAXHEROS; i++) {

			List_hero_sort [i] = List_hero[i].Copy();
			sortcount++;
		}

	}

	//0 拼音 1 比划  lead
	public void SortHeroList(int sorttype)
	{
        herodata nowhero = Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];
        sortcount = 0;

        GlobalData.getInstance().nowherosortstyle = sorttype;

        switch (sorttype) {
		case 0:
			listTotemp ();

			break;
		case 1:
			//CultureInfo PronoCi = new CultureInfo (2052);
			Thread.CurrentThread.CurrentCulture = new CultureInfo (2052);
			//Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us"); 
			System.Array.Sort (List_hero_sort, (s1, s2) => s1.heroname[0].CompareTo (s2.heroname[0]));

			break;
		case 2:
			//CultureInfo StrokCi = new CultureInfo (133124);

			//Thread.CurrentThread.CurrentCulture = StrokCi;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us"); 

			System.Array.Sort (List_hero_sort, (s1, s2) => s1.heroname[0].CompareTo (s2.heroname[0]));

			break;
        case 3:

                for(int i=0;i<nowhero.m_relationship.friendscount;i++)
                {
                    herodata temptargethero = Global_HeroData.getInstance().List_hero[nowhero.m_relationship.friends[i]];
                    if (nowhero.NOWimportanceID== temptargethero.NOWimportanceID)
                    {
                        List_hero_sort[sortcount] = temptargethero;
                        sortcount++;
                    }

                }
                
            break;
        case 4:
        case 5:
        case 6:
        case 7:

                List_hero_sort[sortcount] = nowhero;
                sortcount++;

                for (int i = 0; i < nowhero.m_relationship.subordinatecount; i++)
                {
                    
                    herodata temptargethero = Global_HeroData.getInstance().List_hero[nowhero.m_relationship.subordinateid[i]];
                    //Debug.Log("temptargetheroid:" + temptargethero.id);

                    if (nowhero.NOWimportanceID == temptargethero.NOWimportanceID)
                    {
                        List_hero_sort[sortcount] = temptargethero;
                        sortcount++;
                    }

                }
                break;
        }

		//listTotemp ();
	}


	public void findwithFirstname(string _firstname)
	{
		//SortHeroList (0);//首先将顺序恢复到id排序
		sortcount = 0;

		for (int i = 0; i < List_hero.Length; i++) {

			if (List_hero [i].heroname [0] == _firstname) {
				List_hero_sort [sortcount] = List_hero [i].Copy();
				sortcount++;
			}

		}



	}

	public void herolist_load ()
	{
		Global_XML_IO tempIO = Global_XML_IO.getInstance ();

		tempIO.Load_hero_Xml ();

		//XmlNodeList xmlNodeList =tempIO.xml_herolist.SelectSingleNode("herolist").ChildNodes;

		foreach (XmlElement xl1 in Global_XML_IO.getInstance().xNode_hero.ChildNodes) {

            int id = 0;
			try
			{
				id =int.Parse( xl1.GetAttribute ("id"));
			}
			catch {

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
			try
			{
				List_hero [id].heroname [0] = xl1.GetAttribute ("firstname");

			}
			catch {

			}

			try
			{
				List_hero [id].heroname [1] =xl1.GetAttribute ("lastname");

			}
			catch {

			}

			try
			{
				List_hero [id].herozi =xl1.GetAttribute ("herozi");

			}
			catch {

			}

			try
			{
				List_hero [id].herotitle =xl1.GetAttribute ("herotitle");

			}
			catch {

			}

			try
			{
				List_hero [id].herooldname =xl1.GetAttribute ("herooldname");

			}
			catch {

			}

			try
			{
				List_hero [id].sex =int.Parse(xl1.GetAttribute ("sex"));

			}
			catch {

			}


			//part2
			/*
		   	temphero.m_junwang_zhou=part2_inputfields[1].GetComponent<InputField> ().text;
			temphero.m_junwang_jun=part2_inputfields[2].GetComponent<InputField> ().text;
			temphero.m_junwang_xian=part2_inputfields[3].GetComponent<InputField> ().text;
			temphero.m_relationship.m_family.fatherID=part2_inputfields[4].GetComponent<Dropdown> ().value;//=""+Global_HeroData.getInstance().List_hero[fatherID].GetAllName();
			temphero.m_relationship.m_family.motherID=part2_inputfields[5].GetComponent<Dropdown> ().value;
			*/

			try
			{
				List_hero [id].m_junwang_zhou =xl1.GetAttribute ("m_junwang_zhou");

			}
			catch {

			}

			try
			{
				List_hero [id].m_junwang_jun =xl1.GetAttribute ("m_junwang_jun");

			}
			catch {

			}

			try
			{
				List_hero [id].m_junwang_jun =xl1.GetAttribute ("m_junwang_jun");

			}
			catch {

			}

			try
			{
				List_hero [id].m_relationship.m_family.fatherID =int.Parse(xl1.GetAttribute ("fatherID"));

			}
			catch {

			}

			try
			{
				List_hero [id].m_relationship.m_family.motherID =int.Parse(xl1.GetAttribute ("motherID"));

			}
			catch {

			}

			try
			{
				List_hero [id].NOWimportanceID =int.Parse(xl1.GetAttribute ("NOWimportanceID"));

			}
			catch {

			}
				//part3
			/*	temphero.born_year=int.Parse( part3_inputfields[0].GetComponent<InputField> ().text);
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

			try
			{
				List_hero [id].born_year=int.Parse(xl1.GetAttribute ("born_year"));

			}
			catch {

			}

			try
			{
				List_hero [id].die_year=int.Parse(xl1.GetAttribute ("die_year"));

			}
			catch {

			}

			try
			{
				List_hero [id].born_year_fantasy=int.Parse(xl1.GetAttribute ("born_year_fantasy"));

			}
			catch {

			}
			try
			{
				List_hero [id].die_year_fantasy=int.Parse(xl1.GetAttribute ("die_year_fantasy"));

			}
			catch {

			}

			try
			{
				List_hero [id].die_reason=xl1.GetAttribute ("die_reason");

			}
			catch {

			}

			try
			{
				List_hero [id].biography=xl1.GetAttribute ("biography");

			}
			catch {

			}

			try
			{
				List_hero [id].born=xl1.GetAttribute ("born");

			}
			catch {

			}

			try
			{
				List_hero [id].occupation=xl1.GetAttribute ("occupation");

			}
			catch {

			}

			try
			{
				List_hero [id].idea=xl1.GetAttribute ("idea");

			}
			catch {

			}

			try
			{
				List_hero [id].officeposition=xl1.GetAttribute ("officeposition");

			}
			catch {

			}

			try
			{
				List_hero [id].militaryposition=xl1.GetAttribute ("militaryposition");

			}
			catch {

			}

			try
			{
				List_hero [id].title=xl1.GetAttribute ("title");

			}
			catch {

			}
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

			try
			{
				List_hero [id].lead=int.Parse(xl1.GetAttribute ("lead"));

			}
			catch {

			}

			try
			{
				List_hero [id].might=int.Parse(xl1.GetAttribute ("might"));

			}
			catch {

			}

			try
			{
				List_hero [id].wit=int.Parse(xl1.GetAttribute ("wit"));

			}
			catch {

			}

			try
			{
				List_hero [id].polity=int.Parse(xl1.GetAttribute ("polity"));

			}
			catch {

			}

			try
			{
				List_hero [id].charm=int.Parse(xl1.GetAttribute ("charm"));

			}
			catch {

			}

			try
			{
				List_hero [id].luck=int.Parse(xl1.GetAttribute ("luck"));

			}
			catch {

			}


			try
			{
				List_hero [id].reputation=int.Parse(xl1.GetAttribute ("reputation"));

			}
			catch {

			}

			try
			{
				List_hero [id].bad_reputation=int.Parse(xl1.GetAttribute ("bad_reputation"));

			}
			catch {

			}

			try
			{
				List_hero [id].bad_reputation=int.Parse(xl1.GetAttribute ("bad_reputation"));

			}
			catch {

			}

			for (int i = 0; i < Global_const.getInstance().MAXSKILL_COUNT; i++) {
				string tempskillname = "skill" + i;
				try {
					List_hero [id].skill[i] = xl1.GetAttribute ("tempskillname");

				} catch {

				}
			}

			for (int i = 0; i < Global_const.getInstance().MAX_S_SKILL_COUNT; i++) {
				string tempSskillname = "Sskill" + i;
				try {
					List_hero [id].S_skill[i] = xl1.GetAttribute ("tempSskillname");

				} catch {

				}
			}

			try
			{
				List_hero [id].money=int.Parse(xl1.GetAttribute ("money"));

			}
			catch {

			}

			try
			{
				List_hero [id].forage=int.Parse(xl1.GetAttribute ("forage"));

			}
			catch {

			}
			
            	

			try
			{
                Global_GuardunitData.getInstance().List_guardunit[id].style=xl1.GetAttribute ("style");

			}
			catch {

			}

			try
			{
                Global_GuardunitData.getInstance().List_guardunit[id].Armycount=int.Parse( xl1.GetAttribute ("Armycount"));

			}
			catch {

			}

			try
			{
				List_hero [id].phase=int.Parse(xl1.GetAttribute ("phase"));

			}
			catch {

			}

			try
			{
				List_hero [id].character=int.Parse(xl1.GetAttribute ("character"));

			}
			catch {

			}

			try
			{
				List_hero [id].Born_importanceID=int.Parse(xl1.GetAttribute ("Born_importanceID"));

			}
			catch {

			}

			//mate
			for (int i = 0; i < Global_const.getInstance ().MAXMATS; i++) {
				try
				{
				List_hero [id].m_relationship.mates[i]=int.Parse(xl1.GetAttribute ("Mate"+i));
				}
				catch {

				}
			}
			//friends
			try
			{
				List_hero [id].m_relationship.friendscount=int.Parse(xl1.GetAttribute ("Friendscount"));

			}
			catch {

			}

			for (int i = 0; i <List_hero [id].m_relationship.friendscount ; i++) {
				try
				{
					List_hero [id].m_relationship.friends[i]=int.Parse(xl1.GetAttribute ("Friend"+i));

					int IDtemp = List_hero [id].m_relationship.friends [i];
					int Liketemp=int.Parse(xl1.GetAttribute ("friendsLikeHash"+IDtemp));

					List_hero [id].m_relationship.friendsLikeHash.Add(IDtemp,Liketemp);

					//nowhero_Elem.SetAttribute ("friendsLikeHash"+IDtemp, ""+ List_hero [heroid].m_relationship.friendsLikeHash[IDtemp]);


				}
				catch {

				}
			}
			//righteousbrothers
			for (int i = 0; i <Global_const.getInstance ().MAXRIGHTEOUSBROTHERS ; i++) {
				try
				{
					List_hero [id].m_relationship.righteousbrothers[i]=int.Parse(xl1.GetAttribute ("Rightousbrothers"+i));

				}
				catch {

				}
			}
			//subordinateid
			try
			{
				List_hero [id].m_relationship.subordinatecount=int.Parse(xl1.GetAttribute ("Subordinatecount"));

			}
			catch {

			}

			for (int i = 0; i <List_hero [id].m_relationship.subordinatecount ; i++) {

				try
				{
					List_hero [id].m_relationship.subordinateid[i]=int.Parse(xl1.GetAttribute ("Subordinateid"+i));

				}
				catch {

				}

			}

			///nowhero_Elem.SetAttribute ("Factions_leader_ID", ""+ List_hero [heroid].m_relationship.Factions_leader_ID);

			try
			{
				List_hero [id].m_relationship.Factions_leader_ID=int.Parse(xl1.GetAttribute ("Factions_leader_ID"));
		
			}
			catch {

			}

            try
            {
                List_hero[id].m_relationship.belong_kindom = int.Parse(xl1.GetAttribute("belong_kindom"));

            }
            catch
            {

            }

            

            try
            {
                List_hero[id].now_statue = int.Parse(xl1.GetAttribute("nowstatue"));

            }
            catch
            {

            }
        }



    }
		

}
