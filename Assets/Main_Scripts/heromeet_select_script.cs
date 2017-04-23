using UnityEngine;
using System.Collections;

public class heromeet_select_script : UI_OBJ {

	public GameObject list_pref;

	GameObject [] preftemp_list=new GameObject[10];
	int listbegin=0;
	int herocount=0;

	// Use this for initialization
	void Start () {
	
	/*	for (int i = 0; i < 10; i++) {

			preftemp_list[i] =Object.Instantiate (list_pref, list_pref.transform.position, list_pref.transform.rotation) as GameObject;
			preftemp_list[i].name = "herodata_listbutton (" + i+")";
			preftemp_list[i].transform.parent = this.transform;
			preftemp_list[i].transform.localPosition = new Vector3(0, 190-i*37, 0);
			preftemp_list[i].transform.localScale = new Vector3(1, 1, 1);
			//preftemp.transform.position.y -= 20;
		}
		invalidatelist ();
	*/
		//this.gameObject.SetActive(false);
		//Global_UI.getInstance().MeetList=this.gameObject;
	}

    

    public override void Invalidate ()
	{
			int [] tempheroiddata;
			clearlist ();

			for (int i = 0; i < 10; i++) {

				int i_id_flag = i + listbegin;
				int i_id = 0;

				if (i_id_flag >= herocount) {
					return;
				}

				tempheroiddata=Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].m_relationship.friends;

				i_id = tempheroiddata [i_id_flag];

				herodatashow_script hero_item = preftemp_list[i].GetComponent<herodatashow_script>();

				hero_item.sethero_ID (i_id);
			}


	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		
		//herocount = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid].m_relationship.friendscount;

		//invalidatelist ();
	}

	public void beforelist()
	{
		if (listbegin > 0) {
			listbegin -= 10;
		}
		Invalidate ();
	}

	public void afterlist()
	{
		if ((listbegin + 10) < herocount) {

			listbegin +=10;

		}
		Invalidate ();
	}

	void OnEnable()
	{
		GlobalData.getInstance ().b_update = false;

		listbegin = 0;
		//invalidatelist ();

		for (int i = 0; i < 10; i++) {

			preftemp_list[i] =Object.Instantiate (list_pref, list_pref.transform.position, list_pref.transform.rotation) as GameObject;
			preftemp_list[i].name = "herodata_listbutton (" + i+")";
			preftemp_list[i].transform.parent = this.transform;
			preftemp_list[i].transform.localPosition = new Vector3(0, 190-i*37, 0);
			preftemp_list[i].transform.localScale = new Vector3(1, 1, 1);
			//preftemp.transform.position.y -= 20;

		}


		herocount = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid].m_relationship.friendscount;

		Invalidate ();


	}

	void OnDisable()
	{
		GlobalData.getInstance ().b_update = true;

		Invalidate ();
		clearlist ();
	}

	void clearlist ()
	{
		for (int i = 0; i < 10; i++) {

			herodatashow_script hero_item = preftemp_list[i].GetComponent<herodatashow_script>();

			hero_item.sethero_ID (Global_const.NONE_ID);
			hero_item.meetlist = this.gameObject;
		}

	}


}
