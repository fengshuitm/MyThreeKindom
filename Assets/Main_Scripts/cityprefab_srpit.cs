using UnityEngine;
using System.Collections;

public class cityprefab_srpit : MonoBehaviour
{

	public GameObject flagspirit;
	public GameObject flagspiritanim;

	//public GameObject messagebox;
	public GameObject defandarmy;
	public GameObject importanceName;

	//public GameObject importanceData;

	int old_flagID=Global_const.NONE_ID;

//	public int importanceid;

	// Use this for initialization
	int importanceid=Global_const.NONE_ID;

	void Start () {



		/*GameObject tempanim = flagspiritanim.transform.GetChild (0).gameObject;

		GameObject flagspiritnew = (GameObject)Instantiate (tempanim, flagspirit.transform.position, flagspirit.transform.rotation);
		flagspiritnew.transform.parent = this.transform;
		flagspiritnew.name = flagspirit.name;
		flagspiritnew.transform.localScale = flagspirit.transform.localScale;


		Destroy (flagspirit);
		*/

		//tempflag = Resources.Load ("flags\flag_0.png") as Texture2D;

		//tempflag.transform.SetParent (this.transform);

		importanceid = int.Parse(this.name);




        importanceName.GetComponent<TextMesh>().text = "";
       
        string tempname= Global_ImportanceData.getInstance().List_importance[importanceid].M_name;

        for(int i=0;i<tempname.Length;i++)
        {
            importanceName.GetComponent<TextMesh>().text += tempname.Substring(i, 1) + "\n";

        }


		int KindomID_temp = Global_ImportanceData.getInstance ().List_importance [importanceid].GetKindomID();

		if(KindomID_temp!=Global_const.NONE_ID)
		{
			ChangeFlag (KindomID_temp);
		}
	}


    private void Awake()
    {
        flagspirit= this.transform.FindChild("flag_0_0").gameObject;
        importanceName= this.transform.FindChild("importancename").gameObject;
        Messenger.AddListener("UPDATEALL", UpdateAll);

    }


    private void OnDestroy()
    {
        Messenger.RemoveListener("UPDATEALL", UpdateAll);

    }

    // Update is called once per frame
    void FixedUpdate () {


	}

   void UpdateAll()
    {
        //flagspirit.transform.localScale =new Vector3(GlobalData.getInstance ().cam_lev,GlobalData.getInstance ().cam_lev,1);

        int leaderid = Global_ImportanceData.getInstance().List_importance[importanceid].LeaderID;


        //没有太守
        if (leaderid == Global_const.NONE_ID)
        {
            flagspirit.gameObject.SetActive(false);
            old_flagID = Global_const.NONE_ID;
        }
        else
        {

            //有太守
            //获得当前太守势力id
            int kindomid = Global_HeroData.getInstance().List_hero[leaderid].m_relationship.belong_kindom;

            if (kindomid >= Global_const.getInstance().MAXKINDOM_COUNT)
            {

                return;
            }

            //如果当前势力不同于之前势力，则变换旗帜
            if (kindomid != old_flagID)
            {
                ChangeFlag(kindomid);
                old_flagID = kindomid;

            }
        }
        //Debug.Log ("" + this.gameObject.name);

        if (importanceid != Global_const.NONE_ID)
        {
            switch (Global_ImportanceData.getInstance().List_importance[importanceid].battle_statue)
            {
                case 0:

                    break;
                case 1:

                    break;
                case 2:
                    defaction(Global_ImportanceData.getInstance().List_importance[importanceid].def_count);
                    Global_ImportanceData.getInstance().List_importance[importanceid].battle_statue = 0;
                    Global_ImportanceData.getInstance().List_importance[importanceid].def_count = 0;
                    break;
            }
        }

        int defenderid = int.Parse(Global_ImportanceData.getInstance().List_importance[importanceid].this_Elem.GetAttribute("守将"));

        defandarmy = this.transform.FindChild("armytext").gameObject;

        TextMesh temptext = defandarmy.GetComponent<TextMesh>();

        if (Global_const.NONE_ID == defenderid)
        {
            temptext.text = "无守将\n";

        }
        else
        {
            temptext.text = Global_HeroData.getInstance().List_hero[defenderid].GetAllName() + "\n";
        }

        if (Global_ImportanceData.getInstance().List_importance[importanceid].defenseNOW > 0)
        {
            temptext.color = new Color(0, 0.5f, 0);

            temptext.text += "城防" + Global_ImportanceData.getInstance().List_importance[importanceid].defenseNOW;

        }
        else
        {
            temptext.color = new Color(0.5f, 0.5f, 0);

            if (defenderid != Global_const.NONE_ID)
            {

                temptext.text += "兵力" + Global_GuardunitData.getInstance().List_guardunit[defenderid].Armycount;
            }
            else
            {
                temptext.text = "空城";
            }
        }

    }

    protected void ChangeFlag(int _flagID)
    {
        if (_flagID == Global_const.NONE_ID)
        {
            return;
        }

        flagspirit.GetComponent<SpriteRenderer>().sprite = Global_source_loader.getInstance().Flag_Sprite[_flagID];// Sprite.Create(tex,new Rect(0,6*Flagsprite.rect.height,Flagsprite.rect.width,Flagsprite.rect.height),new Vector2(0.5f,0.5f));//注意居中显示采用0.5f值


    }

    public void defaction(int def_count)
	{
		/*messagebox.SetActive (true);



		TextMesh temptext = messagebox.GetComponent<TextMesh> ();

		if (Global_ImportanceData.getInstance ().List_importance [importanceid].defenseNOW > 0) {
			
			temptext.color = new Color (0, 0, 1);
		} else {

			temptext.color = new Color (1, 0, 0);

		}

		if (def_count > 0) {
			temptext.text = "-" + def_count;
		} else {

			temptext.text = "防御";
		}
        */

	}

	public void OnMouseEnter()
	{
        if (Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID != Global_const.NONE_ID)
        {
            return;
        }
        this.GetComponent<SpriteRenderer> ().color = Color.green ;// 100;


      //  Global_events.getInstance().traggerGlobalEvent(1000, importanceid, GlobalData.getInstance().nowheroid, 0, "城市信息");

        //  Messenger.Broadcast("UPDATEGLOBALMESSAGE");

    }


    public void OnMouseExit()
	{
		//Debug.Log ("exit");
		this.GetComponent<SpriteRenderer> ().color = new Vector4 (1f, 1f, 1f, 1f);
	}

    private void OnMouseUp()
    {
        if (Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID != Global_const.NONE_ID)
        {
            return;
        }
        // Debug.Log("响应外交");

        int LeaderID = Global_ImportanceData.getInstance().List_importance[importanceid].LeaderID;

        if(Global_const.NONE_ID== LeaderID)
        {
            return;
        }

        KindomData LeaderKindom = Global_KindomData.getInstance().list_KindomData[Global_HeroData.getInstance().List_hero[LeaderID].m_relationship.belong_kindom];

        int tempKingID = LeaderKindom.KingID;

        Global_events.getInstance().traggerGlobalEvent(1000, importanceid, GlobalData.getInstance().nowheroid, tempKingID, "城市触发");
            

    }

}
