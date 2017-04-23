using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;
using System.IO;
//修改	for smallbattle

public class Global_Armydata//: MonoBehaviour
{
	public Armydata [] List_army = new Armydata[Global_const.getInstance().MAXHEROS];//= new herodata();
	public int armycount=0;
	public int army_update_flag = 0;
	public int armyupdate_count=1;

	private static  Global_Armydata instance = new Global_Armydata();

    //public GameObject armybase;

    GameObject army_pref = Resources.Load("prefs/armypref") as GameObject;

    public XmlDocument xml_armylist = new XmlDocument();
    //Army数据池
    public XmlNode xNode_army;
    public XmlElement[] nowarmy_Elem_List = new XmlElement[Global_const.getInstance().MAXHEROS];


    private Global_Armydata(){

        InitElem();
        Load_army_Xml();

    }

    public void Load_army_Xml(string _xmlname = "/xml_armylist.xml")
    {
        xml_armylist.Load((Application.dataPath + _xmlname));

        xNode_army = xml_armylist.SelectSingleNode("armylist");

        XmlNodeList xmlNodeList = xNode_army.ChildNodes;

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

            List_army[id].this_Elem = xl1;//这一行最消耗效率

        }
    }


    public void InitElem()
    {
        for (int i = 0; i < Global_const.getInstance().MAXHEROS; i++)
        {
            List_army[i] = new Armydata();
            List_army[i].ID = i;
        }
    }

    public void ArmyCreate(int _id)
    {

        CreateArmyPref(_id);

        /*if(_id==GlobalData.getInstance().nowheroid)
        {
            Messenger.Broadcast("Invalidate");
        }
        */
    }

    public void UpdateArmyheroList(int _id)
    {
        //for(int i=0;i)
        int thisimportanceID =Global_HeroData.getInstance().List_hero[_id].NOWimportanceID;

        if(Global_const.NONE_ID==thisimportanceID)
        {
            return;
        }

        Importance thisimportance = Global_ImportanceData.getInstance().List_importance[thisimportanceID];

        for(int i=0;i<thisimportance.heroCount;i++)
        {
            herodata temphero =Global_HeroData.getInstance().List_hero[thisimportance.heroID_list[i]];

            if(int.Parse(thisimportance.this_Elem.GetAttribute("守将"))==temphero.id)
            {
                continue;
            }

            if(int.Parse(Global_Armydata.getInstance().List_army[_id].this_Elem.GetAttribute("武将数"))>=Global_const.MAXARMYHERONO)
            {
                return;
            }

            if(temphero.m_relationship.Factions_leader_ID==_id)
            {
                Global_Armydata.getInstance().List_army[_id].addhero(temphero.id);

            }

        }

        int bigmight = Global_const.SMALLNO;
        int bigwit = Global_const.SMALLNO;
        int biglead = Global_const.SMALLNO;

        //遍历全体部将
        for (int i=0;i <int.Parse(Global_Armydata.getInstance().List_army[_id].this_Elem.GetAttribute("武将数"));i++)
        {
            int tempid =int.Parse(Global_Armydata.getInstance().List_army[_id].this_Elem.GetAttribute("武将" + i));
            herodata temphero = Global_HeroData.getInstance().List_hero[tempid];                        

            if(temphero.might> bigmight)
            {
                Global_Armydata.getInstance().List_army[_id].this_Elem.SetAttribute("先锋", "" + temphero.id);
                bigmight = temphero.might;
            }
            else if (temphero.wit > bigwit)
            {
                Global_Armydata.getInstance().List_army[_id].this_Elem.SetAttribute("先锋", "" + temphero.id);
                bigwit = temphero.wit;
            }
            else if (temphero.lead > biglead)
            {
                Global_Armydata.getInstance().List_army[_id].this_Elem.SetAttribute("辎重", "" + temphero.id);
                biglead = temphero.lead;
            }
            else
            {
                Global_Armydata.getInstance().List_army[_id].this_Elem.SetAttribute("哨马", "" + temphero.id);
            }

        }

    }
    //武将资料的更新
    public void Update()
    {
        int now_statue = (int)herodata.BORNSTATUE.UNBORN;

        //遍历武将列表update武将之间关系变化
        for (int i = 0; i < Global_const.getInstance().MAXHEROS; i++)
        {
            now_statue = Global_HeroData.getInstance().List_hero[i].now_statue;

            if (now_statue!= (int)herodata.BORNSTATUE.BORN)
            { continue;}

            Armydata nowtarmy = this.List_army[i];
            nowtarmy.FindTarget();

        }

        for (int i = 0; i < Global_const.getInstance().MAXHEROS; i++)
        {
            now_statue = Global_HeroData.getInstance().List_hero[i].now_statue;
            if (now_statue != (int)herodata.BORNSTATUE.BORN)
            { continue; }

            Armydata nowtarmy = this.List_army[i];
            nowtarmy.SetPartersTowardVec3();

            nowtarmy.UpdateTarget();

        }

    }

    private void CreateArmyPref(int _id)
    {
        GameObject preftemp = GameObject.Instantiate(army_pref, army_pref.transform.position, army_pref.transform.rotation) as GameObject;
        preftemp.GetComponent<ArmyScript>().InitBigArmy(_id);

        GridManager.getInstance().SetMap(GridManager.getInstance().ArmyMap, List_army[_id].getvec3(), _id);

    }

    private void DestroyArmyPref(int _id)
    {
       // GameObject preftemp = GameObject.Find();
       // Vector3 postemp = new Vector3(List_army[_id].x, List_army[_id].y, List_army[_id].z);
       // preftemp.GetComponent<ArmyScript>().InitBigArmy(postemp, List_army[_id].m_id);
    }

    public bool MovePCArmy(int _direct)
    {
        bool Move_success = false;

        Armydata temparmy = List_army[GlobalData.getInstance().nowheroid];
        herodata temphero = Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];

        if (true == temparmy.MoveArmy(_direct))
        {
            Messenger.Broadcast("UPDATEALL");
            Messenger.Broadcast<float, float, float>("MoveCam", float.Parse(temphero.this_Elem.GetAttribute("x")), float.Parse(temphero.this_Elem.GetAttribute("y")), 1);
            Move_success = true;
        }

        TestInImportance();
        return Move_success;
    }

    void  TestInImportance()
    {

        if(Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID!=Global_const.NONE_ID)
        {
            Global_events.getInstance().traggerGlobalEvent(1000, 0, GlobalData.getInstance().nowheroid, GlobalData.getInstance().nowheroid, "PLACE0");
        }

    }


    public static Global_Armydata  getInstance() 
	{ 
		
		return instance; 
	}



    public IEnumerator Save_army_Xml(string _xmlname = "/xml_armylist.xml")
    {

        xml_armylist.Save((Application.dataPath + _xmlname));

        yield return 1;
    }
}