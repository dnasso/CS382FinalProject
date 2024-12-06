using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_5 : Room
{
    static private Room_5 S;

    public Room west_door;
    public int west_door_status;

    public Room north_door;
    public int north_door_status;
    
    public Room east_door;
    public int east_door_status;

    private bool power = false;

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_west_edge(); // index 0
        set_north_edge(); // index 1
        set_east_edge(); // index 2

        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        set_elevator_powerless_scene(); // index 1
        set_elevator_powered_scene(); // index 2
    }

    // ===== These are the edge setting functions ===== // 
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

    private void set_east_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = east_door;
        tempEdge.status = east_door_status;
        edges.Add(tempEdge);
    }

    // ===== These are scene_setting functions ===== //
    private void set_main_scene(){
        string roomDesc = "There is an elevator here. Cobwebs line its frame. The hallway continues east and west";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Follow the hallway west. 
        optionDescription = "Follow the hallway west"; // Take us to room 4
        flag = "go_west";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Use Door. 
        optionDescription = "Examine the Elevator"; // Take us to room 1
        flag = "examine_elevator";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 3 - Follow the hallway east. 
        optionDescription = "Follow the hallway east"; // Take us to room 6
        flag = "go_east";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    private void set_elevator_powerless_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The elevator seems to be lacking power.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void set_elevator_powered_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Take Elevator
        optionDescription = "Take Elevator";
        flag = "take_elevator";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The elevator is now powered.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void lookAt() {
        power = main.power;
        Debug.Log("Power Status: " + power.ToString() );
    }
    

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag ==  "go_west") {goWest(); Main.PASS_TIME(); return;}
        if(flag ==  "examine_elevator") {examineElevator(); return;}
        if(flag ==  "go_east") {goEast(); Main.PASS_TIME(); return;}
        if(flag ==  "go_back") {goBack(); return;}
        if(flag ==  "take_elevator") {takeElevator(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    // ===== These are action functions ===== //
    private void goWest() {
        change_option("Follow the hallway west toward the Storage Room", "go_west", 0, 0);
        edges[0].dest.change_option("Follow the hallway east toward the Elevator", "go_east", 0, 2);
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goWest()");
    }

    private void examineElevator() {
        //Main.DISPLAY_ROOM( edges[1].dest );
        if (power) {
            activeScene = scenes[2];
            Main.DISPLAY_ROOM(this);
            return;
        }

        activeScene = scenes[1];
        Main.DISPLAY_ROOM(this);
        return;
        //Debug.Log("Method undefined: examineElevator()");
    }

    private void goEast() {
        change_option("Follow the hallway eest toward the Kitchen", "go_east", 0, 2);
        edges[2].dest.change_option("Follow the hallway west toward the Elevator", "go_west", 0, 0);
        Main.DISPLAY_ROOM( edges[2].dest );
        //Debug.Log("Method undefined: goEast()");
    }

    private void goBack() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM(this);
    }

    private void takeElevator() {
        Main.DISPLAY_ROOM( edges[1].dest );
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
