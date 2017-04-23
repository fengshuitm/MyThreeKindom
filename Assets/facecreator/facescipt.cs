using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class facescipt : MonoBehaviour {

    int Id = Global_const.NONE_ID;
	// Use this for initialization
	void Start () {

        Id = 0;

        Global_source_loader tempsourceloader = Global_source_loader.getInstance();
        Global_facecreator tempfacecreator = Global_facecreator.getInstance();

        Invalidate();

    }

 
    // Update is called once per frame
    void Update () {
       
    }

    private void Awake()
    {
        Messenger.AddListener("Invalidate", Invalidate);


    }

    private void OnDestroy()
    {
        Messenger.RemoveListener("Invalidate", Invalidate);


    }

    public void Invalidate()
    {
 
        if (Global_const.NONE_ID== Id)
        {
            return;
        }

        Clean();

        Facedata tempfacedata = Global_facecreator.getInstance().listfaceData[Id];//.this_Elem.GetAttribute("")
        if(tempfacedata.this_Elem==null)
        { 
        //this.transform.FindChild("back").GetComponent<Image>().sprite = null;
        }
        else
        {
            int sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("add"));

            if(sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("add").GetComponent<Image>().sprite = Global_source_loader.getInstance().Add_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("armor"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("armor").GetComponent<Image>().sprite = Global_source_loader.getInstance().Armor_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("back"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("back").GetComponent<Image>().sprite = Global_source_loader.getInstance().Back_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("beard"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("beard").GetComponent<Image>().sprite = Global_source_loader.getInstance().Beard_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("eye"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("eye").GetComponent<Image>().sprite = Global_source_loader.getInstance().Eye_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("eyebrow"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("eyebrow").GetComponent<Image>().sprite = Global_source_loader.getInstance().Eyebrow_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("hair"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("hair").GetComponent<Image>().sprite = Global_source_loader.getInstance().Hair_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("head"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("head").GetComponent<Image>().sprite = Global_source_loader.getInstance().Head_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("mouse"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("mouse").GetComponent<Image>().sprite = Global_source_loader.getInstance().Mouse_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("moustache"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("moustache").GetComponent<Image>().sprite = Global_source_loader.getInstance().Moustache_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("nose"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("nose").GetComponent<Image>().sprite = Global_source_loader.getInstance().Nose_Sprite[sourceid];
            }

            sourceid = int.Parse(tempfacedata.this_Elem.GetAttribute("weapon"));

            if (sourceid != Global_const.NONE_ID)
            {
                this.transform.FindChild("weapon").GetComponent<Image>().sprite = Global_source_loader.getInstance().Weapon_Sprite[sourceid];
            }
        }

    }

   private void Clean()
    {
        for(int i=0;i<this.transform.childCount;i++)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite=Global_source_loader.getInstance().None_Sprite;
        }

    }

    public void PlusSource(string _source)
    {
        ChangeSource(_source, +1);

    }

    public void MinusSource(string _source)
    {
        ChangeSource(_source, -1);

    }


    public void ChangeSource(string _source,int _changenum)
    {
        Facedata tempfacedata = Global_facecreator.getInstance().listfaceData[Id];//.this_Elem.GetAttribute("")


        int nownum =int.Parse(tempfacedata.this_Elem.GetAttribute(_source));

        nownum += _changenum;

        if(nownum<Global_const.NONE_ID)
        {
            return;
        }

        if (nownum >=Global_const.MAXFACERESOURCE)
        {
            return;
        }

        Global_facecreator.getInstance().listfaceData[this.Id].this_Elem.SetAttribute(_source, ""+nownum);

        Invalidate();
    }

}
