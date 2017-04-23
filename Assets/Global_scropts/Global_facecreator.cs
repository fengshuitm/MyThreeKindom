using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class Global_facecreator  {

    public Facedata[] listfaceData = new Facedata[Global_const.getInstance().MAXHEROS];

    private static Global_facecreator instance = new Global_facecreator();

    public XmlDocument xml_list = new XmlDocument();
    //Army数据池
    public XmlNode xNode;


    // Use this for initialization
     private  Global_facecreator() {
	
        for(int i=0;i<Global_const.getInstance().MAXHEROS;i++)
        {
            listfaceData[i] = new Facedata();

        }

        Load_Xml();

    }

    public void Load_Xml(string _xmlname = "/xml_facelist.xml")
    {
        xml_list.Load((Application.dataPath + _xmlname));

        xNode = xml_list.SelectSingleNode("facelist");

        XmlNodeList xmlNodeList = xNode.ChildNodes;

        foreach (XmlElement xl1 in xmlNodeList)
        {
            int id = 0;
            try
            {
                id = int.Parse(xl1.GetAttribute("id"));
            }
            catch
            {

            }

            listfaceData[id].this_Elem = xl1;//这一行最消耗效率

        }

    }

    public IEnumerator Save_army_Xml(string _xmlname = "/xml_facelist.xml")
    {

        xml_list.Save((Application.dataPath + _xmlname));

        yield return 1;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public static Global_facecreator getInstance()
    {

        return instance;
    }
}
