using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.IO;

public class herodata :TargeObj
{

    public enum BATTLESELECTED {NOTINARMY,INARMY,OUTSIDE};

    public enum ATTRIBUTE{ID};

    public enum BORNSTATUE{UNBORN,BORN,DEAD};

	static int MAXATTRIBUTES=50;

	public object [] AttributeData = new object[MAXATTRIBUTES];
    RandomValue tempRandom = new RandomValue();

    //ID
    public int id;
	//性别
	public int sex;
	//姓名
	public string [] heroname=new string[2];

	public string  herozi="";
	public string  herotitle="";
	public string  herooldname="";

	public string[] Globaleventkeys = new string[100];

    public bool BfirstUpdate = true;

    public herodata()
	{
		/*for(int i=0;i<2;i++)
		{
			name[i]=new string;
		}
		*/
	}

	public herodata Copy()
	{
		return (herodata)MemberwiseClone();
	}

	public bool B_KingAvalible()
	{
		
		if(GlobalData.getInstance().year>(Global_const.getInstance().GrownUp+this.born_year))
		{
			return false;
		}

		return true;
	}

	public string GetAllName()
	{
		string tempheroname="";

		for(int k=0;k<heroname.Length;k++)
		{
			tempheroname+=heroname[k];
		}

        return tempheroname;

	}

	public void Test_in_importance ()
	{
        int newimportanceID = Global_const.NONE_ID;

        Vector3 tilepos = GetPosition();

        newimportanceID = GridManager.getInstance().ImportanceMap[(int)tilepos.x,(int)tilepos.y];

            if (NOWimportanceID!=Global_const.NONE_ID)
            {
                Global_ImportanceData.getInstance().List_importance[NOWimportanceID].DeleteHero(id);
            }
            NOWimportanceID = newimportanceID;

            if(NOWimportanceID != Global_const.NONE_ID)
            {
                Global_ImportanceData.getInstance().List_importance[NOWimportanceID].AddHero(id);

               // Debug.Log("NOWimportanceID:" + NOWimportanceID + "  " + "id:" + id);
            }
        
	}


	//统帅
	public int lead;
	//武力
	public int might;
	//智力
	public int wit;
	//政治
	public int polity;
	//魅力
	public int charm;
	//相性
	public int phase;
	//性格
	public int character;
	//官阶
	public int rank;
	//出生年
	public int born_year;
	//死亡年
	public int die_year;

	//出生年
	public int born_year_fantasy=0;
	//死亡年
	public int die_year_fantasy=0;

	//死亡原因
	public string die_reason="";

	//个人传记
	public string biography="";

	//出身
	public string born="";

	//工作
	public string occupation="";

	//野心
	public int ambition;

	//名声
	public int reputation;
	//恶名
	public int bad_reputation=0;

	//田产
	public int estate=0;
	//商产
	public int businessproduction=0;

    enum NOWSTATUE { UNBORN,BORN,DEAD};
    public int now_statue;//0未出生，1出生，2死亡
	//坐标
	//public float x,y,z;
	//技能ID槽
	public string [] skill=new string[Global_const.getInstance().MAXSKILL_COUNT];
	//S技能ID槽
	public string [] S_skill=new string[Global_const.getInstance().MAX_S_SKILL_COUNT];

	//关系struct
	public Relationship m_relationship=new Relationship();

	//亲兵struct
	//public Guardunit guardunit = new Guardunit();

	public int NOW_mission;//当前任务 // 0 征兵 
	//当前行动
	//public int NOW_action;
	
	//当前所在据点ID
	public int NOWimportanceID=Global_const.NONE_ID;
	
	public int Born_importanceID = Global_const.NONE_ID;

	public string m_junwang_zhou="";
	public string m_junwang_jun="";
	public string m_junwang_xian="";

	//金钱
	public int money;
	//粮草
	public int forage;

	//public int army_work=0x0000;//在军团中从事的任务  0x0000 军团长 军师 先锋 辎重押运 可以兼任 

	//识人
	public int konwpoeple=0;

	//运气
	public int luck=0;

	//勇猛
	public int bold=0;

	//冷静
	public int calm=0;

	//理念
	public string idea="";

