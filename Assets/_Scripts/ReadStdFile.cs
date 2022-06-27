using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//[ExecuteInEditMode]
public class ReadStdFile// : MonoBehaviour
{
    public string std_file = "std_350.txt";
    public string fileName;
    bool debug = true;
    public List<float> listMean;
    public List<float> listVar;
    public float overtimeMean;
    public float overtimeVar;
    public int buffLen = 10;
    /*
    void Awake()
    {
        Read_File(@"C:\Users\ysawa\Documents\unity\vrlost\VR Lost\Assets\_Scripts\Core\MODELS\std_350.txt");
        CompOvertimeVals(buffLen);
        float m, v;
        GetValues(out m, out v);
        Debug.Log("M:" + m + "  V;" + v);
        GetValues(out m, out v);
        Debug.Log("M:" + m + "  V;" + v);
    }
    */
    public void ReadFile()
    {
        string datafile = Application.dataPath + @"\_Scripts\Core\MODELS\" + std_file;
        Read_File(datafile);
        CompOvertimeVals(buffLen);
    }

    public ReadStdFile GetCopy()
    {
        ReadStdFile ret = new ReadStdFile();
        ret.fileName = fileName;
        ret.std_file = std_file;
        ret.listMean = new List<float>(listMean.ToArray());
        ret.listVar = new List<float>(listVar.ToArray());
        ret.overtimeMean = overtimeMean;
        ret.overtimeVar = overtimeVar;
        ret.buffLen = buffLen;
        return ret;
    }
    

    public void GetValues(out float mean,out float vari)
    {
        if(listMean.Count!=0)
        {
            mean = listMean[0];
            vari = listVar[0];
            listMean.RemoveAt(0);
            listVar.RemoveAt(0);
        }else
        {
            mean = overtimeMean;
            vari = overtimeVar;
        }
    }

    void CompOvertimeVals(int len)
    {
        if(listMean.Count==0)
        {
            overtimeMean = 0f;
            overtimeVar = 1f;
            Debug.Log("Error? CompOvertimeVals");
            return;
        }
        overtimeMean = 0f;
        overtimeVar = 0f;
        for(int i=listMean.Count-len;i<listMean.Count;i++)
        {
            overtimeMean += listMean[i];
            overtimeVar += listVar[i];
        }
        overtimeMean /= (float)len;
        overtimeVar /= (float)len;
    }
    public void Read_File(string _fileName)
    {

        fileName = _fileName;

        if (debug)
        {
            Debug.Log("Read File: " + fileName);
        }

        StreamReader sr = new StreamReader(fileName);

        if (sr == null)
        {
            Debug.Log("Read ERR: " + fileName);
            return;
        }
        // read the file line by line
        string line;
        while ((line = sr.ReadLine()) != null)
        {

            line = line.Trim();
            if (line.Length == 0)
                continue;
            if(line.Contains("mean"))
            {
                string numbers = GetNumbers(line);
                listMean = Parse(numbers);
            }else if(line.Contains("var"))
            {
                string numbers = GetNumbers(line);
                listVar = Parse(numbers);
            }
        }
    }
    string GetNumbers(string line)
    {
        int n = line.IndexOf('[');
        string numbers = line.Substring(n + 1);
        n = numbers.IndexOf(']');
        numbers = numbers.Substring(0, n);
   //     Debug.Log(numbers);
        return numbers;
    }
    List<float> Parse(string line)
    {
        List<float> list = new List<float>();
        string[] str_vals = line.Split(',');
        foreach(string s in str_vals)
        {
            string str = s.Trim();
            float val = float.Parse(str);
            list.Add(val);
        }
        return list;
    }
    public int GetListSize()
    {
        return listMean.Count;
    }
}
