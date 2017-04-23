using UnityEngine;
using System.Collections;

public class armycountspirit : MonoBehaviour {

    // Use this for initialization
    int life = 25;
    GameObject armycount;
    void Start () {
	
	}

    private void Awake()
    {

    }

    public void setarmycount(int _count)
    {
        this.transform.FindChild("count").GetComponent<TextMesh>().text = "" + _count;

    }

    // Update is called once per frame
    void Update () {
        life--;
        Vector3 temppos = this.transform.position;

        temppos.y+=0.0025f;

        this.transform.position = temppos;
        if (life<=0)
        {
            DestroyObject(this.gameObject);
        }
	}
}
