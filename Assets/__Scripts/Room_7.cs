using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_7 : Room
{
    static private Room_7 S;

    public Room south_door;
    public int south_door_status;

    public Room west_door;
    public int west_door_status;

    public Room north_door;
    public int north_door_status;

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_south_edge(); // index 0
        set_west_edge(); // index 1
        set_north_edge(); // index 2
        

        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        set_door_locked_scene(); // index 1
        set_door_unlocked_scene(); // index 2 // I actually don't expect to use this one. Might delete. I want to open the door from a distance.
        
    }

    // ===== These are the edge setting functions ===== // 
    private void set_south_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = south_door;
        tempEdge.status = south_door_status;
        edges.Add(tempEdge);
    }

    private void set_west_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = west_door;
        tempEdge.status = west_door_status;
        edges.Add(tempEdge);
    }

    private void set_north_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = north_door;
        tempEdge.status = north_door_status;
        edges.Add(tempEdge);
    }

    // ===== These are scene_setting functions ===== //
    private void set_main_scene(){
        string roomDesc = "The hallway turns here. There is a rusty door on the south wall. The hallway continues west and for quite a ways north.";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Use Door. 
        optionDescription = "Examine the rusty door"; // Take us to room 8
        flag = "examine_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Follow the hallway west. 
        optionDescription = "Follow the hallway west"; // Take us to room 2
        flag = "go_west";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 3 - Follow the hallway north. 
        optionDescription = "Follow the hallway north"; // Take us to room 12
        flag = "go_north";
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
        tempScene.sceneDesc = "The door is jammed and will not budge.";
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
        tempScene.sceneDesc = "The door labeled \"Kitchen\" isn't locked. Would you like to open it?";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }
    private void lookAt() {
        if (main.roomsVisited >= 3 && edges[0].status != 0) {
            breakRustyDoor();
        }
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag == "go_back") {goBack(); return;}
        if(flag ==  "examine_door") {examineDoor(); return;}
        if(flag == "open_door") {openDoor(); Main.PASS_TIME(); return;}
        if(flag == "go_door") {goDoor(); Main.PASS_TIME(); return;}
        if(flag ==  "go_west") {goWest(); Main.PASS_TIME(); return;}
        if(flag ==  "go_north") {goNorth(); Main.PASS_TIME(); return;}
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
        if(edges[0].status == 0) {activeScene = scenes[2];}
        else {activeScene = scenes[1];}
        //activeScene = scenes[2];
        Main.DISPLAY_ROOM(this);
        return;
        //Debug.Log("Method undefined: examineDoor()");
    }
    private void openDoor() {
        // Irrelevant function. Might axe.
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        edge tempEdge = new edge();
        string optionDescription;
        string flag;


        ///// Step 1
        // Option 1 - Go West
        initialize_option(scenes[0].options[0].desc, scenes[0].options[0].flag, tempOptions);
        
        // Option 2 - Go South
        initialize_option(scenes[0].options[2].desc, scenes[0].options[2].flag, tempOptions);

        // Option 3 - Use Door. 
        optionDescription = "Go through the open door labeled \"Kitchen\"";
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        tempScene = scenes[0];
        tempScene.options = tempOptions;
        scenes[0] = tempScene;


        ///// Step 2 and 3
        tempOptions = new List<option>();
        optionDescription = "Go through the open door labeled \"Kitchen\"";
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

    private void goDoor() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goDoor()");
    }

    private void goWest() {
        change_option("Follow the hallway west toward the Starting Room", "go_west", 0, 1);
        edges[1].dest.change_option("Follow the hallway east toward the Rusty Room", "go_east", 0, 2);
        Main.DISPLAY_ROOM( edges[1].dest );
        //Debug.Log("Method undefined: goWest()");
    }


    private void goNorth() {
        change_option("Follow the hallway north toward the Kitchen", "go_north", 0, 2);
        edges[2].dest.change_option("Follow the hallway south toward the Rusty Room", "go_south", 0, 0);
        Main.DISPLAY_ROOM( edges[2].dest );
        //Debug.Log("Method undefined: goEast()");
    }

    private void breakRustyDoor() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        edge tempEdge = new edge();
        string optionDescription;
        string flag;

        
        // Option 3 - Use Door. // Actually Option 1
        // Indexing out of Order, fixed an issue with the code in the easiest possible way. Hopefully no one has to read this later :)
        optionDescription = "Go through the rusty doorway";
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);
        
        // Option 1 - Go West // Actually Option 2
        initialize_option(scenes[0].options[1].desc, scenes[0].options[1].flag, tempOptions);
        
        // Option 2 - Go North // Actually Option 3
        initialize_option(scenes[0].options[2].desc, scenes[0].options[2].flag, tempOptions);

        /*
        // Option 3 - Use Door. 
        optionDescription = "Go through the rusty doorway";
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);
        */

        tempScene = scenes[0];
        tempScene.options = tempOptions;
        tempScene.sceneDesc = "The hallway turns here. There is a rusty door seems to have collapsed. The hallway continues west and for quite a ways north."; //WIP The hallway turns here. There is a rusty door on the south wall. The hallway continues west and for quite a ways north.
        scenes[0] = tempScene;
        
        // Set Edge status
        tempEdge = edges[0];
        tempEdge.status = 0;
        edges[0] = tempEdge;

        // Set Neighbor edge status
        tempEdge = new edge();
        tempEdge = edges[0].dest.edges[0]; // Double Triple check this second index
        tempEdge.status = 0;
        edges[0].dest.edges[0] = tempEdge; // I don't think this works.

        //Debug.Log(edges[0].dest.edges[0].status);
        
        activeScene = scenes[0];
        //Main.DISPLAY_ROOM(this);

        //Debug.Log("Method undefined: openDoor()");
        return;
    }

    // ===== These are public functions ===== //
    public override void OPTION_SELECTED(string flag){
        S.optionSelected(flag);
        return;
    }

    public override void LOOK_AT() {
        S.lookAt();
    }
}
