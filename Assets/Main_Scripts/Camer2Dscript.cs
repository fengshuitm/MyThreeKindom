using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class Camer2Dscript : MonoBehaviour {

	RandomValue tempRandom=new RandomValue();
	KeyCode currentKey;
	private int moveSpeed = 5;
	private RaycastHit hit ;
//    int tempcount=0;
//	int hero_update_count=0;

	Ray ray;
   
//	int hero_update_flag=0;//上次武将更新所到flag
//	int hero_UPDATE_COUNT=1;//Global_const.getInstance().MAXHEROS/100;//每次更新的武将数

	SpriteRenderer sprtemp;
	GameObject tempobject;

    int sleeptime = 0;
    const int Maxsleeptime = 10;

    float movespeed = 0.5f;

    Vector3 movetargetVec;

	//public GameObject herolist;

    //public Text city_panel_cityname;
    //public Text data_panel_nowdata;
	//public Text city_herocount;

	Vector3 transFormValue = new Vector3();
	// Use this for initialization
	void Start () {


	//Global_UI.getInstance().cam=gameObject.GetComponent<Camera>();

		//首次更新武将列表
	/*	for (int i = 0; i < Global_const.getInstance ().MAXHEROS / hero_UPDATE_COUNT; i++) {
			Updateherolistdata();

		}
	*/

	}


    private void Awake()
    {
        Messenger.AddListener<float,float,float> ("MoveCam", MoveCam);

        GridManager.getInstance().Init();
        Global_ImportanceData.getInstance().Init();
        Global_UI.getInstance().Init();
        Global_HeroData.getInstance().Init();
            
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<float, float, float>("MoveCam", MoveCam);

    }

    void  MoveCam(float _x,float _y,float _s)
    {
        Vector3 camvectile=new Vector3(_x, _y, -15f);

        Vector3 camvec = GridManager.getInstance().FromTiledVec2WorldVec(camvectile);

        //this.transform.position =new Vector3(camvec.x, camvec.y,-15);
        this.GetComponent<Camera2DFollow>().targetvec3 = new Vector3(camvec.x, camvec.y, -15);

        //Debug.Log(camvec);
    }

    // Update is called once per frame
    void FixedUpdate () {
	
	/*	if (GlobalData.getInstance ().b_update == false) {
			return;

		}


		tempcount++;


		if (tempcount == 100) {
			tempcount = 0;
			UpdateData ();

			UpdateImportances();

		}

		UpdateArmy();

		Global_events.getInstance ().Update();

		hero_update_count++;
		if(hero_update_count==2)
		{
			hero_update_count=0;

			Updateherolistdata();

		}
*/
	
	}

	void Update()
	{

		UpdateControl();

		if (GlobalData.getInstance ().screen_statue == 0) {
			UpdateSelected ();
		}

	}

      public void UpdateControl()
      {

        if(Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID!=Global_const.NONE_ID)
        {
            return;
        }


			
		/*if (cam.orthographicSize < 2) {
			GlobalData.getInstance ().cam_lev = 1;

		} else {
			GlobalData.getInstance ().cam_lev = 3;
		}
		*/	
		if (GlobalData.getInstance ().screen_statue != 2) {

			if (Input.GetKey (KeyCode.Q)) {
				if (GetComponent<Camera>().orthographicSize < 20) {

                    GetComponent<Camera>().orthographicSize += 0.05f;
				}
			} else if (Input.GetKey (KeyCode.E)) {
				if (GetComponent<Camera>().orthographicSize > 0) {

                    GetComponent<Camera>().orthographicSize -= 0.05f;
				}
			} else if (Input.GetKey (KeyCode.W)) {

				transFormValue = Vector3.up * Time.deltaTime;

				this.transform.Translate (transFormValue * moveSpeed, Space.World);
			} else if (Input.GetKey (KeyCode.S)) {

				transFormValue = -Vector3.up * Time.deltaTime;

				this.transform.Translate (transFormValue * moveSpeed, Space.World);
			} else if (Input.GetKey (KeyCode.A)) {

				transFormValue = Vector3.left * Time.deltaTime;

				this.transform.Translate (transFormValue * moveSpeed, Space.World);
			} else if (Input.GetKey (KeyCode.D)) {

				transFormValue = -Vector3.left * Time.deltaTime;

				this.transform.Translate (transFormValue * moveSpeed, Space.World);
			}
			else if (Input.GetKey (KeyCode.Z)) {

				moveSpeed--;

				if (moveSpeed <= 0) {
					moveSpeed = 0;
				}
			}
			else if (Input.GetKey (KeyCode.X)) {

				moveSpeed++;

				if (moveSpeed >= 10) {
					moveSpeed = 10;
				}
			}
            else if (Input.GetKey(KeyCode.I))
            {
                if (sleeptime <= 0)
                {
                    Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.UP);
                    beginsleep();

                }

            }
            else if (Input.GetKey(KeyCode.J))
            {

                if (sleeptime <= 0)
                {
                    Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.LEFT);
                    beginsleep();

                }

            }
            else if (Input.GetKey(KeyCode.L))
            {
                if (sleeptime <= 0)
                {
                    Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.RIGHT);
                    beginsleep();

                }

            }
            else if (Input.GetKey(KeyCode.K))
            {
                if (sleeptime<=0)
                {
                    Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.DOWN);
                    beginsleep();

                }

            }
            else if (Input.GetKey(KeyCode.U))
            {
                if (sleeptime <= 0)
                {
                    Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.WAIT);
                    beginsleep();

                }

            }
        }

        sleepupdate();

    }


    public void UpdateSelected()
    {
    

    }

    void beginsleep()
    {
        sleeptime = Maxsleeptime;

    }

    void sleepupdate()
    {
        sleeptime--;

        //Debug.Log(sleeptime);

        if(sleeptime<=0)
        {
            sleeptime = 0;

        }

    }

	public void SetScreenStatue(int _ScreenStatue)
	{

		GlobalData.getInstance ().screen_statue = _ScreenStatue;


		switch (_ScreenStatue) {
		case 0:

			break;
		case 2:
                GetComponent<Camera>().orthographicSize = 1;
			break;


		}
	}

	public void BackMain()
	{

		SceneManager.LoadScene("MainUI");

	}
}
