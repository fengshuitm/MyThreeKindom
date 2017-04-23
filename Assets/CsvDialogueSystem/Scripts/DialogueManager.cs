using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Xml;
using System.IO;
/// <summary>
/// CSV based dialogue manager
/// </summary>
public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Support languages
    /// </summary>
    public enum Languages
    {
        English,
        Italian
    }
    /// <summary>
    /// How to display inactive answers
    /// </summary>
    public enum InactiveAnswersMode
    {
        Gray,                                                                       // Will have blured color
        Invisible                                                                   // Will not be displayed
    }

    public static DialogueManager Instance;                                         // Singleton

	public Text Message;                                                            // Non-Player Character name
	public Text PCName;                                                            // Non-Player Character name
    public Text npcName;                                                            // Non-Player Character name
    public Text npcSay;                                                             // Non-Player Character text
    public GameObject answerPrefab;                                                 // Prefab for answer object
    public GameObject answerFolder;                                                 // Parent folder for answers
    public InactiveAnswersMode answersMode = InactiveAnswersMode.Gray;              // Inactive answers display mode
    public Color inactiveAnswerColor = Color.gray;                                  // Inactive answers color (only for InactiveAnswersMode.Gray)
    public float typingDelay = 0.02f;                                               // Delay between letters typing on screen

    private bool dialogInProgress = false;                                          // Is dialog active now
    private TextAsset dialogueCsv;                                                  // Comma-separated values dialogue descriptor file
    private IEnumerator npcTextRoutine;                                             // NPC text routine
    private bool npcIsTalking = false;                                              // Is NPC talking now
    private IEnumerator playerTextRoutine;                                          // Player text routine
    private bool playerIsTalking = false;                                           // Is player talking now
    private IEnumerator answersDisplayRoutine;                                      // Answers display routine
    private int answersCounter = 0;                                                 // Current answers counter
    private Languages language = Languages.English;                                 // Current language
    private string languageSign = "Eng";                                            // Current dialog language separator
    private string currentPage;                                                     // Current dialog page
    private string npcOnStartName, npcOnStartSay;                                   // NPC name and text before dialog start
    private string[] stuffLineName = { "NpcAnswer", "Requirements", "Effect" };     // Stuff lines will be excluded from answers
    private List<string> clickedAnswers = new List<string>();                       // List of answers were clicked during dialog
    private List<string> blockedAnswers = new List<string>();                       // List of answers manualy blocked during dialog

	TextAsset [] dilogs=new TextAsset[2000];

	/// <summary>
	/// fs new
	/// </summary>
	public Image event_image;
	public Image head_image_PC;
	public Image head_image_NPC;

	public GameObject Object_PC;
	public GameObject Object_NPC;

	public GameObject EventPlane;

    public GameObject MessageScroll;

    //public GameObject PC_SAY_back;


    //public GameObject [] UI_list;

    int tempcount =0;
	const float Maxcount = 50.0f;
	int m_hero1_id=Global_const.NONE_ID;
	int m_hero2_id=Global_const.NONE_ID;
	int m_EventID=Global_const.NONE_ID;
    int m_EventParam = Global_const.NONE_ID;


    //public GameObject hero_datashow_window;

    public Scrollbar timeScrollbar;

	public bool b_forever = false;

	Hashtable attributeHash=new Hashtable();

	string Tempstr="";
    void Awake()
    {
        Instance = this;                                                            // Make singleton Instance
    
		attributeHash.Add ("money", "金钱");
		attributeHash.Add ("forage", "粮草");
		attributeHash.Add ("might", "武力");
		attributeHash.Add ("polity", "政治");
		attributeHash.Add ("wit", "智力");


        //Messenger.Broadcast("UPDATEMESSAGE");
        Messenger.AddListener("UPDATEGLOBALMESSAGE", UpdateGlobalMessage);
        Messenger.AddListener<string>("SETGLOBALMESSGAE", SetGlobalMessage);
        Messenger.AddListener<int>("Selecthero", Selecthero);

        int m_hero1_id = GlobalData.getInstance().nowheroid;
        int m_hero2_id = GlobalData.getInstance().nowheroid;

        DontDestroyOnLoad(this.gameObject);
    }

    

    private void OnDestroy()
    {
        Messenger.RemoveListener("UPDATEGLOBALMESSAGE", UpdateGlobalMessage);
        Messenger.RemoveListener<string>("SETGLOBALMESSGAE", SetGlobalMessage);
        Messenger.RemoveListener<int>("Selecthero", Selecthero);

    }
    
    private void Selecthero(int _id)
    {
        if(_id==Global_const.NONE_ID)
        {
            return;
        }

        Armydata nowarmy = Global_Armydata.getInstance().List_army[GlobalData.getInstance().nowheroid];
        int oldtempid = Global_const.NONE_ID;
        switch (GlobalData.getInstance().nowherosortstyle)
        {
            case (int)GlobalData.NOWSORTSTYLE.FRIENDS:
                if (GlobalData.getInstance().nowheroid == _id)
                {
                    Global_events.getInstance().traggerGlobalEvent(1000, 0, GlobalData.getInstance().nowheroid, _id, "自宅");

                }
                else
                {
                    Global_events.getInstance().traggerGlobalEvent(379, 0, GlobalData.getInstance().nowheroid, _id, "begin");

                }
                break;
            case (int)GlobalData.NOWSORTSTYLE.FORWARD:


                    oldtempid = int.Parse(nowarmy.this_Elem.GetAttribute("先锋"));

                    nowarmy.addhero(_id);
                    nowarmy.this_Elem.SetAttribute("先锋", "" + _id);
                if (false == nowarmy.haswork(oldtempid))
                {
                    nowarmy.deletehero(oldtempid);
                }
                GlobalData.getInstance().global_message = "先锋已经被设置成" + Global_HeroData.getInstance().List_hero[_id].GetAllName()+"\n"+ GlobalData.getInstance().global_message;
                UpdateGlobalMessage();                
                break;
            case (int)GlobalData.NOWSORTSTYLE.COUNSELLOR:


                oldtempid = int.Parse(nowarmy.this_Elem.GetAttribute("军师"));

                nowarmy.addhero(_id);
                nowarmy.this_Elem.SetAttribute("军师", "" + _id);

                if (false == nowarmy.haswork(oldtempid))
                {
                    nowarmy.deletehero(oldtempid);
                }

                GlobalData.getInstance().global_message = "军师已经被设置成" + Global_HeroData.getInstance().List_hero[_id].GetAllName() + "\n" + GlobalData.getInstance().global_message;
                UpdateGlobalMessage();
                break;
            case (int)GlobalData.NOWSORTSTYLE.GUARD:

                oldtempid = int.Parse(nowarmy.this_Elem.GetAttribute("哨马"));

                nowarmy.addhero(_id);
                nowarmy.this_Elem.SetAttribute("哨马", "" + _id);

                if (false == nowarmy.haswork(oldtempid))
                {
                    nowarmy.deletehero(oldtempid);
                }
                GlobalData.getInstance().global_message = "哨马已经被设置成" + Global_HeroData.getInstance().List_hero[_id].GetAllName() + "\n" + GlobalData.getInstance().global_message;
                UpdateGlobalMessage();
                break;
            case (int)GlobalData.NOWSORTSTYLE.IMPEDIMENTA:

                oldtempid = int.Parse(nowarmy.this_Elem.GetAttribute("辎重"));

                nowarmy.addhero(_id);
                nowarmy.this_Elem.SetAttribute("辎重", "" + _id);

                if (false == nowarmy.haswork(oldtempid))
                {
                    nowarmy.deletehero(oldtempid);
                }
                GlobalData.getInstance().global_message = "辎重已经被设置成" + Global_HeroData.getInstance().List_hero[_id].GetAllName() + "\n" + GlobalData.getInstance().global_message;
                UpdateGlobalMessage();
                break;
        }

        

    }


    private void InitEventTriggerList(string page)
	{
		//Tempstr="";
		if (page == null)
		{
			Debug.Log("Wrong input data");
			return ;
		}
		List<string> lines = CsvParser.Instance.GetLines(page);                     // Get all lines from page
		if (lines != null)
		{
			foreach (string line in lines)
			{
				string lineName = CsvParser.Instance.GetLineName(line);             // Get line name
				if (lineName == "Effect")                                           // Find lines named "Effect"
				{


				}

			}

		}

	}

	void InitEventUI ()
	{
        //timeScrollbar.gameObject.SetActive (false);
       

        Object_NPC.SetActive (false);
		Object_PC.SetActive (false);
		event_image.gameObject.SetActive(false);

        Global_UI.getInstance().TextBack.GetComponent<UI_OBJ>().moveout();

        //Message.gameObject.SetActive (false);
        /*	for (int i = 0; i < UI_list.Length; i++) {
                UI_list [i].SetActive (false);
            }
        */
    }

    public void SetEvent(int EventID,int param1=0,int param2=0,int param3=0,string _dilgbegin="begin")
	{



        m_EventID = EventID;

		m_hero1_id = param2;
		m_hero2_id = param3;
        m_EventParam = param1;

		InitEventUI ();

		StartDialogue (EventID,_dilgbegin);

    


    }

    void ReloadProcess()
	{
		tempcount = (int)Maxcount;
	}

	void FixedUpdate()
	{
		if (tempcount >0) {
			tempcount--;
			//不一定有进度条
			UpdateScrollbar();

			if (tempcount == 0) {
				SetEvent (m_EventID, 0, m_hero1_id, m_hero2_id);

				Messenger.Broadcast ("UPDATEALL");
			}

		} else {
			tempcount = 0;

		}

	}

	void UpdateScrollbar()
	{
		//timeScrollbar.gameObject.SetActive (true);
		try
		{
			timeScrollbar.value = tempcount/Maxcount;
		}
		catch {

		}

	}

    void Start()
    {
        if (    npcName == null
            ||  npcSay == null
            ||  answerPrefab == null
            ||  answerFolder == null)
        {
            Debug.LogError("Wrong default settings");
            return;
        }
		//TextAsset dilog1=Resources.Load("Descs/Dialogue1") as TextAsset;
		//StartDialogue(dilog1);


		for (int i = 0; i < dilogs.Length; i++) {

			try
			{
				dilogs[i]=Resources.Load ("Descs/"+i) as TextAsset;
			}
			catch
			{

			}
		}

		Global_events.getInstance ().DilogMana = this;

    }

	public void StartDialogue(int csvFileID,string _dilgbegin="")
	{
		//Debug.Log ("_dilgbegin:"+_dilgbegin);
		StartDialogue (dilogs [csvFileID],_dilgbegin);

	}

    /// <summary>
    /// Start dialog from CSV descriptor
    /// </summary>
    /// <param name="csvFile"> CSV dialog descriptor </param>
	public void StartDialogue(TextAsset csvFile,string _dilgbegin)
    {
        if (csvFile == null)
        {
            Debug.Log("Wrong input data");
            return;
        }
        dialogInProgress = true;
        npcOnStartName = npcName.text;                                              // Save NPC name before dialog start
        npcOnStartSay = npcSay.text;                                                // Save NPC text before dialog start
        dialogueCsv = csvFile;
        DisplayCommonInfo();                                                        // Display info from descriptor common page
        string page;
		page = CsvParser.Instance.GetPage(_dilgbegin, dialogueCsv);                  // Find start page named "Welcome"
        if (page != null)
        {
            DisplayNpcAnswer(GetNpcAnswer(page));                                   // Display NPC answer from start page
			if (DisplayAnswers(page) == true)                                       // Display player answers from start page
            {
                currentPage = page;                                                 // Set current page
            }
            else
            {
                EndDialogue();                                                      // If no valid answers - end dialog
            }
			ApplyEffects(page);

        }
    }

    /// <summary>
    /// Check if dialog in progress
    /// </summary>
    /// <returns> true - in progress, false - not started </returns>
    public bool IsDialogInProgress()
    {
        return dialogInProgress;
    }

    /// <summary>
    /// Choose dialog language
    /// </summary>
    /// <param name="newLanguage"> Language from Languages enum </param>
    public void SetLanguage(Languages newLanguage)
    {
        language = newLanguage;
        switch (newLanguage)
        {
            case Languages.English:
                {
                    languageSign = "Eng";
                    break;
                }
            case Languages.Italian:
                {
                    languageSign = "Ital";
                    break;
                }
            default:
                {
                    Debug.Log("Unknown language");
                    languageSign = "Eng";
                    language = Languages.English;
                    break;
                }
        }
        if (IsDialogInProgress())                                                   // If dialog in progress update current screen
        {
            if (npcTextRoutine != null)                                             // Stop dialog coroutines
            {
                StopCoroutine(npcTextRoutine);
            }
            if (playerTextRoutine != null)
            {
                StopCoroutine(playerTextRoutine);
            }
            if (answersDisplayRoutine != null)
            {
                StopCoroutine(answersDisplayRoutine);
            }
            DisplayCommonInfo();                                                    // Update NPC name
            DisplayNpcAnswer(GetNpcAnswer(currentPage));                            // Update NPC answer
            CleanAnswers();                                                         // Remove current answers from screen
            DisplayAnswers(currentPage);                                            // Update answers from current page
        }
    }

    /// <summary>
    /// Get current language
    /// </summary>
    /// <returns> Current language </returns>
    public Languages GetLanguage()
    {
        return language;
    }

    /// <summary>
    /// End current dialog
    /// </summary>
    public void EndDialogue()
    {
        if (npcTextRoutine != null)                                                 // Stop dialog coroutines
        {
            StopCoroutine(npcTextRoutine);
        }
        if (playerTextRoutine != null)
        {
            StopCoroutine(playerTextRoutine);
        }
        if (answersDisplayRoutine != null)
        {
            StopCoroutine(answersDisplayRoutine);
        }
        CleanAnswers();                                                             // Remove answers from diplay
        npcName.text = npcOnStartName;                                              // Restore NPC name
        npcOnStartName = null;
        npcSay.text = npcOnStartSay;                                                // Restore NPC text
        npcOnStartSay = null;
        currentPage = null;
        clickedAnswers.Clear();                                                     // Clear list of clicked answers
        blockedAnswers.Clear();                                                     // Clear list of manualy blocked answers
        dialogInProgress = false;
    }

    /// <summary>
    /// Actions on user's answer click
    /// </summary>
    /// <param name="answer"> Clicked answer </param>
    public void OnAnswerClick(GameObject answer)
    {
        if (answer == null)
        {
            Debug.Log("Wrong input data");
            return;
        }
			

        string page = CsvParser.Instance.GetPage(answer.name, dialogueCsv);         // Find page with answer's name
        if (page != null)
        {
            if (clickedAnswers.Contains(answer.name) == false)
            {
                clickedAnswers.Add(answer.name);                                    // Add answer to clicked answers list
            }
            string npcAnswer = GetNpcAnswer(page);                                  // Get NPC answer from page
            if (npcAnswer != null)
            {
                DisplayNpcAnswer(npcAnswer);                                        // Display NPC answer
            }
            if (ApplyEffects(page) == true)                                         // Apply answer effects
            {
                return;                                                             // true - need to stop dialog
            }
            if (DisplayAnswers(page) == true)                                       // Try to display player answers from page
            {
                currentPage = page;                                                 // Save current page
            }
            else
            {
                if (DisplayAnswers(currentPage) == false)                           // If no active answers in new page - stay on current page
                {
                    Debug.Log("No active answers");
                    EndDialogue();                                                  // If no active answers on current page - end dialog
                }
            }
        }
    }

    /// <summary>
    /// Display data from dialog descriptor page
    /// </summary>
    private void DisplayCommonInfo()
    {
        string page;
        page = CsvParser.Instance.GetPage("Desc", dialogueCsv);                     // Get page named "Desc"
        if (page != null)
        {
            string name;
            List<string> lines = CsvParser.Instance.GetLines(page);                 // Get all lines from page
            if (lines != null)
            {
                foreach (string line in lines)
                {
                    ///
                    /// Dislay NPC name
                    ///
                    if (CsvParser.Instance.GetLineName(line) == "NpcName")          // Find line named "NpcName"
                    {
                        if (CsvParser.Instance.GetTextValue(out name, languageSign, line) == true)
                        {
                            if (name != null)                                       // Get text from line
                            {
                                //npcName.text = name;                                // Display NPC name
                            }
                        }
                        continue;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Letter by letter NPC answer display
    /// </summary>
    /// <param name="text"> NPC anwer text </param>
    /// <returns></returns>
    private IEnumerator NpcAnswerCoroutine(string text="")
    {

		if (text != null) {
			
		
	        npcIsTalking = true;
	        if (typingDelay > 0)                                                        // If display delay setted
	        {
	            foreach (char letter in text)
	            {
	                npcSay.text += letter;                                              // Add letter to answer
	                yield return new WaitForSeconds(typingDelay);                       // Wait for delay
	            }
	        }
	        else
	        {
	            npcSay.text += text;                                                    // If no delay needed - display whole text
	        }
	        npcIsTalking = false;
		}
    }

    /// <summary>
    /// Display NPC answer
    /// </summary>
    /// <param name="text"> NPC answer text </param>
    private void DisplayNpcAnswer(string text="")
    {
		
        if (npcSay != null)
        {
            if (npcTextRoutine != null)
            {
                StopCoroutine(npcTextRoutine);                                      // Stop current coroutine if it is
            }
            npcSay.text = "";                                                       // Clear answer text field
            npcTextRoutine = NpcAnswerCoroutine(text);
            StartCoroutine(npcTextRoutine);                                         // Start coroutine
        }
    }

    /// <summary>
    /// Get NPC answer text from page
    /// </summary>
    /// <param name="page"> Page from CSV dialogue descriptor </param>
    /// <returns> NPC answer text </returns>
    private string GetNpcAnswer(string page)
    {
        string res = null;
        if (page == null)
        {
            Debug.Log("Wrong input data");
            return res;
        }
        List<string> lines = CsvParser.Instance.GetLines(page);                     // Get all lines from page
        if (lines != null)
        {
            foreach (string line in lines)
            {
                if (CsvParser.Instance.GetLineName(line) == "NpcAnswer")            // Find line named "NpcAnswer"
                {
                    if (CsvParser.Instance.GetTextValue(out res, languageSign, line) == true)
                    {
                        break;                                                      // Get text from line
                    }
                }
            }
        }
        return res;
    }

    /// <summary>
    /// Letter by letter player answer display
    /// </summary>
    /// <param name="text"> Answer text </param>
    /// <param name="answer"> Answer display text field </param>
    /// <returns></returns>
    private IEnumerator PlayerAnswerCoroutine(string text, Text answer)
    {
        if (answer != null)
        {
            playerIsTalking = true;
            if (typingDelay > 0)                                                    // If display delay setted
            {
                foreach (char letter in text)
                {
                    answer.text += letter;                                          // Add letter to answer
                    yield return new WaitForSeconds(typingDelay);                   // Wait for delay
                }
            }
            else
            {
                answer.text += text;                                                // If no delay needed - display whole text
            }
            playerIsTalking = false;
        }
    }

    /// <summary>
    /// Add answer into answer folder
    /// </summary>
    /// <param name="name"> Answer name </param>
    /// <param name="answer"> Answer text </param>
    /// <param name="isActive"> true - answer is interactive, false - answer is inactive </param>
    private void AddAnswer(string name, string answer, bool isActive)
    {
        if (name == null || answer == null)
        {
            Debug.Log("Wrong input data");
            return;
        }
        GameObject newAnswer = Instantiate(answerPrefab) as GameObject;             // Clone answer prefab
        if (newAnswer != null)
        {
            newAnswer.transform.SetParent(answerFolder.transform);                  // Place it into anwer folder
            newAnswer.name = name;                                                  // Set answer name
            Text text =newAnswer.GetComponent<AnswerHandler>()._Text.GetComponent<Text>();
            if (text != null)
            {
                answersCounter++;                                                   // Increase answers counter (cleared by ClearAnswers)
                AnswerHandler answerHandler = newAnswer.GetComponent<AnswerHandler>();
                if (isActive == false)                                              // If answer inactive
                {
                    answerHandler.active = false;                                   // Make answer not clickable
                    text.color = inactiveAnswerColor;                               // Set inactive color
                }
                else
                {
                    answerHandler.active = true;                                    // Make answer clickable
                }
                if (playerTextRoutine != null)
                {
                    StopCoroutine(playerTextRoutine);                               // Stop current coroutine if it is
                }
				text.text = ">";//answersCounter.ToString() + ". ";                       // Display answer counter
                playerTextRoutine = PlayerAnswerCoroutine(answer, text);
                StartCoroutine(playerTextRoutine);                                  // Start coroutine
            }
        }
    }

    /// <summary>
    /// Remove current answers from screen and reset answers counter
    /// </summary>
    private void CleanAnswers()
    {
        answersCounter = 0;                                                         // Reset answers counter
        if (playerTextRoutine != null)                                              // Stop current coroutine if it is
        {
            StopCoroutine(playerTextRoutine);
        }
        if (answersDisplayRoutine != null)
        {
            StopCoroutine(answersDisplayRoutine);
        }
        List<GameObject> children = new List<GameObject>();                         // Get list of current answers
        foreach (Transform child in answerFolder.transform)
        {
            children.Add(child.gameObject);
        }
        answerFolder.transform.DetachChildren();
        foreach (GameObject child in children)
        {
            Destroy(child);                                                         // Destroy current answers
        }
    }

    /// <summary>
    /// Make active answers clickable
    /// </summary>
    private void EnableAnswersRaycast()
    {
        foreach (Transform child in answerFolder.transform)                         // Get current answers list from answers folder
        {
            AnswerHandler answerHandler = child.gameObject.GetComponent<AnswerHandler>();
            if (answerHandler != null)
            {
                if (answerHandler.active)                                           // If answer setted as clickable
                {
                    Text text = child.gameObject.GetComponent<Text>();
                    if (text != null)
                    {
                        text.raycastTarget = true;                                  // Enable text raycast
                    }
                }
            }
        }
    }

    /// <summary>
    /// Get all answers line from page
    /// </summary>
    /// <param name="page"> Page from CSV dialog descriptor </param>
    /// <returns> List of player answers lines </returns>
    private List<string> GetAnswersLines(string page)
    {
        List<string> res = new List<string>();
        List <string> lines = CsvParser.Instance.GetLines(page);                    // Get all lines from page
        if (lines != null)
        {
            foreach (string line in lines)
            {
                string lineName = CsvParser.Instance.GetLineName(line);             // Get name of line
                if (lineName != null)
                {
                    bool stuff = false;
                    foreach (string stuffLine in stuffLineName)
                    {
                        if (lineName == stuffLine)                                  // Compare with stuff lines names
                        {
                            stuff = true;
                            break;
                        }
                    }
                    if (stuff == false)
                    {
                        res.Add(line);                                              // If line is not stuff - add it to list
                    }
                }
            }
        }
        return res;
    }

    /// <summary>
    /// Display player answers one by one followed
    /// </summary>
    /// <param name="answers"> List of answers line and their flags of clickable state </param>
    /// <returns></returns>
    private IEnumerator AnswersDisplayCoroutine(Dictionary<string, bool> answers)
    {
        while ((typingDelay > 0) && (npcIsTalking == true))                         // Wait while NPC stop talking
        {
            yield return new WaitForSeconds(typingDelay);
        }
        foreach (KeyValuePair<string, bool> answer in answers)
        {
            string text;
                                                                                    // Get answer text from line
            if (CsvParser.Instance.GetTextValue(out text, languageSign, answer.Key) == true)
            {
                                                                                    // Add answer to answer folder
                AddAnswer(CsvParser.Instance.GetLineName(answer.Key), text, answer.Value);
                while ((typingDelay > 0) && (playerIsTalking == true))              // Wait for previous answer stop display
                {
                    yield return new WaitForSeconds(typingDelay);
                }
            }
        }
        EnableAnswersRaycast();                                                     // Make active answers clickable
    }

    private void TestPlace(string _place,Dictionary<string, bool> _answersLines)
    {
        
            herodata nowhero = Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];

            if(nowhero.NOWimportanceID==Global_const.NONE_ID)
            {
                 return;
            }

            XmlElement xl1 = Global_ImportanceData.getInstance().nowimportance_Elem_List[nowhero.NOWimportanceID];

            int tempplace = 0;

            try
            {
                tempplace = int.Parse(xl1.GetAttribute(_place));

            }
            catch
            {

            }

            if (1 == tempplace)
            {
                string addtemp = "__Line_" + _place + ";_Eng;_" + _place + ";";
                _answersLines.Add(addtemp, true);
            }

    }

    private void ShowPlaces(Dictionary<string, bool> _answersLines)
    {
       
        TestPlace("议事厅", _answersLines);
        TestPlace("民居", _answersLines);
        TestPlace("商铺", _answersLines);
        TestPlace("森林", _answersLines);
        TestPlace("田间", _answersLines);
        TestPlace("兵营", _answersLines);
        TestPlace("大市集", _answersLines);
        TestPlace("城门", _answersLines);

        /*   try
           {
               tempplace = int.Parse(xl1.GetAttribute("民居"));
           }
           catch
           {

           }

           if (1 == tempplace)
           {
               _answersLines.Add("__Line_民居;_Eng;_民居;", true);

           }

           try
           {
               tempplace = int.Parse(xl1.GetAttribute("商铺"));

           }
           catch
           {

           }

           if (1 == tempplace)
           {
               _answersLines.Add("__Line_商铺;_Eng;_商铺;", true);

           }

           try
           {
               tempplace = int.Parse(xl1.GetAttribute("野外"));

           }
           catch
           {

           }

           if (1 == tempplace)
           {
               _answersLines.Add("__Line_野外;_Eng;_野外;", true);

           }

           try
           {
               tempplace = int.Parse(xl1.GetAttribute("兵营"));

           }
           catch
           {

           }

           if (1 == tempplace)
           {
               _answersLines.Add("__Line_兵营;_Eng;_兵营;", true);

           }

           try
           {
               tempplace = int.Parse(xl1.GetAttribute("田间"));

           }
           catch
           {

           }

           if (1 == tempplace)
           {
               _answersLines.Add("__Line_田间;_Eng;_田间;", true);

           }

           try
           {
               tempplace = int.Parse(xl1.GetAttribute("城门"));

           }
           catch
           {

           }

           if (1 == tempplace)
           {
               _answersLines.Add("__Line_城门;_Eng;_城门;", true);

           }
           // _answersLines.Add("__Line_GOTOMEETINGROOM;_Eng;_进入议事厅;", true);
           */
    }

    /// <summary>
    /// Display all player answers from page
    /// </summary>
    /// <param name="page"> Page from CSV dialog descriptor </param>
    /// <returns> true - have active answers, false - no active answers </returns>
    private bool DisplayAnswers(string page)
    {
        bool res = false;
        if (page == null)
        {
            Debug.Log("Wrong input data");
            return res;
        }
        List<string> lines = GetAnswersLines(page);                                 // Get list of answers from page
        if ((lines != null) && (lines.Count > 0))
        {
            bool hasActiveAnswers = false;
            Dictionary<string, bool> answersLines = new Dictionary<string, bool>();
            foreach (string line in lines)
            {
                string answerName = CsvParser.Instance.GetLineName(line);           // Get answer name
                                                                                    // Get page named "answerName"
                if("SHOWPLACES"==answerName)
                {
                    ShowPlaces(answersLines);

                    hasActiveAnswers = true;
                    continue;
                }

                string answerPage = CsvParser.Instance.GetPage(answerName, dialogueCsv);
                                                                                    // Check for active answer requirements
                bool isActive = CheckAnswerRequirements(answerPage) && !IsAnswerBlocked(answerName);
                if ((answersMode == InactiveAnswersMode.Invisible) && isActive == false)
                {
                    continue;                                                       // If no need to display inactive answers - skip it
                }
                if (isActive)
                {
                    hasActiveAnswers = true;                                        // If at least one active answer - save it
                }
                answersLines.Add(line, isActive);                                   // Add answer to display list
            }
            if ((answersLines.Count > 0) && (hasActiveAnswers == true))             // If have active answers
            {
                res = true;
                if (answersDisplayRoutine != null)
                {
                    StopCoroutine(answersDisplayRoutine);                           // Stop current coroutine if it is
                }
                answersDisplayRoutine = AnswersDisplayCoroutine(answersLines);
                CleanAnswers();                                                     // Remove current answers
                StartCoroutine(answersDisplayRoutine);                              // Start coroutine
            }
        }
        return res;
    }

    /// <summary>
    /// Check if answer was clicked before in current dialog
    /// </summary>
    /// <param name="answerName"> Answer name </param>
    /// <returns> true - was clicked before, false - was not clicked </returns>
    private bool WasAnswerClickedBefore(string answerName)
    {
        return clickedAnswers.Contains(answerName);
    }

    /// <summary>
    /// Manualy block answer and make it inactive untill dialog end
    /// </summary>
    /// <param name="answerName"> Answer name </param>
    /// <param name="condition"> true - block answer, false - remove from blocked </param>
    private void BlockAnswer(string answerName, bool condition)
    {
        if (answerName != null)
        {
            if ((blockedAnswers.Contains(answerName) == false) && (condition == true))
            {
                blockedAnswers.Add(answerName);
            }
            else if ((blockedAnswers.Contains(answerName) == true) && (condition == false))
            {
                blockedAnswers.Remove(answerName);
            }
        }
    }

    /// <summary>
    /// Check if answer in blocked list
    /// </summary>
    /// <param name="answerName"> Answer name </param>
    /// <returns> true - answer blocked, false - answer not blocked manualy </returns>
    private bool IsAnswerBlocked(string answerName)
    {
        return blockedAnswers.Contains(answerName);
    }

    /// <summary>
    /// Check if answer meet requirements described in CSV dialog page
    /// </summary>
    /// <param name="page"> Page from CSV dialog descriptor with the same name as answer </param>
    /// <returns> true - meet requirements (active), false - fail requirements (inactive) </returns>
    private bool CheckAnswerRequirements(string page)
    {
        if (page == null)
        {
            Debug.Log("Wrong input data");
            return false;
        }
        bool res = true;
        List<string> reqLines = CsvParser.Instance.GetLines("Requirements", page);  // Get all lines named "Requirements" from page
        if (reqLines != null)
        {
            if (reqLines.Count > 0)
            {
                res = false;
            }
            ///
            /// Lines requirements will be united with logical OR
            ///
            foreach (string line in reqLines)
            {
                bool localRes = true;
                // Split line by value named "Req"
                List<string> reqs = CsvParser.Instance.SplitLineByValue("Req", line);
                if (reqs != null)
                {
                    ///
                    /// Field requirements (in one line) will be united with logical AND
                    ///
                    foreach (string req in reqs)
                    {
                        string data;
                        // Get requirement data from CSV
                        if (CsvParser.Instance.GetTextValue(out data, "Req", req) == true)
                        {
                            ///
                            /// Example: gold requirement
                            ///
                            if (data == "Gold")
                            {
                               /* int num;
                                if (CsvParser.Instance.GetNumValue(out num, data, line) == true)
                                {
                                    if (InventoryControl.Instance.GetGold() < num)
                                    {
                                        localRes = false;
                                        break;
                                    }
                                }
                                */
                                continue;
                            }
                            ///
                            /// Example: one timed answer
                            ///
                            else if (data == "OneOff")
                            {
                                if (WasAnswerClickedBefore(CsvParser.Instance.GetPageName(page)) == true)
                                {
                                    localRes = false;
                                    break;
                                }
                                continue;
                            }
                            ///
                            /// Example: free item slot in inventory
                            ///
                            else if (data == "FreeItemCell")
                            {
                              /*  if (InventoryControl.Instance.IsItemCellFree() == false)
                                {
                                    localRes = false;
                                    break;
                                }
                              */
                                continue;
                            }
                            ///
                            /// Example: player has no money
                            ///
                            else if (data == "NoGold")
                            {
                              /*  if (InventoryControl.Instance.GetGold() > 0)
                                {
                                    localRes = false;
                                    break;
                                }
                               */
                                continue;
                            }
                            ///
                            /// Template for new requirement
                            ///
                            else if (data == "MyOwnRequirement")
                            {
                                ///
                                /// Conditions
                                /// 
                                /// if conditions fails
                                /// localRes must be setted false
                                /// and break added
                                ///
                                continue;
                            }
                            else
                            {
                                Debug.Log("Unknown requirement");
                                continue;
                            }
                        }
                    }
                }
                res = res || localRes;                                              // Requirements lines united by OR and fields united by AND
            }
        }
        return res;
    }

    /// <summary>
    /// Aply effects described in clicked answer's page
    /// </summary>
    /// <param name="page"> Page from CSV dialog descriptor </param>
    /// <returns></returns>
	/// 
	/// //脚本响应
    private bool ApplyEffects(string page)
    {
        //Debug.Log("m_EventID:" + m_EventID);
        //Debug.Log("m_hero1_id:" + m_hero1_id);
        //Debug.Log("m_hero2_id:" + m_hero2_id);
        //herodata nowhero = Global_HeroData.getInstance().List_hero[m_hero1_id];
        //herodata targethero = Global_HeroData.getInstance().List_hero[m_hero2_id];

        //Tempstr="";
        if (page == null)
        {
            Debug.Log("Wrong input data");
            return false;
        }
        List<string> lines = CsvParser.Instance.GetLines(page);                     // Get all lines from page
        if (lines != null)
        {
            foreach (string line in lines)
            {
                string lineName = CsvParser.Instance.GetLineName(line);             // Get line name
                if (lineName == "Effect")                                           // Find lines named "Effect"
                {
                    string data;
                    // Get effect data
                    if (CsvParser.Instance.GetTextValue(out data, lineName, line) == true)
                    {
                        ///
                        /// Example: end dialog
                        ///
						if (data == "Exit") {
							EndDialogue ();
							//InventoryControl.Instance.ResetInventory();
							tempcount = 0;
							EventPlane.GetComponent<UI_OBJ> ().moveout ();
							return true;
						}
                        ///
                        /// Example: add or remove gold
                        ///
                        else if (data == "Gold") {
							/*  int num;
                            if (CsvParser.Instance.GetNumValue(out num, data, line) == true)
                            {
                                InventoryControl.Instance.AddGold(num);
                            }
                            */
							continue;
						}
                        ///
                        /// Example: add food
                        ///
                        else if (data == "Food") {
							/*  int num;
                            if (CsvParser.Instance.GetNumValue(out num, data, line) == true)
                            {
                                InventoryControl.Instance.AddFood(num);
                            }
                            */
							continue;
						}
                        ///
                        /// Example: add item to inventory
                        ///
                        else if (data == "Item") {
							/* if (CsvParser.Instance.GetTextValue(out data, data, line) == true)
                            {
                                if (data == "Add")
                                {
                                    if (CsvParser.Instance.GetTextValue(out data, data, line) == true)
                                    {
                                        InventoryControl.Instance.AddItem(data);
                                    }
                                }
                            }
                            */
							continue;
						} else if (data == "JUMPTO") {
							Jumpto (data, line);
							continue;
						} else if (data == "SHOWUI") {
							EventPlane.GetComponent<UI_OBJ> ().move ();

							continue;
						} else if (data == "SHOWPC") {
							Object_PC.SetActive (true);
							head_image_PC.sprite = Global_source_loader.getInstance ().hero_L_face [m_hero1_id];

							continue;
						} else if (data == "HIDEPC") {

							Object_PC.SetActive (false);

						} else if (data == "SHOWNPC") {
							int num;
							if (CsvParser.Instance.GetNumValue (out num, data, line) == true) {
								head_image_NPC.sprite = Global_source_loader.getInstance ().hero_L_face [num];
							} else {
								head_image_NPC.sprite = Global_source_loader.getInstance ().hero_L_face [m_hero2_id];
							}

							Object_NPC.SetActive (true);

							continue;
						}
                        else if (data == "HIDENPC")
                        {

                            Object_NPC.SetActive(false);

                            continue;
                        }
                        ///
                        /// Example: manualy block answer and make it inactive
                        ///
                        else if (data == "BlockIt") {
							BlockAnswer (CsvParser.Instance.GetPageName (page), true);
							continue;
						}
                        ///
                        /// Example: play sound
                        ///
                        else if (data == "Sound") {
							if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {
								SoundLoader.Instance.PlaySound (data);
							}
							continue;
						} else if (data == "WAITANDRELOAD") {
							///
							/// Effect handler
							///
							ReloadProcess ();
							//timeScrollbar.gameObject.SetActive (true);
							continue;
						} else if (data == "IF") {
                            // IF;_wit;_>;_100;_1000;_place1;_1000;_place2;;;

                            if (CsvParser.Instance.GetTextValue(out data, data, line) == true)
                            {
                                string classename = data;

                                object temp=null;

                                switch (classename)
                                {
                                    case "Hero":

                                        herodata tempherodata = Global_HeroData.getInstance().List_hero[m_hero1_id];

                                        temp = tempherodata;

                                        break;
                                    case "Guard":
                                        Guardunit tempguardunitdata = Global_GuardunitData.getInstance().List_guardunit[m_hero1_id];

                                        temp = tempguardunitdata;

                                        break;
                                    default:
                                        break;


                                }

                                if (CsvParser.Instance.GetTextValue(out data, data, line) == true)
                                {
                                    string attributename = data;
                                    
                                    int attributedatatemp = (int)temp.GetType().GetField(attributename).GetValue(temp);



                                    string symbol;
                                    if (CsvParser.Instance.GetTextValue(out symbol, "SYMBOL", line) == true)
                                    {

                                        string attributedatatemp_contrast;

                                        if (CsvParser.Instance.GetTextValue(out attributedatatemp_contrast, "CONTRAST", line) == true)
                                        {

                                            //place1 part1 place2 part2
                                            string event1 = "", event2 = "", page1 = "", page2 = "";

                                            if (CsvParser.Instance.GetTextValue(out event1, "EVENT1", line) == true)
                                            {
                                                if (CsvParser.Instance.GetTextValue(out page1, "PAGE1", line) == true)
                                                {
                                                    if (CsvParser.Instance.GetTextValue(out event2, "EVENT2", line) == true)
                                                    {
                                                        if (CsvParser.Instance.GetTextValue(out page2, "PAGE2", line) == true)
                                                        {


                                                        }

                                                    }

                                                }

                                            }


                                            switch (symbol)
                                            {
                                                case "<":

                                                    if (attributedatatemp < int.Parse(attributedatatemp_contrast))
                                                    {
                                                        Global_events.getInstance().traggerGlobalEvent(int.Parse(event1), 1, m_hero1_id, m_hero2_id, page1);
                                                    }
                                                    else
                                                    {
                                                        Global_events.getInstance().traggerGlobalEvent(int.Parse(event2), 1, m_hero1_id, m_hero2_id, page2);
                                                    }
                                                    break;
                                                case ">":
                                                    if (attributedatatemp > int.Parse(attributedatatemp_contrast))
                                                    {
                                                        Global_events.getInstance().traggerGlobalEvent(int.Parse(event1), 1, m_hero1_id, m_hero2_id, page1);
                                                    }
                                                    else
                                                    {
                                                        Global_events.getInstance().traggerGlobalEvent(int.Parse(event2), 1, m_hero1_id, m_hero2_id, page2);
                                                    }
                                                    break;
                                                case "=":
                                                    if (attributedatatemp == int.Parse(attributedatatemp_contrast))
                                                    {
                                                        Global_events.getInstance().traggerGlobalEvent(int.Parse(event1), 1, m_hero1_id, m_hero2_id, page1);
                                                    }
                                                    else
                                                    {
                                                        Global_events.getInstance().traggerGlobalEvent(int.Parse(event2), 1, m_hero1_id, m_hero2_id, page2);
                                                    }
                                                    break;
                                            }

                                        }


                                    }

                                }
                            }
							continue;
						} else if (data == "ROLL") {

							string olddata = "";
							while (CsvParser.Instance.GetTextValue (out data, data, line) == true) {
								//SoundLoader.Instance.PlaySound(data);

								if (data == "") {
									StartDialogue (m_EventID, olddata);

									continue;
								}
								olddata = data;

								//Global_events.getInstance ().traggerGlobalEvent (m_EventID, 1, m_hero1_id, m_hero2_id,data);
								if (0 == Random.Range (0, 2)) {
									//Debug.Log(data);
									StartDialogue (m_EventID, data);
									break;
								}
							}

							continue;
						} else if (data == "SHOWMEET") {

                           if(CsvParser.Instance.GetTextValue(out data, data, line) == true)
                            { 

                            Global_HeroData.getInstance().SortHeroList(int.Parse(data));

                            Global_UI.getInstance ().MeetList.GetComponent<UI_OBJ>().move();
                            Global_UI.getInstance().MeetList.transform.FindChild("search_items").GetComponent<UI_OBJ>().moveout();
                            Global_UI.getInstance().MeetList.transform.FindChild("Panel_herodatalist_select").GetComponent<hero_selectscript>().invalidatelist();
                           
                            

                            }
                            continue;

						}
                        else if(data == "录志")
                        {
                            this.Message.text = "正在书写自传";

                            StartCoroutine(AutoSave());


                            continue;
                        }
                        else if (data == "HIDEMEET") {

                            Global_UI.getInstance ().MeetList.GetComponent<UI_OBJ> ().moveout ();
							continue;

						} else if (data == "SETATTRIBUTE") {
							//attribute name
							if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {
								string attributename = data;

								//attribute 
								if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {

									try {
										herodata tempherodata = Global_HeroData.getInstance ().List_hero [m_hero1_id];

										int attributedatatemp = (int)tempherodata.GetType ().GetField (attributename).GetValue (tempherodata);
										int addtemp = int.Parse (data);
										addtemp = addtemp * Random.Range (100, 150) / 100;

										//修正值
										string reviseattribute = "";
										if (CsvParser.Instance.GetTextValue (out reviseattribute, data, line) == true) {
											if (reviseattribute != "") {
												Debug.Log (reviseattribute);
												try {
													addtemp += (int)tempherodata.GetType ().GetField (reviseattribute).GetValue (tempherodata);
												} catch {

												}
											}
										}


										attributedatatemp += addtemp;
										tempherodata.GetType ().GetField (attributename).SetValue (tempherodata, attributedatatemp);
									
										string attributename_CHINESE = (string)attributeHash [attributename];

										Tempstr = attributename_CHINESE + "增加" + addtemp;
									} catch {

									}
								}
							
							}
							continue;

						} else if (data == "TRANARMYSUCCESS") {

							herodata tempherodata = Global_HeroData.getInstance ().List_hero [m_hero1_id];
                            Guardunit tempguardunit= Global_GuardunitData.getInstance().List_guardunit[m_hero1_id];

                            int addtemp = tempherodata.lead * Random.Range (0, 150) / 200;
                            tempguardunit.Morale += addtemp;

							if (tempguardunit.Morale >= 255) {
                                tempguardunit.Morale = 255;
								Tempstr = "士气并没有增加";
							} else {
								if (addtemp > 0) {
									Tempstr = "士气增加" + addtemp;
								} else {
									Tempstr = "士气并没有增加";
								}
							}
							continue;

						} else if (data == "CHAT") {

							chat ();
							continue;

						} else if (data == "FIGHT") {
							fight ();
							continue;

						} else if (data == "STEPJUMP") {
							//下一步触发place
							string stepevent = "";
							string stepplace = "";
							string stepstep = "";

							if (CsvParser.Instance.GetTextValue (out stepevent, "EVENT", line) == true) {
								if (CsvParser.Instance.GetTextValue (out stepplace, "PLACE", line) == true) {
									if (CsvParser.Instance.GetTextValue (out stepstep, "STEP", line) == true) {

										if (Global_events.getInstance ().EventTrigger_list [int.Parse (stepevent)].step == -1) {

											continue;
										}
										Global_events.getInstance ().EventTrigger_list [int.Parse (stepevent)].place = int.Parse (stepplace);
										Global_events.getInstance ().EventTrigger_list [int.Parse (stepevent)].step = int.Parse (stepstep);
									}

								}

							}
							continue;

						} else if (data == "HERODATA") {
                            //Global_UI.getInstance ().HerodataUI.GetComponent<hero_datashow_script> ().hero_id = m_hero2_id;
                            //Global_UI.getInstance ().HerodataUI.GetComponent<hero_datashow_script> ().move ();

                            continue;

						} else if (data == "MESSAGE") {

							if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {

                                if("CITYNAME"== data)
                                {
                                    //herodata nowhero = Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];
                                    data = "";

                                    herodata nowhero = Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];

                                    if(nowhero.NOWimportanceID==Global_const.NONE_ID)
                                    {
                                        continue;
                                    }
                                    XmlElement xl1 = Global_ImportanceData.getInstance().nowimportance_Elem_List[nowhero.NOWimportanceID];

                                    try
                                    {
                                        
                                        data += xl1.GetAttribute("M_name");
                                        data += "\n";
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {

                                        data += xl1.GetAttribute("介绍");
                                        //data += '\n';
                                    }
                                    catch
                                    {

                                    }

                                }

                                GlobalData.getInstance().global_message = data +"\n"+ GlobalData.getInstance().global_message;

                                UpdateGlobalMessage();

                                //Message.text = data + Tempstr;
								//Tempstr = "";
                                //Message.gameObject.SetActive(true);
                                // PC_SAY_back.GetComponent<UI_OBJ>().move();
                                Global_UI.getInstance().TextBack.GetComponent<UI_OBJ>().move();
                            }
							continue;

						} else if (data == "PLACECHANGE") {

							if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {

                                    Messenger.Broadcast<int>("PLACECHANGE", int.Parse(data));

							}

							tempcount = 0;

							continue;

						} else if (data == "PLACESHOW") {

							if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {

                                    Messenger.Broadcast<int>("PLACECHANGE", int.Parse(data));

							}
							continue;

						}
                        else if(data == "SHOWPLACES")
                        {
                            Debug.Log("SHOWPLACES");
                            //XmlElement importance_Elem_temp = Global_XML_IO.getInstance().nowkindom_Elem_List[GlobalData.getInstance().nowimportanceID];
                            XmlElement xl1 = Global_ImportanceData.getInstance().nowimportance_Elem_List[GlobalData.getInstance().nowimportanceID];

                            int tempplace = 0;
                    
                            try
                            {
                                tempplace = int.Parse(xl1.GetAttribute("宫殿"));
                                //Debug.Log(tempplace);
                            }
                            catch
                            {
                                //importance_Elem_temp.SetAttribute("Places", "0");

                            }

                            if(1==tempplace)
                            {
                                //Debug.Log("宫殿");
                                //AddAnswer("议事厅", "议事厅", true);

                                //DisplayAnswers("__Line_PLACEINSIDE; _Eng;入城;");
                            }
                            
                            continue;
                        }
                        else if (data == "BACKSHOW") {
							if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {

                                    Messenger.Broadcast<int>("BACKSHOW", int.Parse(data));

							}
							continue;

						}
                        else if(data == "ArmyCreate")//生成军团
                        {

                            Global_Armydata.getInstance().ArmyCreate(m_hero1_id);

                        }
                        else if(data == "显示全局信息")
                        {
                            Message.text = GlobalData.getInstance().global_message;

                        }
                        else if (data == "同盟")//同盟
                        {
                           

                            int targetKindomID = Global_HeroData.getInstance().List_hero[m_hero2_id].m_relationship.belong_kindom;

                            if(Global_const.NONE_ID == targetKindomID)
                            {
                                continue;
                            }

                            int pcKindomID= Global_HeroData.getInstance().List_hero[m_hero1_id].m_relationship.belong_kindom;

                            if (Global_const.NONE_ID == pcKindomID)
                            {
                                continue;

                            }
                            Global_KindomData.getInstance().list_KindomData[pcKindomID].setRelation(targetKindomID,"同盟");
                            Global_KindomData.getInstance().list_KindomData[targetKindomID].setRelation(pcKindomID,"同盟");

                            Debug.Log("同盟成立");

                            continue;                            

                        }
                        else if(data == "招募")
                        {
                            herodata pchero = Global_HeroData.getInstance().List_hero[m_hero1_id];
                            herodata targethero = Global_HeroData.getInstance().List_hero[m_hero2_id];

                            if(targethero.m_relationship.belong_kindom != Global_const.NONE_ID)
                            {

                                if(targethero.m_relationship.belong_kindom== pchero.m_relationship.belong_kindom)
                                {
                                    //GlobalData.getInstance().global_message = targethero.GetAllName()+"是同一阵营\n" + GlobalData.getInstance().global_message;
                                    Global_events.getInstance().traggerGlobalEvent(379, 0, m_hero1_id, m_hero2_id, "begin");
                                    this.npcSay.text = "您开什么玩笑，我们已经是同僚了啊";
                                }
                                else
                                {
                                    KindomData temptargetkindom = Global_KindomData.getInstance().list_KindomData[targethero.m_relationship.belong_kindom];
                                    if (temptargetkindom.KingID == targethero.id)
                                    {
                                        Global_events.getInstance().traggerGlobalEvent(m_EventID, 0, m_hero1_id, m_hero2_id, "招募失败");
                                    }
                                    else
                                    {

                                        Global_Armydata.getInstance().List_army[m_hero2_id].Surrender(m_hero1_id);
                                        Global_events.getInstance().traggerGlobalEvent(m_EventID, 0, m_hero1_id, m_hero2_id, "招募成功");
                                    }
                                }

                            }
                            else
                            {
                                Global_Armydata.getInstance().List_army[m_hero2_id].Surrender(m_hero1_id);

                                //pchero.m_relationship.AddSubordinate(targethero);
                                Global_events.getInstance().traggerGlobalEvent(m_EventID, 0, m_hero1_id, m_hero2_id, "招募成功");
                            }

                            continue;
                        }
                        else if (data == "开战")//开战
                        {

                            int targetKindomID = Global_HeroData.getInstance().List_hero[m_hero2_id].m_relationship.belong_kindom;

                            if (Global_const.NONE_ID == targetKindomID)
                            {
                                continue;
                            }

                            int pcKindomID = Global_HeroData.getInstance().List_hero[m_hero1_id].m_relationship.belong_kindom;

                            if (Global_const.NONE_ID == pcKindomID)
                            {
                                continue;

                            }

                            Global_KindomData.getInstance().list_KindomData[pcKindomID].setRelation(targetKindomID, "开战");
                            Global_KindomData.getInstance().list_KindomData[targetKindomID].setRelation(pcKindomID, "开战");

                            Debug.Log("开战");
                            continue;
                        }
                        else if (data == "停战")//停战
                        {

                            int targetKindomID = Global_HeroData.getInstance().List_hero[m_hero2_id].m_relationship.belong_kindom;

                            if (Global_const.NONE_ID == targetKindomID)
                            {
                                continue;
                            }

                            int pcKindomID = Global_HeroData.getInstance().List_hero[m_hero1_id].m_relationship.belong_kindom;

                            if (Global_const.NONE_ID == pcKindomID)
                            {
                                continue;

                            }

                            Global_KindomData.getInstance().list_KindomData[pcKindomID].setRelation(targetKindomID, "停战");
                            Global_KindomData.getInstance().list_KindomData[targetKindomID].setRelation(pcKindomID, "停战");

                            Debug.Log("停战");
                            continue;
                        }
                        else if (data == "BATTLE") {

							//Global_UI.getInstance ().BattleList.GetComponent<UI_OBJ> ().move ();
							continue;

						}
                        else if (data == "MOVEARMY")
                        {
                            bool CanMove = false;

                            if (CsvParser.Instance.GetTextValue(out data, data, line) == true)
                            {
                                  switch(data)
                                {
                                    case "UP":
                                        CanMove=Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.UP);

                                        break;
                                    case "DOWN":
                                        CanMove = Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.DOWN);

                                        break;
                                    case "LEFT":
                                        CanMove = Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.LEFT);

                                        break;
                                    case "RIGHT":
                                        CanMove = Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.RIGHT);

                                        break;
                                    case "WAIT":
                                        CanMove = Global_Armydata.getInstance().MovePCArmy((int)Armydata.direct.WAIT);

                                        break;
                                }
                            }
                                //Global_UI.getInstance().BattleList.GetComponent<UI_OBJ>().move();

                            if(CanMove ==true)
                            {
                                continue;
                            }
                            else
                            {
                                GlobalData.getInstance().global_message = "城门被堵住，无法出城\n" + GlobalData.getInstance().global_message;
                                UpdateGlobalMessage();
                                continue;
                            }

                        }
                        else if (data == "偶遇")
                        {
                            Importance tempnowimportance = Global_ImportanceData.getInstance().List_importance[Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid].NOWimportanceID];

                            if(tempnowimportance.heroCount<1)
                            {
                                GlobalData.getInstance().global_message = "你遇到一个气宇轩昂的人\n但是仔细聊过之后发现只是一个资质平平的人\n";

                                this.UpdateGlobalMessage();

                                ReloadProcess();
                                continue;
                            }

                            int temptargetheroID = Random.Range(0, tempnowimportance.heroCount-1);
                            herodata targethero = Global_HeroData.getInstance().List_hero[tempnowimportance.heroID_list[temptargetheroID]];
                            herodata nowhero = Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];

                            if(targethero.reputation<= nowhero.reputation)
                            {
                                if(true==nowhero.m_relationship.friendsLikeHash.Contains(targethero.id))
                                {
                                    GlobalData.getInstance().global_message = "你偶遇了" + targethero.GetAllName()+"相谈甚欢" + "\n"+ GlobalData.getInstance().global_message;
                                    this.UpdateGlobalMessage();

                                    nowhero.UpdateLike(10, targethero.id);
                                    targethero.UpdateLike(10, nowhero.id);
                                   
                                }
                                else
                                {
                                    GlobalData.getInstance().global_message = "你遇到一个气宇轩昂的人\n他自称" + targethero.GetAllName() + "\n" + GlobalData.getInstance().global_message;
                                    this.UpdateGlobalMessage();
                                    nowhero.m_relationship.AddFriends(targethero);
                                    targethero.m_relationship.AddFriends(nowhero);

                                }

                                ReloadProcess(); 

                            }
                            else
                            {
                                GlobalData.getInstance().global_message = "你遇到一个气宇轩昂的人\n但是对方根本不想理你\n" + GlobalData.getInstance().global_message;
                                this.UpdateGlobalMessage();

                                ReloadProcess();
                            }

                            continue;

                        }
                        else if (data == "SHOWPIC") {
							///
							/// Effect handler
							///
							int num;
							if (CsvParser.Instance.GetNumValue (out num, data, line) == true) {
								event_image.sprite = Global_source_loader.getInstance ().Event_Sprite [num];
								event_image.gameObject.SetActive (true);
							}
							continue;
						}
                        else if (data == "军团信息")
                        {
                            Armydata temparmy = Global_Armydata.getInstance().List_army[this.m_hero2_id];
                            herodata templeader = Global_HeroData.getInstance().List_hero[this.m_hero2_id];

                            string tempmessage = "";
                            tempmessage += "几个时辰后，获得了"+templeader.GetAllName() + "军团的信息" + "\n" + GlobalData.getInstance().global_message;
                            tempmessage += "主将:" + Global_HeroData.getInstance().List_hero[int.Parse(temparmy.this_Elem.GetAttribute("主将"))].GetAllName() + " ";
                            tempmessage += "军师:" + Global_HeroData.getInstance().List_hero[int.Parse(temparmy.this_Elem.GetAttribute("军师"))].GetAllName() + " ";
                            tempmessage += "先锋:" + Global_HeroData.getInstance().List_hero[int.Parse(temparmy.this_Elem.GetAttribute("先锋"))].GetAllName() + " ";
                            tempmessage += "\n";

                            tempmessage += "哨马:" + Global_HeroData.getInstance().List_hero[int.Parse(temparmy.this_Elem.GetAttribute("哨马"))].GetAllName() + " ";
                            tempmessage += "辎重:" + Global_HeroData.getInstance().List_hero[int.Parse(temparmy.this_Elem.GetAttribute("辎重"))].GetAllName() + " ";

                            tempmessage += "\n";
                            tempmessage += "\n";
                            tempmessage += "\n";
                            tempmessage += "\n";
                            tempmessage += "\n";
                            tempmessage += "\n";

                            GlobalData.getInstance().global_message = tempmessage +"\n"+ GlobalData.getInstance().global_message;

                            UpdateGlobalMessage();
                            continue;
                        }
                        else if (data == "城市信息")
                        {
                            Importance tempimportance = Global_ImportanceData.getInstance().List_importance[m_EventParam];
                            string tempmessage = "";
                            tempmessage += tempimportance.M_name + " ID:" + m_EventParam + "\n";
                            tempmessage += "城中武将数:" + tempimportance.heroCount + "\n";

                            for (int i = 0; i < tempimportance.heroCount; i++)
                            {
                                int heroIDtemp = tempimportance.heroID_list[i];

                                if (heroIDtemp == Global_const.NONE_ID)
                                {
                                    continue;
                                }

                                tempmessage += Global_HeroData.getInstance().List_hero[heroIDtemp].GetAllName() + "  ";

                                if ((i + 1) % 7 == 0)
                                {
                                    tempmessage += "\n";
                                }
                            }


                            tempmessage += "\n";
                            tempmessage += "\n";
                            tempmessage += "\n";
                            tempmessage += "\n";

                            GlobalData.getInstance().global_message = tempmessage + GlobalData.getInstance().global_message;
                            UpdateGlobalMessage();

                            continue;
                        }
                        else if (data == "NPCNAME") {
							
							if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {
								if (data == "TARGET") {
									npcName.text = Global_HeroData.getInstance ().List_hero [m_hero2_id].GetAllName ();
								} else {
									npcName.text = data;
								}
							}
							continue;
						} else if (data == "PCNAME") {

							if (CsvParser.Instance.GetTextValue (out data, data, line) == true) {
								if (data == "") {
									PCName.text = Global_HeroData.getInstance ().List_hero [m_hero1_id].GetAllName ();
								} else {
									PCName.text = data;
								}
							}
							continue;
						}
                        else if(data == "认识")
                        {
                            herodata nowherodata = Global_HeroData.getInstance().List_hero[m_hero1_id];
                            herodata targetherodata = Global_HeroData.getInstance().List_hero[m_hero2_id];

                            nowherodata.m_relationship.AddFriends(targetherodata);

                            string tempstr = "认识了" + m_hero2_id + "\n";
                            GlobalData.getInstance().global_message = tempstr + GlobalData.getInstance().global_message;

                            this.UpdateGlobalMessage();

                            continue;
                        }
                        else if (data == "MOVECAM")
                        {

                            if (CsvParser.Instance.GetTextValue(out data, data, line) == true)
                            {

                                herodata temphero;
                                if (data == "1")
                                {
                                    temphero = Global_HeroData.getInstance().List_hero[m_hero1_id];

                                    Messenger.Broadcast<float, float, float>("MoveCam", temphero.GetPosition().x, temphero.GetPosition().y, 1);
                                }
                                else if (data == "2")
                                {
                                    temphero = Global_HeroData.getInstance().List_hero[m_hero2_id];
                                    Messenger.Broadcast<float, float, float>("MoveCam", temphero.GetPosition().x, temphero.GetPosition().y, 1);

                                }
                            }
                            continue;
                        }
                        else if (data == "TRANARMY") {
							herodata herotemp = Global_HeroData.getInstance ().List_hero [m_hero1_id];
                            Guardunit tempguardunit = Global_GuardunitData.getInstance().List_guardunit[m_hero1_id];

                            if (tempguardunit.Armycount > 0) {
								Global_events.getInstance ().traggerGlobalEvent (402, 1, m_hero1_id, m_hero2_id);

							} else {
								Global_events.getInstance ().traggerGlobalEvent (1000, 1, m_hero1_id, m_hero2_id, "NOARMY");

							}
							continue;
						}
						else if (data == "CONSCRIPTION") {

							Conscription();

							continue;
						}
                        else if(data == "释放")
                        {
                            if(false==Global_Armydata.getInstance().List_army[m_hero2_id].Flee())
                            {
                                Global_HeroData.getInstance().List_hero[m_hero2_id].Die(m_hero1_id);
                            }

                            continue;
                        }
                        else if (data == "斩首")
                        {
                         
                            Global_HeroData.getInstance().List_hero[m_hero2_id].Die(m_hero1_id);

                            continue;
                        }
                        else
                        {
                            Debug.Log("Unknown effect");
                            continue;
                        }
                    }


                }
            }

            MessageScroll = GameObject.Find("Canvas/mainui/SceneManager/Event_plane/Back/Scroll View");
            MessageScroll.GetComponent<ScrollRect>().verticalScrollbar.value = 1;

        }


        timeScrollbar.value = 0;
        return false;
    }

	void Conscription ()
	{

		herodata tempherodata = Global_HeroData.getInstance ().List_hero [m_hero1_id];
        Guardunit tempguardunit = Global_GuardunitData.getInstance().List_guardunit[m_hero1_id];

        if (tempguardunit.Armycount > tempherodata.GetMaxReP())
        {
            Tempstr = "没有足够的影响力";

        }
        else
        {
            int addtemp = tempherodata.charm * Random.Range(100, 150) / 100;

            if (tempherodata.money >= addtemp)
            {

                tempguardunit.Armycount += addtemp;
                tempherodata.money -= addtemp;
                Tempstr = "士兵增加" + addtemp;

            }
            else
            {
                Tempstr = "没有足够的金钱";
            }
        }

	}

	void Jumpto (string _data,string _line)
	{
		string num;
		if (CsvParser.Instance.GetTextValue (out num, _data, _line) == true) {
			//InventoryControl.Instance.AddFood(num);

			if (CsvParser.Instance.GetTextValue (out _data, num, _line) == true) {

				if (_data == "") {
					Global_events.getInstance ().traggerGlobalEvent (int.Parse(num), 1, m_hero1_id, m_hero2_id);
				} else {
					Global_events.getInstance ().traggerGlobalEvent (int.Parse(num), 1, m_hero1_id, m_hero2_id, _data);
				}

			}
		}
	}

	void chat ()
	{
		npcSay.text = "闲聊闲聊";
	}

	void fight()
	{
		Global_fighter_Sprites.getInstance().Init(m_hero1_id,m_hero2_id);
		SceneManager.LoadScene("fight");// .LoadScene("fight")
	}

    void UpdateGlobalMessage()
    {
        int Textlenthtemp = 5000;

        if(GlobalData.getInstance().global_message.Length>=Textlenthtemp)
        {
            this.Message.text = GlobalData.getInstance().global_message.Substring(0, 5000);
        }
        else
        {
            this.Message.text = GlobalData.getInstance().global_message;
        }

    }

    public void SetGlobalMessage(string _newMessage)
    {
        GlobalData.getInstance().global_message = _newMessage + "\n" + GlobalData.getInstance().global_message;

        UpdateGlobalMessage();
    }

    public IEnumerator AutoSave()
       {

        yield return new WaitForSeconds(1);

           StartCoroutine(Global_ImportanceData.getInstance().Save_importance_Xml("/xml_importancelist.xml"));
           StartCoroutine(Global_XML_IO.getInstance().Save_hero_Xml("/xml_herolist.xml"));
           StartCoroutine(Global_XML_IO.getInstance().Save_kindom_Xml("/xml_kindomlist.xml"));
           StartCoroutine(Global_Armydata.getInstance().Save_army_Xml("/xml_armylist.xml"));

          this.Message.text = "自传书写完毕";

    }

}
