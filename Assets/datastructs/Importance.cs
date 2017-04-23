using System;
using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

//据点
public class Importance :TargeObj
    {

	public int id;//据点ID

	//public bool Bactive;//是否有效

	public string M_name;//据点名字
	public int incomeNOW;//收入now
	public int agricultureNOW;//当前农业
	public int PeoplecountNOW;//当前人口
	public int defenseNOW;//当前城防
	public int money;//金钱
	public int forage;//粮草
	public int LeaderID=Global_const.NONE_ID;//太守ID
	//public int KindomID=Global_const.NONE_ID;//所属势力ID


	//public float x,y,z;//据点位置

    //public Vector3 vec3 = new Vector3(0.0f, 0.0f, 0.0f );
	public int battle_statue=0;//0 normal 1 attack 2 def
	public int def_count=0;//被斩杀兵数或城防数
	public bool bedestroy=false;

	//public int [] heroID_list=new int[Global_const.getInstance().MAXHEROS];

	public int [] heroID_list =new int[Global_const.getInstance().MAXHEROS];
	public int heroCount=0;//武将数

    public const float RectLenth = 0.5f;


    public Importance()
	{
		for(int i=0;i<Global_const.getInstance().MAXHEROS;i++)
		{
			heroID_list[i]=Global_const.NONE_ID;
		}
		heroCount=0;
	}

    public int GetKindomID()
    {
        int ThisKindomID = Global_const.NONE_ID;

        if(LeaderID!=Global_const.NONE_ID)
        {
            herodata leadertemp = Global_HeroData.getInstance().List_hero[LeaderID];
            ThisKindomID = leadertemp.m_relationship.belong_kindom;
        }

        return ThisKindomID;
    }


    public void BaseDataCopy(Importance _Importance)
	{
		/*public string M_name;//据点名字
		public int incomeNOW;//收入now
		public int agricultureNOW;//当前农业
		public int PeoplecountNOW;//当前人口
		public int defenseNOW;//当前城防
		public int money;//金钱
		public int forage;//粮草
		public int LeaderID=Global_const.NONE_ID;//太守ID
		*/

		this.M_name = _Importance.M_name;
		this.incomeNOW = _Importance.incomeNOW;
		this.agricultureNOW = _Importance.agricultureNOW;
		this.PeoplecountNOW = _Importance.PeoplecountNOW;
		this.defenseNOW = _Importance.defenseNOW;
		this.money = _Importance.money;
		this.forage = _Importance.forage;
		this.LeaderID = _Importance.LeaderID;
		//vec3.x = _Importance.vec3.x;
        //vec3.y = _Importance.vec3.y;
        //vec3.z = _Importance.vec3.z;


	}
	public void DataUpdate()
	{
		
	}

	void findnewLeader(int oldLeaderID)
	{
	

	}

    public void AddHero(int _id)
    {
        for(int i=0;i<heroCount;i++)
        {
            if(heroID_list[i]==_id)
            {

                return;
            }

        }

        heroID_list[heroCount] = _id;
        //Debug.Log(this.id+" add ID:" + _id);
        heroCount++;

    }

    public void DeleteHero(int _id)
    {
        //Debug.Log(this.id + " -del ID:" + _id);

        for (int i=0;i<heroCount;i++)
        {
            if (_id==heroID_list[i])
            {
                for (int j = i; j < (heroCount - 1); j++)
                {
                    heroID_list[j] = heroID_list[j + 1];
                }
                heroID_list[heroCount - 1] = Global_const.NONE_ID;

                heroCount--;

                return;
            }
            
        }


    }
}
