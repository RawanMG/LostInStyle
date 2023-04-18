## _Scripts/Core

### Arrow.cs
Controls the behavior of the Arrow navigation aid
### FindPath.cs
Controls the behavior of the embedded path navigation aid

### Database.cs &  DataCollector.cs & DataCollector2.cs
Writes user data (user position, user direction, and gaze direction) into a text file
### DataReplayer.cs & DataReplayer2.cs
Reads user's data from a text file and simulates it in the environment

### GData.cs
Enumerators for describing the user study 

### MissionSystem.cs & VRMainSystem.cs
Scripts for initializing and controlling the study session protocol. Sets the current scene, destination, and navigation aid. Displays the starting flag.

### PortalSystem.cs 
Initializes the portals (i.e. starting flags) and places the user there using ```Place_Player()``` (which is usually called in the beginning of the user study session. 

### PortalSensor.cs
Checks whether the user entered the portal (i.e. starting flag) and behaves accordingly (if it's a goal, completes the study session). 

### StressMeter.cs & SocketComm.py & get_stress.py
Communicates with and deploys the ML model.
