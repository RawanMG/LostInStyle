using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUI : MonoBehaviour 
{
	public Text mission_description;
	public TextPanel tips;
    Text logText;

	[Range(0,120)]
	public float tips_delay_time = 60.0f;
	float tips_current_time = 0.0f;

	// Use this for initialization
	void Awake () 
	{
        // tips.gameObject.SetActive(false);
        GameObject go = GameObject.Find("LogText");
        if (go!=null)
            logText = go.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (tips_current_time <= 0.0f)
		{
//			if (tips.isActiveAndEnabled)
//				tips.gameObject.SetActive(false);
			tips.tipsText.text = "";

		}
		else
		{
			tips_current_time -= Time.deltaTime;
		}
	}

	// set tips
	public void SetTips(string note)
	{

		if (note == "")
		{
			// tips.gameObject.SetActive(false);
			tips.tipsText.text = "";
		}
		else
		{
			tips.tipsText.text = note;
			tips_current_time = tips_delay_time;
			// tips.gameObject.SetActive(true);
		}
	}
	
    public void ShowLogMessage(string str)
    {
        if (logText != null)
            logText.text = str;
    }
    public void SetTranlucent(bool b)
    {
        Image[] imgs = GetComponentsInChildren<Image>();
        Image img=null;
        foreach(Image im in imgs)
        {
            if(im.name == "Panel")
            {
                img = im;
            }
        }
        if (img == null)
        {
            print("No panel");
            return;
        }

        Color col = img.color;
        if (b)
            col.a = 0.5f;
        else
            col.a = 0f;
        img.color = col;
    }
}
