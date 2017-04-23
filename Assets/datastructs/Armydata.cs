using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Xml;
using System.IO;
using UnityEngine;

public class Armydata
{
    //int TargetArmyID = Global_const.NONE_ID;
    enum TargetStyle {NONE,ENEMY,FRIENDIMPORTANCE,LEADER};

    public int MTargetStyle=(int)TargetStyle.NONE;

    //public bool HasTarget = false;

    Vector3 TargetVec3=new Vector3();

    ArrayList pathArray = new ArrayList();

    int armysearchwidth = 10;
    int importancesearchwidth = 20;


    const float NormalAttackrate = 1.1f;
    const float CriticalAttackrate = 1.5f;

    public XmlElement this_Elem;

    enum SEARCHTYPE {FRIEND,HATE};
    enum DestroyType { FLEE, SURRENDER, DIE };

    bool Moved = false;

    public Armydata ()
	{

	}


	public int All_armycount=0;
    public int ID = Global_const.NONE_ID;
	
    public enum direct { UP, LEFT, DOWN, RIGHT ,WAIT};

    const float MOVESTEP = 0.05f;
    public void TestAll_armycount()
	{
		All_armycount = 0;

        int heroCount =int.Parse(this_Elem.GetAttribute("武将数"));

        for (int i = 0; i < heroCount; i++) {

            int heroidtemp= int.Parse(this_Elem.GetAttribute("武将" +i));

            Guardunit tempguardunit = Global_GuardunitData.getInstance().List_guardunit[heroidtemp];

            int armycount = tempguardunit.Armycount;
			All_armycount += armycount;

		}
        
	}

    public void MoveArmy(Vector3 _vec3)
    {
        float deltax = _vec3.x - this.getvec3().x;
        float deltay = _vec3.y - this.getvec3().y;

        if(Math.Abs(deltax)>Math.Abs(deltay))
        {
            if(deltax>0.001)
            {
                MoveArmy((int)direct.RIGHT);

            }
            else if(deltax < -0.001)
            {
                MoveArmy((int)direct.LEFT);

            }

        }
        else
        {
            if (deltay > 0.001)
            {
                MoveArmy((int)direct.UP);

            }
            else if (deltay < -0.001)
            {
                MoveArmy((int)direct.DOWN);

            }


        }

     
    }

    public bool MoveArmy(int _direct)
    {
        

        Vector3 _vec3 = getvec3();
        Vector3 tempoldvec3 = _vec3;

          switch(_direct)
          {
              case (int)direct.UP:

                _vec3.y += 1;
                  break;
              case (int)direct.LEFT:
                _vec3.x -= 1;

                  break;
              case (int)direct.RIGHT:
                _vec3.x += 1;

                  break;
              case (int)direct.DOWN:
                _vec3.y -= 1;

                  break;
          }



        if(true==GridManager.getInstance().TestMove(_vec3))
          {

            if(_vec3.x<0||_vec3.x>GridManager.getInstance().maxwidth||
                _vec3.y < 0 || _vec3.y > GridManager.getInstance().maxwidth)
            {
                return false;
            }

            int targetimportanceID = GridManager.getInstance().ImportanceMap[(int)_vec3.x,(int)_vec3.y];// GetTargetplaceImportance(vec3);

            if(targetimportanceID!=Global_const.NONE_ID)
            {
                setvec3(_vec3);
                return true;
            }

            int targetplacearmyID = GridManager.getInstance().ArmyMap[(int)_vec3.x, (int)_vec3.y];

            if (targetplacearmyID == Global_const.NONE_ID)
            {
                if(this.ID==GlobalData.getInstance().nowheroid)
                {
                    GridManager.getInstance().OpenMask(_vec3, 5);
                }

                setvec3(_vec3);
                return true;
            }
            else
            {
                int thisKindomID = Global_HeroData.getInstance().List_hero[this.ID].m_relationship.belong_kindom;
                int targetKindomID = Global_HeroData.getInstance().List_hero[targetplacearmyID].m_relationship.belong_kindom;

                 if ((thisKindomID ==targetKindomID)||
                    (targetKindomID == Global_const.NONE_ID))
                {
                    if(targetplacearmyID!=GlobalData.getInstance().nowheroid)
                    {
                        Global_Armydata.getInstance().List_army[targetplacearmyID].setvec3(getvec3());
                        setvec3(_vec3);
                    }
                    return true;
                }
                else
                {


                    if(Global_KindomData.getInstance().list_KindomData[thisKindomID].Kindomrelation[targetKindomID] <0)
                    {
                        Global_Armydata.getInstance().List_army[this.ID].ArmyAttack(this.ID, targetplacearmyID);
                        return true;
                    }
                    else
                    {
                        _vec3 = tempoldvec3;
                        return false;
                    }
                }

            }

          }
          else
          {
            _vec3 = tempoldvec3;
            return false;
          }
        

        return true;
    }


