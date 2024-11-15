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

    public virtual void OPTION_SELECTED(string flag){
        return;
    }
    
    public virtual void ITEM_USED(string flag) {
        Debug.Log("ITEM_USED() called");
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
