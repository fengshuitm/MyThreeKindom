using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class forageTextscript : MonoBehaviour {
	Text foragetext;
	// Use this for initialization
	void Start () {
		foragetext=this.GetComponent<Text>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foragetext.text =Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid].forage+"石";

	}
}