	//年表
	public string chronology;

	public string officeposition="";
	public string militaryposition="";
	public string title="";

	//public int 

	public void ResetKindom(int newkindom)
	{
		//如果原来是国王，则销毁国家
		KindomData oldkingdomdata=Global_KindomData.getInstance().list_KindomData[this.m_relationship.belong_kindom];
		if (oldkingdomdata.KingID == this.id) {

			oldkingdomdata.destroy ();
		}

		//重置所有从属的kindom
		for (int i = 0; i < this.m_relationship.subordinatecount; i++) {
			herodata subordinatehero = Global_HeroData.getInstance ().List_hero [this.m_relationship.subordinateid [i]];

			if (subordinatehero.id != this.id) {
				subordinatehero.ResetKindom (newkindom);
			}
		}

		this.m_relationship.belong_kindom =newkindom;

		int newkingid = Global_KindomData.getInstance ().list_KindomData [newkindom].KingID;

		this.m_relationship.Factions_leader_ID = newkingid;

	}


    public int GetMaxReP()
    {
        return (reputation > bad_reputation ? reputation : bad_reputation);

    }
    public void Update()
    {


        if (now_statue == (int)BORNSTATUE.DEAD)
        {//死亡

            return;
        }
        else if (now_statue == (int)BORNSTATUE.BORN)
        {//正常
            if(true==BfirstUpdate)
            {
                Global_Armydata.getInstance().ArmyCreate(this.id);
            }

            Test_in_importance();

            if (id != GlobalData.getInstance().nowheroid)
            {
                NormalUpdateherodata();
            }
        }
        else if (now_statue == (int)BORNSTATUE.UNBORN)
        {//未出世
            TestBorn();
        }


        if (true == BfirstUpdate)
        {
            BfirstUpdate = false;
        }
    }

    public void TestBorn()
    {
        if (now_statue == (int)BORNSTATUE.UNBORN)
        {//未出世
            if ((born_year + Global_const.getInstance().GrownUp) < GlobalData.getInstance().year)
            {
                now_statue = (int)BORNSTATUE.BORN;
                NOWimportanceID = Born_importanceID;

                SetPosition(Global_ImportanceData.getInstance().List_importance[NOWimportanceID].GetPosition());

                Global_ImportanceData.getInstance().List_importance[NOWimportanceID].AddHero(this.id);

                Global_Armydata.getInstance().ArmyCreate(this.id);

            }
        }
    }
    //正常武将更新
    void NormalUpdateherodata()
    {
        //temphero.lead++;
        //获得收入 区部x10+500
        //temphero.money+=temphero.guardunit.Armycount+100;

        //根据当前行为更新状态 征兵
        NOW_mission = tempRandom.getRandMission();

        money += 10;

        switch (NOW_mission)
        {
            case 0://征兵

                //如果不在据点中
                if (NOWimportanceID == Global_const.NONE_ID)
                {
                    break;
                }

                //如果所在据点的势力跟武将所在势力关系不敌对，则可以征兵
                //如果超过名声则不能招兵
                if (Global_GuardunitData.getInstance().List_guardunit[id].Armycount < GetMaxReP())
                {
                    int tempmoney = (int)(lead * 1.5f);
                    if (money >= tempmoney)
                    {
                        money -= tempmoney;

                        Global_GuardunitData.getInstance().List_guardunit[id].Armycount += tempmoney;

                    }
                }
                break;
        }


        //且没有所属势力
        if ((m_relationship.belong_kindom == Global_const.NONE_ID) &&
            (ambition >= 9)
        )
        {
            //成立新势力
            int newkindomID = Global_KindomData.getInstance().createnewKindom(this);
            if (newkindomID != Global_const.NONE_ID)//如果返回值不为空
            {
                //武将所属势力赋值
                m_relationship.belong_kindom = newkindomID;

                //武将成为此势力国王
                Global_KindomData.getInstance().list_KindomData[newkindomID].KingID = id;

                //成立了势力
                string stradd = "成立了势力：" + heroname [0]+"君主是："+GetAllName()+"\n";
                GlobalData.getInstance ().global_message = stradd + GlobalData.getInstance ().global_message;
                //从属的所有武将都进行势力赋值
                Messenger.Broadcast("UPDATEGLOBALMESSAGE");

            }
        }

        //如果所在城市没有太守
        //成为所在城池太守

        if (NOWimportanceID != Global_const.NONE_ID)
        {

            //Debug.Log("NOWimportanceID:" + NOWimportanceID);

            if (Global_ImportanceData.getInstance().List_importance[NOWimportanceID].LeaderID == Global_const.NONE_ID)
            {

                //如果此武将有国家
                if (m_relationship.belong_kindom != Global_const.NONE_ID)
                {
                    //扬旗
                    Global_ImportanceData.getInstance().List_importance[NOWimportanceID].LeaderID = id;

                    string stradd = "势力" + heroname[0] + "占领了" + Global_ImportanceData.getInstance().List_importance[NOWimportanceID].M_name + "\n";
                    GlobalData.getInstance().global_message = stradd + GlobalData.getInstance().global_message;

                    Messenger.Broadcast("UPDATEGLOBALMESSAGE");

                }
            }
        }

            UpdateMeeting();
            UpdateImportanceLeader();

    }

