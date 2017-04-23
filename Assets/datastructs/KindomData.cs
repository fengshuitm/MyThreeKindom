using UnityEngine;
using System.Collections;
using System.Collections;
using System.Xml;
using System.IO;

public class KindomData{

	public int id;
	public int KingID;
	public int [] Kindomrelation=new int[Global_const.getInstance().MAXKINDOM_COUNT];

    public XmlElement this_Elem = null;// new XmlElement[Global_const.getInstance().MAXARMY];

    public KindomData()
	{
		initrelation ();

	}

    public void setnewKing(int _id)
    {
        //变更从属关系
        herodata oldKing = Global_HeroData.getInstance().List_hero[KingID];
        herodata newKing = Global_HeroData.getInstance().List_hero[_id];

        for (int i=0;i<oldKing.m_relationship.subordinatecount;i++)
        {
            herodata temphero = Global_HeroData.getInstance().List_hero[oldKing.m_relationship.subordinateid[i]];
            newKing.m_relationship.AddSubordinate(temphero);

        }

        this.KingID = _id;

        GlobalData.getInstance().global_message = Global_HeroData.getInstance().List_hero[this.KingID].heroname[0] + "势力拥立了新君主"+
            Global_HeroData.getInstance().List_hero[this.KingID].GetAllName()+"\n"
                + GlobalData.getInstance().global_message;
        Messenger.Broadcast("UPDATEGLOBALMESSAGE");
    }

	public void destroy()
	{
		

        GlobalData.getInstance().global_message = Global_HeroData.getInstance().List_hero[this.KingID].heroname[0] + "势力灭亡了"+"\n"
                + GlobalData.getInstance().global_message;

        for(int i=0;i<Global_const.MAXIMPORTANCE;i++)
        {
            Importance tempimportance = Global_ImportanceData.getInstance().List_importance[i];

            if (tempimportance.GetKindomID()==id)
            {
                tempimportance.LeaderID = Global_const.NONE_ID;
            }
        }

        this.id = Global_const.NONE_ID;
        this.KingID = Global_const.NONE_ID;
        initrelation();

        Messenger.Broadcast("UPDATEGLOBALMESSAGE");
    }

	void initrelation()
	{
		for (int i = 0; i < Global_const.getInstance ().MAXKINDOM_COUNT; i++) {

			Kindomrelation [i] = 0;//友好度，0为无关，-10为恶劣，10为友好
		}
	}

   public void setRelation(int _targetKindomID,string _relationship)
   {
        switch(_relationship)
        {
            case "同盟":
                Kindomrelation[_targetKindomID] = 10;
                break;
            case "开战":
                Kindomrelation[_targetKindomID] = -10;
                break;
            case "停战":
                Kindomrelation[_targetKindomID] = 0;
                break;
        }

    }
 

}
