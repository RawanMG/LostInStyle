using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ScreensRecoder : MonoBehaviour
{
	public KeyCode[] screenCaptureKeys;
	public KeyCode[] keyModifiers;

	public int minimumWidth = 1024;
    public int minimumHeight = 768;
    public string directory = @"C:\Users\ysawa\Documents\mov\";
    public string base_directory = @"C:\Users\ysawa\Documents\mov\";
    public string baseFilename;
    public int framerate = 60;
    public bool isRecording = false;
    public int endFrameno = 0;

    private int frameno = -1;

	void Reset ()
	{
		screenCaptureKeys = new KeyCode[]{ KeyCode.R };
		keyModifiers = new KeyCode[] { KeyCode.LeftShift, KeyCode.RightShift };

        baseFilename = System.DateTime.Now.ToString("yyyyMMdd");
    }
    public void UpdateBaseFilename()
    {
        VRMainSystem vrms = GameObject.FindObjectOfType<VRMainSystem>();
        baseFilename = Path.GetFileNameWithoutExtension(vrms.dataPath);
        string path = base_directory + baseFilename;
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
        directory = path + Path.DirectorySeparatorChar;

        Text txt = GetComponentInChildren<Text>();
        txt.text = baseFilename;
        frameno = 0;
    }
    void Start()
    {
        Time.captureFramerate = framerate;
    }

	void Update ()
	{
        checkRecodingKey();

        if (isRecording == true)
        {
            TakeScreenShot();
        }
	}

    bool checkRecodingKey()
    {
        bool isModifierPressed = false;
        bool ret = false;
        if (keyModifiers.Length > 0)
        {
            foreach (KeyCode keyCode in keyModifiers)
            {
                if (Input.GetKey(keyCode))
                {
                    isModifierPressed = true;
                    break;
                }
            }
        }

        if (isModifierPressed)
        {
            foreach (KeyCode keyCode in screenCaptureKeys)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    isRecording = !isRecording;
                }
            }
        }
        return ret;
    }

	public void TakeScreenShot ()
	{
		float rw = (float)minimumWidth / Screen.width;
		float rh = (float)minimumHeight / Screen.height;
		int scale = (int)Mathf.Ceil(Mathf.Max(rw, rh));

        frameno++;
        string path = directory + baseFilename + "_" + frameno.ToString("D6") + ".png";

		ScreenCapture.CaptureScreenshot(path, scale);
		Debug.Log(string.Format("screen shot : path = {0}, scale = {1} (screen = {2}, {3})",
			path, scale, Screen.width, Screen.height), this);

        if (endFrameno > 0 && frameno >= endFrameno)
        {
            isRecording = false;
        }
	}
}