    public void addhero(int _id)
	{
        //heroID_list[heroCount] = _id;
        //heroCount++;
        //int heroCount=

        //测试有没有重复武将
        int herocount = 0;

        try
        {
            herocount = int.Parse(this_Elem.GetAttribute("武将数"));
        }
        catch
        {

        }

        if(herocount!=0)
        {
            for(int i=0;i< herocount;i++)
            {
              int tempid=int.Parse(this_Elem.GetAttribute("武将" + i));
                if(tempid== _id)
                {
                    Debug.Log("已有此武将");
                    return;
                }

            }
        }

        this_Elem.SetAttribute("武将" + herocount, "" + _id);
        herocount++;
        this_Elem.SetAttribute("武将数", "" + herocount);


    }

    public bool haswork(int _id)
    {
        if(int.Parse(this.this_Elem.GetAttribute("主将"))==_id)
        {
            return true;
        }
        if (int.Parse(this.this_Elem.GetAttribute("先锋")) == _id)
        {
            return true;
        }
        if (int.Parse(this.this_Elem.GetAttribute("军师")) == _id)
        {
            return true;
        }
        if (int.Parse(this.this_Elem.GetAttribute("哨马")) == _id)
        {
            return true;
        }
        if (int.Parse(this.this_Elem.GetAttribute("辎重")) == _id)
        {
            return true;
        }


        return false;
    }



    public void deletehero(int _id)
	{
		int id_pos = Global_const.NONE_ID;

        int herocount = 0;

        if(_id==this.ID)
        {
            return;
        }

        try
        {
            herocount = int.Parse(this_Elem.GetAttribute("武将数"));
        }
        catch
        {

        }

        for (int i = 0; i < herocount; i++) {
            int tempid = int.Parse(this_Elem.GetAttribute("武将" + i));
            if (tempid == _id)
            {
                id_pos = i;
                break;
            }
        }

		if (id_pos == Global_const.NONE_ID) {

            Debug.Log("没找到要移除的武将");

			return;//没找到，退出
		}

		for (int i = id_pos; i < herocount; i++) {

            //heroID_list [i] = heroID_list [i + 1];
            int nextID = int.Parse(this_Elem.GetAttribute("武将" + i));
            this_Elem.SetAttribute("武将" + i, "" + nextID);
        }

        herocount--;
        this_Elem.SetAttribute("武将数", "" + herocount);

    }
		
	public void clean ()
	{
        int herocount =int.Parse(this_Elem.GetAttribute("武将数"));


		for (int i = 0; i < herocount; i++) {
            this_Elem.SetAttribute("武将" + i, "" + Global_const.NONE_ID);

		}
        this_Elem.SetAttribute("武将数", "" + 0);

    }


