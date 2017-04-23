using UnityEngine;
using System.Collections;

public class Global_events
{
	public EventTrigger [] EventTrigger_list=new EventTrigger[500];

	public GameObject Event_anim;
	public GameObject Event_Text_Middle;
	public GameObject Event_Text_Up;
	public GameObject Event_Text_Down;
	public GameObject Event_Hero1;
	public GameObject Event_Hero2;

	public GameObject Event_plane;

	public GameObject Event_hero_text;

	//GameObject EventImage;

	public bool re_init=false;

	private static  Global_events instance = new Global_events(); 

	public DialogueManager DilogMana;

	//TextAsset dilog1;

	private Global_events(){

		init ();

	}


	public static Global_events  getInstance() 
	{ 
		return instance; 
	}

	public void Update()
	{


	}

	public void init()
	{
		//dilog1 = Resources.Load ("Descs/Dialogue2") as TextAsset;
		initEventTriggerList();

	}

	//0 城市  1 城墙  2 议事厅外  3  民宅  4  议事厅   5  街道  6  集市   7  农村   8   城外  9 练兵场
	void initEventTriggerList()
	{
		for(int i=0;i<EventTrigger_list.Length;i++)
		{
			EventTrigger_list[i]=new EventTrigger();
		}

		//for(int i=0;i
		//事件0 城墙
		//EventTrigger_list[1].step=1;
		EventTrigger_list[2].step=1;

	}

	public void TestEventTriggerList(int placenum,int citynum=-1)
	{
		//for(int i=0;i
		//事件0 城墙

		for (int i = 0; i < EventTrigger_list.Length; i++) {

			//如果此事件还未开始则退出
			if (EventTrigger_list [i].step == 0 ||EventTrigger_list [i].step == -1) {

				continue;
			}

			//如果满足触发地点条件，按照step触发事件
			if (placenum == EventTrigger_list [i].place) {

				//没有城市变量
				if (Global_const.NONE_ID == EventTrigger_list [i].city) {
					traggerGlobalEvent (1000 + i, 0, GlobalData.getInstance ().nowheroid, 0, "STEP" + EventTrigger_list [i].step);
				} else if (citynum == EventTrigger_list [i].city) {
					traggerGlobalEvent (1000 + i, 0, GlobalData.getInstance ().nowheroid, 0, "STEP" + EventTrigger_list [i].step);

				}


				break;
			}

		}

	}


	public void traggerGlobalEvent(int EventID,int param1=0,int param2=0,int param3=0,string _dilgbegin="begin")
	{
		/*switch (EventID) {

		case 0://步兵冲阵
			//Event_anim.GetComponent<Event_Spirite> ().move();
			Event_anim.SetActive(true);
			Event_anim.GetComponent<Event_Spirite> ().setanim (0);
			Event_Text_Middle.GetComponent<TextMesh> ().text = "" + param1;
			Event_anim.GetComponent<Event_Spirite> ().b_forever = false;
			Event_anim.SetActive (true);
			Event_Hero1.GetComponent<SpriteRenderer>().sprite=Global_source_loader.getInstance().hero_L_face[param2];
			Event_Hero2.GetComponent<SpriteRenderer>().sprite=Global_source_loader.getInstance().hero_L_face[param3];
			Event_Text_Up.GetComponent<TextMesh> ().text = "让你尝尝长矛的威力！";
			Event_Text_Down.GetComponent<TextMesh> ().text = "输了一阵。。。";

			break;

		case 1:

			break;
		case 38:

			traggerGlobalEvent_Static(EventID, param1, param2, param3);
			Global_events.getInstance ().DilogMana.StartDialogue (EventID);

			break;
		case 441:

			traggerGlobalEvent_Static(EventID, param1, param2, param3);
			Global_events.getInstance ().DilogMana.StartDialogue (EventID);

			break;
		case 373:

			traggerGlobalEvent_Static(EventID, param1, param2, param3);

			break;
		case 379:

			traggerGlobalEvent_Static(EventID, param1, param2, param3);

			break;

		case 387://触发遭遇战,地图号param1
			traggerGlobalEvent_Static(EventID, param1, param2, param3);

			break;

		case 391:
			traggerGlobalEvent_Static(EventID, param1, param2, param3);


			break;
		case 392:
			traggerGlobalEvent_Static(EventID, param1, param2, param3);

			break;


		}

		*/

		Global_events.getInstance ().DilogMana.SetEvent(EventID, param1, param2, param3,_dilgbegin);

	}
		

}

