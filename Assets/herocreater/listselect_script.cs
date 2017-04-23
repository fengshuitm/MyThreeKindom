using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class listselect_script : UI_OBJ
{
	public GameObject list_pref;
	public GameObject search_items;

	public GameObject [] preftemp_list=new GameObject[10];
	protected int listbegin=0;

	protected int listcount=0;

	//public int Maxitems=10;


	bool inited=false;

	protected int step=0;

	void Awake()
	{
		step = preftemp_list.Length;
		Messenger.AddListener( "invalidatelist", invalidatelist);

	}

	void OnDestory()
	{

		//Messenger.RemoveListener( "invalidatelist", invalidatelist);

	}

	void OnDisable()
	{
		GlobalData.getInstance ().b_update = true;

		invalidatelist ();
		clearlist ();

		//Messenger.RemoveListener( "invalidatelist", invalidatelist);

	}

	public void findwithFirstname(InputField _firstname)
	{
		Global_HeroData.getInstance ().findwithFirstname (_firstname.text);
		invalidatelist ();

	}

	public listselect_script ()
	{
		step = preftemp_list.Length;
	}


	public void Sort(int sorttype)
	{

		Global_HeroData.getInstance ().SortHeroList (sorttype);
		invalidatelist ();
	}
		

	public void beforelist()
	{
		listbegin -= step;

		if (listbegin <= 0) {

			listbegin = 0;
		}
		invalidatelist ();
	}

	public void afterlist()
	{

		listbegin += step;
		if (listbegin >=listcount) {

			listbegin -= step;
		}
		invalidatelist ();
	}

	public  void InitItems ()
	{
		for (int i = 0; i < preftemp_list.Length; i++) {

			preftemp_list[i] =GameObject.Instantiate (list_pref, list_pref.transform.position, list_pref.transform.rotation) as GameObject;
			preftemp_list[i].name = "herodata_listbutton (" + i+")";
			preftemp_list[i].transform.parent = this.transform;
			//preftemp_list [i].GetComponent<RelationShip_selectItem> ().hero_id = Global_const.NONE_ID;
			//preftemp_list[i].transform.localPosition = new Vector3(0, 190-i*37, 0);
			//preftemp_list[i].transform.localScale = new Vector3(1, 1, 1);
		}

	}
	 public void clearlist ()
	{
		for (int i = 0; i < preftemp_list.Length; i++) {

			ItemScript hero_item = preftemp_list[i].GetComponent<ItemScript>();

			hero_item.set_ID (Global_const.NONE_ID);

            hero_item.Invalidate();

        }
    }


	void OnEnable()
	{
		if (Global_HeroData.getInstance ().sortcount <= 0) {

			return;
		}
		
		GlobalData.getInstance ().b_update = false;

		listbegin = 0;

		if (inited == false) {
			inited = true;
			InitItems ();
		}
		invalidatelist();


	}



	public virtual void invalidatelist ()
	{


	}


}

