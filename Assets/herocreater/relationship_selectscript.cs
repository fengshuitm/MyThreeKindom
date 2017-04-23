using UnityEngine;
using System.Collections;

public class relationship_selectscript : listselect_script {

	public override void invalidatelist()
	{
		listcount =Global_HeroData.getInstance().sortcount;

		//int [] tempheroiddata;
		clearlist ();

		for (int i = 0; i < preftemp_list.Length; i++) {

			int i_id_flag = i + listbegin;
			//int i_id = 0;

			if (i_id_flag >= listcount) {
				return;
			}

			//tempheroiddata=Global_HeroData.getInstance().List_hero;

			//i_id = tempheroiddata [i_id_flag];

			RelationShip_selectItem hero_item = preftemp_list[i].GetComponent<RelationShip_selectItem>();
			hero_item.set_ID (i_id_flag);
		}


	}


}