    bool ArmyAttack(int _attackID, int _defID)
    {

        //Debug.Log("_attackID:" + _attackID);
       // Debug.Log("_defID:" + _defID);

        Armydata attackArmy = Global_Armydata.getInstance().List_army[_attackID];
        Armydata defArmy = Global_Armydata.getInstance().List_army[_defID];

            int attackhero_id = Global_const.NONE_ID;
            int temprand = Global_const.NONE_ID;

            attackhero_id =  this.ID ;// int.Parse(attackArmy.this_Elem.GetAttribute("先锋"));// .battleworksID[(int)Armydata.BATTLEWORK.FORWARD];

                herodata attack_hero = Global_HeroData.getInstance().List_hero[attackhero_id];
                Guardunit attack_guardunit = Global_GuardunitData.getInstance().List_guardunit[attackhero_id];

                //判断士气，如果士气不够则不发动攻击
               /* if (UnityEngine.Random.Range(0, 100) > attack_guardunit.Morale)
                {

                    GlobalData.getInstance().global_message = attack_hero.GetAllName() + "催促士兵发动攻击，由于士气低下士兵们踌躇不前\n" + GlobalData.getInstance().global_message;

                    return true;
                }
                */

                float attack_count_add = NormalAttackrate;
                //暴击
                if (UnityEngine.Random.Range(0, 10) > 3)
                {

                    //Global_events.getInstance ().traggerGlobalEvent (0);
                    attack_count_add = CriticalAttackrate;
                }

                //获得被攻击方一个随机武将
                temprand = UnityEngine.Random.Range(0,int.Parse(defArmy.this_Elem.GetAttribute("武将数")) - 1);


                int defhero_id =int.Parse(defArmy.this_Elem.GetAttribute("武将" + temprand));

                herodata def_hero = Global_HeroData.getInstance().List_hero[defhero_id];
                Guardunit def_guardunit = Global_GuardunitData.getInstance().List_guardunit[defhero_id];

                //发动攻击
                int attck_m = (int)(((float)attack_hero.might) * attack_count_add - (float)def_hero.lead);

                if (attck_m > 0)
                {

                }
                else
                {
                    attck_m = 0;

                }

                int attack_count = attck_m * attack_guardunit.Armycount / 1000;

                //暴击
                if (UnityEngine.Random.Range(0, 10) > 5)
                {

                    //Global_events.getInstance ().traggerGlobalEvent (0,attack_count,attack_hero.id,def_hero.id);
                }


                def_guardunit.Armycount -= attack_count;

            string attackmessge = "发动攻击";
            if(attack_count_add== CriticalAttackrate)
            {
                attackmessge = "发动猛攻";
            }
                
                GlobalData.getInstance().global_message = Global_HeroData.getInstance().List_hero[attackhero_id].GetAllName() + "向" +
                    Global_HeroData.getInstance().List_hero[defhero_id].GetAllName() + attackmessge +
                    "斩首" + attack_count + "记\n" +
                     GlobalData.getInstance().global_message;
                
               
               Messenger.Broadcast("UPDATEGLOBALMESSAGE");
               Messenger.Broadcast<int, Vector3>("SHOWATTACKSPIRITE", attack_count, def_hero.GetPosition());       
         
                //判断是否被击溃
                if (def_guardunit.Armycount <= 0)
                {
                    def_guardunit.Armycount = 0;
                    GlobalData.getInstance().global_message = def_hero.GetAllName() + "被击溃了\n" + GlobalData.getInstance().global_message;

                    //判断击溃后果
                    int rand_end_of_hero = UnityEngine.Random.Range(1, 10);

                    /*defArmy.deletehero(def_hero.id);
                    def_hero.battleseleced = 0;

                    defArmy.destroyTest(def_hero.id);
                    */
                
                    CatchTest(defhero_id);
                }

                //更改被攻击army状态
                //defArmy.def_count = attack_count;
                //defArmy.battle_statue = 2;
                //GlobalData.getInstance ().global_message = attackArmy.m_id + "攻击了" + defArmy.m_id + "\n" + GlobalData.getInstance ().global_message;
                return true;

    }

    public bool Flee()
    {
        herodata thishero = Global_HeroData.getInstance().List_hero[ID];
        KindomData thisKindom = Global_KindomData.getInstance().list_KindomData[thishero.m_relationship.belong_kindom];
        herodata thisKindomKing = Global_HeroData.getInstance().List_hero[thisKindom.KingID];
        Armydata thisKingArmy = Global_Armydata.getInstance().List_army[thisKindom.KingID];

        //当随机数大于5时，逃跑
        //撤除targethero的上司的部队中本hero
        //将本hero的位置置于最近的一个城池
        int FriendImportanceID = FindNearestImportance(thishero.id, (int)SEARCHTYPE.FRIEND);

        if (FriendImportanceID != Global_const.NONE_ID)
        {
            Importance FriendImportance = Global_ImportanceData.getInstance().List_importance[FriendImportanceID];

            if (ID != thisKindomKing.id)
            {
                thisKingArmy.deletehero(ID);
            }

            GridManager.getInstance().ArmyMap[(int)thishero.GetPosition().x, (int)thishero.GetPosition().y] = Global_const.NONE_ID;

            thishero.SetPosition(FriendImportance.GetPosition());

            GlobalData.getInstance().global_message = thishero.GetAllName() + "逃回了" + FriendImportance.M_name + "\n" + GlobalData.getInstance().global_message;

            return true;
        }
        else
        {

            return false;

        }
    }

