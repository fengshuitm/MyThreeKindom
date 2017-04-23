using UnityEngine;
using System.Collections;
using System;
using System.IO;  
using UnityEngine.UI;
using System.Collections.Generic;

public class father_dropdown_script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
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
			herodata tempfatherhero = Global_HeroData.getInstance ().List_hero [i];

			int bornyear_diff = tempnowhero.born_year - tempfatherhero.born_year;

			bool samefirstnam=false;

			if (tempnowhero.heroname [0] == tempfatherhero.heroname [0]) {
				samefirstnam = true;
			}

			if (samefirstnam==true&&((bornyear_diff >= 15) && (bornyear_diff <= 60)) &&
				tempfatherhero.sex == 0) {
				Dropdown.OptionData temp = new Dropdown.OptionData ();
				temp.text = Global_HeroData.getInstance ().List_hero [i].GetAllName () + "." + i;

				templist.Add (temp);
			}
		}

		this.GetComponent<Dropdown> ().AddOptions (templist);


		int fatherID=temphero.m_relationship.m_family.fatherID;
		if (fatherID != Global_const.NONE_ID) {
			this.GetComponent<Dropdown> ().captionText.text = "" + Global_HeroData.getInstance ().List_hero [fatherID].GetAllName ()+"."+fatherID;
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

			temphero.m_relationship.m_family.fatherID=int.Parse(fatherID[1]);
		}
		else
		{
			temphero.m_relationship.m_family.fatherID=-1;
		}


	}
}