    public void Die(int _targetid)
    {
        herodata targethero = Global_HeroData.getInstance().List_hero[_targetid];

        Armydata thisarmy = Global_Armydata.getInstance().List_army[id];
        Armydata targetarmy = Global_Armydata.getInstance().List_army[_targetid];

        KindomData targetKindom = Global_KindomData.getInstance().list_KindomData[targethero.m_relationship.belong_kindom];
        herodata targetKindomKing = Global_HeroData.getInstance().List_hero[targetKindom.KingID];
        Armydata targetKingArmy = Global_Armydata.getInstance().List_army[targetKindom.KingID];

        KindomData thisKindom = Global_KindomData.getInstance().list_KindomData[m_relationship.belong_kindom];
        herodata thisKindomKing = Global_HeroData.getInstance().List_hero[thisKindom.KingID];
        Armydata thisKingArmy = Global_Armydata.getInstance().List_army[thisKindom.KingID];


        if (id != thisKindomKing.id)
        {
            thisKingArmy.deletehero(targethero.id);
        }
        else
        {
            //在此人的手下中寻找一个新的君主
            int newKingID = FindNewKing();
            //找不到新君主，势力解散
            if(newKingID==Global_const.NONE_ID)
            {
                thisKindom.destroy();
            }
            else
            {
                thisKindom.setnewKing(newKingID);
            }

        }

        thisKindomKing.m_relationship.DeleteSubordinate(this);
        now_statue = (int)herodata.BORNSTATUE.DEAD;
        GridManager.getInstance().ArmyMap[(int)GetPosition().x, (int)GetPosition().y] = Global_const.NONE_ID;
        GlobalData.getInstance().global_message = GetAllName()+" 死亡了"+ "\n" + GlobalData.getInstance().global_message;

        Messenger.Broadcast<int>("DEAD",id);

    }

    int FindNewKing()
    {
        int NewKingID = Global_const.NONE_ID;
        int reputationMax = -10000;
        //for(int i=0;i<re)
        //挑出子女中名声最高的为新君主
        for(int i=0;i<m_relationship.subordinatecount;i++)
        {
            herodata tempking =Global_HeroData.getInstance().List_hero[m_relationship.subordinateid[i]];
            
            if(tempking.m_relationship.m_family.fatherID==this.id)
            {
                 if(tempking.GetMaxReP()> reputationMax)
                {
                    reputationMax = tempking.GetMaxReP();
                    NewKingID = tempking.id;
                }
            }
        }

        //没有找到子女
        if(NewKingID==Global_const.NONE_ID)
        {
            for (int i = 0; i < m_relationship.subordinatecount; i++)
            {
                herodata tempking = Global_HeroData.getInstance().List_hero[i];

                if (tempking.GetMaxReP() > reputationMax)
                {
                    reputationMax = tempking.GetMaxReP();
                    NewKingID = tempking.id;
                }
            }

        }

        return NewKingID;
    }