    public bool Surrender(int targetID)
    {


        herodata thishero = Global_HeroData.getInstance().List_hero[ID];

        if (thishero.m_relationship.belong_kindom!=Global_const.NONE_ID)
        {
            KindomData thisKindom = Global_KindomData.getInstance().list_KindomData[thishero.m_relationship.belong_kindom];
            herodata thisKindomKing = Global_HeroData.getInstance().List_hero[thisKindom.KingID];
            Armydata thisKingArmy = Global_Armydata.getInstance().List_army[thisKindom.KingID];

            thisKingArmy.deletehero(thishero.id);
            thisKindomKing.m_relationship.DeleteSubordinate(thishero);
        }

        herodata targethero = Global_HeroData.getInstance().List_hero[targetID];
        Armydata targetarmy = Global_Armydata.getInstance().List_army[targetID];
        KindomData targetKindom = Global_KindomData.getInstance().list_KindomData[targethero.m_relationship.belong_kindom];
        herodata targetKindomKing = Global_HeroData.getInstance().List_hero[targetKindom.KingID];
        Armydata targetKingArmy = Global_Armydata.getInstance().List_army[targetKindom.KingID];

        //当随机数大于3时，归降
        //撤除targethero的上司的部队中本hero
        //撤除targethero的上司的从属中本hero
        //将本hero从属于thishero
        //将本hero的位置置于最近的一个城池


        targetKindomKing.m_relationship.AddSubordinate(thishero);

        targetKindomKing.UpdateLike(30, thishero.id);

        int FriendImportanceID = FindNearestImportance(targethero.id, (int)SEARCHTYPE.FRIEND);

        if (FriendImportanceID != Global_const.NONE_ID)
        {
            Importance FriendImportance = Global_ImportanceData.getInstance().List_importance[FriendImportanceID];

            GridManager.getInstance().ArmyMap[(int)thishero.GetPosition().x, (int)thishero.GetPosition().y] = Global_const.NONE_ID;

            thishero.SetPosition(FriendImportance.GetPosition());

            GlobalData.getInstance().global_message = thishero.GetAllName() + "归顺了" + targethero.GetAllName() + "\n" + GlobalData.getInstance().global_message;

            return true;
        }
        else
        {
            return false;
        }
    }

    void CatchTest(int targetID)
    {
        herodata targethero = Global_HeroData.getInstance().List_hero[targetID];
        Armydata targetarmy = Global_Armydata.getInstance().List_army[targetID];

        int catchRand = UnityEngine.Random.Range(1, 10);

        int destoryType = (int)DestroyType.FLEE;

        if(catchRand>5)
        {
            destoryType = (int)DestroyType.FLEE;
        }
        else if(catchRand>2)
        {
            destoryType = (int)DestroyType.SURRENDER;
        }
        else
        {
            destoryType= (int)DestroyType.DIE;
        }

        destoryType = (int)DestroyType.SURRENDER;

        //不能逃则投降，不能投降则死

        if (destoryType == (int)DestroyType.FLEE)
        {
            if(false==targetarmy.Flee())
            {
                destoryType = (int)DestroyType.SURRENDER;
            }
        }

        if (destoryType == (int)DestroyType.SURRENDER)
        {

            if (ID == GlobalData.getInstance().nowheroid)
            {
                Global_events.getInstance().traggerGlobalEvent(31, 0, ID, targethero.id);
            }
            else
            {
                if(false==targetarmy.Surrender(ID))
                {
                    destoryType = (int)DestroyType.DIE;
                }
            }
        }

        if (destoryType == (int)DestroyType.DIE)
        {
            targethero.Die(ID);
        }

        Messenger.Broadcast("UPDATEGLOBALMESSAGE");
        
    }

