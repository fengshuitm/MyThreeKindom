using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Xml;
using System.IO;

public class citys : MonoBehaviour {

	// Use this for initialization
	//public GameObject importance_pref;

	void Start () {

        //importance_pref = Resources.Load("prefs/cityprefab") as GameObject;


        //Messenger.AddListener ("RESETIMORTANCE", Invalidate);
        //Invalidate ();

        Global_ImportanceData.getInstance().cityobj = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Invalidate()
    {

    }
		/*for(int i = 0;i<this.transform.childCount;i++)
		{
			GameObject go = this.transform.GetChild(i).gameObject;
			Destroy(go);
		}
		*/



	/*	for (int i = 0; i < Global_ImportanceData.getInstance ().List_importance.Length; i++) {
			Importance tempimportance = Global_ImportanceData.getInstance ().List_importance [i];

		 	if (tempimportance.Bactive == false) {
				continue;
			} else {
				GameObject tempimportanceObj = GameObject.Find ("" + i);

                XmlElement xl1 = Global_ImportanceData.getInstance().nowimportance_Elem_List[i];

                int templv = 0;

                try
                {
                    templv = int.Parse(xl1.GetAttribute("级别"));

                }
                catch
                {

                }



                if (tempimportanceObj == null) {
					GameObject importancetemp = GameObject.Instantiate (importance_pref, importance_pref.transform.position, importance_pref.transform.rotation) as GameObject;

					importancetemp.name = "" + i;
					importancetemp.transform.parent = this.gameObject.transform;
				
					importancetemp.transform.localPosition =tempimportance.vec3;

                    importancetemp.GetComponent<SpriteRenderer>().sprite = Global_source_loader.getInstance().ImportanceList[templv];
                    //importancetemp.transform.localPosition.y = tempimportance.y;
                    //importancetemp.transform.localPosition.z = tempimportance.z;

                } else {
					tempimportanceObj.transform.localPosition =tempimportance.vec3;
					//tempimportanceObj.transform.localPosition.x = tempimportance.x;
					//tempimportanceObj.transform.localPosition.y = tempimportance.y;
					//tempimportanceObj.transform.localPosition.z = tempimportance.z;
				}



			}

		}


	}

    */
}
