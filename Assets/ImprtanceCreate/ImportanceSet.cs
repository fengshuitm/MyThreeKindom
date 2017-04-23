using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImportanceSet : MonoBehaviour {


	enum SETSTATUE{NONE,SET};
	SETSTATUE SetStatue;

	GameObject ImportancechangePanel;

	Ray ray;
	private RaycastHit hit ;
	GameObject tempobject;

	// Use this for initialization
	void Start () {
		ImportancechangePanel = GameObject.Find ("Canvas/importancechange");
		SetStatue = ImportanceSet.SETSTATUE.NONE;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			SetStatue = ImportanceSet.SETSTATUE.NONE;
			Messenger.Broadcast ("Invalidate");

			ImportancechangePanel.GetComponent<UI_OBJ> ().move ();
		}

		if (ImportanceSet.SETSTATUE.SET == SetStatue) {
		
			if (Input.GetMouseButtonDown (0)) {

				//ray=Camera.main.ScreenPointToRay(Input.mousePosition);              
				//if (Physics.Raycast (ray, out hit, 1000)) {



                    Vector3 temphit = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    //string tempstr = "tempx: " + tempx + "tempy :" + tempy + "tempz:" + tempz;
                    //Debug.Log (temphit);
					Importance tempnowImportance=Global_ImportanceData.getInstance().List_importance[GlobalData.getInstance().nowimportanceID];
					tempnowImportance.this_Elem.SetAttribute("Active","1");

                    Vector3 temphittile = GridManager.getInstance().FromWorldVec2TiledVec(temphit);
                    tempnowImportance.SetPosition(temphittile);

					Messenger.Broadcast ("RESETIMORTANCE");
				//}
			}
		}
		/*if (ImportanceSet.SETSTATUE.SET == SetStatue) {
			ray=Camera.main.ScreenPointToRay(Input.mousePosition);              
			if (Physics.Raycast(ray, out hit, 1000))
			{
				tempobject = hit.collider.gameObject;
				cityprefab_srpit tempScript = hit.collider.gameObject.GetComponent<cityprefab_srpit>();

				if (null != tempScript) {
					//	Importance tempImportance = tempScript.importanceData.GetComponent<Importance>();


					sprtemp=hit.collider.gameObject.GetComponent<SpriteRenderer>();

					sprtemp.enabled = true;

					//importanceScript temp = hit.collider.gameObject.GetComponent<importanceScript>();
					//temp.
					//	int cityidtemp =tempImportance.id;// tempScript.importanceid;// System.Int32.Parse(tempobject.name);

					//Debug.Log (cityidtemp);

					/*  if (Input.GetMouseButtonDown(0))
                {
                  GlobalData.getInstance().nowselectedimportance = cityidtemp;
                  //city_panel_cityname.text =""+cityidtemp;// Global_ImportanceData.getInstance().List_importance[System.Int32.Parse(tempobject.name)].M_name;
					int leaderidofnowimportance=Global_const.NONE_ID;


					//当前武将为当前选择城池的太守
					leaderidofnowimportance = Global_ImportanceData.getInstance ().List_importance [cityidtemp].LeaderID;

					if (leaderidofnowimportance != Global_const.NONE_ID) {
						GlobalData.getInstance ().nowheroid = leaderidofnowimportance;

					}


                }
                */
			//	}


				//Debug.Log (hit.collider.gameObject.name);


				/*if (hit.collider.gameObject.name == this.gameObject.name){//"armyspirite_red") {
                b_selected = true;
                //如果碰撞物体为floor，讲物体运行的目的地设置为碰撞点

            } else {

                b_selected = false;
            }
            */

		//	}


	//	}
			

	}


	public void SetImportance()
	{
		SetStatue = ImportanceSet.SETSTATUE.SET;

	}
}
