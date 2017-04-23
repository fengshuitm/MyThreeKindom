using UnityEngine;
using System.Collections;
using CreativeSpore.SuperTilemapEditor;

public class movetest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        

	}

    private void Awake()
    {
        Debug.Log(transform.position);

      /*  transform.position = GridManager.getInstance().GetGridCellCenter(
            GridManager.getInstance().GetGridIndex(transform.position));
        */
    }

    int sleeptime = 0;
    const int Maxsleeptime = 2;

    //Vector2 Position;

    // Update is called once per frame
    void Update () {
       
    }


    void beginsleep()
    {
        sleeptime = Maxsleeptime;

    }

    
}
