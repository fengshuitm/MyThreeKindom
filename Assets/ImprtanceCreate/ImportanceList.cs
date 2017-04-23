using UnityEngine;
using System.Collections;

public class ImportanceList : listselect_script {


	public override void invalidatelist()
	{
		listcount =Global_const.MAXIMPORTANCE;

		clearlist ();

		for (int i = 0; i < preftemp_list.Length; i++) {

			int i_id_flag = i + listbegin;

			if (i_id_flag >= listcount) {
				return;
			}


			ItemScript Importance_item = preftemp_list[i].GetComponent<ItemScript>();
			Importance_item.set_ID (i_id_flag);
		}

	}
}
