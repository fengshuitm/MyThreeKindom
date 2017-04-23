using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading;

class Global_GuardunitData
{
    private static Global_GuardunitData instance = new Global_GuardunitData();

    public Guardunit[] List_guardunit = new Guardunit[Global_const.getInstance().MAXHEROS];

    private Global_GuardunitData()
    {

        for (int i = 0; i < Global_const.getInstance().MAXHEROS; i++)
        {
            List_guardunit[i] = new Guardunit();
        }

    }

    public static Global_GuardunitData getInstance()
    {

        return instance;
    }

}



