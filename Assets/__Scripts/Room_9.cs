using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_9 : Room
{
    static private Room_9 S;
    
    public Room north_door;
    public int north_door_status;
    bool visited = false;
    bool genFull = false;
    bool genOn = false;
    

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_north_edge(); // index 0
        

        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        set_generator_empty_scene(); // index 1
        set_generator_full_scene(); // index 2
        set_generator_on_scene(); // index 3
        
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
        string roomDesc = "The Generator room is cramped and full of broken machinery, although the generator seems to be intact. The generator door is open";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Examine the generator UNIMPLEMENTED. 
        optionDescription = "Examine the Generator"; // Take us to room 7
        flag = "examine_generator";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Leave via the north door. 
        optionDescription = "Leave the room"; // Take us to room 6
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    private void set_generator_empty_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - pull the lever
        optionDescription = "Pull the lever";
        flag = "pull_lever";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The generator seems to be intact.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void set_generator_full_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - pull the lever
        optionDescription = "Pull the lever";
        flag = "pull_lever";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The generator now has a full tank.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }
    private void set_generator_on_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The generator has been brought to life. Its engine gives off a steady hum.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void lookAt() {
        // Just remembered that I cannot freely access player without a referrence. I don't want to make a referrence. So I'm going to make a global that can ask player for the relevant data.
        //Debug.Log("lookAt() called");
        //Debug.Log(Player.IS_LIGHT());
        if (!visited) {
            visited = !visited;
            Main.VISITED();
        }
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag ==  "examine_generator") {examineGenerator(); return;}
        if(flag ==  "go_door") {goDoor(); Main.PASS_TIME(); return;}
        if(flag ==  "go_back") {goBack(); return;}
        if(flag ==  "pull_lever") {pullLever(); return;}
        if(flag == "gas_can") {pourGas(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    private void itemUsed(string flag) {
        if(flag == "gas_can" && activeScene.sceneDesc == scenes[1].sceneDesc) {optionSelected(flag); return;}
        Main.DISPLAY_ITEM_USELESS();
    }

    // ===== These are action functions ===== //
    private void examineGenerator() {
        if(genOn) {
            activeScene = scenes[3];
            Main.DISPLAY_ROOM(this);
            return;
        }
        if(genFull) {
            activeScene = scenes[2];
            Main.DISPLAY_ROOM(this);
            return;
        }
        activeScene = scenes[1];
        Main.DISPLAY_ROOM(this);
        return;
        //Debug.Log("examineGenerator() undefined");
    }

    private void goDoor() {
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goEast()");
    }

    private void goBack() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM(this);
    }

    private void pullLever() {
        if(genFull) {
            genOn = true;
            activeScene = scenes[3];
            Main.DISPLAY_ROOM(this);

            // Shhhh, I'm feeling tired, we'll fix this later, but rn we are doing this here because it is simpler.
            main.power = true;
            return;
        }
        string desc = "You attempt to start the generator, but it needs fuel.";
        Main.DISPLAY_TEXT(desc);
    }

    private void pourGas() {
        // I have not the sanity or patience currently to implement the following, so I will log it here for future me: I need to delete the gas can after it is successfully used.
        genFull = true;
        activeScene = scenes[2];
        Main.DISPLAY_ROOM(this);

        // Important to avoid a bug
        string desc = "You pour the gas into the generator";
        Main.DISPLAY_TEXT(desc);
    }

    // ===== These are public functions ===== //
    public override void OPTION_SELECTED(string flag){
        S.optionSelected(flag);
        return;
    }

    public override void ITEM_USED(string flag){
        S.itemUsed(flag);
        return;
    }

    public override void LOOK_AT() {
        S.lookAt();
    }
}

