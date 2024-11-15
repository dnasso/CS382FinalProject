using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_8 : Room
{
    static private Room_8 S;
    
    public Room north_door;
    public int north_door_status;
    

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_north_edge(); // index 0
        

        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        set_pile_scene(); // index 1
        
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
        string roomDesc = "This appears to be a bed room. A maise of bunk beds fills the room. Every surface is covered with a thick plaque of rust. In the center of the room is a thick pile of shredded cloth."; // Finish this
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Examine the pile UNIMPLEMENTED. 
        optionDescription = "Examine the pile of cloth."; // Take us to room 7
        flag = "examine_pile";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Leave via the north door. 
        optionDescription = "Leave the room"; // Take us to room 6
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);


        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    private void set_pile_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Go back
        optionDescription = "Take the bolt cutters";
        flag = "grab_boltcutters";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "Inside the pile you find a set of rusty bolt cutters.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag ==  "examine_pile") {examinePile(); return;}
        if(flag ==  "go_door") {goDoor(); Main.PASS_TIME(); return;}
        if(flag ==  "grab_boltcutters") {grabBoltcutters(); return;}
        if(flag ==  "go_back") {goBack(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    // ===== These are action functions ===== //
    private void examinePile() {
        activeScene = scenes[1];
        Main.DISPLAY_ROOM(this);
        //Debug.Log("examinePile() undefined");
    }

    private void goDoor() {
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goEast()");
    }

    private void grabBoltcutters() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string desc;
        string itemDesc;
        string itemFlag;

        // Option 1 - Go Back
        initialize_option(scenes[1].options[1].desc, scenes[1].options[1].flag, tempOptions);

        tempScene = scenes[1];
        tempScene.options = tempOptions;
        tempScene.sceneDesc = "There is nothing left inside the pile";
        scenes[1] = tempScene;
        activeScene = scenes[1];
        Main.DISPLAY_ROOM(this);

        desc = "You pick up the rusty bolt cutters.";
        itemDesc = "Rusty Bolt Cutters";
        itemFlag = "bolt_cutters";
        Player.ADD_ITEM(itemDesc, itemFlag);
        Main.DISPLAY_TEXT(desc);

        //Debug.Log("Method undefined: grabFlashlight()");
    }

    private void goBack() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM(this);
    }

/*
    // ===== These are bandaid solutions ===== //
    private void open_door() {
        
    }
*/  

    // ===== These are public functions ===== //
    public override void OPTION_SELECTED(string flag){
        S.optionSelected(flag);
        return;
    }

/*
    static public void OPEN_DOOR() {
        // this is a bandaid on a thick plaque of bandaids. Bear with me. I'll fix it later.
        S.open_door();
    }
*/

}


