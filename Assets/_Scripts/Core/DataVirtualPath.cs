using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

// ****************************************************************************************************************
// read the user path and place the virtual path
// ****************************************************************************************************************

/*
 * File format
 * consume time.
 * lenght
 * start portal
 * destination
 * {position, direction,  eye's position, eye's direction, hit(0,1) <, hitpoint, normal>} // each line, the hitpoint’s normal =-direction
 * */

public class DataVirtualPath : DataBase {

	Transform user;
	Transform eye;

	Queue<Vector3> quene;
	
	override public void StartFunc (string filePath, GameObject goArrow, GameObject goUser)
	{
		if (goArrow == null)
			throw new ArgumentNullException ("goArrow");
		// find User Object;
		user = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform;
		eye = user.gameObject.GetComponentInChildren<Eyes>().transform;

		quene = new Queue<Vector3>();
		
		// load the data from file
		StreamReader sr = new StreamReader(filePath);
		
		// consume time.
		if (!sr.EndOfStream)
		{
			tips += ("        Comsume time: " +sr.ReadLine() + "\n");
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
			tips += ("        Start portal: " +sr.ReadLine()+ "\n");
		} 
		
		
		// destination
		if (!sr.EndOfStream)
		{
			string dest = sr.ReadLine();
			dest = dest.Replace("_", " ");
			tips += ("        Destination: " + dest + "\n");
		}
		
		// {position, direction, eye's direction, hit(0,1) <, hitpoint, normal>} // each line, the hitpoint’s normal =-direction
		while(!sr.EndOfStream)
		{
			// read line by line
			string line = sr.ReadLine();
			string[] info = line.Split(',');
			// the first 3 data are elements of position
			quene.Enqueue(
				new Vector3(float.Parse(info[0]),
			            float.Parse(info[1]),
			            float.Parse(info[2])));
			
			// the next 3 data are elements of direction
			quene.Enqueue(
				new Vector3(float.Parse(info[3]),
			            float.Parse(info[4]),
			            float.Parse(info[5])));

			// the next 3 data are elements of direction
			quene.Enqueue(
				new Vector3(float.Parse(info[6]),
			            float.Parse(info[7]),
			            float.Parse(info[8])));

			// the next 3 data are elements of direction
			quene.Enqueue(
				new Vector3(float.Parse(info[9]),
			            float.Parse(info[10]),
			            float.Parse(info[11])));
            
        }
        sr.Close();

		// place arrows
		Place_Arrow(goArrow, goUser);
	}

	// Update is called once per frame
	override public void FixedUpdateFunc () 
	{
		Ticket_Object to = GameObject.FindObjectOfType<Ticket_Object>();
		to.ticket = true;
	}

	void Place_Arrow(GameObject goArrow, GameObject goUser)
	{
		int index = 0;

		bool placePlayer = false;

		while(quene.Count > 0)
		{
			if (index <= 0)
			{
				Vector3 userPos = quene.Dequeue();
				Vector3 userDir = quene.Dequeue();
				Vector3 pos = quene.Dequeue();
				Vector3 dir = quene.Dequeue();

				if (!placePlayer)
				{
					placePlayer = true;
					user.transform.position = userPos;
					user.transform.forward = userDir;
				}

				GameObject goU = Instantiate(goUser) as GameObject;
				goU.transform.position = userPos;
				goU.transform.forward = userDir;

				GameObject goAr = Instantiate(goArrow) as GameObject;
				goAr.transform.position = pos;
				goAr.transform.forward = dir;

				// reset the index
				index = GData.interal_of_arrow;
			}
			else
			{
				quene.Dequeue();
				quene.Dequeue();
				quene.Dequeue();
				quene.Dequeue();

				index--;
			}
		}

	}

}
