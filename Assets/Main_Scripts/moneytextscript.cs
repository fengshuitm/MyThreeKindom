using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class moneytextscript : MonoBehaviour {
	Text moneytext;

	// Use this for initialization
	void Start () {
		moneytext=this.GetComponent<Text>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		moneytext.text =Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid].money+"两";

	}
}
