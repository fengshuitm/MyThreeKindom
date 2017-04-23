using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class image_text_hero : UI_OBJ {

	public Image head_image;
	public Text meet_text;
	public int m_hero1_id=Global_const.NONE_ID;
	//int tempcount=0;

	// Use this for initialization
	void Start () {
		Global_events.getInstance ().Event_hero_text = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{



	/*	tempcount++;

		try
		{
			if(tempcount==500)
			{
				this.gameObject.SetActive(false);
				tempcount=0;
			}

		}
		catch {

		}
	*/
	}

	public override void Invalidate ()
	{

	}
}
