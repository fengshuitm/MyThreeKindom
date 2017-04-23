using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemScript :MonoBehaviour
{

	public Text [] Text_item;
	public int id=Global_const.NONE_ID;

	public ItemScript ()
	{
	}

	public void clear()
	{
		for (int i = 0; i < Text_item.Length; i++) {

			if (Text_item [i] != null) {
				Text_item [i].text = "";
			}
		}

	}

	public bool set_ID(int _id)
	{

		//hero_id=Global_HeroData.getInstance ().SortHeroList [_id];
		id = _id;

		if (id==Global_const.NONE_ID||id<0) {

			clear ();
			return false;
		}


		return Invalidate ();
	}

	virtual public bool Invalidate ()
	{

		return true;
	}
}

