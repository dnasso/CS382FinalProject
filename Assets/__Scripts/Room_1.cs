using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room_1 : Room
{
    static private Room_1 S;
    // I'm thinking this will be a placeholder goal room.

    void Awake() {
        S = this;

        


        // Setting options
        // This room should have two. An option to leave, and an option to interact with a cupboard.
        /*
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
        */
        activeScene = new scene();
        scenes = new List<scene>();

        set_main_scene(); // index 0
        
    }

    // ===== These are scene_setting functions ===== //
    private void set_main_scene(){
        string roomDesc = "You have escaped.";
        List<option> tempOptions = new List<option>();
        string optionDescription;
        string flag;

        // Option 1 - Play Again. 
        optionDescription = "Play Again"; // Just reloaded the whole scene
        flag = "play_again";
        initialize_option(optionDescription, flag, tempOptions);

        activeScene.sceneDesc = roomDesc;
        activeScene.options = tempOptions;
        scenes.Add(activeScene);
    }

    // ===== This is the option flag handling function ===== //
    private void optionSelected(string flag) {
        if(flag ==  "play_again") {playAgain(); return;}
        Debug.Log("<Error - Flag not found>");
    }

    // ===== These are action functions ===== //
    private void playAgain() {
        SceneManager.LoadScene( "_Scene_0" );
        //Debug.Log("Method undefined: exmineDoor()");
        return;
    }


    public override void OPTION_SELECTED(string flag){
        S.optionSelected(flag);
        return;
    }

}
