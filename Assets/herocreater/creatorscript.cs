using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class creatorscript : UI_OBJ {

	public GameObject global_cam;

	public GameObject [] part1_inputfields;
	public GameObject [] part2_inputfields;
	public GameObject [] part3_inputfields;
	public InputField [] part4_inputfields;


	public GameObject head_img;
	borndata tempborndata = new borndata ();
	Workdata tempworkdata = new Workdata ();

	// Use this for initialization
	void Start () {
		this.GetComponent<UI_OBJ>().moveout();

        Messenger.AddListener<int>("Selecthero",Show);

        
    }

    // Update is called once per frame
    void Update () {
	
	}

    void Show(int _id)
    {
        GlobalData.getInstance().nowheroid = _id;

        this.GetComponent<UI_OBJ>().move();

    }

/*	public void move()
	{
		this.gameObject.SetActive(true);
		//this.OnEnable ();
		this.transform.localPosition =new Vector3(0,0,0);

	}
*/
	void OnEnable()
	{

		herodata temphero = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid];

		head_img.GetComponent<Image> ().sprite = Global_source_loader.getInstance ().hero_L_face [GlobalData.getInstance ().nowheroid];

		//part1
		part1_inputfields[0].GetComponent<InputField>().text= temphero.heroname[0];
		part1_inputfields[1].GetComponent<InputField>().text= temphero.heroname[1];
		part1_inputfields [2].GetComponent<InputField> ().text = temphero.herozi;
		part1_inputfields [3].GetComponent<InputField> ().text = temphero.herotitle;
		part1_inputfields [4].GetComponent<InputField> ().text = temphero.herooldname;

		int sexint = temphero.sex;
		if (sexint == 0) {
			part1_inputfields [5].GetComponent<Dropdown> ().captionText.text = "男";
		} else {
			part1_inputfields [5].GetComponent<Dropdown> ().captionText.text = "女";

		}

		//part2
		part2_inputfields[0].GetComponent<InputField>().text=""+temphero.id;
		part2_inputfields[1].GetComponent<InputField> ().text=""+temphero.m_junwang_zhou;
		part2_inputfields[2].GetComponent<InputField> ().text=""+temphero.m_junwang_jun;
		part2_inputfields[3].GetComponent<InputField> ().text=""+temphero.m_junwang_xian;
		//part2_inputfields [4].GetComponent<father_dropdown_script> ().OnEnable ();
		//part2_inputfields [5].GetComponent<mother_dropdown_script> ().OnEnable ();
		part2_inputfields [4].GetComponent<Dropdown> ().captionText.text = ""+temphero.NOWimportanceID;

		//part3
		part3_inputfields [0].GetComponent<InputField>().text =""+ temphero.born_year;
		part3_inputfields [1].GetComponent<InputField>().text =""+ temphero.die_year;
		part3_inputfields [2].GetComponent<InputField>().text =""+ temphero.born_year_fantasy;
		part3_inputfields [3].GetComponent<InputField>().text =""+ temphero.die_year_fantasy;
		part3_inputfields [4].GetComponent<InputField>().text =""+ temphero.die_reason;
		part3_inputfields [5].GetComponent<InputField>().text =""+ temphero.biography;
		part3_inputfields [6].GetComponent<InputField> ().text =""+ temphero.born;
		part3_inputfields [7].GetComponent<InputField> ().text =""+ temphero.occupation;
		part3_inputfields [8].GetComponent<InputField> ().text =""+ temphero.idea;
		part3_inputfields [9].GetComponent<InputField> ().text =""+ temphero.officeposition;
		part3_inputfields [10].GetComponent<InputField> ().text =""+ temphero.militaryposition;
		part3_inputfields [11].GetComponent<InputField> ().text =""+ temphero.title;

		/*	temphero.lead=int.Parse( part4_inputfields [0].text);
			temphero.might =int.Parse( part4_inputfields [1].text);
			temphero.wit =int.Parse( part4_inputfields [2].text);
			temphero.polity =int.Parse( part4_inputfields [3].text);
			temphero.charm =int.Parse( part4_inputfields [4].text);
			temphero.luck =int.Parse( part4_inputfields [5].text);
			temphero.reputation =int.Parse( part4_inputfields [6].text);
			temphero.bad_reputation =int.Parse( part4_inputfields [7].text);

			temphero.skill[0] = part4_inputfields [8].text;
			temphero.skill[1] = part4_inputfields [9].text;
			temphero.skill[2] = part4_inputfields [10].text;
			temphero.skill[3] = part4_inputfields [11].text;
			temphero.skill[4] = part4_inputfields [12].text;
			temphero.skill[5] = part4_inputfields [13].text;
			temphero.S_skill[0] = part4_inputfields [14].text;
			temphero.S_skill[1] = part4_inputfields [15].text;

			temphero.money =int.Parse( part4_inputfields [16].text);
			temphero.forage =int.Parse( part4_inputfields [17].text);
			temphero.guardunit.style =part4_inputfields [18].text;
			temphero.guardunit.Armycount =int.Parse( part4_inputfields [19].text);
		*/

		//part4
		part4_inputfields [0].text =""+ temphero.lead;
		part4_inputfields [1].text =""+ temphero.might;
		part4_inputfields [2].text =""+ temphero.wit;
		part4_inputfields [3].text =""+ temphero.polity;
		part4_inputfields [4].text =""+ temphero.charm;
		part4_inputfields [5].text =""+ temphero.luck;
		part4_inputfields [6].text =""+ temphero.reputation;
		part4_inputfields [7].text =""+ temphero.bad_reputation;

		part4_inputfields [8].text =""+ temphero.skill[0];
		part4_inputfields [9].text =""+ temphero.skill[1];
		part4_inputfields [10].text =""+ temphero.skill[2];
		part4_inputfields [11].text =""+ temphero.skill[3];
		part4_inputfields [12].text =""+ temphero.skill[4];
		part4_inputfields [13].text =""+ temphero.skill[5];
		part4_inputfields [14].text =""+ temphero.S_skill[0];
		part4_inputfields [15].text =""+ temphero.S_skill[1];

		part4_inputfields [16].text =""+ temphero.money;
		part4_inputfields [17].text =""+ temphero.forage;
		part4_inputfields [18].text =""+ Global_GuardunitData.getInstance().List_guardunit[temphero.id].style;
		part4_inputfields [19].text =""+ Global_GuardunitData.getInstance().List_guardunit[temphero.id].Armycount;

	}

	public void save()
	{
		herodata temphero = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid];

		try
		{
			//part1
			temphero.heroname[0]=part1_inputfields[0].GetComponent<InputField>().text;
			temphero.heroname[1]=part1_inputfields[1].GetComponent<InputField>().text;
			temphero.herozi=part1_inputfields[2].GetComponent<InputField>().text;
			temphero.herotitle=part1_inputfields[3].GetComponent<InputField>().text;
			temphero.herooldname=part1_inputfields[4].GetComponent<InputField>().text;
			temphero.sex=part1_inputfields [5].GetComponent<Dropdown> ().value;

			//part2

			temphero.m_junwang_zhou=part2_inputfields[1].GetComponent<InputField> ().text;
			temphero.m_junwang_jun=part2_inputfields[2].GetComponent<InputField> ().text;
			temphero.m_junwang_xian=part2_inputfields[3].GetComponent<InputField> ().text;

		/*	if(part2_inputfields[4].GetComponent<Dropdown> ().captionText.text!="")
			{
				string [] fatherID = part2_inputfields[4].GetComponent<Dropdown> ().captionText.text.Split(new char[] { '.' });

				temphero.m_relationship.m_family.fatherID=int.Parse(fatherID[1]);
			}
			else
			{
				temphero.m_relationship.m_family.fatherID=-1;
			}

			if(part2_inputfields[5].GetComponent<Dropdown> ().captionText.text!="")
			{
				string [] motherID = part2_inputfields[5].GetComponent<Dropdown> ().captionText.text.Split(new char[] { '.' });

				temphero.m_relationship.m_family.motherID=int.Parse(motherID[1]);
			}
			else
			{
				temphero.m_relationship.m_family.motherID=-1;
			}
	*/
			if(part2_inputfields[4].GetComponent<Dropdown> ().captionText.text!="")
			{

				temphero.NOWimportanceID=part2_inputfields[4].GetComponent<Dropdown> ().value;
			
			}
			else
			{
				temphero.NOWimportanceID=Global_const.NONE_ID;
			}

			//part3
			temphero.born_year=int.Parse( part3_inputfields[0].GetComponent<InputField> ().text);
			temphero.die_year=int.Parse( part3_inputfields[1].GetComponent<InputField> ().text);
			temphero.born_year_fantasy=int.Parse( part3_inputfields[2].GetComponent<InputField> ().text);
			temphero.die_year_fantasy=int.Parse( part3_inputfields[3].GetComponent<InputField> ().text);
			temphero.die_reason=part3_inputfields[4].GetComponent<InputField> ().text;
			temphero.biography=part3_inputfields[5].GetComponent<InputField> ().text;
			temphero.born=part3_inputfields[6].GetComponent<InputField> ().text;
			temphero.occupation=part3_inputfields[7].GetComponent<InputField> ().text;
			temphero.idea=part3_inputfields[8].GetComponent<InputField> ().text;
			temphero.officeposition=part3_inputfields[9].GetComponent<InputField> ().text;
			temphero.militaryposition=part3_inputfields[10].GetComponent<InputField> ().text;
			temphero.title=part3_inputfields[11].GetComponent<InputField> ().text;

			//part4
			temphero.lead=int.Parse( part4_inputfields [0].text);
			temphero.might =int.Parse( part4_inputfields [1].text);
			temphero.wit =int.Parse( part4_inputfields [2].text);
			temphero.polity =int.Parse( part4_inputfields [3].text);
			temphero.charm =int.Parse( part4_inputfields [4].text);
			temphero.luck =int.Parse( part4_inputfields [5].text);
			temphero.reputation =int.Parse( part4_inputfields [6].text);
			temphero.bad_reputation =int.Parse( part4_inputfields [7].text);

			temphero.skill[0] = part4_inputfields [8].text;
			temphero.skill[1] = part4_inputfields [9].text;
			temphero.skill[2] = part4_inputfields [10].text;
			temphero.skill[3] = part4_inputfields [11].text;
			temphero.skill[4] = part4_inputfields [12].text;
			temphero.skill[5] = part4_inputfields [13].text;
			temphero.S_skill[0] = part4_inputfields [14].text;
			temphero.S_skill[1] = part4_inputfields [15].text;

			temphero.money =int.Parse( part4_inputfields [16].text);
			temphero.forage =int.Parse( part4_inputfields [17].text);
            Global_GuardunitData.getInstance().List_guardunit[temphero.id].style =part4_inputfields [18].text;
            Global_GuardunitData.getInstance().List_guardunit[temphero.id].Armycount =int.Parse( part4_inputfields [19].text);

            //temphero.chronology=part4_inputfields [15].text;
            //StartCoroutine(Global_XML_IO.getInstance().Save_hero_Xml());
			global_cam.GetComponent<herocreater_camera> ().SaveHeroList ();
		
		}
		catch {


		}

		Global_HeroData.getInstance ().listTotemp ();

	}

	public void save_all()
	{
		save ();

		global_cam.GetComponent<herocreater_camera> ().SaveHeroList ();

	}

	public void lasthero()
	{
		GlobalData.getInstance ().nowheroid -= 1;

		if (GlobalData.getInstance ().nowheroid <= 0) {
			GlobalData.getInstance ().nowheroid = 0;
		}

		OnEnable ();
	}

	public void nexthero()
	{
		GlobalData.getInstance ().nowheroid += 1;

		if (GlobalData.getInstance ().nowheroid >= Global_const.getInstance().MAXHEROS) {
			GlobalData.getInstance ().nowheroid = (Global_const.getInstance().MAXHEROS-1);
		}

		OnEnable ();
	}

	public override void Invalidate ()
	{

	}
}
