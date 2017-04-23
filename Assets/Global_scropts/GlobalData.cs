using System;
using UnityEngine;


public class GlobalData 
{
	public int nowimportanceID=0;
	public int year=194;
	public int month=1;
    public int day=1;
	public int time=0;

	public string global_message="";

	public int UI_army_create_statue=0;// 0 组建 1 军团长选择 2 军师选择  3 先锋选择 4 运粮官选择 

    public int nowheroid = 1;
	public int nowkindomid = 0;

    public int nowselectedimportance = 0;
	public int nowselectedarmy = 0;

	public float cam_lev=1;
	public float cam_size=1;

	public int screen_statue=0;//0:bigmap 1:ui 2:incity

    public enum  NOWSORTSTYLE { FRIENDS=3,FORWARD,COUNSELLOR,GUARD,IMPEDIMENTA};

    public int nowherosortstyle = Global_const.NONE_ID;
	//州名称
	private static string Global_ZhouList =
		"司州," +
		"幽州," +
		"翼州," +
		"并州," +
		"青州," +
		"兖州," +
		"豫州," +
		"徐州," +
		"雍州," +
		"凉州," +
		"荆州," +
		"益州," +
		"扬州," +
		"交州";

	public string [] Global_ZhouListName = Global_ZhouList.Split(new char[] { ',' });




    public bool b_update=true;

		private static GlobalData _instance;

		// Lock synchronization object
		private static readonly object _syncLock = new object();

		// Constructor is 'private'
		private GlobalData()
		{


		}

		public static GlobalData getInstance()
		{
			// Support multithreaded applications through
			// 'Double checked locking' pattern which (once
			// the instance exists) avoids locking each
			// time the method is invoked
			if (_instance == null)
			{
				lock (_syncLock)
				{
					if (_instance == null)
					{
						_instance = new GlobalData();
					}
				}
			}

			return _instance;
		}


       /* public void AutoSave()
        {
            StartCoroutine(Global_ImportanceData.getInstance().Save_importance_Xml("/xml_importancelist.xml"));
            StartCoroutine(Global_XML_IO.getInstance().Save_hero_Xml("/xml_herolist.xml"));
            StartCoroutine(Global_XML_IO.getInstance().Save_kindom_Xml("/xml_kindomlist.xml"));
            StartCoroutine(Global_Armydata.getInstance().Save_army_Xml("/xml_armylist.xml"));

        }
        */
}

