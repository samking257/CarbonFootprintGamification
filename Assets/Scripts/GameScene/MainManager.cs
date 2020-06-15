using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject txtHealth;
    public GameObject txtLevel;
    public GameObject avatar;
    public GameObject bee;
    public GameObject bee2;
    public GameObject worm;

    public GameObject SpeachBubble;
    public GameObject Emote;
    public Sprite[] sprites = new Sprite[5];
    public GameObject healthPanel;
    public GameObject exitPanel;
    public GameObject exitButton;

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform taskContainer;

    public GameObject taskPanel;
    public GameObject taskTitle;
    public GameObject taskDescription;
    public GameObject taskPoints;
    public GameObject completedButton;
    public GameObject completedText;

    public GameObject GameoverPanel;
    public GameObject txtHours;

    public GameObject background1;
    public GameObject background2;
    public GameObject background3;
    public GameObject background4;

    public GameObject ground1;
    public GameObject ground2;
    public GameObject ground3;
    public GameObject ground4;

    private List<List<string>> dailyTasks = new List<List<string>>();
    private List<List<string>> weeklyTasks = new List<List<string>>();
    private bool completed = false;
    private int healthInt;

    // Start is called before the first frame update
    void Start()
    {
        createTaskButtons();
        
        if(!PlayerPrefs.HasKey("StartTime")){
            PlayerPrefs.SetString("StartTime", DateTime.Now.ToString());
            SpeachBubble.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        txtHealth.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("health");
        beeFlying();

        healthInt = Convert.ToInt32(txtHealth.GetComponent<Text>().text);
        setLevel(healthInt);

        if(healthInt > 501){
            setStage1();
        } else if(healthInt < 501 && healthInt > 251){
            setStage2();
        } else if(healthInt < 251){
            setStage3();
        } 
        if(healthInt == 0){
            setStage4();
        }
    }

    void setLevel(int healthNum){
        int level = 1;
        if(PlayerPrefs.HasKey("PlayerLevel")){
            level = PlayerPrefs.GetInt("PlayerLevel");
        } 
        txtLevel.GetComponent<Text>().text = level.ToString();
        switch (level)
            {
                case 1:
                    PlayerPrefs.SetInt("TotalDailyTasks", 2);
                    PlayerPrefs.SetInt("TotalWeeklyTasks", 1);
                    PlayerPrefs.SetInt("hourPoints", 5);
                    if(healthInt > 750){
                        PlayerPrefs.SetInt("PlayerLevel", 2);
                        createTaskButtons();
                    }
                    break;
                case 2:
                    PlayerPrefs.SetInt("TotalDailyTasks", 3);
                    PlayerPrefs.SetInt("TotalWeeklyTasks", 2);
                    PlayerPrefs.SetInt("hourPoints", 6);
                    if(healthInt > 1000){
                        PlayerPrefs.SetInt("PlayerLevel", 3);
                        createTaskButtons();
                    }
                    break;
                case 3:
                    PlayerPrefs.SetInt("TotalDailyTasks", 4);
                    PlayerPrefs.SetInt("TotalWeeklyTasks", 3);
                    PlayerPrefs.SetInt("hourPoints", 7);
                    if(healthInt > 1500){
                        PlayerPrefs.SetInt("PlayerLevel", 4);
                        createTaskButtons();
                    }
                    break;
                case 4:
                    PlayerPrefs.SetInt("TotalDailyTasks", 5);
                    PlayerPrefs.SetInt("TotalWeeklyTasks", 3);
                    PlayerPrefs.SetInt("hourPoints", 9);
                    if(healthInt > 2500){
                        PlayerPrefs.SetInt("PlayerLevel", 5);
                        createTaskButtons();
                    }
                    break;
                case 5:
                    PlayerPrefs.SetInt("TotalDailyTasks", 6);
                    PlayerPrefs.SetInt("TotalWeeklyTasks", 4);
                    PlayerPrefs.SetInt("hourPoints", 10);
                    break;
            }
        
    }

    void createTaskButtons(){
        List<List<string>> buttonTasks = new List<List<string>>();
        setTaskLists();
        DateTime currentDate = DateTime.Now;
        if(PlayerPrefs.HasKey("DailyTask1")){
            int totalDailyTasks = PlayerPrefs.GetInt("TotalDailyTasks");
            if(PlayerPrefs.GetString("SavedDate") != getStringTime()){
                List<int> checkRandom = new List<int>();
                for (int i = 0; i < totalDailyTasks; i++){
                    int r;
                    while(true){
                        r = UnityEngine.Random.Range(0, dailyTasks.Count);
                        if(!checkRandom.Contains(r)){
                            checkRandom.Add(r);
                            break;
                        }
                    }
                    buttonTasks.Add(dailyTasks[r]);     
                    String taskId = "DailyTask" + i;    
                    List<string> task = dailyTasks[r];
                    PlayerPrefs.SetInt(taskId, Convert.ToInt32(task[0]));
                }
                PlayerPrefs.SetString("SavedDate", getStringTime());
                PlayerPrefs.DeleteKey("CompletedDaily");

            } else{
                //Set existing
                for(int i = 0; i < totalDailyTasks; i++){
                    String taskId = "DailyTask" + i;
                    int ID = PlayerPrefs.GetInt(taskId);
                    buttonTasks.Add(dailyTasks[ID]); 
                }
            }
            
            //Check Weekly
            int totalWeeklyTasks =  PlayerPrefs.GetInt("TotalWeeklyTasks");
            if(PlayerPrefs.GetInt("SavedWeek") != getWeekNum(currentDate)){  
                List<int> checkRandom = new List<int>();
                for (int i = 0; i < totalWeeklyTasks; i++){
                    int r;
                    while(true){
                        r = UnityEngine.Random.Range(0, weeklyTasks.Count);
                        if(!checkRandom.Contains(r)){
                            checkRandom.Add(r);
                            break;
                        }
                    }
                    buttonTasks.Add(weeklyTasks[r]);
                    String taskId = "WeeklyTask" + i;
                    List<string> task = dailyTasks[r];
                    PlayerPrefs.SetInt(taskId, Convert.ToInt32(task[0]));
                    totalWeeklyTasks++;
                }
                PlayerPrefs.SetInt("SavedWeek", getWeekNum(currentDate));
                PlayerPrefs.DeleteKey("CompletedWeekly");

            } else{
                //Set exisitng
                for(int i = 0; i < totalWeeklyTasks; i++){
                    String taskId = "WeeklyTask" + i;
                    int ID = PlayerPrefs.GetInt(taskId);
                    buttonTasks.Add(weeklyTasks[ID]);
                }
            }

        } else{
            //Create new Daily
            int totalDailyTasks = 2;
            List<int> checkRandom = new List<int>();
            for (int i = 0; i < totalDailyTasks; i++){
                int r;  //get random - not repeated
                while(true){
                    r = UnityEngine.Random.Range(0, dailyTasks.Count);
                    if(!checkRandom.Contains(r)){
                        checkRandom.Add(r);
                        break;
                    }
                }
                buttonTasks.Add(dailyTasks[r]);  //Add to button list   
                String taskId = "DailyTask" + i;    
                List<string> task = dailyTasks[r];
                PlayerPrefs.SetInt(taskId, Convert.ToInt32(task[0]));   //Get task ID and save
            }
            PlayerPrefs.SetInt("TotalDailyTasks", totalDailyTasks);     //save total and date
            PlayerPrefs.SetString("SavedDate", getStringTime());

            //Create new Weekly
            int totalWeeklyTasks = 1;
            checkRandom = new List<int>();
            for (int i = 0; i < totalWeeklyTasks; i++){
                int r;
                while(true){
                    r = UnityEngine.Random.Range(0, weeklyTasks.Count);
                    if(!checkRandom.Contains(r)){
                        checkRandom.Add(r);
                        break;
                    }
                }
                buttonTasks.Add(weeklyTasks[r]);
                String taskId = "WeeklyTask" + i;
                List<string> task = weeklyTasks[r];
                PlayerPrefs.SetInt(taskId, Convert.ToInt32(task[0]));
            }
            PlayerPrefs.SetInt("TotalWeeklyTasks", totalWeeklyTasks);   //save total and week num
            PlayerPrefs.SetInt("SavedWeek", getWeekNum(currentDate));
        }
        int height = 140 + (buttonTasks.Count * 120);
        taskContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(325, height);

        int posY = 40;
        for(int i = 0; i < buttonTasks.Count; i++){
            List<string> task = buttonTasks[i];
            int index = Convert.ToInt32(task[0]);
            GameObject button = (GameObject) Instantiate(buttonPrefab);

            Text[] ts = button.transform.GetComponentsInChildren<Text>(true);
            foreach (Text t in ts){
                if(t.gameObject.name == "NameTxt"){
                    t.text = task[1];
                } else{
                    t.text = task[2] + " Points";
                }
            }

            if(task[1].Contains("Daily") && PlayerPrefs.HasKey("CompletedDaily")){
                string completed = PlayerPrefs.GetString("CompletedDaily");
                if(completed.Contains(index.ToString())){
                    Color newColor = new Color(0.62f, 0.62f, 0.62f);
                    button.GetComponent<Image>().color = newColor;
                }
            }

            if(task[1].Contains("Weekly") && PlayerPrefs.HasKey("CompletedWeekly")){
                string completed = PlayerPrefs.GetString("CompletedWeekly");
                if(completed.Contains(index.ToString())){
                    Color newColor = new Color(0.62f, 0.62f, 0.62f);
                    button.GetComponent<Image>().color = newColor;
                }
            }

            button.GetComponent<Button>().onClick.AddListener(delegate{setPanel(task, index);} );
            posY = posY - 120;
            button.transform.position = new Vector3(0, posY, 0);
            button.transform.SetParent(taskContainer, false);
        }
        
    }

    void setPanel(List<string> task, int index){
        taskPanel.SetActive(true);
        completedButton.SetActive(true);
        completedText.SetActive(false);

        taskTitle.GetComponentInChildren<Text>().text = task[1];
        taskPoints.GetComponentInChildren<Text>().text = task[2] + " Points";
        taskDescription.GetComponentInChildren<Text>().text = task[3] + "\n \n Tips: " + task[4];

        string type;
        if(task[1].Contains("Daily")){
            type = "CompletedDaily";

            if(PlayerPrefs.HasKey("CompletedDaily")){
                string tasks = PlayerPrefs.GetString("CompletedDaily");
                if(tasks.Contains(index.ToString())){
                    completedButton.SetActive(false);
                    completedText.SetActive(true);
                }
            }
        } else{
            type = "CompletedWeekly";

            if(PlayerPrefs.HasKey("CompletedWeekly")){
                string tasks = PlayerPrefs.GetString("CompletedWeekly");
                if(tasks.Contains(index.ToString())){
                    completedButton.SetActive(false);
                    completedText.SetActive(true);
                }
            }
        }

        completedButton.GetComponent<Button>().onClick.AddListener(delegate{taskCompleted(index, Convert.ToInt32(task[2]), type);} );
    }

    void taskCompleted(int index, int points, string type){
        if(PlayerPrefs.HasKey(type)){
            string tasks = PlayerPrefs.GetString(type);
            if(!tasks.Contains(index.ToString())){
                string newTasks = tasks + index;
                PlayerPrefs.SetString(type, newTasks);
                PlayerPrefs.Save();

                completedButton.SetActive(false);
                completedText.SetActive(true);
                completed = true;

                int newHealth = PlayerPrefs.GetInt("health") + points;
                PlayerPrefs.SetInt("health", newHealth);
            }
        } else{
            string num = "" + index;
            PlayerPrefs.SetString(type, num);

            completedButton.SetActive(false);
            completedText.SetActive(true);
            completed = true;

            int newHealth = PlayerPrefs.GetInt("health") + points;
            PlayerPrefs.SetInt("health", newHealth);
        }
        taskPanel.SetActive(false);
        createTaskButtons();
    }

    int getWeekNum(DateTime time){
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    string getStringTime(){
        DateTime now = DateTime.Now;
        return now.Day + "/" + now.Month + "/" + now.Year;
    }

    public void closeHealthPanel(){
        healthPanel.SetActive(false);

        if(completed){
            StartCoroutine(InvokeMethod(avatarCheer, 2, 3));
            StartCoroutine(InvokeMethod(setEmote, 6, 2));
            completed = false;
        }
    }

    void setEmote(){
        int r = UnityEngine.Random.Range(0, 4);
        Sprite random = sprites[r];
        Emote.GetComponent<Image>().sprite = random;

        if(Emote.activeInHierarchy){
            Emote.SetActive(false);
        } else{
            Emote.SetActive(true);
        }
    }

    void avatarCheer(){
        if(avatar.transform.position.y < -1.4f){
            avatar.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 30000f)); 
        }
    }

    public IEnumerator InvokeMethod(Action method, float interval, int invokeCount)
     {
         for (int i = 0; i < invokeCount; i++)
         {
             method();
             yield return new WaitForSeconds(interval);
         }
     }

    public void closeTaskPanel(){
        taskPanel.SetActive(false);
    }

    void setTaskLists(){
        dailyTasks.Add(new List<string> { "0", "Daily: Recycle", "20", 
            "This will help reduce your overall waste by recycling. Contamination within recycled waste is a big issue so ensure you've recycled correctly.", 
            "Inform yourself by reading up on your districts recyling rules and carefully read the packaging. Not all materials from one product is recyclable. To improve your recycling further only buy recyclable/recycled products." });
        dailyTasks.Add(new List<string> { "1", "Daily: Cycle", "20", 
            "Traveling are some people's biggest contribution to their carbon footprint. Condsider cycling or walking to work.", 
            "If you can't do either to reduce your emissions you could carpool or take public transport to reduce the number of vehicles on the road." });
        dailyTasks.Add(new List<string> { "2", "Daily: No Waste", "20", 
            "Today try not to create any waste. This can change the way you look at your daily waste and find small ways to improve.", "Look at your shopping, buy items that have less packaging." });
        dailyTasks.Add(new List<string> { "3", "Daily: Grow your Own", "20", 
            "Growing your own fruit and vegetables reduces waste. It also is another less item to buy, an item that has its own carbon footprint.", "Easy ones are herbs, tomatoes, cucumber etc. This can also improve your health as they are less contaminated." });
        dailyTasks.Add(new List<string> { "4", "Daily: No Plastic", "20", 
            "Try go a day without using any plastic", "This helps you look at what you are using and all that extra plastic you don't need." });
        dailyTasks.Add(new List<string> { "5", "Daily: Electricity", "20", 
            "Today take into account your electricity and try to reduce it.", "Turn off switches, don't leave electronics on standby, turn off heating if not needed etc." });

        weeklyTasks.Add(new List<string> { "0", "Weekly: Bills", "60", 
            "Reducing your using will not only reduce your carbon footprint but also save you money.", "Change to a green energy provider, invest into solar panels etc." });
        weeklyTasks.Add(new List<string> { "1", "Weekly: Plastic", "60", 
            "Plastic is a big world issue as it doen't degrade.", "Buy things with less platcis packaging/recycled plastic/use resuable plastic." });
        weeklyTasks.Add(new List<string> { "2", "Weekly: Stuff", "60", 
            "Buying less items will save you money in the long term and reduces your waste", "When buying look at quality as they'll last longer or ask yourself if you really need this item." });
        weeklyTasks.Add(new List<string> { "3", "Weekly: Travel", "60", 
            "You can offset your travel", "Offsetting is where you take out the carbon you've produced like plant a tree." });
    }

    void beeFlying(){

        Vector3 bee1View = bee.transform.position;
        Vector3 bee2View = bee2.transform.position;

        bee1View.x = Mathf.Clamp(bee1View.x, 1.1f, 1.9f);
        bee1View.y = Mathf.Clamp(bee1View.y, -3.0f, 3.0f);
        bee.transform.position = bee1View;

        bee2View.x = Mathf.Clamp(bee2View.x, -1.9f, -1.1f);
        bee2View.y = Mathf.Clamp(bee2View.y, -3.0f, 3.0f);
        bee2.transform.position = bee2View;

        if(bee.transform.position.y > 2){
            bee.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -4));
        } else if(bee.transform.position.y < 2){
            bee.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 12));
        }

        if(bee2.transform.position.y > 1){
            bee2.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -4));
        } else if(bee2.transform.position.y < 1){
            bee2.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 12));
        }
    }

    public void showHealthPanel(bool trigger){
        if(SpeachBubble.activeInHierarchy){
            SpeachBubble.SetActive(false);
        }
        healthPanel.SetActive(!healthPanel.activeInHierarchy);
        if(taskPanel.activeInHierarchy){
            taskPanel.SetActive(false);
        }
    }

    public void showExitPanel(bool trigger){
        exitPanel.SetActive(!exitPanel.activeInHierarchy);
    }

    public void exitGame(bool trigger){
        SceneManager.LoadScene("StartScene");
        PlayerPrefs.DeleteAll();
    }

    void setStage1(){
        background1.SetActive(true);
        background2.SetActive(false);

        ground1.SetActive(true);
        ground2.SetActive(false);

        bee.GetComponent<Animator>().SetBool("Die", false);
        bee.GetComponent<Rigidbody2D>().mass = 1.1f;
    }

    void setStage2(){
        background1.SetActive(false);
        background2.SetActive(true);

        ground1.SetActive(false);
        ground2.SetActive(true);

        //bee 1
        bee.GetComponent<Animator>().SetBool("Die", true);
        bee.GetComponent<Rigidbody2D>().mass = 10000;

        //bee 2
        bee2.GetComponent<Animator>().SetBool("Die", false);
        bee2.GetComponent<Rigidbody2D>().mass = 1;

        //Worm
        worm.GetComponent<Animator>().SetBool("DeadWorm", false);
    }

    void setStage3(){
        background2.SetActive(false);
        background3.SetActive(true);

        ground2.SetActive(false);
        ground3.SetActive(true);

        //bee 1
        bee.GetComponent<Animator>().SetBool("Die", true);
        bee.GetComponent<Rigidbody2D>().mass = 10000;

        //bee2
        bee2.GetComponent<Animator>().SetBool("Die", true);
        bee2.GetComponent<Rigidbody2D>().mass = 1000;

        //kill worm
        worm.GetComponent<Animator>().SetBool("DeadWorm", true);
    }

    void setStage4(){
        background3.SetActive(false);
        background4.SetActive(true);

        ground3.SetActive(false);
        ground4.SetActive(true);

        bee.SetActive(false);
        bee2.SetActive(false);
        worm.SetActive(false);
        avatar.SetActive(false);
        exitButton.SetActive(false);

        TimeSpan totalTime = DateTime.Now - Convert.ToDateTime(PlayerPrefs.GetString("StartTime"));
        double totalHours = totalTime.TotalHours;
        double rounded = Math.Round( totalHours, 2);
        GameoverPanel.SetActive(true);
        txtHours.GetComponent<Text>().text = rounded.ToString() + " Hours";
    }
}
