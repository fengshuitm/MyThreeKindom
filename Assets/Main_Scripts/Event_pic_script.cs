using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Event_pic_script : UI_OBJ {

	public Image event_image;
	public Image head_image;
	public Image head_image_AI;

	public GameObject Object_PC;
	public GameObject Object_AI;

	public Text meet_text_PC;
	public Text meet_text_AI;

	//public Texture tex;

	public GameObject [] UI_list;

	int tempcount=0;
	int m_hero1_id=Global_const.NONE_ID;
	int m_hero2_id=Global_const.NONE_ID;
	int m_EventID=Global_const.NONE_ID;

	public GameObject hero_datashow_window;

	public Scrollbar timeScrollbar;

	public bool b_forever = false;
	// Use this for initialization
	void Start () {
		
		Global_events.getInstance ().Event_plane = this.gameObject;

	}
	
	// Update is called once per frame
	void Update () {


	}


	void OnEnable()
	{
		tempcount = 100;
		FixedUpdate ();
	}


	public override void Invalidate ()
	{
		switch (m_EventID) {
		case 38:

			Object_PC.SetActive (true);
			Object_AI.SetActive (true);

			//TextAsset dilog1 = Resources.Load ("Descs/Dialogue1") as TextAsset;
			//Global_events.getInstance ().DilogMana.StartDialogue (dilog1);
			//timeScrollbar.gameObject.SetActive (true);
			//timeScrollbar.gameObject.SetActive (false);
			//UI_list [0].SetActive (true);
			//MeetingRoll ();
			head_image.sprite = Global_source_loader.getInstance ().hero_L_face [GlobalData.getInstance().nowheroid];
			head_image_AI.sprite = Global_source_loader.getInstance ().hero_L_face [m_hero2_id];

			//HuntRoll();
			break;

		case 373:
			Object_PC.SetActive (true);

			//timeScrollbar.gameObject.SetActive (true);
			UI_list [0].SetActive (true);
			//MeetingRoll ();
			head_image.sprite = Global_source_loader.getInstance ().hero_L_face [GlobalData.getInstance().nowheroid];
			HuntRoll();
			break;
		case 379:
			Object_PC.SetActive (true);

			//UpdateScrollbar();
			//timeScrollbar.gameObject.SetActive (true);
			UI_list [0].SetActive (true);
			UI_list [1].SetActive (true);
			head_image.sprite = Global_source_loader.getInstance ().hero_L_face [m_hero2_id];
			MeetingRoll ();
			break;
		case 380:
			break;
		case 387:
			Object_PC.SetActive (true);

			//timeScrollbar.gameObject.SetActive (false);
			UI_list [2].SetActive (true);

			head_image.sprite = Global_source_loader.getInstance ().hero_L_face [GlobalData.getInstance().nowheroid];
			meet_text_PC.text = "我们遭遇敌军营寨！！！击溃他们！！";

			break;
		case 391:
			Object_PC.SetActive (true);

			//timeScrollbar.gameObject.SetActive (false);

			head_image.sprite = Global_source_loader.getInstance ().hero_L_face [GlobalData.getInstance().nowheroid];
			meet_text_PC.text = "我们胜利了";
			break;
		case 392:
			Object_PC.SetActive (true);
			//timeScrollbar.gameObject.SetActive (false);

			head_image.sprite = Global_source_loader.getInstance ().hero_L_face [GlobalData.getInstance().nowheroid];
			meet_text_PC.text = "我们受到重创";
			break;
		case 441:
			Object_PC.SetActive (true);
			Object_AI.SetActive (true);

			//timeScrollbar.gameObject.SetActive (false);

			head_image.sprite = Global_source_loader.getInstance ().hero_L_face [GlobalData.getInstance().nowheroid];
			head_image_AI.sprite = Global_source_loader.getInstance ().hero_L_face [m_hero2_id];

			break;
		}

	}

	void FixedUpdate()
	{
		
	/*	tempcount++;

		//不一定有进度条



		//process.GetComponent<RectTransform> ().position = new Vector2 (376 + 536 * tempcount / 100, 222);

		//process.GetComponent<RectTransform> ().position = new Vector3 (640, -253, 0);

		UpdateScrollbar();

		try
		{
		if (tempcount >= 100) {

				Invalidate();

			

			tempcount = 0;
		}
		}
		catch {

		}
		*/
	}

	void UpdateScrollbar()
	{
		//timeScrollbar.gameObject.SetActive (true);
		try
		{
			timeScrollbar.value = tempcount/100.0f;
		}
		catch {

		}

	}

	void HuntRoll()
	{
		try
		{
			int Rolltemp = Random.Range (0, 10);

			switch (Rolltemp) {
			case 0:
			case 1:
			case 2:

				//int friendadd=Mathf.Abs(Global_HeroData.getInstance ().List_hero [m_hero2_id].phase-Global_HeroData.getInstance ().List_hero [m_hero1_id].phase)+Global_HeroData.getInstance ().List_hero [m_hero1_id].charm/10+Random.Range(0,5);
				herodata nowhero=Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];

				int hunt_forage_temp=Random.Range (1, 10)*nowhero.might;

				meet_text_PC.text = "收获猎物"+hunt_forage_temp;

				nowhero.forage+=hunt_forage_temp;

				break;
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				meet_text_PC.text = "啥也没打到";
				break;
			}


		}
		catch {


		}

	}

	void MeetingRoll()
	{
		try
		{
			int Rolltemp = Random.Range (0, 10);

		/*	if(Global_HeroData.getInstance ().List_hero [m_hero2_id].m_relationship.likelist [m_hero1_id]<0)
			{
				meet_text_PC.text = "对不起，请回吧";
				return;
			}
		*/
			switch (Rolltemp) {
			case 0:
			case 1:
			case 2:

				int friendadd=Mathf.Abs(Global_HeroData.getInstance ().List_hero [m_hero2_id].phase-Global_HeroData.getInstance ().List_hero [m_hero1_id].phase)+Global_HeroData.getInstance ().List_hero [m_hero1_id].charm/10+Random.Range(0,5);

				/*Global_HeroData.getInstance ().List_hero [m_hero2_id].m_relationship.likelist [m_hero1_id]+=friendadd;

				if(Global_HeroData.getInstance ().List_hero [m_hero2_id].m_relationship.likelist [m_hero1_id]>=100)
				{
					Global_HeroData.getInstance ().List_hero [m_hero2_id].m_relationship.likelist [m_hero1_id]=100;
					meet_text_PC.text = Global_HeroData.getInstance ().List_hero [m_hero2_id].GetAllName()+"已经与你亲密无间";

				}
				else
				{
					meet_text_PC.text = "友好度+"+friendadd;
				}
		*/
				break;
			case 5:
			case 6:
			case 7:
				meet_text_PC.text = "您来了，请坐请坐";
				break;
			case 8:
			case 9:
			case 10:
				meet_text_PC.text = "对不起今天正好要出门，有时间去您府上回拜";
				break;
			}
		}
		catch {


		}

	}

	void InitEventUI ()
	{
		//timeScrollbar.gameObject.SetActive (true);
		Object_AI.SetActive (false);
		Object_PC.SetActive (false);

		for (int i = 0; i < UI_list.Length; i++) {
			UI_list [i].SetActive (false);
		}

	}


	public void SetEvent(int EventID,int param1=0,int param2=0,int param3=0)
	{
		m_EventID = EventID;
		m_hero1_id = param2;
		m_hero2_id = param3;

		event_image.sprite=Global_source_loader.getInstance().Event_Sprite[EventID];

		InitEventUI ();
		Invalidate ();

	}


	public void herodatashow()
	{
		hero_datashow_window.GetComponent<hero_datashow_script> ().hero_id = m_hero2_id;
		hero_datashow_window.GetComponent<hero_datashow_script> ().move();

	}

	public void GotoSmallBattle()
	{
	
		//Global_Armydata.getInstance().SavePosition ();

		//SceneManager.LoadScene("smallbattle");
		//SceneManager.LoadScene("smallbattle");
		//SceneManager.SetActiveScene (Global_source_loader.getInstance().SceneList[0]);
	}

}
