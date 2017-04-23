using UnityEngine;
using System.Collections;
using System;
using System.IO;  
using UnityEngine.UI;
using System.Collections.Generic;

public class importance_dropdown_script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnEnable()
	{

		//DataTable table = ds.Tables[0];
		this.GetComponent<Dropdown> ().ClearOptions();

		List<Dropdown.OptionData> templist=new List<Dropdown.OptionData>();

		//for (int i = 0; i < Global_ImportanceData.getInstance().importance_jun.Length; i++) {

		foreach(Importance tempstrlist in Global_ImportanceData.getInstance().List_importance)
			{

					Dropdown.OptionData temp = new Dropdown.OptionData ();
					temp.text =""+tempstrlist.id;

					templist.Add (temp);
				
			}
		//}

		this.GetComponent<Dropdown> ().AddOptions (templist);

	}
}
