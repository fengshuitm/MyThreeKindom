using UnityEngine;
using System.Collections;

public class hero_selectscript : listselect_script {

	void Awake()
	{
		search_items = GameObject.Find ("Canvas/heroselectpanl/search_items");
        //Hidesearch_items();
        Messenger.AddListener<int>("Selecthero", Selecthero);

        //DontDestroyOnLoad(this.transform.parent);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<int>("Selecthero", Selecthero);

    }

    public void Selecthero(int _id)
    {
        if(_id!=Global_const.NONE_ID)
        {
            this.transform.parent.GetComponent<UI_OBJ>().moveout();
        }

    }

    public void Hidesearch_items()
	{

        search_items.GetComponent<UI_OBJ>().moveout();


    }

	public  override void invalidatelist()
	{
        clearlist();

        listcount = Global_HeroData.getInstance().sortcount;

		//int [] tempheroiddata;

		for (int i = 0; i < preftemp_list.Length; i++) {

			int i_id_flag = i + listbegin;
			//int i_id = 0;

			if (i_id_flag >= listcount) {
				return;
			}

			//tempheroiddata=Global_HeroData.getInstance().List_hero;

			//i_id = tempheroiddata [i_id_flag];

			ItemScript hero_item = preftemp_list[i].GetComponent<ItemScript>();
			hero_item.set_ID (i_id_flag);


        }

	}
		
}
