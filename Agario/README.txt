Author:     Aaron Morgan and Xavier Davis
Partner:    None
Date:       4/1/2020
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment #8 - Agario Client
Copyright:  CS 3500, Xavier Davis and Aaron Morgan - This work may not be copied for use in Academic Coursework.
Github Repository: https://github.com/uofu-cs3500-spring20/assignment-eight-agario-client-team-yoshi.git
Commit #:  

1. Comments to Evaluators:

    None at this time.

2. Assignment Specific Topics

    A) Time Tracking

            Expected Time to Complete: 25 hours.

                Time spent on Analysis: 10 hours.
                Time spent on Implementation: 9.5 hours.
                Time spent Debugging: 6 hours.
                Time spent Testing: 2.5 hours.

                Aaron's time spent on Implementation: 2 hours.

            Total Time: 30 hours.

    B) User Interface and Game Design Decisions

        We thought the Login/Game Over screen should be smaller than the actual game screen, mostly because it didn't need as much information displayed
        on it, so we adjusted it accordingly.
        
        We decided to center the screen at all times because we thought it made it look a bit more organized.

3. Partnership:

    A) Contributions

        We worked together throughout the vast majority of the assignment. The only exception to this was when Aaron was trying to convert the original List
        we were using to the World's Dictionary. He added the methods found in World that allowed us to access/modify the Dictionary (Add, Remove, etc).

    B) Branching

       ListToDictionaryConversion: We were initially using a List to keep track of the Circles in the Client_and_GUI. After some experimentation and analysis,
       we decided to try to utilize a Dictionary instead for storage purposes. We started a new branch called "MovementTest" to begin working on it (we worked
       on it together), but we realized it wasn't really working.

       Aaron wanted to see if he could get it to work correctly, but forgot to pull from Git, and ended up making an entirely new branch. The branch included
       the changes to the World class to get the Dictionary to work correctly in the Client_and_GUI class.

4. Testing:

    We mostly tested in three different ways:

        1) Starting the Client/Server - Primarily used to the drawing/displaying of the screen, as well as all of the objects. This is also how we tried to figure out
        how to center the "camera" on the player and zoom in.

        2) The Logger: We used this for general information gathering, such as when the player has connected to the server.

        3) The Debug Output window: We noticed that we needed information printed out as the Client/Server was running, so we used a number of Debug.WriteLine
        statements to check our mathematical equations, such as the conversion from the World's set size to the screen size.

5. Consulted Peers:

    None.

6. References:

    1) StackOverflow: How do I get the current screen cursor position? https://stackoverflow.com/questions/6165828/how-do-i-get-the-current-screen-cursor-position
    2) StackOverflow: How do I center a window onscreen in C#? https://stackoverflow.com/questions/4601827/how-do-i-center-a-window-onscreen-in-c 
    



