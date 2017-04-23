using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class nametextscript : MonoBehaviour {

	Text nametext;

	// Use this for initialization
	void Start () {
		nametext=this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		nametext.text = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid].GetAllName ();
	}
}
