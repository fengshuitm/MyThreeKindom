using UnityEngine;
using System.Collections;

public class KindomList : listselect_script {



	public override void invalidatelist()
	{
		listcount =Global_const.getInstance().MAXKINDOM_COUNT;

		//int [] tempheroiddata;
		clearlist ();

		for (int i = 0; i < preftemp_list.Length; i++) {

			int i_id_flag = i + listbegin;
			//int i_id = 0;

			if (i_id_flag >= listcount) {
				return;
			}


			ItemScript kindom_item = preftemp_list[i].GetComponent<ItemScript>();
			kindom_item.set_ID (i_id_flag);
		}

	}

}
