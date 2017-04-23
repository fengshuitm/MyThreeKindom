using UnityEngine;
using System.Collections;

public class importanceScript : MonoBehaviour {

	SpriteRenderer spr;//= gameObject.GetComponent<SpriteRenderer>();
	// Use this for initialization

	GameObject m_obj;

	public int importanceID=0;
	void Start () {

		//m_obj = gameObject;  
		//Debug.Log("GameObject Instance ID:"+gameObject.GetInstanceID()); 

		//spr= this.GetComponent<SpriteRenderer>();
		//spr.enabled = false;

		//importanceID = GlobalData.getInstance().nowimportanceID;
		//GlobalData.getInstance().nowimportanceID++;
		//spr
		//this.SetActive(false);

		//Global_ImportanceData.getInstance ().List_importance [importanceID] = this.gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		//this.transform.localScale=new Vector3(GlobalData.getInstance ().cam_lev/2.0f, GlobalData.getInstance ().cam_lev/2.0f,1);
		//this.GetComponent<SpriteRenderer>().gameObject.SetActive(false);

	}


}
