using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.IO;
using UnityEngine;

public class StressMeter : MonoBehaviour {


	// Use this for initialization
	void Start () {

		getStress (getRandomArray(10));
	}
	
	// Update is called once per frame
	void Update () {
		getStress (getRandomArray(10));
	}


	double getStress(double[] seq){
		int exitCode=-1; 
		string path_to_python = Environment.GetEnvironmentVariable("TENSOR_PATH") + "\\python"; //path to python in virtual environmanet containing keras
		double score=Math.PI;
		try{
			Process p = new Process();
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.FileName = path_to_python;
			string working_dir = Application.dataPath + "/_Scripts/Core/";
			p.StartInfo.WorkingDirectory = working_dir; 
			string seq_str = ""; 

			for(int i=0; i<seq.Length; i++){
				seq_str += " " + seq[i]; //String.Format("{0:0.0}", seq[i]);
			}
			p.StartInfo.Arguments = working_dir + "get_stress.py" + seq_str;
			print(working_dir+"get_stress.py");
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.EnableRaisingEvents = true;
			p.Start();
			p.WaitForExit();
			exitCode = p.ExitCode;
			print("Exit Code: " + exitCode);

			StreamReader serr = p.StandardError;

			string line = serr.ReadToEnd();
			UnityEngine.Debug.Log(line);

			StreamReader sr = p.StandardOutput;

			while(!sr.EndOfStream){
				line = sr.ReadLine();
				print("Python output: " + line);
				//score = Convert.ToDouble(line);
			}


//			print("Score: " + score);



		}catch(Exception e){
			print (e);
		}
		return score;
	}

	double[] getRandomArray(int size){
		System.Random random = new System.Random ();
		double[] arr = new double[size];
		for (int i = 0; i < size; i++) {
			arr [i] = random.NextDouble ();
//			print (arr [i]);
		}
		return arr;
	}
}
