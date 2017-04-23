using UnityEngine;
using System.Collections;

public class city_panel_script : MonoBehaviour {

	public GameObject Image_city;
	public GameObject place_back;
	bool Bfirst=true;
    int oldplaceid = 0;
	// Use this for initialization
	void Start () {

		place_back.GetComponent<back_script>().move();

	}
	
	// Update is called once per frame
	void Update () {
		if (true == Bfirst) {
			Global_events.getInstance ().traggerGlobalEvent (1000, GlobalData.getInstance().nowimportanceID, GlobalData.getInstance ().nowheroid, GlobalData.getInstance().nowheroid, "PLACE" + 0);
			Bfirst = false;
		}
	}

/*	public void hunt()
	{
		Global_events.getInstance ().traggerGlobalEvent (373,0,GlobalData.getInstance().nowheroid,0);

	}
*/

	public void BackShow(int _id)
	{
		/*for(int i=0;i< placeList.Length;i++)
		{
			placeList [i].SetActive (false);

		}

		if (_id >= 0) {
			placeList [_id].GetComponent<back_script> ().move ();
		}*/

		place_back.GetComponent<back_script> ().SetBack(_id);


    }

    public void PlaceChange(int _id)
	{

		Messenger.Broadcast("UPDATEALL");


		if (_id>=-1) {

            BackShow(_id);

            Global_events.getInstance ().traggerGlobalEvent (1000, Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID, GlobalData.getInstance ().nowheroid, GlobalData.getInstance().nowheroid, "PLACE" + _id);
			Global_events.getInstance ().TestEventTriggerList (_id);

            //Debug.Log(oldplaceid);

            oldplaceid = _id;


        }
        else if (-2==_id)
        {
            PlaceChange(oldplaceid);
        }



    }


}
