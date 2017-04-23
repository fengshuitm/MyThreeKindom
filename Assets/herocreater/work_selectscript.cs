using UnityEngine;
using System.Collections;
using System;
using System.IO;  
using UnityEngine.UI;
using System.Collections.Generic;

public class work_selectscript : MonoBehaviour {

	Workdata tempWorkdata=new Workdata();

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

		for (int i = 0; i <tempWorkdata.work_listName.Length; i++) {



			Dropdown.OptionData temp = new Dropdown.OptionData ();
			temp.text =tempWorkdata.work_listName[i];

			templist.Add (temp);
		}

		this.GetComponent<Dropdown> ().AddOptions (templist);

	}
}
