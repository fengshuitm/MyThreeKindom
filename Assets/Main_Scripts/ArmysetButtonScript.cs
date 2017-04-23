using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ArmysetButtonScript : UI_OBJ {

	// Use this for initialization
	void Start () {
	
	}

    private void Awake()
    {

    }

  /*  private void OnDestroy()
    {
        Messenger.RemoveListener("Invalidate", Invalidate);

    }
    */
    // Update is called once per frame
    void Update () {



    }

    private void FixedUpdate()
    {

        //Invalidate();

    }

    public override void  Invalidate()
    {

      /*  int tempactive = int.Parse(Global_Armydata.getInstance().List_army[GlobalData.getInstance().nowheroid].this_Elem.GetAttribute("Active"));

        if (1 == tempactive)
        {
            this.GetComponent<Button>().interactable = true;
        }
        */
    }
}