    int  FindNearestImportance(int _thisID,int _Type)
    {
        int ThisKindomID = Global_HeroData.getInstance().List_hero[_thisID].m_relationship.belong_kindom;

        KindomData _ThisKindom = null;
        if (ThisKindomID == Global_const.NONE_ID)
        {
            //return Global_const.NONE_ID;

        }
        else
        {
            _ThisKindom = Global_KindomData.getInstance().list_KindomData[ThisKindomID];

        }

        Vector3 tilemap = Global_HeroData.getInstance().List_hero[_thisID].GetPosition();

        for (int tempsearchwidth = 1; tempsearchwidth < importancesearchwidth; tempsearchwidth++)
        {
            for (int x = (int)tilemap.x - tempsearchwidth; x < (int)tilemap.x + tempsearchwidth; x++)
            {
                for (int y = (int)tilemap.y - tempsearchwidth; y < (int)tilemap.y + tempsearchwidth; y++)
                {
                    if (x < 0 || x >= GridManager.getInstance().maxwidth || y < 0 || y > GridManager.getInstance().maxheight)
                    {
                        continue;
                    }

                    //不再搜索已经搜索过的地方
                    if (x > ((int)tilemap.x - tempsearchwidth) && x < ((int)tilemap.x + tempsearchwidth - 1) &&
                       y > ((int)tilemap.y - tempsearchwidth) && y < ((int)tilemap.y + tempsearchwidth - 1)
                       )
                    {
                        continue;
                    }


                    int TempID = GridManager.getInstance().ImportanceMap[x, y];

                    if(TempID==Global_const.NONE_ID)
                    {
                        continue;
                    }

                    //判断此城所属势力与本势力的关系，如果敌对则不能进入
                    Importance tempimportance = Global_ImportanceData.getInstance().List_importance[TempID];

                    /*Debug.Log("TempID:" + TempID);
                    Debug.Log("tempimportance.GetKindomID():" + tempimportance.GetKindomID());
                    */
                    int tempKindomID = tempimportance.GetKindomID();
                    if (tempKindomID==Global_const.NONE_ID)
                    {
                        continue;
                    }

                    KindomData tempimportanceKindom =Global_KindomData.getInstance().list_KindomData[tempKindomID];

                    switch (_Type)
                    {
                        case (int)SEARCHTYPE.FRIEND:

                            if(_ThisKindom==null)
                            {
                                return tempimportance.id;

                            }
                            else
                            {
                                if (_ThisKindom.Kindomrelation[tempimportanceKindom.id] >= 0)
                                {
                                    return tempimportance.id;

                                }
                                else
                                {
                                    continue;
                                }
                            }
                            

                            break;
                        case (int)SEARCHTYPE.HATE:
                            if (_ThisKindom == null)
                            {
                                return tempimportance.id;

                            }
                            else
                            {
                                if (_ThisKindom.Kindomrelation[tempimportanceKindom.id] < 0)
                                {
                                    return tempimportance.id;

                                }
                                else
                                {
                                    continue;
                                }
                            }
                            break;


                    }

     

                }
            }
        }

        return Global_const.NONE_ID;
    }

    public void UpdateTarget()
    {


        if (GlobalData.getInstance().nowheroid == ID)
              {
                      
              }
              else
              {
                 // Armydata temptarget = null;
                  if (MTargetStyle == (int)TargetStyle.NONE)
                  {

                        return;
                  }
                  else
                  {
                  //  temptarget = Global_Armydata.getInstance().List_army[TargetArmyID];
                  }

                      Node startNode;
                      Node goalNode;

                      //startPos = ;
                      //endPos = objEndCube.transform;
                      Tilemap tiledmap = GridManager.getInstance().tiledmap.GetComponent<Tilemap>();

                      if(getvec3()== TargetVec3)
                      {
                          return;
                      }
                        
                      

                      startNode = new Node(getvec3());

                      goalNode = new Node(TargetVec3);

            //Tile tiletemp = tiledmap.GetComponent<Tilemap>().GetTile(_tempx, _tempy);

            //Debug.Log("startNode.position" + startNode.position+"id"+ _id);
            //Debug.Log("goalNode.position" + goalNode.position);



            /* if(this.ID!=4)
             {
                 return;
             }
             */
                          GridManager.getInstance().TargetVec3 = TargetVec3;
                          GridManager.getInstance().NowThisID = this.ID;

                          pathArray = AStar_FS.FindPath(startNode, goalNode);
                            
                   
                          if (pathArray!=null&&pathArray.Count > 1)
                          {
                              Node nextNode = (Node)pathArray[1];
                              MoveArmy(nextNode.position);
                          }

                          GridManager.getInstance().NowThisID = Global_const.NONE_ID;


            //     }

        }

    }


    public void SetPartersTowardVec3()
    {
        for (int i = 0; i < int.Parse(this.this_Elem.GetAttribute("武将数")); i++)
        {
            int idtemp = int.Parse(this_Elem.GetAttribute("武将" + i));
            //Debug.Log("idtemp" + idtemp+ "this.ID"+this.ID);
            //Debug.Log("武将数"+int.Parse(this.this_Elem.GetAttribute("武将数")));
            if(idtemp!=this.ID)
            {
                if ((int)TargetStyle.ENEMY!=Global_Armydata.getInstance().List_army[idtemp].MTargetStyle)
                {

                    Vector3 randvec = this.getvec3();

                    /*if (Global_HeroData.getInstance().List_hero[ID].NOWimportanceID!=Global_const.NONE_ID)
                    {
                      //  Debug.Log("randvec:" + randvec);
                    }
                    else
                    {
                        int randplace = UnityEngine.Random.Range(0, 4);
                        switch (randplace)
                        {
                          case  (int)direct.UP:
                                randvec.y += 1;
                                break;
                        case (int)direct.DOWN:
                                randvec.y -= 1;

                                break;
                            case (int)direct.LEFT:
                                randvec.x -= 1;

                                break;
                            case (int)direct.RIGHT:
                                randvec.x += 1;

                                break;
                        }
                       // Debug.Log("rand randvec:" + randvec);

                    }
                    */

                    Global_Armydata.getInstance().List_army[idtemp].TargetVec3 = randvec;
                    Global_Armydata.getInstance().List_army[idtemp].MTargetStyle =(int)TargetStyle.LEADER;
                }
            }
        }

    }

