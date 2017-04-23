using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

public class Facedata
{

    int Id;
    public XmlElement this_Elem = null;// new XmlElement[Global_const.getInstance().MAXARMY];

    public Facedata()
    {
        Id = Global_const.NONE_ID;
    }


}
