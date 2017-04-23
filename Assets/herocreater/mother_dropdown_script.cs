using UnityEngine;
using System.Collections;
using System;
using System.IO;  
using UnityEngine.UI;
using System.Collections.Generic;

public class mother_dropdown_script: MonoBehaviour
{
	public mother_dropdown_script ()
	{
	}



	public void Init()
	{
		herodata temphero = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid];

		//DataTable table = ds.Tables[0];
		this.GetComponent<Dropdown> ().ClearOptions();

		List<Dropdown.OptionData> templist=new List<Dropdown.OptionData>();

		Dropdown.OptionData temp_first = new Dropdown.OptionData ();
		temp_first.text = "无" + "." + -1;
		templist.Add (temp_first);

		for (int i = 0; i < Global_const.getInstance().MAXHEROS; i++) {

			herodata tempnowhero = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid];
			herodata tempmohterhero = Global_HeroData.getInstance ().List_hero [i];

			int bornyear_diff = tempnowhero.born_year - tempmohterhero.born_year;


			if (((bornyear_diff >= 15) && (bornyear_diff <= 40)) &&
				tempmohterhero.sex == 1) {
				Dropdown.OptionData temp = new Dropdown.OptionData ();
				temp.text = Global_HeroData.getInstance ().List_hero [i].GetAllName () + "." + i;

				templist.Add (temp);
			}
		}

		this.GetComponent<Dropdown> ().AddOptions (templist);

		int motherID=temphero.m_relationship.m_family.motherID;
		if (motherID != Global_const.NONE_ID) {

			this.GetComponent<Dropdown> ().captionText.text = "" + Global_HeroData.getInstance ().List_hero [motherID].GetAllName ()+"."+motherID;
		} else {
			this.GetComponent<Dropdown> ().captionText.text = "";
		}
	}

	public void Set()
	{
		herodata temphero = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid];

		if(this.GetComponent<Dropdown> ().captionText.text!="")
		{
			string [] fatherID = this.GetComponent<Dropdown> ().captionText.text.Split(new char[] { '.' });

			temphero.m_relationship.m_family.motherID=int.Parse(fatherID[1]);
		}
		else
		{
			temphero.m_relationship.m_family.motherID=-1;
		}


	}
}


