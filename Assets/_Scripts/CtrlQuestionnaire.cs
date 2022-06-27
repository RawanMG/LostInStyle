using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

struct Question
{
    public string question;
    public string left_note;
    public string right_note;
    public int grades;
    public Material[] mat;
}

[ExecuteInEditMode]
public class CtrlQuestionnaire : MonoBehaviour {
    int nb_choices = 5;
    bool qTypeImage = false;
    string queston_string = "questionquestion";
    public int nb_questions = 1;
    public int width = 800;
    public int height = 800;
    public int w_step_txt = 120;
    public int w_step_img = 200;
    public GameObject imageFrame = null;
    public GameObject numberText = null;
    public Text questionText;
    public Text left_note;
    public Text right_note;
    public Transform trPanel;

    Question curQ;
    GameObject[] curQItems = null;
    int curQSelItemId = 0;
    int curQId = 0;


    bool bDone = false;
    string responses = "";
    MissionSystem missionSys;

    List<Question> listQs;
    void Awake()
    {
        InitComponents();
        LoadQuestionAll();
        //       ShowQueston(1);
        //       SelectItem(1);
        // HideQuestion();
    }
    public void StartQuestionnair()
    {
        missionSys = GameObject.FindObjectOfType<MissionSystem>();
        bDone = false;
        responses = "";
        ShowQueston(0);
    }

    void InitComponents()
    {
        bool panelstate = false;
        if (trPanel != null)
        {
            panelstate = trPanel.gameObject.activeSelf;
            trPanel.gameObject.SetActive(true);
        }
        Image[] imgs = GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            if (img.gameObject.name == "Frame")
            {
                imageFrame = img.gameObject;
                img.gameObject.SetActive(false);
            }
            if (trPanel == null && img.gameObject.name == "Panel")
            {
                trPanel = img.transform;
                //               img.gameObject.SetActive(false);
            }
        }

