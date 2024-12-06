using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_4 : Room

{
    static private Room_4 S;

    public Room south_door;
    public int south_door_status;

    public Room north_door;
    public int north_door_status;
    
    public Room east_door;
    public int east_door_status;

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_south_edge(); // index 0
        set_north_edge(); // index 1
        set_east_edge(); // index 2

        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        set_door_locked_scene(); // index 1
        set_door_unlocked_scene(); // index 2
        
    }

    // ===== These are the edge setting functions ===== // 
    private void set_south_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = south_door;
        tempEdge.status = south_door_status;
        edges.Add(tempEdge);
    }

    private void set_north_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = north_door;
        tempEdge.status = north_door_status;
        edges.Add(tempEdge);
    }

    private void set_east_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = east_door;
        tempEdge.status = east_door_status;
        edges.Add(tempEdge);
    }

    // ===== These are scene_setting functions ===== //
    private void set_main_scene(){
        string roomDesc = "The hallway turns here. A patch of mold clings to the wall. There is a door labeled \"Storage\" on the north wall. It continues south and east.";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Follow the hallway south. 
        optionDescription = "Follow the hallway south"; // Take us to room 3
        flag = "go_south";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Use Door. 
        optionDescription = "Examine the door labeled \"Storage\""; // Take us to room 10
        flag = "examine_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 3 - Follow the hallway east. 
        optionDescription = "Follow the hallway east"; // Take us to room 5
        flag = "go_east";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
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
        tempScene.sceneDesc = "The door labeled \"Storage\" isn't locked. Would you like to open it?";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag == "go_back") {goBack(); return;}
        if(flag == "examine_door") {examineDoor(); return;}
        if(flag == "open_door") {openDoor(); Main.PASS_TIME(); return;}
        if(flag ==  "go_south") {goSouth(); Main.PASS_TIME(); return;}
        if(flag ==  "go_door") {goDoor(); Main.PASS_TIME(); return;}
        if(flag ==  "go_east") {goEast(); Main.PASS_TIME(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    // ===== These are action functions ===== //
    private void goBack() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM(this);
        //Debug.Log("Method undefined: goBack()");
        return;
    }
    private void examineDoor() {
        activeScene = scenes[2];
        Main.DISPLAY_ROOM(this);
        return;
        //Debug.Log("Method undefined: examineDoor()");
    }
    private void openDoor() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        edge tempEdge = new edge();
        string optionDescription;
        string flag;


        ///// Step 1
        // Option 1 - Go South
        initialize_option(scenes[0].options[0].desc, scenes[0].options[0].flag, tempOptions);
        
        // Option 2 - Use Door. 
        optionDescription = "Go through the open door labeled \"Storage\"";
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 3 - Go East
        initialize_option(scenes[0].options[2].desc, scenes[0].options[2].flag, tempOptions);
        tempScene = scenes[0];
        tempScene.options = tempOptions;
        scenes[0] = tempScene;


        ///// Step 2 and 3
        tempOptions = new List<option>();
        optionDescription = "Go through the open door labeled \"Storage\"";
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2
        initialize_option(scenes[2].options[1].desc, scenes[2].options[1].flag, tempOptions);
        tempScene = scenes[2];
        tempScene.options = tempOptions;
        tempScene.sceneDesc = "The door is now open";
        scenes[2] = tempScene;
        
        ///// Step 4
        
        tempEdge = edges[0];
        tempEdge.status = 0;
        edges[0] = tempEdge;

        //Debug.Log(edges[0].status);

        tempEdge = new edge();
        tempEdge = edges[0].dest.edges[0]; // Double Triple check this second index
        tempEdge.status = 0;
        edges[0].dest.edges[0] = tempEdge; // I don't think this works.

        //Debug.Log(edges[0].dest.edges[0].status);
        
        activeScene = scenes[2];
        Main.DISPLAY_ROOM(this);

        //Debug.Log("Method undefined: openDoor()");
        return;
    }
    private void goSouth() {
        change_option("Follow the hallway south toward the Generator", "go_south", 0, 0);
        edges[0].dest.change_option("Follow the hallway north toward the Storage Room", "go_north", 0, 1);
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goSouth()");
    }

    private void goDoor() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM( edges[1].dest );
        //Debug.Log("Method undefined: goDoor()");
    }

    private void goEast() {
        change_option("Follow the hallway east toward the Elevator", "go_east", 0, 2);
        edges[2].dest.change_option("Follow the hallway west toward the Storage Room", "go_west", 0, 0);
        Main.DISPLAY_ROOM( edges[2].dest );
        //Debug.Log("Method undefined: goEast()");
    }

    // ===== These are public functions ===== //
    public override void OPTION_SELECTED(string flag){
        S.optionSelected(flag);
        return;
    }
}

