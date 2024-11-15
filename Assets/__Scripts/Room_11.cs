using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_11 : Room
{
    static private Room_11 S;
    
    public Room south_door;
    public int south_door_status;
    public int times_searched = 0;
    bool FreezerOpen = false;
    bool visited = false;
    

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_south_edge(); // index 0
        

        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        set_freezer_closed_scene(); // index 1
        set_freezer_open_scene(); // index 2
        
    }

    // ===== These are the edge setting functions ===== // 

    private void set_south_edge(){
        edge tempEdge = new edge();
        tempEdge.dest = south_door;
        tempEdge.status = south_door_status;
        edges.Add(tempEdge);
    }

    // ===== These are scene_setting functions ===== //
    private void set_main_scene(){
        string roomDesc = "The Kitchen is in disarray. Mold covers many surfaces. There is a door labeled \"Freezer\" on the far side of the room.";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Examine the Freezer. 
        optionDescription = "Examine the Freezer"; // Take us to room 6
        flag = "examine_freezer";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Search the trash
        optionDescription = "Search the Room";
        flag = "search_room";
        initialize_option(optionDescription, flag, tempOptions);
        
        // Option 3 - Leave. 
        optionDescription = "Leave the room"; // Take us to room 6
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);
        
        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    private void set_freezer_closed_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Go back
        optionDescription = "Pry the door open";
        flag = "open_freezer";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "The door is jammed and doesn't want to open.";
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void set_freezer_open_scene() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Go back
        optionDescription = "Grab the Flashlight";
        flag = "grab_flashlight";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Go back
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        // Scene description
        tempScene.sceneDesc = "Inside the Freezer, you find rotten and spoiled meat. The power long since failed. Tucked away in a corner is a flashlight.";
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
        if(flag ==  "examine_freezer") {examineFreezer(); return;}
        if(flag == "open_freezer") {openFreezer(); return;}
        if(flag == "grab_flashlight") {grabFlashlight(); return;}
        if(flag == "go_back") {goBack(); return;}
        if(flag == "search_room") {searchRoom(); return;}
        if(flag ==  "go_door") {goDoor(); Main.PASS_TIME(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    // ===== These are action functions ===== //
    private void examineFreezer() {
        if (FreezerOpen) {
            activeScene = scenes[2];
            Main.DISPLAY_ROOM(this);
            return;
        }
        activeScene = scenes[1];
        //activeScene = scenes[2];
        Main.DISPLAY_ROOM(this);
        return;
        //Debug.Log("Method undefined: examineFreezer()");
    }

    private void openFreezer() {
        FreezerOpen = true;
        activeScene = scenes[2];
        Main.DISPLAY_ROOM(this);
        return;
        //Debug.Log("Method undefined: openFreezer()");
    }

    private void grabFlashlight() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string desc;
        string itemDesc;
        string itemFlag;

        // Option 1 - Go Back
        initialize_option(scenes[2].options[1].desc, scenes[2].options[1].flag, tempOptions);

        tempScene = scenes[2];
        tempScene.options = tempOptions;
        tempScene.sceneDesc = "Inside the Freezer, you find rotten and spoiled meat. The power long since failed. There is nothing left here.";
        scenes[2] = tempScene;
        activeScene = scenes[2];
        Main.DISPLAY_ROOM(this);

        desc = "You pick up the flashlight. After some testing, you discover it still works.";
        itemDesc = "Flash Light";
        itemFlag = "flash_light";
        Player.ADD_ITEM(itemDesc, itemFlag);
        Main.DISPLAY_TEXT(desc);

        //Debug.Log("Method undefined: grabFlashlight()");
    }

    private void searchRoom() {
        string desc;
        string itemDesc;
        string itemFlag;
        if (times_searched == 0) {
            times_searched += 1;
            desc = "You find a rusty knife hidden in the trash.";
            itemDesc = "Rusty Knife";
            itemFlag = "rusty_knife";
            Player.ADD_ITEM(itemDesc, itemFlag);
            Main.DISPLAY_TEXT(desc);
            return;
        }
        if (times_searched >= 0) {
            times_searched += 1;
            desc = "You rummage around, but can't find anything.";
            Main.DISPLAY_TEXT(desc);
            return;
        }
    }

    private void goBack() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM(this);
        //Debug.Log("Method undefined: goBack()");
    }

    private void goDoor() {
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goEast()");
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

