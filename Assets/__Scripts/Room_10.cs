using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_10 : Room
{
    static private Room_10 S;
    
    public Room south_door;
    public int south_door_status;
    bool cabinetOpen = false;
    bool visited = false;
    

    void Awake() {
        S = this;

        edges = new List<edge>();
        set_south_edge(); // index 0
        

        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        set_scene_light(); // index 1
        set_cabinet_locked_scene(); // index 2
        set_cabinet_unlocked_scene(); // index 3
        
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
        string roomDesc = "The Storage room is pitch black. It's impossible to see in here. The only light emanates from the hallway";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Leave. 
        optionDescription = "Leave the room"; // Take us to room 6
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    private void set_scene_light(){
        string roomDesc = "The Storage room is still dark, but with your flashlight, you can see finally. Most of the room has been ransacked, but there is a cabinet labeled \"Gasoline\"";
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Examine the gas cabinet. 
        optionDescription = "Examine the cabinet";
        flag = "examine_cabinet";
        initialize_option(optionDescription, flag, tempOptions);


        // Option 2 - Leave
        optionDescription = "Leave the room"; // Take us to room 6
        flag = "go_door";
        initialize_option(optionDescription, flag, tempOptions);

        // Issue here, pls fix
        // Fixed. Copied the wrong code, silly me

        /*
        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
        */

        tempScene.sceneDesc = roomDesc;
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void set_cabinet_locked_scene(){
        string roomDesc = "The Gas cabinet is chained shut. There's no way you can get in here without something to cut the chain.";
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - Leave
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        tempScene.sceneDesc = roomDesc;
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    private void set_cabinet_unlocked_scene(){
        string roomDesc = "The Gas cabinet has been broken open.";
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string optionDescription;
        string flag;

        // Option 1 - gAs
        optionDescription = "Grab a gas can";
        flag = "grab_gas";
        initialize_option(optionDescription, flag, tempOptions);

        // Option 2 - Leave
        optionDescription = "Go back";
        flag = "go_back";
        initialize_option(optionDescription, flag, tempOptions);

        tempScene.sceneDesc = roomDesc;
        tempScene.options = tempOptions;
        scenes.Add(tempScene);
    }

    // Throwing this in scene handling because where else?
    private void lookAt() {
        // Just remembered that I cannot freely access player without a referrence. I don't want to make a referrence. So I'm going to make a global that can ask player for the relevant data.
        //Debug.Log("lookAt() called");
        //Debug.Log(Player.IS_LIGHT());
        Debug.Log("Player looked at room 10");
        if (!visited) {
            Debug.Log("Player is visiting room 10");
            visited = !visited;
            Main.VISITED();
        }
        // Hope I don't break anything with this second condition
        /*if( Player.IS_LIGHT() && activeScene.sceneDesc == scenes[0].sceneDesc) {
            //Placeholder
            activeScene = scenes[1];
        }*/
        if( Player.IS_LIGHT() && activeScene.sceneDesc == scenes[0].sceneDesc) {
            //Placeholder
            activeScene = scenes[1];
        }
        else if( !Player.IS_LIGHT() ) {
            activeScene = scenes[0];
        }
        return;
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag ==  "go_door") {goDoor(); Main.PASS_TIME(); return;}
        if(flag ==  "examine_cabinet") {examineCabinet(); return;}
        if(flag ==  "go_back") {goBack(); return;}
        if(flag ==  "bolt_cutters") {cutChain(); return;} //WIP
        if(flag == "grab_gas") {grabGas(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    private void itemUsed(string flag) {
        if(flag == "bolt_cutters" && activeScene.sceneDesc == scenes[2].sceneDesc) {optionSelected(flag); return;}
        if(flag == "flash_light_off" ) {Player.TOGGLE_FLASHLIGHT(); return;} 
        if(flag == "flash_light_on" ) {Player.TOGGLE_FLASHLIGHT(); return;} 
        Main.DISPLAY_ITEM_USELESS();
    }

    // ===== These are action functions ===== //
    private void goDoor() {
        Main.DISPLAY_ROOM( edges[0].dest );
        //Debug.Log("Method undefined: goEast()");
    }

    private void examineCabinet() {
        if (cabinetOpen) {
            activeScene = scenes[3]; //How could I so perfectly precipitate my genius?
            Main.DISPLAY_ROOM(this);
            return;
        }
        activeScene = scenes[2];
        Main.DISPLAY_ROOM(this);
        return;
        //Debug.Log("Method undefined: examineCabinet()");
    }

    private void goBack() {
        activeScene = scenes[0];
        Main.DISPLAY_ROOM(this);
        //Debug.Log("Method undefined: goBack()");
    }

    private void cutChain() {
        string desc = "You used the bolt cutters to remove the chain";
        activeScene = scenes[3];
        Main.DISPLAY_ROOM(this);
        // Big Brian Hours
        Main.DISPLAY_TEXT(desc);
        cabinetOpen = true;
    }
    
    private void grabGas() {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        string desc;
        string itemDesc;
        string itemFlag;

        // Option 1 - Go Back
        initialize_option(scenes[3].options[1].desc, scenes[3].options[1].flag, tempOptions);

        tempScene = scenes[3];
        tempScene.options = tempOptions;
        tempScene.sceneDesc = "The Gas cabinet has been broken open.";
        scenes[3] = tempScene;
        activeScene = scenes[3];
        Main.DISPLAY_ROOM(this);

        desc = "You pick up a gas can.";
        itemDesc = "Gas Can";
        itemFlag = "gas_can";
        Player.ADD_ITEM(itemDesc, itemFlag);
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