    public void UpdateMeeting()
    {
        //随机认识一名同城武将
        if (NOWimportanceID != Global_const.NONE_ID)
        {
            Importance nowimportance = Global_ImportanceData.getInstance().List_importance[NOWimportanceID];

            bool b_king = false;

            if (m_relationship.belong_kindom != Global_const.NONE_ID)
            {
                b_king = (Global_KindomData.getInstance().list_KindomData[m_relationship.belong_kindom].KingID == id);
            }

            for (int i = 0; i < nowimportance.heroCount; i++)
            {
                herodata temp = Global_HeroData.getInstance().List_hero[nowimportance.heroID_list[i]];

                if(temp.id==GlobalData.getInstance().nowheroid)
                {
                    continue;
                }

                int relation = Global_const.NONE_ID;

                int meetrand = UnityEngine.Random.Range(1, 10);
                if (meetrand == 1)
                {

                    if (false == temp.m_relationship.friendsLikeHash.Contains(id))
                    {
                        //认识
                        m_relationship.AddFriends(temp);
                        temp.m_relationship.AddFriends(this);

                    }

                    int liketemp = (int)this.m_relationship.friendsLikeHash[temp.id] + 10 - Math.Abs(this.phase - temp.phase);

                    if(liketemp>=100)
                    {
                        liketemp = 100;
                    }

                    this.m_relationship.friendsLikeHash[temp.id] = liketemp;
                    temp.m_relationship.friendsLikeHash[this.id] = liketemp;


                    //Debug.Log(this.id+"-like-"+temp.id+"-" + this.m_relationship.friendsLikeHash[temp.id]);

                    if (((int)this.m_relationship.friendsLikeHash[temp.id] >= 60)
                        &&b_king
                        && (temp.m_relationship.belong_kindom == Global_const.NONE_ID))
                    {
                        temp.m_relationship.belong_kindom = m_relationship.belong_kindom;
                        m_relationship.AddSubordinate(temp);
                        temp.m_relationship.Factions_leader_ID = id;

                        string stradd = "<color=ff0000>"+temp.GetAllName()+"</color>" + " belongs " + GetAllName() + " friendlike " + this.m_relationship.friendsLikeHash[temp.id] + "\n";

                        GlobalData.getInstance().global_message = stradd + GlobalData.getInstance().global_message;

                        Messenger.Broadcast("UPDATEGLOBALMESSAGE");

                    }

                    //如果已经遇见武将则不再进行访问
                    return;
                }


            }
        }

    }

    public void UpdateLike(int _like,int _targetID)
    {
        this.m_relationship.AddFriends(Global_HeroData.getInstance().List_hero[_targetID]);

        int templike = (int)this.m_relationship.friendsLikeHash[_targetID];

        templike += _like;

        if(templike>=100)
        {
            templike = 100;
        }
        else if(templike<=-100)
        {
            templike = -100;
        }

        this.m_relationship.friendsLikeHash[_targetID]= templike;

        herodata targethero = Global_HeroData.getInstance().List_hero[_targetID];
        targethero.m_relationship.AddFriends(this);
        targethero.m_relationship.friendsLikeHash[this.id] = templike;

    }


    public void UpdateImportanceLeader()
    {
        if (NOWimportanceID != Global_const.NONE_ID)
        {
            //如果是太守
            if (Global_ImportanceData.getInstance().List_importance[NOWimportanceID].LeaderID == id)
            {
                int oldtempfriendLike = Global_const.SMALLNO;
                //选择守军
                for (int i = 0; i < this.m_relationship.subordinatecount; i++)
                {
                    herodata temphero = Global_HeroData.getInstance().List_hero[this.m_relationship.subordinateid[i]];

                    //寻找友好度最高的武将
                    if (false == this.m_relationship.friendsLikeHash.Contains(temphero.id))
                    {
                        continue;
                    }

                    int tempfriendLike = (int)this.m_relationship.friendsLikeHash[temphero.id];

                    if (tempfriendLike > oldtempfriendLike)
                    {
                        if (temphero.NOWimportanceID == NOWimportanceID)
                        {
                            
                            //Global_ImportanceData.getInstance().List_importance[NOWimportanceID].this_Elem.SetAttribute("守将", "" + temphero.id);
                            oldtempfriendLike = tempfriendLike;
                        }
                    }


                }

            }
        }

    }

   
}
