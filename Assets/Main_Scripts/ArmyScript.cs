using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.IO;
//修改	for smallbattle


//example
//[RequireComponent(typeof(PolyNavAgent))]
public class ArmyScript : BattleScriptsFather {

	//private PolyNavAgent _agent;
	private RaycastHit hit ;

	bool b_selected;

	Ray ray;

	private TextMesh textName;
	public GameObject armycountbox;
	public GameObject armyspirit;

	TextMesh temptext;

	//public int messageboxtimer = 100;

	public int bigarmyID=Global_const.NONE_ID;

	public bool Bactive=false;

	void Start()
	{
		
		Init ();

	}

    private void Awake()
    {
        Messenger.AddListener("UPDATEALL", UpdateALL);
        Messenger.AddListener<int>("DEAD", DeadTest);

    }

    private void OnDestroy()
    {
        Messenger.RemoveListener("UPDATEALL", UpdateALL);
        Messenger.RemoveListener<int>("DEAD", DeadTest);

    }

    void DeadTest(int _id)
    {
        if(_id==this.bigarmyID)
        {

            Destroy(this.gameObject);
        }

    }
    /*public PolyNavAgent agent{
		get
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}
    */

    public void UpdateALL()
    {
        //Debug.Log("UpdateALL");
       // BigMapUpdate();
       if(this.gameObject!=null)
        {
            UpdateFlag();
            UpdateArmycount();

        }


    }

    public void Init()
	{
        temptext = transform.FindChild("army_walk_0/armytext").GetComponent<TextMesh>();// armycountbox.GetComponent<TextMesh>();

        //agent.SetDestination(new Vector2(50, 50));


		/*if (bigarmyID != Global_const.NONE_ID) {
			Global_Armydata.getInstance().List_army[bigarmyID].Update();//第一次刷新army

        }
        */
        
        //	FixedUpdate ();
    }

	public void InitBigArmy (int _armyid)
	{

		GameObject armys = GameObject.Find("armys");

		bigarmyID=_armyid;

		this.name = "" + _armyid;
		this.transform.parent = armys.transform;

        GetPosFromData();

        //this.transform.localPosition =_pos;

        //Bactive = true;

        //this.gameObject.SetActive(true);

    }

    void BigMapUpdate ()
	{
		/*if(Input.GetMouseButtonDown(0))
		{

			//得到射线碰撞点
			ray=Camera.main.ScreenPointToRay(Input.mousePosition);                

			LayerMask mask = 1 << 9;
			if (Physics.Raycast (ray, out hit, 100, mask.value)) {

				if (hit.collider.gameObject.name == this.gameObject.name) {//"armyspirite_red") {
					b_selected = true;
					//如果碰撞物体为floor，讲物体运行的目的地设置为碰撞点

					SetNowSelecetedHero ();

					return;

				} else {

					b_selected = false;
				}
			} else {
				b_selected = false;
			}
		}
        */
		/*if (true == b_selected) {
			Vector3 Vectemp = this.transform.position;

			if (Input.GetKey (KeyCode.I)) {
				Vectemp.y += 0.5f;

			} else if (Input.GetKey (KeyCode.K)) {
				Vectemp.y -= 0.5f;
			} else if (Input.GetKey (KeyCode.J)) {
				Vectemp.x -= 0.5f;
			} else if (Input.GetKey (KeyCode.L)) {
				Vectemp.x += 0.5f;
			}
			agent.SetDestination (Vectemp);
		}
	*/
		if (Input.GetMouseButtonDown (1)) {

			/*ray=Camera.main.ScreenPointToRay(Input.mousePosition);                
			if (Physics.Raycast(ray, out hit, 100))
			{

				//if (hit.collider.gameObject.name == "Plane") {
				//如果碰撞物体为floor，讲物体运行的目的地设置为碰撞点

					nma.SetDestination (hit.point);

				}
				//} else {

				//}
			}
			*/
			/*if (true == b_selected) {
				agent.SetDestination (Camera.main.ScreenToWorldPoint (Input.mousePosition));
			} else {

			}
            */
		}


	}


    void Update()
    {
        /*	if (Bactive == false) {
                return;
            }


            Global_Armydata.getInstance().List_army[bigarmyID].Update();

            //int tempid=int.Parse(this.gameObject.name);
            //tempid=int.Parse(this.gameObject.name);

            //this.transform.localScale=new Vector3(GlobalData.getInstance ().cam_lev/2.0f, 1,GlobalData.getInstance ().cam_lev/2.0f);
            //flagspirit.transform.localScale==new Vector3(GlobalData.getInstance ().cam_lev,GlobalData.getInstance ().cam_lev,1);
            flagspirit.transform.localScale =new Vector3(GlobalData.getInstance ().cam_lev/2.0f,GlobalData.getInstance ().cam_lev/2.0f,1);

            temptext.text = ""+Global_Armydata.getInstance ().List_army [bigarmyID].All_armycount;

            BigMapUpdate ();

            TestAlive ();

            SetForward ();
            */

        

        UpdateBigmap();

       
      }


    

    void SetNowSelecetedHero()
	{

		//GlobalData.getInstance ().nowheroid =Global_Armydata.getInstance ().List_army [bigarmyID].battleworksID [0];
	
	}


	void SetForward ()
	{
		Vector3 cameraDirection = Camera.main.transform.forward;
		
		armyspirit.transform.rotation = Quaternion.LookRotation(cameraDirection);

	}

	/*void TestAlive ()
	{

		if (true==Global_Armydata.getInstance ().List_army [bigarmyID].bedestroy) {
				//this.gameObject.SetActive (false);
				Destroy (this.gameObject);

				return;
			}
			
	}
    */


	void UpdateMapPos()
	{
        GetPosFromData();

    }

