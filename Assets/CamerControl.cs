using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;
using CreativeSpore.SuperTilemapEditor;

public class CamerControl : MonoBehaviour {
    float movespeed = 0.5f;
    Vector3 transFormValue = new Vector3();
    private int moveSpeed = 5;

    // Use this for initialization
    void Start (){
	
	}

    // Update is called once per frame
    void Update()
    {


        if (Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID != Global_const.NONE_ID)
        {
            return;
        }

        if (GlobalData.getInstance().screen_statue != 2)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                if (GetComponent<Camera>().orthographicSize < 20)
                {

                    GetComponent<Camera>().orthographicSize += 0.05f;
                }
            }
            else if (Input.GetKey(KeyCode.E))
            {
                if (GetComponent<Camera>().orthographicSize > 0)
                {

                    GetComponent<Camera>().orthographicSize -= 0.05f;
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {

                transFormValue = Vector3.up * Time.deltaTime;

                this.transform.Translate(transFormValue * moveSpeed, Space.World);
            }
            else if (Input.GetKey(KeyCode.S))
            {

                transFormValue = -Vector3.up * Time.deltaTime;

                this.transform.Translate(transFormValue * moveSpeed, Space.World);
            }
            else if (Input.GetKey(KeyCode.A))
            {

                transFormValue = Vector3.left * Time.deltaTime;

                this.transform.Translate(transFormValue * moveSpeed, Space.World);
            }
            else if (Input.GetKey(KeyCode.D))
            {

                transFormValue = -Vector3.left * Time.deltaTime;

                this.transform.Translate(transFormValue * moveSpeed, Space.World);
            }
            else if (Input.GetKey(KeyCode.Z))
            {

                moveSpeed--;

                if (moveSpeed <= 0)
                {
                    moveSpeed = 0;
                }
            }
            else if (Input.GetKey(KeyCode.X))
            {

                moveSpeed++;

                if (moveSpeed >= 10)
                {
                    moveSpeed = 10;
                }
            }
        }
    }
}
