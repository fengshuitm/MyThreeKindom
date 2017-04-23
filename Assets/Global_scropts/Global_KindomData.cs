using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading;


public class Global_KindomData {

	public KindomData[] list_KindomData=new KindomData[Global_const.getInstance().MAXKINDOM_COUNT];

	private static  Global_KindomData instance = new Global_KindomData(); 


	private Global_KindomData()
	{
		for(int i=0;i<Global_const.getInstance().MAXKINDOM_COUNT;i++)
		{
			list_KindomData [i] = new KindomData ();
			list_KindomData[i].id=i;
			list_KindomData[i].KingID=Global_const.NONE_ID;

		}

		Init_List_kindom ();

	}

	public static Global_KindomData  getInstance() 
	{ 

		return instance; 
	}

	public int createnewKindom(herodata _king)
	{
		for (int i = 0; i < Global_const.getInstance().MAXKINDOM_COUNT; i++) {
			//找到第一个空的势力
			if (list_KindomData [i].KingID== Global_const.NONE_ID) {

				list_KindomData [i].id = i;
				list_KindomData [i].KingID = _king.id;
				_king.m_relationship.belong_kindom = list_KindomData [i].id;

				return list_KindomData [i].id;
			}
		}


		return Global_const.NONE_ID;
	}


	//生成国家信息
	public void Init_List_kindom()
	{


		kindomlist_load ();
		//InitHerosubordinateidLists ();
	}

	private void kindomlist_load ()
	{
		Global_XML_IO tempIO = Global_XML_IO.getInstance ();

		tempIO.Load_kindom_Xml ();
		//XmlNodeList xmlNodeList =tempIO.xml_kindomlist.SelectSingleNode("kindomlist").ChildNodes;

		foreach (XmlElement xl1 in Global_XML_IO.getInstance().xNode_kindom.ChildNodes) {

			int id = 0;
			try
			{
				id =int.Parse( xl1.GetAttribute ("id"));
			}
			catch {

			}

			try
			{
				list_KindomData[id].KingID
				=int.Parse( xl1.GetAttribute ("KingID"));
			}
			catch {

			}

            
                for (int i = 0; i < Global_const.getInstance().MAXKINDOM_COUNT; i++)
                {
                    try
                    {
                        list_KindomData[id].Kindomrelation[i]
                        = int.Parse(xl1.GetAttribute("Kindomrelation" + i));
                    }
                    catch
                    {

                    }
                }


        }
	}


}
