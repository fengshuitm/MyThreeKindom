using UnityEngine;
using System.Collections;

public class wife_selectscript: listselect_script
{
	public wife_selectscript ()
	{
	}

	public  void invalidatelist()
	{
		//int [] tempheroiddata;
		clearlist ();

		for (int i = 0; i < 10; i++) {

			int i_id_flag = i + listbegin;
			//int i_id = 0;

			if (i_id_flag >= listcount) {
				return;
			}

			//tempheroiddata=Global_HeroData.getInstance().List_hero;

			//i_id = tempheroiddata [i_id_flag];

			hero_selectscript_item hero_item = preftemp_list[i].GetComponent<hero_selectscript_item>();

			hero_item.set_ID (i_id_flag);
		}


	}
}

