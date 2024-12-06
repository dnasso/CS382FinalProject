using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_12 : Room
{
    static private Room_12 S;

    public Room south_door;
    public int south_door_status;
    
    public Room north_door;
    public int north_door_status;
    

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_south_edge(); // index 0
        set_north_edge(); // index 1
        

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

    private void set_north_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = north_door;
        tempEdge.status = north_door_status;
        edges.Add(tempEdge);
    }

    // ===== These are scene_setting functions ===== //
    private void set_main_scene(){
        string roomDesc = "The hallway is long and bleak. It continues north and south.";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Follow the hallway south. 
        optionDescription = "Follow the hallway south"; // Take us to room 7
        flag = "go_south";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Follow the hallway north. 
        optionDescription = "Follow the hallway north"; // Take us to room 6
        flag = "go_north";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag ==  "go_south") {goSouth(); Main.PASS_TIME(); return;}
        if(flag ==  "go_north") {goNorth(); Main.PASS_TIME(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    // ===== These are action functions ===== //

    private void goSouth() {
        change_option("Follow the hallway south toward the Rusty Room", "go_south", 0, 0);
        edges[0].dest.change_option("Follow the hallway north toward the Kitchen", "go_north", 0, 2);
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goWest()");
    }


    private void goNorth() {
        change_option("Follow the hallway north toward the Kitchen", "go_north", 0, 1);
        edges[1].dest.change_option("Follow the hallway south toward the Rusty Room", "go_south", 0, 2);
        Main.DISPLAY_ROOM( edges[1].dest );
        //Debug.Log("Method undefined: goEast()");
    }

    // ===== These are public functions ===== //
    public override void OPTION_SELECTED(string flag){
        S.optionSelected(flag);
        return;
    }
}
