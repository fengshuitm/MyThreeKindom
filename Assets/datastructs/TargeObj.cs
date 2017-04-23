using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading;

public class TargeObj {

    public XmlElement this_Elem = null;// new XmlElement[Global_const.getInstance().MAXARMY];

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPosition(Vector3 _Vec)
    {
        this_Elem.SetAttribute("x", "" + _Vec.x);
        this_Elem.SetAttribute("y", "" + _Vec.y);
        this_Elem.SetAttribute("z", "" + _Vec.z);
    }

    public Vector3 GetPosition()
    {
        int x = int.Parse(this_Elem.GetAttribute("x"));
        int y = int.Parse(this_Elem.GetAttribute("y"));
        int z = int.Parse(this_Elem.GetAttribute("z"));

        return new Vector3(x, y, z);
    }
}