    public Vector3 getvec3()
    {
        Vector3 vec3 = new Vector3(0, 0, 0);

        XmlElement tempXml = Global_HeroData.getInstance().List_hero[ID].this_Elem;

        try
        {
            vec3 = new Vector3(int.Parse(tempXml.GetAttribute("x")), int.Parse(tempXml.GetAttribute("y")), float.Parse(tempXml.GetAttribute("z")));
        }
        catch
        {


        }

        return vec3;
    }

    public void setvec3(Vector3 _vec3)
    {
        XmlElement tempXml = Global_HeroData.getInstance().List_hero[ID].this_Elem;

        if(GridManager.getInstance().ArmyMap[(int)getvec3().x,(int)getvec3().y]==ID)
        {
            GridManager.getInstance().SetMap(GridManager.getInstance().ArmyMap, getvec3(), Global_const.NONE_ID);
        }

        tempXml.SetAttribute("x", "" + _vec3.x);
        tempXml.SetAttribute("y", "" + _vec3.y);
        tempXml.SetAttribute("z", "" + _vec3.z);

        if (GridManager.getInstance().ImportanceMap[(int)getvec3().x, (int)getvec3().y]==Global_const.NONE_ID)
        {
            GridManager.getInstance().SetMap(GridManager.getInstance().ArmyMap, getvec3(), this.ID);
        }

    }

	public void destroyTest(int last_heroid)
	{
        int herocount = 0;
        herocount = int.Parse(this_Elem.GetAttribute("武将数"));

        if (herocount <= 0) {

			clean ();

			//Global_Armydata.getInstance ().armycount--;

			if (Global_const.NONE_ID != last_heroid) {
				GlobalData.getInstance ().global_message = Global_HeroData.getInstance ().List_hero [last_heroid].GetAllName () + "军团被击溃了\n" + GlobalData.getInstance ().global_message;
			}
		}
	}

	public void WinSmallBattle()
	{
		/*for (int i = 0; i < heroCount; i++) {
			if (heroID_list[i]!=Global_const.NONE_ID) {

                Guardunit tempguardunit = Global_GuardunitData.getInstance().List_guardunit[heroID_list[i]];

                tempguardunit.Morale += 10;

			}

		}
        */
	}

	public void LoseSmallBattle()
	{
		/*for (int i = 0; i < heroCount; i++) {
			if (heroID_list[i]!=Global_const.NONE_ID) {


                Guardunit tempguardunit = Global_GuardunitData.getInstance().List_guardunit[heroID_list[i]];

                tempguardunit.Morale -= 10;
            }

		}
        */
	}

    public bool ImprotanceAttackTest(int attack_m_id)
    {
        /*	for (int i = 0; i < Global_const.getInstance ().MAXIMPORTANCE; i++) {

                Importance defimportance = Global_ImportanceData.getInstance ().List_importance [i];

                Armydata attackArmy = Global_Armydata.getInstance ().List_army [attack_m_id];

                    //如果是同势力，则continue
                herodata LeaderData=Global_HeroData.getInstance().List_hero[defimportance.LeaderID];

                if (attackArmy.Belong ==LeaderData.m_relationship.belong_kindom) {
                        continue;

                    }

                    //判断是否是敌对势力，如果是则进行进攻判断
                    if (true == ImportanceAttack (ref attackArmy, ref defimportance)) {

                        return true;
                    }

            }
    */

        return false;
    }


