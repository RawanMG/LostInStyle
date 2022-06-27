using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;


public class DataAnalyzeMenu : MonoBehaviour 
{
	string resultPath = "";

	public Text listText;
	public InputField inputField;

	public Toggle tog_vrMode;

	public InputField startField, endField;


	// Use this for initialization
	void Start () 
	{
		tog_vrMode.isOn = GData.VR_Mode;

		startField.text = GData.filter_startPoint;
		endField.text = GData.filter_endPoint;

		// create the results' path
		resultPath = Application.dataPath + "/DataResults/results";

		Find_All_Results_FilePath();

		inputField.ActivateInputField();

	}
	
	// Update is called once per frame
	void Update () 
	{
//		// shout cut
//		if (Input.GetKeyDown(KeyCode.P) && Input.GetKeyDown(KeyCode.LeftControl))
//		{
//			Btn_Replayer();
//		}
//		else if (Input.GetKeyDown(KeyCode.V) && Input.GetKeyDown(KeyCode.LeftControl))
//		{
//			Btn_Virtual_Path();
//		}
//		else if (Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.LeftControl))
//		{
//			Btn_Refresh();
//		}
	}

	void Find_All_Results_FilePath()
	{
		listText.text = "";

		// C#遍历指定文件夹中的所有文件 
		DirectoryInfo TheFolder=new DirectoryInfo(resultPath);
		
		bool matchStart = false;
		bool matchEnd = false;

		//遍历文件夹
		//foreach(DirectoryInfo NextFolder in TheFolder.GetDirectories())
		//	this.listBox1.Items.Add(NextFolder.Name);
		//遍历文件
		foreach(FileInfo nextFile in TheFolder.GetFiles("*.txt"))
		{
			// listText.text += ("<color=#ffff00ff><b>["+nextFile.Name+"]</b></color>" + "\n");
			string info = ("<color=#ffff00ff><b>["+nextFile.Name+"]</b></color>" + "\n");
			// open this file and get some info
			// load the data from file
			StreamReader sr = new StreamReader(nextFile.FullName);
			
			// consume time.
			if (!sr.EndOfStream)
			{
				info += ("    Comsume time: <b>" + Math.Round(float.Parse(sr.ReadLine())) + " sec</b>\n");
			}
			
			// lenght
			if (!sr.EndOfStream)
			{
				sr.ReadLine();
				// tips += ("Comsume time: " +sr.ReadLine());
			}
			
			// start portal
			if (!sr.EndOfStream)
			{
				string sp = sr.ReadLine();
				info += ("    Start portal: <b>" +sp + "</b>\n");

				matchStart = sp.Contains(GData.filter_startPoint);

			} 
		
			// destination
			if (!sr.EndOfStream)
			{
				string dest = sr.ReadLine();
				dest.Replace("_", " ");
				info += ("    Destination: <b>" + dest + "</b>\n\n");

				matchEnd = dest.Contains(GData.filter_endPoint);
			}

			// match
			if (matchStart && matchEnd)
			{
				listText.text += info;
			}

			//Debug.Log("results file: " + nextFile.Name);
		}
	}


	// ***********************************************************************************************************
	// Function for button
	// ***********************************************************************************************************
	public void Btn_Back()
	{
		Application.LoadLevel("MainMenu");
	}

	public void Btn_Replayer()
	{
		GData.playMode = GData.PlayMode.Replayer;
		GData.VR_Mode = tog_vrMode.isOn;

		GData.replayer_filePath = resultPath +"/Kenmore-" + inputField.text +".txt";
		
		Application.LoadLevel("Kenmore");
	}

	public void Btn_Virtual_Path()
	{
		GData.playMode = GData.PlayMode.Virtual_Path;
		GData.VR_Mode = tog_vrMode.isOn;

		GData.replayer_filePath = resultPath +"/Kenmore-" + inputField.text +".txt";

		Application.LoadLevel("Kenmore");


	}

	public void Btn_Refresh()
	{
		GData.filter_startPoint = startField.text ;
		GData.filter_endPoint = endField.text;

		Find_All_Results_FilePath();
	}

}
