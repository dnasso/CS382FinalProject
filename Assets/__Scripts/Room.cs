using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public struct option {
        // You can't put initializers in structs? what the the hecc? Since when?
        public string desc; //= "<not Set>";
        public string flag; // = new List<string>(); // Does it work?
        public int index; // Speeds things up

    }
    
    public struct edge {
        // I wanted to be able to set these in the unity interface. Oh well. Apologies for whatever jank work around I conncocted.
        public Room dest;
        public int status;
            // -1 is null connection
            // 0 is open connection
            // 1 is a closed door
    }

    public struct scene {
        public string sceneDesc;
        public List<option> options;
    }

    //public string roomDesc = "<roomDesc Not Set>";
    //public string currentDesc = "<currentDesc Not Set>";
    //public List<option> options = new List<option>();
    //public string[] options = {"Not SSet"};
    public Main main;
    public scene activeScene;
    public List<scene> scenes;
    public List<edge> edges;

    protected void initialize_option(string newDesc, string newFlag, List<option> options) {
        option tempOption;
        tempOption.desc = newDesc;
        tempOption.flag = newFlag;
        tempOption.index = options.Count - 1;
        options.Add(tempOption);
        Debug.Log("initialize_option() called");
    }
    
    // I just needed to do this sooner
    public void change_option(string desc, string flag, int scene_index, int option_index) {
        List<option> tempOptions = new List<option>();
        scene tempScene = new scene();
        option tempOption = new option();

        // Its starting to look like I need to start finding by now. But, alas, no find, only remember.
        tempOption.desc = desc;
        tempOption.flag = flag;

        for(int i = 0; i < scenes[scene_index].options.Count; i++) {
            if (i == option_index) {
                initialize_option(desc, flag, tempOptions);
            }
            else {
                initialize_option(scenes[scene_index].options[i].desc, scenes[scene_index].options[i].flag, tempOptions);
            }
        }
        
        tempScene = scenes[scene_index];
        tempScene.options = tempOptions;
        scenes[scene_index] = tempScene;

        //Bandaid
        activeScene = scenes[scene_index];

        // It just makes sense
    }

    public void change_scene_desc(string desc, int scene_index) {
        scene tempScene = new scene();

        tempScene = scenes[scene_index];
        
        tempScene.sceneDesc = desc;

        scenes[scene_index] = tempScene;
    }

    public virtual void OPTION_SELECTED(string flag){
        return;
    }
    
    // New idea. We make it so that this function can take an item like "Flashlight"
    // That way we can define behaviour for the flashlight globaly
    // This will only be over ridden in places with pre-existing definitions for this function
    
    // I'd like to define a function that can modify the flashlight. The question is, do I define most or all of it in the Player file. 
    // I'm defining the interaction here.
    // I have functions that can let me manipulate it from here. I would have to make some sort of weird global flashlight editing function if I made that function in Player.
    
    // I'm gonna be cursed. Why not. Only I'm working on this, I don't need to worry about driving anyone except myself insance.
    public virtual void ITEM_USED(string flag) {
        Debug.Log("ITEM_USED() called");
        if(flag == "flash_light_off" ) {Player.TOGGLE_FLASHLIGHT(); return;} // Gross. I'm sorry. I know. // Actually nevermind // I need an index I don't have.
        if(flag == "flash_light_on" ) {Player.TOGGLE_FLASHLIGHT(); return;} // Insert thousand yard stare meme here // #Consequencesofmyownactions :)
        Main.DISPLAY_ITEM_USELESS();
    }

    public virtual void LOOK_AT() {
        // Basically, main is blind about the going's on of the rooms.
        // And the rooms only respond to stimuli.
        // Before this function, rooms would do nothing if you looked at them
        // They waited for your input before running any code.
        // I don't want to use the update function, although maybe I could.
        // Seems excessive.
        // Regardless, the program is simply this:
        // Call this for active room everytime display_room is called.
        // This will let us react to the player walking into a room/
        // For all contextual awareness, this is too give the player
        //      different feedback if they have a flashlight.
        return;
    }
}