    bool ImportanceAttack(ref Armydata attackArmy, ref Importance defImportance)
    {
      /*  if ((Math.Abs(attackArmy.vec3.x - defImportance.vec3.x) < 0.3) && (Math.Abs(attackArmy.vec3.y - defImportance.vec3.y) < 0.3))
        {
            int attackhero_id = Global_const.NONE_ID;
            int temprand = Global_const.NONE_ID;

            //判断先锋攻击
            if (1 == UnityEngine.Random.Range(1, 2))
            {
                
                attackhero_id = attackArmy.battleworksID[2];
            }
            else
            {
                //如果先锋攻击判定否
                //获得攻击方一个随机武将

                temprand = UnityEngine.Random.Range(0, attackArmy.heroCount - 1);

                attackhero_id = attackArmy.heroID_list[temprand];

            }

            //如果没有先锋
            if (attackhero_id == Global_const.NONE_ID)
            {
                return true;
            }

            herodata attack_hero = Global_HeroData.getInstance().List_hero[attackhero_id];
            Guardunit attack_guardunit = Global_GuardunitData.getInstance().List_guardunit[attackhero_id];


            //判断士气，如果士气不够则不发动攻击
            if (UnityEngine.Random.Range(0, 100) > attack_guardunit.Morale)
            {

                GlobalData.getInstance().global_message = attack_hero.GetAllName() + "催促士兵发动攻击，由于士气低下士兵们踌躇不前\n" + GlobalData.getInstance().global_message;

                return true;
            }

            if (defImportance.LeaderID == Global_const.NONE_ID)
            {

                return false;

            }


            //获得守城太守
            herodata def_hero = Global_HeroData.getInstance().List_hero[defImportance.LeaderID];
            Guardunit def_guardunit = Global_GuardunitData.getInstance().List_guardunit[defImportance.LeaderID];


            //发动攻击
            int attck_m = attack_hero.might * 2 - def_hero.lead;

            if (attck_m > 0)
            {

            }
            else
            {
                attck_m = 0;

            }

            int attack_count = attck_m * def_guardunit.Armycount / 30;


            //城防未破，攻击城防
            if (defImportance.defenseNOW > 0)
            {
                defImportance.defenseNOW -= attack_count;

                defImportance.battle_statue = 2;
                defImportance.def_count = attack_count;

                GlobalData.getInstance().global_message = Global_HeroData.getInstance().List_hero[attackhero_id].GetAllName() + "向" +
                    defImportance.M_name + "发动猛攻" +
                    "城墙被破坏了" + attack_count + "\n" +
                    GlobalData.getInstance().global_message;

                if (defImportance.defenseNOW < 0)
                {
                    defImportance.defenseNOW = 0;

                }

            }
            else
            {//城防已破，攻击太守
                def_guardunit.Armycount -= attack_count;

                if (def_guardunit.Armycount < 0)
                {

                    def_guardunit.Armycount = 0;

                }

                defImportance.battle_statue = 2;
                defImportance.def_count = attack_count;

                GlobalData.getInstance().global_message = Global_HeroData.getInstance().List_hero[attackhero_id].GetAllName()
                    + "冲进了城池，与"
                    + defImportance.M_name
                    + "守军发生巷战,斩敌"
                    + attack_count + "记\n" +
                    GlobalData.getInstance().global_message;
            }

            if ((defImportance.defenseNOW <= 0) && (def_guardunit.Armycount <= 0))
            {

                GlobalData.getInstance().global_message =
                    Global_HeroData.getInstance().List_hero[attackhero_id].GetAllName()
                    + "攻陷了"
                    + defImportance.M_name
                    + "记\n"
                    + GlobalData.getInstance().global_message;

                //KindomData armykindom = Global_KindomData.getInstance ().list_KindomData [attackArmy.Belong];
                //herodata king =Global_HeroData.getInstance().List_hero[armykindom.KingID];


                //def_hero.ResetKindom (attackArmy.Belong);

                //def_hero.m_relationship.belong_kindom = attackArmy.Belong;
                //def_hero.m_relationship.Factions_leader_ID = armykindom.KingID;

                //defImportance.LeaderID = attackArmy.battleworksID[0];
            }

        }
    */
        return false;
    }

    bool ISEnemeyArmy(int _attackID,int _defendID)
    {
        herodata attackLeader = Global_HeroData.getInstance().List_hero[_attackID];
        KindomData attackKindom = Global_KindomData.getInstance().list_KindomData[attackLeader.m_relationship.belong_kindom];

        herodata defendLeader = Global_HeroData.getInstance().List_hero[_defendID];

        if (defendLeader.m_relationship.belong_kindom == Global_const.NONE_ID)
        {
            return false;
        }

        KindomData defendKindom = Global_KindomData.getInstance().list_KindomData[defendLeader.m_relationship.belong_kindom];


        if (attackKindom.Kindomrelation[defendKindom.id] >= 0)
        {
            return false;
        }

        return true;
    }


