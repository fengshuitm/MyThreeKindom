using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class back_script : UI_OBJ {

    bool Bfirst = true;
    int oldplaceid = 0;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
        if(Bfirst==true)
        {
            Global_events.getInstance().traggerGlobalEvent(1000, GlobalData.getInstance().nowimportanceID, GlobalData.getInstance().nowheroid, GlobalData.getInstance().nowheroid, "PLACE" + 0);
            Bfirst = false;
        }

    }

    private void Awake()
    {
        Messenger.AddListener<int>("PLACECHANGE", PlaceChange);
        Messenger.AddListener<int>("BACKSHOW", SetBack);

    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<int>("PLACECHANGE", PlaceChange);
        Messenger.RemoveListener<int>("BACKSHOW", SetBack);

    }

    public void SetBack(int _id)
	{
		if (_id < 0) {
			this.moveout();

        }
        else {

			this.GetComponent<Image> ().sprite = Global_source_loader.getInstance ().Back_List [_id];
            this.move();
        }
	}

    public void PlaceChange(int _id)
    {

        Messenger.Broadcast("UPDATEALL");


        if (_id >= -1)
        {

            SetBack(_id);

            Global_events.getInstance().traggerGlobalEvent(1000, Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID, GlobalData.getInstance().nowheroid, GlobalData.getInstance().nowheroid, "PLACE" + _id);
            Global_events.getInstance().TestEventTriggerList(_id);

            //Debug.Log(oldplaceid);

            oldplaceid = _id;


        }
        else if (-2 == _id)
        {
            PlaceChange(oldplaceid);
        }



    }


}
