using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class relationship_set : MonoBehaviour {

	public GameObject NameText;

	int NowSelectedHeroID=Global_const.NONE_ID;

	herodata Hero_Now;
	herodata Hero_RelationShip_Selected;

	public GameObject HeroList;
	public GameObject MateList;
	public GameObject SubordinateList;
	public GameObject RighteousbrotherList;
	public GameObject FriendsList;

	public GameObject LikeField;

	// Use this for initialization
	void Start () {
	
	}

    private void Awake()
    {
        Messenger.AddListener<GameObject>("RelationShipItem", GetNowSelectedHero);

    }

    // Update is called once per frame
    void Update () {
	
	}

	public void InvalidateLists()
	{
		HeroList.GetComponent<listselect_script> ().invalidatelist ();
		MateList.GetComponent<listselect_script> ().invalidatelist ();
		SubordinateList.GetComponent<listselect_script> ().invalidatelist ();
		RighteousbrotherList.GetComponent<listselect_script> ().invalidatelist ();
		FriendsList.GetComponent<listselect_script> ().invalidatelist ();
	}

	public void GetNowSelectedHero(GameObject _SelectedItem)
	{
		//string nowSelectedHeroName = _SelectedItem.GetComponent<RelationShip_selectItem> ().Text_item [0].text;
		Hero_Now = Global_HeroData.getInstance ().List_hero [GlobalData.getInstance ().nowheroid];

		NowSelectedHeroID = _SelectedItem.GetComponent<RelationShip_selectItem> ().id;
		if (NowSelectedHeroID == Global_const.NONE_ID) {
			return;
		}

		Hero_RelationShip_Selected=Global_HeroData.getInstance ().List_hero [NowSelectedHeroID];

		NameText.GetComponent<Text> ().text = Global_HeroData.getInstance ().List_hero [NowSelectedHeroID].GetAllName ();// nowSelectedHeroName;
		this.GetComponent<messageboxscript>().move();
	}

	public void AddMate()
	{
		if (true == Hero_Now.m_relationship.addMate (Hero_RelationShip_Selected)) {
			Hero_RelationShip_Selected.m_relationship.addMate (Hero_Now);
			AddFriends ();

		}
		InvalidateLists ();
	}

	public void DeleteMate()
	{
		if (true == Hero_Now.m_relationship.DeleteMate (Hero_RelationShip_Selected)) {
			Hero_RelationShip_Selected.m_relationship.DeleteMate (Hero_Now);
			DeleteFriends ();

		}
		InvalidateLists ();
	}

	public void AddSubordinate()
	{
		if (true == Hero_Now.m_relationship.AddSubordinate (Hero_RelationShip_Selected)) {
			Hero_RelationShip_Selected.m_relationship.Factions_leader_ID = Hero_Now.id;
			InvalidateLists ();
		}
	}

	public void DeleteSubordinate()
	{
		if (true == Hero_Now.m_relationship.DeleteSubordinate (Hero_RelationShip_Selected)) {
			Hero_RelationShip_Selected.m_relationship.Factions_leader_ID =Global_const.NONE_ID;
			InvalidateLists ();
		}
	}

	public void AddRighteousbrother()
	{
		if (true == Hero_Now.m_relationship.AddRighteousbrother (Hero_RelationShip_Selected)) {
			Hero_RelationShip_Selected.m_relationship.AddRighteousbrother (Hero_Now);
			AddFriends ();
		}
		InvalidateLists ();
	}

	public void DeleteRighteousbrother()
	{
		if (true == Hero_Now.m_relationship.DeleteRighteousbrother (Hero_RelationShip_Selected)) {
			Hero_RelationShip_Selected.m_relationship.DeleteRighteousbrother (Hero_Now);
			DeleteFriends ();
		}
		InvalidateLists ();
	}

	public void AddFriends()
	{
		if (true == Hero_Now.m_relationship.AddFriends (Hero_RelationShip_Selected)) {
			Hero_RelationShip_Selected.m_relationship.AddFriends (Hero_Now);

		}
		InvalidateLists ();
	}

	public void DeleteFriends()
	{
		if (true == Hero_Now.m_relationship.DeleteFriends (Hero_RelationShip_Selected)) {
			Hero_RelationShip_Selected.m_relationship.DeleteFriends (Hero_Now);

		}
		InvalidateLists ();
	}

	public void ChangeLike()
	{
		try
		{
			//Hero_Now.m_relationship.likelist[Hero_RelationShip_Selected.id] =int.Parse( LikeField.GetComponent<InputField>().text);
			Hero_Now.m_relationship.friendsLikeHash[Hero_RelationShip_Selected.id]=int.Parse( LikeField.GetComponent<InputField>().text);
			//Global_HeroData.getInstance ().listTotemp ();

			InvalidateLists ();
		}
		catch {

		}
	}

}
