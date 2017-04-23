using System;
using UnityEngine;

public class Global_UI
{
	private static Global_UI _instance;

	//public GameObject HerodataUI;
	//public GameObject PlaceUI;

    //public Camera cam;

    //public GameObject ArmySetUI;

    public GameObject MeetList;
	//public GameObject BattleList;

    public GameObject TextBack;                                                            // Non-Player Character name

    private Global_UI()
	{

    }

    public void Init()
    {
        MeetList = GameObject.Find("Canvas/heroselectpanl");
        TextBack = GameObject.Find("Canvas/mainui/SceneManager/Event_plane/Back");

    }

    private static readonly object _syncLock = new object();

    public static Global_UI getInstance()
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
					_instance = new Global_UI();
				}
			}
		}

		return _instance;
	}
}