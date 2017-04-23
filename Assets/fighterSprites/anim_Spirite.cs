using UnityEngine;
using System.Collections;

public class anim_Spirite : MonoBehaviour {


	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
			
	}

    private void Awake()
    {
        Messenger.AddListener<int, Vector3>("SHOWATTACKSPIRITE", ShowSpirite);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<int, Vector3>("SHOWATTACKSPIRITE", ShowSpirite);

    }

    public void ShowSpirite(int _param, Vector3 _vec3)
    {
        /*   GameObject Spirite_pref = Resources.Load("prefs/attackspirit") as GameObject;
           GameObject SpiriteObj = GameObject.Instantiate(Spirite_pref, Spirite_pref.transform.position, Spirite_pref.transform.rotation) as GameObject;
           SpiriteObj.name = "tempspirit";
           SpiriteObj.transform.parent = this.gameObject.transform;
           SpiriteObj.GetComponent<armycountspirit>().setarmycount(_param);
           SpiriteObj.transform.localPosition = GridManager.getInstance().FromTiledVec2WorldVec(_vec3);
           */
        StartCoroutine(MyMethod(_param, _vec3));

    }

    IEnumerator MyMethod(int _param, Vector3 _vec3)
    {
        GameObject Spirite_pref = Resources.Load("prefs/attackspirit") as GameObject;
        GameObject SpiriteObj = GameObject.Instantiate(Spirite_pref, Spirite_pref.transform.position, Spirite_pref.transform.rotation) as GameObject;
        SpiriteObj.name = "tempspirit";
        SpiriteObj.transform.parent = this.gameObject.transform;
        SpiriteObj.GetComponent<armycountspirit>().setarmycount(_param);
        SpiriteObj.transform.localPosition = GridManager.getInstance().FromTiledVec2WorldVec(_vec3);

        yield return new WaitForSeconds(1000);
    }

    void Update () {

	}
}
