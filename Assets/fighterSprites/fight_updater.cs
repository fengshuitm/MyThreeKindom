 using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class fight_updater : MonoBehaviour {

	public GameObject face_left;
	public GameObject face_right;

	public GameObject name_left;
	public GameObject name_right;

	public GameObject might_left;
	public GameObject might_right;

	public GameObject hp_scroll_left;
	public GameObject hp_scroll_right;

	public GameObject scroll_colddown;

	public GameObject anim_left_controler;
	public GameObject anim_right_controler;

	public GameObject winsprite;
	public GameObject losesprite;

	int [] cardleft=new int[5];//玩家卡数
	int [] cardright=new int[5];//玩家卡数

	public GameObject [] leftcards;
	public GameObject [] rightcards;

	int timecolddown=0;

	int timeend=100;

	const float MAXTIME = 100.0f;
	const float MAXLIFE = 100.0f;
	readonly string [] actionname_stringlist={"牵制","中斩","重斩","断头斩","闪电斩","无双","拖刀计","螺旋攻击","防御"};


	public bool end=false;

	// Use this for initialization
	void Start () {
		//actionname_stringlist={"牵制","中斩","重斩","断头斩","闪电斩","无双","拖刀计","螺旋攻击","防御"};

		face_left = GameObject.Find ("Canvas/border_left/Image");
		face_right = GameObject.Find ("Canvas/border_right/Image");

		name_left = GameObject.Find ("Canvas/border_left/Text_name");
		name_right = GameObject.Find ("Canvas/border_right/Text_name");

		might_left=GameObject.Find ("Canvas/border_left/Text_might");
		might_right=GameObject.Find ("Canvas/border_right/Text_might");

		Init ();
		regetcard ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		if (end == false) {
			timecolddown--;
			if (timecolddown <= 0) {
				timecolddown = 0;

				SetHp ();

			}

			scroll_colddown.GetComponent<Scrollbar> ().value = timecolddown / MAXTIME;
		} else {

			timeend--;
			if (timeend <= 0) {
				timeend = 100;

				Global_events.getInstance ().re_init = true;

				SceneManager.LoadScene("three kindom 3d");

			}
				


		}

	}

	public void Updatedata(int cardid)
	{
        Global_fighter_Sprites fighertemp = Global_fighter_Sprites.getInstance();

        if (timecolddown == 0&&end == false) {
		
			timecolddown = (int)MAXTIME;


			int actionid = cardleft [cardid];
			int secussroll = 10;
			//随机数
			//0 牵制  1 中斩  2  重斩  3  断头斩   4  闪电斩   5  无双   6  拖刀计   7   螺旋攻击  8  防御
			string actiontemp = null;

			switch (actionid) {
			
			case 0:
				actiontemp = "left_ac10";
				fighertemp.righthero_hp -= ((fighertemp.lefthero.might/10)/2);

				break;
			case 1:
				actiontemp = "left_ac12";

				secussroll = Random.Range (0, 10);

				if (secussroll <7) {

					fighertemp.righthero_hp -= (fighertemp.righthero.might/10);

				}

				break;
			case 2:
				actiontemp = "left_ac8";

				secussroll = Random.Range (0, 10);

				if (secussroll < 7) {

					fighertemp.righthero_hp -= (fighertemp.lefthero.might / 10)*3/2;

				} else {
					fighertemp.righthero_hp -= (fighertemp.lefthero.might / 10);
					fighertemp.lefthero_hp -=  (fighertemp.righthero.might / 10);
				}
				break;
			case 3:
				actiontemp = "left_ac2";
				secussroll = Random.Range (0, 10);
				if (secussroll < 5) {

					fighertemp.righthero_hp -= 40;

				} else {
					fighertemp.righthero_hp -= ((fighertemp.lefthero.might/10)/2);

				}

				break;
			case 4:
				actiontemp = "left_ac3";

				secussroll = Random.Range (0, 10);
				if (secussroll < 7) {

					fighertemp.righthero_hp -= (fighertemp.lefthero.might / 10)*3/2;

				} else {
					fighertemp.righthero_hp -= ((fighertemp.lefthero.might/10)/2);

				}

				break;
			case 5:
				actiontemp = "left_ac6";


				fighertemp.righthero_hp -= (fighertemp.lefthero.might / 10)*3;

				break;
			case 6:
				actiontemp = "left_ac9";
				secussroll = Random.Range (0, 10);
				if (secussroll < 3) {

					fighertemp.righthero_hp -= 50;

				} else {

				}
				break;
			case 7:
				actiontemp = "left_ac7";
				fighertemp.righthero_hp -= (fighertemp.lefthero.might / 10)*2;

				break;
			case 8:
				actiontemp = "left_ac5";
				fighertemp.lefthero_hp += 10;

				break;

			}

			anim_left_controler.GetComponent<Animator> ().Play (actiontemp);

			int actionid_AI = RollAIACTION ();

			//0 牵制  1 中斩  2  重斩  3  断头斩   4  闪电斩   5  无双   6  拖刀计   7   螺旋攻击  8  防御
			switch (actionid_AI) {

			case 0:
				actiontemp = "right_ac14";
				fighertemp.lefthero_hp -= ((fighertemp.righthero.might/10)/2);

				break;
			case 1:
				actiontemp = "right_ac6";

				secussroll = Random.Range (0, 10);

				if (secussroll <7) {

					fighertemp.lefthero_hp -= (fighertemp.righthero.might/10);

				}

				break;
			case 2:
				actiontemp = "right_ac12";

				secussroll = Random.Range (0, 10);

				if (secussroll < 7) {

					fighertemp.lefthero_hp -= (fighertemp.righthero.might / 10)*3/2;

				} else {
					fighertemp.lefthero_hp -= (fighertemp.righthero.might / 10);
					fighertemp.righthero_hp -=  (fighertemp.lefthero.might / 10);
				}

				break;
			case 3:
				actiontemp = "right_ac2";
				secussroll = Random.Range (0, 10);
				if (secussroll < 5) {

					fighertemp.lefthero_hp -= 40;

				} else {
					fighertemp.lefthero_hp -= ((fighertemp.righthero.might/10)/2);

				}

				break;
			case 4:
				actiontemp = "right_ac3";
			
				secussroll = Random.Range (0, 10);
				if (secussroll < 7) {

					fighertemp.lefthero_hp -= (fighertemp.righthero.might / 10)*3/2;

				} else {
					fighertemp.lefthero_hp -= ((fighertemp.righthero.might/10)/2);

				}
				break;
			case 5:
				actiontemp = "right_ac9";
				secussroll = Random.Range (0, 10);

				fighertemp.lefthero_hp -= (fighertemp.righthero.might / 10)*3;

				break;
			case 6:
				actiontemp = "right_ac13";
				secussroll = Random.Range (0, 10);
				if (secussroll < 3) {

					fighertemp.lefthero_hp -= 50;

				} else {

				}
				break;
			case 7:
				actiontemp = "right_ac11";
				fighertemp.lefthero_hp -= (fighertemp.righthero.might / 10)*2;

				break;
			case 8:
				actiontemp = "right_ac5";
				fighertemp.righthero_hp += 10;

				break;

			}
			//Debug.Log (actiontemp);

			anim_right_controler.GetComponent<Animator> ().Play (actiontemp);


			/*switch (_action) {

		case 2://断头斩
			fighertemp.righthero_hp -= ((fighertemp.lefthero_might/10)*3/2);
			break;
		case 3://闪电斩
			fighertemp.righthero_hp -= ((fighertemp.lefthero_might/10)*3/2);
			break;
		case 5://防御
			//fighertemp.lefthero_hp += 10;
			break;
		case 6://无双
			fighertemp.righthero_hp -= ((fighertemp.lefthero_might/10)*3);
			break;
		case 7://螺旋攻击
			fighertemp.righthero_hp -= ((fighertemp.lefthero_might/10)*3/2);
			break;
		case 8://重斩
			fighertemp.righthero_hp -= ((fighertemp.lefthero_might/10)*2);
			break;
		case 9://拖刀计
			fighertemp.righthero_hp -= ((fighertemp.lefthero_might/10)*3);
			break;
		case 10://牵制
			fighertemp.righthero_hp -= ((fighertemp.lefthero_might/10)/2);
			break;
		case 12://中斩
			fighertemp.righthero_hp -= (fighertemp.lefthero_might/10);
			break;
		}
		*/


			cardleft [cardid] = -1;
			regetcard ();
		
		}
		//Debug.Log (hp_scroll_right.GetComponent<Scrollbar> ().value);
	}

	public void Init()
	{
		//战斗采取抽牌制，每次发5张牌，使用后补充
		end=false;
		timeend=100;

		winsprite.SetActive (false);
		losesprite.SetActive (false);



		face_left.GetComponent<Image>().sprite=Global_source_loader.getInstance().hero_L_face[Global_fighter_Sprites.getInstance().lefthero.id];
		face_right.GetComponent<Image>().sprite=Global_source_loader.getInstance().hero_L_face[Global_fighter_Sprites.getInstance().righthero.id];

		name_left.GetComponent<Text> ().text = Global_fighter_Sprites.getInstance ().lefthero.GetAllName ();
		name_right.GetComponent<Text> ().text = Global_fighter_Sprites.getInstance ().righthero.GetAllName ();

		might_left.GetComponent<Text> ().text = ""+Global_fighter_Sprites.getInstance ().lefthero.might;
		might_right.GetComponent<Text> ().text = ""+Global_fighter_Sprites.getInstance ().righthero.might;

		for (int i = 0; i < cardleft.Length; i++) {

			cardleft [i] = -1;
		}

		for (int i = 0; i < cardright.Length; i++) {

			cardright [i] = -1;
		}


	}

	public void regetcard()
	{
		for (int i = 0; i < cardleft.Length; i++) {

			if (cardleft [i] == -1) {
				cardleft [i] = Random.Range (0, 9);
			}

			leftcards [i].GetComponent<Text> ().text =actionname_stringlist[cardleft [i]];
		}

		for (int i = 0; i < cardright.Length; i++) {

			if (cardright [i] == -1) {
				cardright [i] = Random.Range (0, 9);
			}

			rightcards [i].GetComponent<Text> ().text =actionname_stringlist[cardright[i]];
		}



	}

	private int RollAIACTION ()
	{
		int rollid = Random.Range (0, 5);
		int rollaction = cardright [rollid];

		cardright [rollid] = -1;

		//Debug.Log (rollid);

		regetcard ();

		return rollaction;

	}


	private void SetHp ()
	{
        Global_fighter_Sprites fighertemp = Global_fighter_Sprites.getInstance();

        string actiontemp;
		if (fighertemp.lefthero_hp <= 0) {
			fighertemp.lefthero_hp = 0;
			//玩家失败
			actiontemp="left_ac11";
			anim_left_controler.GetComponent<Animator> ().Play (actiontemp);

			actiontemp="right_ac4";
			anim_right_controler.GetComponent<Animator> ().Play (actiontemp);

			losesprite.SetActive (true);
			winsprite.SetActive (false);

			end = true;

			//return;
		}

		if (fighertemp.righthero_hp <= 0) {
			fighertemp.righthero_hp = 0;
			//玩家胜利
			actiontemp="left_ac4";
			anim_left_controler.GetComponent<Animator> ().Play (actiontemp);

			actiontemp="right_ac16";
			anim_right_controler.GetComponent<Animator> ().Play (actiontemp);
			winsprite.SetActive (true);
			losesprite.SetActive (false);

			end = true;
			//return;
			fighertemp.righthero=null;//.now_statue=
		}


		hp_scroll_left.GetComponent<Scrollbar> ().size = fighertemp.lefthero_hp / MAXLIFE;
		hp_scroll_right.GetComponent<Scrollbar> ().size = fighertemp.righthero_hp / MAXLIFE;

	}

}
