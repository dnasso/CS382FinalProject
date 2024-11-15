using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_0 : Room
{
    static private Room_0 S;

    public bool button = false;

    //public edge north_door;

    // Setting data for edges, hopefully leveraging the unity editor to simplify this task
    public Room north_door;
    public int north_door_status;

    void Awake() {
        S = this;
        //Description = "The room is dark. The walls are cold and grey. There is door on the north wall. There is a cupboard here";
        
        /*
        // Setting initial description of room
        string roomDesc = "The walls are cold and grey.";

        // Setting options
        // This room should have two. An option to leave, and an option to interact with a cupboard.

        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Examine Door. 
        optionDescription = "Examine door"; // We set this to examine door. We'll add a flag to change this to "Try door" Once its been examined and "use door" once it has been unlocked. Maybe too much?
        flag = "examine_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Examine Cupboard.
        optionDescription = "Examine Cupboard"; // We set this to examine door. We'll add a flag to change this to "Try door" Once its been examined and "use door" once it has been unlocked. Maybe too much?
        flag = "examine_cupboard";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
        */

        // Setting north door edge. Could be a function. I made it a function.
        edges = new List<edge>();
        set_north_edge(); // index 0
        
        activeScene = new scene();
        scenes = new List<scene>();
        

        set_main_scene(); // index 0
        set_cupboard_scene(); // index 1
        set_door_locked_scene(); // index 2
        set_door_unlocked_scene(); // index 3

        /*
        activeScene = new scene();
        tempOptions = 
        activeScene.sceneDesc = "The cupboard was bare, except for a small red button, hidden within its frame.";
        */
    }

    // ===== These are the edge setting functions ===== // 
    private void set_north_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = north_door;
        tempEdge.status = north_door_status;
        edges.Add(tempEdge);
    }

    // ===== These are scene_setting functions ===== //
    private void set_main_scene(){
        string roomDesc = "You are in a dreadful room. The walls are cold and grey. There is a door on the north wall, and a dusty old cabinet in the corner.";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Examine Door. 
        optionDescription = "Examine door"; // We set this to examine door. We'll add a flag to change this to "Try door" Once its been examined and "use door" once it has been unlocked. Maybe too much?
        flag = "examine_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Examine Cupboard.
        optionDescription = "Examine Cupboard"; // We set this to examine door. We'll add a flag to change this to "Try door" Once its been examined and "use door" once it has been unlocked. Maybe too much?
        flag = "examine_cupboard";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    private void set_cupboard_scene(){
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Press the button
        optionDescription = "Press the button";
        flag = "press_button";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The cupboard was bare, except for a small red button, hidden within its frame.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void set_door_locked_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The door is locked and will not budge.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void set_door_unlocked_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Press the button
        optionDescription = "Open the door";
        flag = "open_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The door is now unlocked. Dare you open it?";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag ==  "examine_door") {examineDoor(); return;}
        if(flag ==  "examine_cupboard") {examineCupboard(); return;}
        if(flag == "press_button") {pressButton(); Main.PASS_TIME(); return;}
        if(flag == "open_door") {openDoor(); Main.PASS_TIME(); return;}
        if(flag == "go_back") {goBack(); return;}
        if(flag == "go_door") {goDoor(); Main.PASS_TIME(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    // ===== These are action functions ===== //
    private void examineDoor() {
        if(button) {activeScene = scenes[3];}
        else {activeScene = scenes[2];}
        Main.DISPLAY_ROOM(this);
        //Debug.Log("Method undefined: exmineDoor()");
        return;
    }

    private void examineCupboard() {
        activeScene = scenes[1];
        Main.DISPLAY_ROOM(this);
        //Debug.Log("Method undefined: exmineCupboard()");
        return;
    }

    private void pressButton() {
        /*
        if(button) {activeScene}
        else {}
        */
        activeScene.sceneDesc = "You pushed the button.";
        button = true;
        //Main.PASS_TIME();
        Main.DISPLAY_ROOM(this);
        //Debug.Log("Method undefined: pressButton()");
        return;
    }

    private void openDoor() {
        // We are going to change the option in the first scene when this option is picked.
        // We are also going to change the option in this scene to allow the player to go through the door.
        // We are also going to change the text to let the player know they opened the door.
        // Pretty simple. That's just three things!
        // Actually four!!!
        // Have to update the edges
        // Five things. Or four. Forgot to update the return edge.

        //tempOption
        //scenes[0].options[0] = tempOption;

        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        edge tempEdge = new edge();
        string optionDescription;
        string flag;


        // Step 1
        // Option 1 - Examine Door. 
        optionDescription = "Go through the open door"; // We set this to examine door. We'll add a flag to change this to "Try door" Once its been examined and "use door" once it has been unlocked. Maybe too much?
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2
        initialize_option(scenes[0].options[1].desc, scenes[0].options[1].flag, tempOptions);
        //activeScene.options = tempOptions;
        tempScene = scenes[0];
        tempScene.options = tempOptions;
        scenes[0] = tempScene;
        //scenes[0].options = tempOptions; // PROBLEM CHILD


        // Step 2 and 3
        tempOptions = new List<option>();
        optionDescription = "Go through the open door"; // We set this to examine door. We'll add a flag to change this to "Try door" Once its been examined and "use door" once it has been unlocked. Maybe too much?
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2
        initialize_option(scenes[3].options[1].desc, scenes[3].options[1].flag, tempOptions);
        //activeScene.options = tempOptions;
        tempScene = scenes[3];
        tempScene.options = tempOptions;
        tempScene.sceneDesc = "The door is now open";
        scenes[3] = tempScene;
        //scenes[3].options = tempOptions; // PROBLEM CHILD
        //scenes[3].sceneDesc = "The door is now open"; // PROBLEM CHILD
        
        // Step 4
        
        tempEdge = edges[0];
        tempEdge.status = 0;
        edges[0] = tempEdge;

        //Debug.Log(edges[0].status);

        tempEdge = new edge();
        tempEdge = edges[0].dest.edges[0]; // Double Triple check this second index
        tempEdge.status = 0;
        edges[0].dest.edges[0] = tempEdge; // I don't think this works.

        //Debug.Log(edges[0].dest.edges[0].status);
        
        //Main.PASS_TIME();
        activeScene = scenes[3];
        Main.DISPLAY_ROOM(this);

        //Debug.Log("Method undefined: openDoor()");
        return;
    }

    private void goBack() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM(this);
        //Debug.Log("Method undefined: goBack()");
        return;
    }

    private void goDoor() {
        activeScene = scenes[0]; // This is incase we return to this room.
        //Main.PASS_TIME();
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goDoor()");
    }

    // ===== These are public functions ===== //
    // I'm learning so much what the heck
    public override void OPTION_SELECTED(string flag){
        S.optionSelected(flag);
        return;
    }
    //Main.PASS_TIME();
}
