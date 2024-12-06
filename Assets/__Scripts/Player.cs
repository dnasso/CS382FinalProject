using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    static private Player S;
    public List<item> inventory; //= new List<item>();

    private int flashlight_index; // I hate finding. // Only remembering. // NO FIND // ONLY REMEMBER

    /*
        So, the player class is going to be fairly simply, hopefully. I implemented a large portion of this game, before implementing this class. 
        All postion/perspective stuff is handled by main, and updated as the player gives input. So, there is no need to track location here. Although,
        I might do it down the road. This whole program needs to be refactored anyways.

        Biggest reason we are here is that we want an "inventory" of "items". This will allow the player to pick up tools to solve the puzzles we are 
        presenting them with.

        Ideally, the items will be used as such: While "interacting" with a specific point of interest in a room. We'll grab a flag from the item, and
        pass it to the flag pole function for the active room. It should then check that the active "scene" is correct (Name change is priority if I 
        refactor. Thinking of calling them "interactions" instead.)
    */

    void Awake() {
        S = this;
        inventory = new List<item>();
    }

    public struct item {
        public string desc; // Item description
        public string flag; // Item flag
        public int index; // Do I need this?, no right?

    }
    
    private void add_item(string desc, string flag) {
        item tempItem;
        tempItem.desc = desc;
        tempItem.flag = flag;

        // I could make this a function. But why the heck would I do that? This is """easier"""".
        if (flag == "flash_light_off") {
            flashlight_index = inventory.Count; //- 1;
        }

        tempItem.index = inventory.Count; //- 1; // Silly me
        inventory.Add(tempItem);
        Debug.Log("initialize_option() called");
    }

    private bool isLight() {
        Debug.Log("isLight() called");
        for (int i = 0; i < inventory.Count; i++) {
            if(inventory[i].flag == "flash_light_on") {
                Debug.Log("isLight returned true");
                return true;
            }
        }
        Debug.Log("isLight returned false");
        return false;
    }

    private void toggleFlashlight() {
        string scene_desc;
        string item_desc;
        string item_flag;
        if( inventory[flashlight_index].flag == "flash_light_off") {
            item_desc = "Flash Light (on)";
            item_flag = "flash_light_on";
            scene_desc = "You click the flashlight on.";
            changeItem(item_desc, item_flag, flashlight_index);
            Main.DISPLAY_TEXT(scene_desc);
        }
        else if( inventory[flashlight_index].flag == "flash_light_on") {
            item_desc = "Flash Light (off)";
            item_flag = "flash_light_off";
            scene_desc = "You click the flashlight off.";
            changeItem(item_desc, item_flag, flashlight_index);
            Main.DISPLAY_TEXT(scene_desc);
        }
    }

    private void changeItem(string desc, string flag, int index) {
        item tempItem = new item();
        tempItem.desc = desc;
        tempItem.flag = flag;
        tempItem.index = index; // Again do I need this value

        inventory[index] = tempItem;
  
        // inventory[index].desc = desc;
        // inventory[index].flag = flag;
        // inventory[index].index = index; // Again do I need this value?
    }

    static public void ADD_ITEM(string desc, string flag) {
        Debug.Log("ADD_ITEM() called");
        S.add_item(desc, flag);
    }

    static public bool IS_LIGHT() {
        Debug.Log("IS_LIGHT() called");
        Debug.Log(S.isLight());
        return S.isLight();
    }

    static public void TOGGLE_FLASHLIGHT() {
        Debug.Log("TOGGLE_FLASHLIGHT() called");
        S.toggleFlashlight();
        return;
    }
    
}