    void GetPosFromData()
    {
        //Debug.Log(bigarmyID);

        if(bigarmyID<0)
        {
            return;
        }

        Armydata temparmy = Global_Armydata.getInstance().List_army[bigarmyID];
        transform.position =GridManager.getInstance().FromTiledVec2WorldVec(temparmy.getvec3());
        transform.position = new Vector3(transform.position.x, transform.position.y, 2);

    }


    void FixedUpdate () {

       // UpdateFlag();

       // UpdateArmycount();
    }


	void UpdateFlag ()
	{
        XmlElement temparmy = Global_Armydata.getInstance().List_army[bigarmyID].this_Elem;

        int leaderID = Global_const.NONE_ID;
        try
        {
            int.Parse(temparmy.GetAttribute("主将"));// armytemp.battleworksID[(int)Armydata.BATTLEWORK.LEADER];

        }
        catch
        {

        }

        if (Global_const.NONE_ID==leaderID)
        {
            return;
        }

        int belong = Global_HeroData.getInstance().List_hero[leaderID].m_relationship.belong_kindom;

		//没有军团长
		if (belong == Global_const.NONE_ID) {
			flagspirit.gameObject.SetActive (false);
			old_flagID = Global_const.NONE_ID;
		} else {
            flagspirit.gameObject.SetActive(true);

            //如果当前势力不同于之前势力，则变换旗帜
            if (belong != old_flagID) {
				ChangeFlag (belong);
				old_flagID = belong;
			}
		}
        
	}

	void UpdateBigmap()
	{
        /*if (false == Global_Armydata.getInstance ().List_army [bigarmyID].Bactive) {

			return;
		}
        */
        //Global_Armydata.getInstance ().List_army [bigarmyID].Bactive = false;
        //Debug.Log("UpdateMapPos");

            UpdateMapPos ();




    }


    void UpdateArmycount()
    {

        int EnemyStyle = GetEnemyStyle();

        switch(EnemyStyle)
        {
            case (int)ENEMYSTYLE.HATE:
                temptext.color = new Color(0.5f, 0, 0);
                break;
            case (int)ENEMYSTYLE.NOMAL:
                temptext.color = new Color(0.5f, 0.5f, 0.5f);
                break;
            case (int)ENEMYSTYLE.FRIEND:
                temptext.color = new Color(0.5f, 0.5f, 0);
                break;
            case (int)ENEMYSTYLE.SAMEKINDOM:
                temptext.color = new Color(0,0, 0.5f);
                break;

        }
        

        //Global_Armydata.getInstance().List_army[bigarmyID].TestAll_armycount();

        string armyleadername = Global_HeroData.getInstance().List_hero[bigarmyID].GetAllName();
        temptext.text = armyleadername+ "军\n" + Global_GuardunitData.getInstance().List_guardunit[bigarmyID].Armycount;

        /*if(Global_Armydata.getInstance().List_army[bigarmyID].All_armycount<=0)
        {
            Destroy(this.gameObject);

        }
        */
    }

    public void defaction(int def_count)
	{
		/*messagebox.SetActive (true);

		TextMesh temptext = messagebox.GetComponent<TextMesh> ();


		if (def_count > 0) {
			temptext.text = "-" + def_count;
		} else {

			temptext.text = "防御";
		}
        */
	}

   enum ENEMYSTYLE { HATE,NOMAL,FRIEND,SAMEKINDOM}
   int GetEnemyStyle()
    {

        herodata temphero = Global_HeroData.getInstance().List_hero[bigarmyID];

        herodata nowSelectedhero= Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];

        if(temphero.m_relationship.belong_kindom == Global_const.NONE_ID)
        {
            return (int)ENEMYSTYLE.SAMEKINDOM;
        }
        else if(nowSelectedhero.m_relationship.belong_kindom==Global_const.NONE_ID)
        {

            return (int)ENEMYSTYLE.SAMEKINDOM;

        }
        else if (temphero.m_relationship.belong_kindom == nowSelectedhero.m_relationship.belong_kindom)
        {

            return (int)ENEMYSTYLE.SAMEKINDOM;
        }
        else
        {
            KindomData armykindom = Global_KindomData.getInstance().list_KindomData[temphero.m_relationship.belong_kindom];
            KindomData nowselectedarmykindom = Global_KindomData.getInstance().list_KindomData[nowSelectedhero.m_relationship.belong_kindom];

            int Kindomrelationtemp = nowselectedarmykindom.Kindomrelation[armykindom.id];

            if (Kindomrelationtemp>0)
            {
                return (int)ENEMYSTYLE.FRIEND;

            }
            else if(Kindomrelationtemp==0)
            {
                return (int)ENEMYSTYLE.NOMAL;

            }
            else if (Kindomrelationtemp < 0)
            {

              //  Debug.Log("armykindom.id" + armykindom.id);

              //  Debug.Log("Kindomrelationtemp" + Kindomrelationtemp);

                return (int)ENEMYSTYLE.HATE;

            }
        }


        return (int)ENEMYSTYLE.SAMEKINDOM;
    }

    public void OnMouseEnter()
    {

        armyspirit.GetComponent<SpriteRenderer>().color = Color.green;// 100;

    }

    public void OnMouseExit()
    {
        //Debug.Log ("exit");
        armyspirit.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
    }

    private void OnMouseUp()
    {
        if (Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID != Global_const.NONE_ID)
        {
            return;
        }

        if (GlobalData.getInstance().nowheroid!= bigarmyID)
        {
            Global_events.getInstance().traggerGlobalEvent(1000, 0,GlobalData.getInstance().nowheroid, bigarmyID, "部队触发");
        }
        else
        {
            Global_events.getInstance().traggerGlobalEvent(1000, 0, bigarmyID, bigarmyID, "军团信息");
        }

    }

}
