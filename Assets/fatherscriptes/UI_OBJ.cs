using UnityEngine;
using System.Collections;


public class UI_OBJ: MonoBehaviour
{
	public UI_OBJ ()
	{
	}

    private void Awake()
    {
        Messenger.AddListener("Invalidate", Invalidate);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener("Invalidate", Invalidate);

    }

    public void hide()
    {

        this.transform.localPosition = new Vector3(5000, 50000, 0);


    }

    public void show()
    {

        this.transform.localPosition = new Vector3(0, 0, 0);


    }

    public void move()
	{
		this.gameObject.SetActive(true);

		this.transform.localPosition =new Vector3(0,0,0);
	
		Invalidate ();
	}

	public void moveout()
	{
		Messenger.Broadcast("Invalidate");

		this.transform.localPosition =new Vector3(5000,50000,0);
		//this.gameObject.SetActive(false);

	}

	virtual public void Invalidate ()
	{

	}

}