    int FindNearestEnemy(int _id)
    {
        Vector3 thisvec = getvec3();
        int thisx = (int)thisvec.x;
        int thisy = (int)thisvec.y;

        for (int tempsearchwidth = 0; tempsearchwidth < armysearchwidth; tempsearchwidth++)
        {
            for (int x = thisx - tempsearchwidth; x < thisx + tempsearchwidth; x++)
            {
                for (int y = thisy - tempsearchwidth; y < thisy + tempsearchwidth; y++)
                {
                    if (x < 0 || x >= GridManager.getInstance().maxwidth || y < 0 || y > GridManager.getInstance().maxheight)
                    {
                        continue;
                    }

                    //不再搜索已经搜索过的地方
                    if (x > (thisx - tempsearchwidth) && x < (thisx + tempsearchwidth - 1) &&
                       y > (thisy - tempsearchwidth) && y < (thisy + tempsearchwidth - 1))
                    {
                        continue;
                    }

                    int TempID = GridManager.getInstance().ArmyMap[x, y];

                    if (TempID == this.ID)
                    {
                        continue;
                    }

                    if (TempID != Global_const.NONE_ID)
                    {
                        //Debug.Log("this.ID:" + this.ID + "\n");
                        //Debug.Log("thisPlaceArmyID:" + TempID + "\n");
                        /* if(this.ID!=4)
                         {
                             continue;
                         }
                         */                      

                        int targetKindomID = Global_HeroData.getInstance().List_hero[TempID].m_relationship.belong_kindom;

                        if (Global_const.NONE_ID == targetKindomID)
                        {
                            //return Global_const.NONE_ID;
                            continue;
                        }

                        int thisKindomID = Global_HeroData.getInstance().List_hero[_id].m_relationship.belong_kindom;

                        if (Global_const.NONE_ID == thisKindomID)
                        {
                            //return Global_const.NONE_ID;
                            continue;
                        }

                        if (thisKindomID == targetKindomID)
                        {
                            continue;
                        }
                        /* int relationtemp = Global_KindomData.getInstance().list_KindomData[thisKindomID].Kindomrelation[targetKindomID];


                         Debug.Log("_id" + _id);

                         Debug.Log("thisKindomID" + thisKindomID);

                         Debug.Log("relationtemp"+relationtemp);
                         */

                        KindomData thiskindom = Global_KindomData.getInstance().list_KindomData[thisKindomID];
                        KindomData nowselectedarmykindom = Global_KindomData.getInstance().list_KindomData[targetKindomID];

                        int Kindomrelationtemp = thiskindom.Kindomrelation[nowselectedarmykindom.id];

                        if (Kindomrelationtemp < 0)
                        {
                            Guardunit targetunit = Global_GuardunitData.getInstance().List_guardunit[TempID];
                            Guardunit thisunit = Global_GuardunitData.getInstance().List_guardunit[this.ID];

                            if(thisunit.Armycount> targetunit.Armycount/4)
                            {
                                return TempID;

                            }
                            else
                            {
                                return Global_const.NONE_ID;
                            }

                        }
                        else
                        {
                            continue;
                        }


                    }


                }


            }

        }
        return Global_const.NONE_ID;
    }

    public void FindTarget()
    {
        //当兵力大于最大兵力的一半时，搜索最近的敌人
       herodata thishero = Global_HeroData.getInstance().List_hero[ID];
       Guardunit thisguard = Global_GuardunitData.getInstance().List_guardunit[ID];
       
       MTargetStyle = (int)TargetStyle.NONE;

       if (thisguard.Armycount> thishero.GetMaxReP()/2)
       {
            int EnemyID = FindNearestEnemy(ID);

            if(EnemyID!=Global_const.NONE_ID)
            {
                MTargetStyle = (int)TargetStyle.ENEMY;
                TargetVec3 = Global_Armydata.getInstance().List_army[EnemyID].getvec3();
            }
       }

       if (MTargetStyle == (int)TargetStyle.NONE&&
        thishero.NOWimportanceID==Global_const.NONE_ID)
       {
            //否则搜索最近的友好城市，目标城市
            int FriendImportanceID = FindNearestImportance(ID, (int)SEARCHTYPE.FRIEND);
            if (FriendImportanceID != Global_const.NONE_ID)
            {
                MTargetStyle = (int)TargetStyle.FRIENDIMPORTANCE;
                TargetVec3 = Global_ImportanceData.getInstance().List_importance[FriendImportanceID].GetPosition();
            }
       }


    }
}

