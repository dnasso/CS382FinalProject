using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_2 : Room
{
    // Start is called before the first frame update
    static private Room_2 S;

    public Room south_door;
    public int south_door_status;

    public Room west_door;
    public int west_door_status;
    
    public Room east_door;
    public int east_door_status;

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_south_edge(); // index 0
        set_west_edge(); // index 1
        set_east_edge(); // index 2

        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        
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

    private void set_east_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = east_door;
        tempEdge.status = east_door_status;
        edges.Add(tempEdge);
    }

    // ===== These are scene_setting functions ===== //
    private void set_main_scene(){
        string roomDesc = "You are in a dark and dusty hallway. There is a door on the south wall. The hallway continues to the east and to the west.";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Use Door. 
        optionDescription = "Go through the door"; // Take us to room 0
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Follow the hallway west. 
        optionDescription = "Follow the hallway west"; // Take us to room 3
        flag = "go_west";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 3 - Follow the hallway east. 
        optionDescription = "Follow the hallway east"; // Take us to room 7
        flag = "go_east";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag ==  "go_door") {goDoor(); Main.PASS_TIME(); return;}
        if(flag ==  "go_west") {goWest(); Main.PASS_TIME(); return;}
        if(flag ==  "go_east") {goEast(); Main.PASS_TIME(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    // ===== These are action functions ===== //
    private void goDoor() {
        //Main.PASS_TIME();
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goDoor()");
    }

    private void goWest() {
        //Main.PASS_TIME();
        Main.DISPLAY_ROOM( edges[1].dest );
        //Debug.Log("Method undefined: goWest()");
    }

    private void goEast() {
        //Main.PASS_TIME();
        Main.DISPLAY_ROOM( edges[2].dest );
        //Debug.Log("Method undefined: goEast()");
    }

    // ===== These are public functions ===== //
    public override void OPTION_SELECTED(string flag){
        S.optionSelected(flag);
        return;
    }
}
