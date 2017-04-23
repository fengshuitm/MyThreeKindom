using UnityEngine;
using System.Collections;

public class Event_Spirite : MonoBehaviour {

	// Use this for initialization
	int count=100;
	public int eventspirite=3;
	Animator tempanim;

	public bool b_forever=false;

	void Start () {
		tempanim = GetComponent<Animator> ();
		tempanim.SetInteger ("statue", 0);

		Global_events.getInstance().Event_anim=GameObject.Find("Main_Camera/Event_Anim");
		Global_events.getInstance().Event_Text_Middle=GameObject.Find("Main_Camera/Event_Anim/Event_Text_Middle");
		Global_events.getInstance().Event_Text_Up=GameObject.Find("Main_Camera/Event_Anim/Event_Text_Up");
		Global_events.getInstance().Event_Text_Down=GameObject.Find("Main_Camera/Event_Anim/Event_Text_Down");
		Global_events.getInstance().Event_Hero1=GameObject.Find("Main_Camera/Event_Anim/Event_Hero1");
		Global_events.getInstance().Event_Hero2=GameObject.Find("Main_Camera/Event_Anim/Event_Hero2");

		////.SetInteger ("statue", 0);
		//tempanim.SetInteger ("statue", 0);
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.localScale = new Vector3 (GlobalData.getInstance ().cam_size/2.0f, GlobalData.getInstance ().cam_size/2.0f, 1);
	
		if (false == b_forever) {
			if (true == tempanim.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {

				GlobalData.getInstance ().b_update = true;

				this.gameObject.SetActive (false);

			}
		}


		/*if(count--<0)
		{
			count = 0;
			this.gameObject.SetActive (false);

		}
		*/
	}

	void OnEnable()
	{
		count=100;
		//GlobalData.getInstance ().b_update = false;

	}

	public void setanim(int _animID)
	{
		tempanim.SetInteger ("statue", _animID);

	}

	public void move()
	{
		this.gameObject.SetActive(true);

		this.transform.localPosition =new Vector3(0,0,0);

	}
}