        Text[] txts = GetComponentsInChildren<Text>();
        foreach (Text txt in txts)
        {
            print(txt.name);
            if (txt.gameObject.name == "Value")
            {
                numberText = txt.gameObject;
                txt.gameObject.SetActive(false);
            }
            if (txt.gameObject.name == "question")
            {
                questionText = txt;
            }
            if (txt.gameObject.name == "left_note")
            {
                left_note = txt;
            }
            if (txt.gameObject.name == "right_note")
            {
                right_note = txt;
            }
        }
        trPanel.gameObject.SetActive(panelstate);
    }
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    float lastTime = 0f;
	void Update () {
        if (!IsShown())
            return;
        float wait = 0.2f;

		if(Input.GetAxis("DpadX") > 0.5f && Time.time-lastTime > wait)
        {
            MoveSelectionRight();
            lastTime = Time.time;
        }
        else if (Input.GetAxis("DpadX") < -0.5f && Time.time - lastTime > wait)
        {
            MoveSelectionLeft();
            lastTime = Time.time;
        }
        else
        {
            //nothing
        }
        if(Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.JoystickButton1) || Input.GetKeyUp(KeyCode.JoystickButton2) || Input.GetKeyUp(KeyCode.JoystickButton3))
        {
            responses += "," + GetMissionQid(curQId) + "," + curQSelItemId;

//            if (curQId == listQs.Count - 1)
            if (curQId == GetMissionQCount() - 1)
            {
                bDone = true;
                HideQuestion();
            }
            else
            {
                curQId++;
                ShowQueston(curQId);
            }
        }
    }

    int GetMissionQid(int id)
    {
        return missionSys.Get_Current_Mission().question_ids[id];
    }
    int GetMissionQCount()
    {
        return missionSys.Get_Current_Mission().question_ids.Length;
    }
    void ShowQueston(int id)
    {
        InitScreen();
        print(id);
        ShowPanel(true);
        Question q = listQs[GetMissionQid(id)];
        GameObject[] items;
        if (q.mat != null)
        {
            items = PrepImage(q.grades);
            for(int i=0;i<items.Length;i++)
            {
                Image[] imgs = items[i].GetComponentsInChildren<Image>();
                foreach (Image img in imgs)
                {
                    print(img.gameObject.name);
                    if (img.gameObject.name.Contains("Image"))
                    {
                        img.material = q.mat[i];
                        print(q.mat[i]);
                    }
                }
            }
        }
        else
        {
            items = PrepText(q.grades);
        }
        questionText.text = q.question;
        left_note.text = q.left_note;
        right_note.text = q.right_note;

        curQ = q;
        curQItems = items;
        curQId = id;
        SelectItem(q.grades / 2);
//        curQSelItemId = q.grades/2;

    }

    void MoveSelectionRight()
    {
        if (curQSelItemId == curQ.grades - 1)
            return;
        curQSelItemId++;
        SelectItem(curQSelItemId);
    }
    void MoveSelectionLeft()
    {
        if (curQSelItemId == 0)
            return;
        curQSelItemId--;
        SelectItem(curQSelItemId);
    }
    void SelectItem(int n)
    {
        if(curQItems==null)
        {
            print("ERROR: no question items");
            return;
        }

        for(int i=0;i<curQItems.Length;i++)
        {
            if(i==n)
            {
                SelectItem(curQItems[i], true);
            }
            else
            {
                SelectItem(curQItems[i], false);
            }
        }
        curQSelItemId = n;
    }

    void SelectItem(GameObject go,bool sel)
    {
        Text txt = go.GetComponent<Text>();
        if(txt!=null)
        {
            if (sel)
                txt.color = Color.red;
            else
                txt.color = Color.white;
        }
        else
        {
            Image[] imgs = go.GetComponentsInChildren<Image>();
            foreach(Image img in imgs)
            {
                if(img.name.Contains("frameimg"))
                {
                    if (sel)
                        img.color = Color.red;
                    else
                        img.color = Color.gray;
                }
            }
        }
    }

    void LoadQuestion(int id)
    {

    }
    void LoadQuestionAll()
    {
        string datafile = Application.dataPath + @"\DataResults\questions.txt";
        StreamReader sr = new StreamReader(datafile);

        if (sr == null)
        {
            Debug.Log("Read ERR: " + datafile);
            return;
        }
        // read the file line by line
        listQs = new List<Question>();

        string line;
        while ((line = sr.ReadLine()) != null)
        {
           Question q = new Question();
            line = line.Trim();
            string[] items = line.Split(',');
            q.question = items[0].Trim();
            q.left_note = q.right_note = "";
            items[1] = items[1].Trim();
            if(items[1]=="TXT")
            {
                print(items[1]);
                q.grades = int.Parse(items[2]);
                if(items.Length > 3)
                {
                    q.left_note = items[3];
                    q.right_note = items[4];
                }
                q.mat = null;
            }
            else
            {
                print(items[1]);
                q.grades = items.Length - 2;
                q.mat = new Material[q.grades];
                for(int i=0;i<q.grades;i++)
                {
                    string mat_name = "Materials/" + items[i + 2];
                    q.mat[i] = Resources.Load(mat_name, typeof(Material)) as Material;
                    if (q.mat[i] == null)
                        print("Cannot find :" + mat_name);
                }
            }
            listQs.Add(q);
        }
    }
    GameObject[] PrepText(int n)
    {
        GameObject[] gos = new GameObject[n];
        float shift = ((float)n-1) / 2f * (float)w_step_txt;
        for(int i=0;i<n;i++)
        {
            float pos_x = (float)w_step_txt * i - shift;
            print("make txt" + i);
            numberText.SetActive(true);
            GameObject newtxt = GameObject.Instantiate(numberText);
            numberText.SetActive(false);
            newtxt.name = "number" + (i+1);
            newtxt.SetActive(true);

            RectTransform rt = newtxt.GetComponent<RectTransform>();
            rt.parent = trPanel;
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.Euler(0, 0, 0);

            Vector3 p = numberText.GetComponent<RectTransform>().position;
            p = numberText.transform.worldToLocalMatrix.MultiplyPoint3x4(p);
            p.x += pos_x;
            p = transform.localToWorldMatrix.MultiplyPoint3x4(p);
            rt.position = p;
            newtxt.GetComponent<Text>().text = (i + 1).ToString();
            
            gos[i] = newtxt;
        }
        return gos;
    }
    GameObject[] PrepImage(int n)
    {
        GameObject[] gos = new GameObject[n];
        float shift = ((float)n - 1) / 2f * (float)w_step_img;
        for (int i = 0; i < n; i++)
        {
            float pos_x = (float)w_step_img * i - shift;
            imageFrame.SetActive(true);
            GameObject newimg = GameObject.Instantiate(imageFrame);
            imageFrame.SetActive(false);
            newimg.name = "frameimg" + (i + 1);
            newimg.SetActive(true);

            RectTransform rt = newimg.GetComponent<RectTransform>();
            rt.parent = trPanel;
            rt.localScale = imageFrame.GetComponent<RectTransform>().localScale;
            rt.localRotation = Quaternion.Euler(0, 0, 0);

            Vector3 p = imageFrame.GetComponent<RectTransform>().position;
            p = imageFrame.transform.worldToLocalMatrix.MultiplyPoint3x4(p);
            p.x += pos_x;
            p = transform.localToWorldMatrix.MultiplyPoint3x4(p);
            rt.position = p;
            gos[i] = newimg;
        }
        return gos;
    }

    void InitScreen()
    {
        Text[] txts = GetComponentsInChildren<Text>();
        foreach (Text txt in txts)
        {
            if (txt.gameObject.name.Contains("number"))
                DestroyImmediate(txt.gameObject);
        }
        Image[] imgs = GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            if (img.gameObject.name.Contains("frameimg"))
                Destroy(img.gameObject);
        }
    }

    void HideQuestion()
    {
        InitScreen();

        ShowPanel(false);
    }
    void ShowPanel(bool b)
    {
        if (trPanel != null)
            trPanel.gameObject.SetActive(b);
    }
    bool IsShown()
    {
        if (trPanel != null && trPanel.gameObject.activeSelf)
            return true;
        return false;
    }
    public bool IsDone()
    {
        return bDone;
    }
    public string GetResponseString()
    {
        return responses;
    }
}
