using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    /*
    // Maybe I can have my cake and eat it too
    public struct option {
        // I like using structs to transfer variables between scopes. 
        // Oh boy though, I'm starting to feel the weight of regret.
        // However, we are learning.
        public string desc;
        public string flag;
    }
    */


    static private Main S;

    [Header("Inscribed")]
    public GameObject canvas;
    //public Room[] rooms;    // Do I want to do an array? Or do I want to have each room link to another room?
    public Text uiDesc;
    public Text uiOption;
    public Text uiTime;
    public Room currentRoom;
    public Player player;
    //public option tempOption;

    public int time_passed = 0; // Could use a private setter, but a public getter, probably. Oh well, gonna be lazy and leave it public for now.
    public int roomsVisited = 0; // Same boat, different shores.
    public bool power = false; // let's use another, what could it hurt?

    //private Text[] options; // Pretty proficient with c++, not super proficient with c#. Can I get away with dynamic arrays any easier?
    private List<Text> options = new List<Text>(); // I know this is dynamically allocated. HOWEVER, you can ONLY display 7 options on screen at the same time currently. So this might have been a waste of effort :P
    private int optionSelected = 0; //Defaults to first option
    private int optionsDisplayed = 0;
    
    private string display_mode = "normal"; // Hey, future me, I say this from the bottom of my heckin soul. I AM sorry. And I sure will be. 

    //private Room currentRoom;

    /*
        I think I want to have two seperate data objects holding information about rooms and their connectivity.
        One to have an overall map for pathfinding algorithms

        I'm going to have room objects that only have functions/descriptions attached to them. And I'll load their
        descriptions on wake up.

        Okay.   
            Function called that instantiates rooms. 
            Is passed a name. 
            Instantiate's it from prefabs. 
            Finds it by name in children.
            Deactivate current room. 
            Replace current room with new child room that we still have the name of
                load its text.

        If rooms have any mechanical effects, 

        I think each option will have a set of flags. Those flags will be stored in the room file. The functions that use the flags will be stored in main
    */
    /*
        Second set of thoughts. If I instantiate the rooms as they become relevant, then how will I store information related to them? 
        Lets say a room has a button that lets you flip a lever. Lets say another room does a thing if that leverl is flipped. Where am I going to store that information?
        If, as I instantiate the rooms, I delete the previous rooms, then 
            A) they won't be able to access previous rooms to query their information and
            B) if those rooms get reinstantiated, they won't be able to save knowledge about state changes.
        I would have to save this information in main, and remind each room about its state as it gets reinstantiated.

        Alternatively, I could just create every room prematurely and have each exist simultaneously. This would allow me to directly access each one and check the information they hold.
        It shouldn't be too taxing? Each one is just a receptacle for data. Hm. I think it'll be fine to do it this way.


        -------


        Some new musings. I like using structs to communicate information. It may be an issue.
    */

    


    void Awake() {
        S = this;
        //currentRoom = rooms[0];
        //Debug.Log(currentRoom.Description);
        DISPLAY_ROOM(currentRoom);
        //DISPLAY_OPTION();
        //DISPLAY_OPTION();
        //DISPLAY_OPTION();
        //DISPLAY_OPTION();
        //DISPLAY_OPTION();
        //DISPLAY_OPTION();
        //DISPLAY_OPTION();
        Debug.Log(options.Count);
    }

    void Update() {

        if (Input.GetKeyDown("up"))
        {
            //Debug.Log("up key was pressed");
            if(optionSelected == 0) {
                return;
            }
            else {
                change_option_selected(optionSelected-1);
            }
        }
        if (Input.GetKeyDown("down"))
        {
            //Debug.Log("down key was pressed");
            if( (optionSelected+1) == options.Count) {
                return;
            }
            else {
                change_option_selected(optionSelected+1);
            }
        }
        if (Input.GetKeyDown("return"))
        {
            //Debug.Log("enter key was pressed");
            //Debug.Log("Option " + (optionSelected+1) + " was selected");
            

            if(display_mode == "display_text"){
                // Fastest solution. Dirtiest Solution. I will fix it later
                //display_mode = "inventory";
                display_mode = "normal";
                display_room(currentRoom);
                return;
            }

            if(display_mode == "item_failed"){
                // Fastest solution. Dirtiest Solution. I will fix it later
                //display_mode = "inventory";
                display_inventory();
                return;
            }
            // Hey Sorry about this
            // Honestly could make this a function call // How many times have I said this...
            if(display_mode == "inventory") {

                // Oh yeah, we nesting if statements baby!
                if ( optionSelected == player.inventory.Count ) {
                    // options[optionSelected].desc should equal "Go Back", right?
                    display_mode = "normal";
                    display_room(currentRoom);
                    return;
                }
                currentRoom.ITEM_USED( player.inventory[optionSelected].flag );
                return;
            }
            if(options[optionSelected].text == "Wait") {Debug.Log("Wait"); pass_time(); return;}
            if(options[optionSelected].text == "Check Inventory") {display_inventory(); display_mode = "inventory"; Debug.Log("Check Inventory"); return;}

            currentRoom.OPTION_SELECTED( currentRoom.activeScene.options[optionSelected].flag ); // THIS DOESN'T WORK?!? WHAT?!?
            //currentRoom.optionSelected( currentRoom.activeScene.options[optionSelected].flag );
        }

        // Race Conditions Moment?
        if (roomsVisited == 3) {
            // This may fire while the room is being set. In a magic world, this doesn't cause any issues.
            roomsVisited += 1;
            string desc = "You hear a grating, crashing boom in the distance.";
            display_text(desc);
            //OPEN_DOOR(); // This is so horrifically bad. I need a better system. // Wait maybe I have one
        }

        // Little trolling
        // Just a placeholder for now. Will hopefully do what it needs to.
        if (time_passed >= 20 && time_passed % 9 == 0) {
            time_passed += 1;
            string desc = "You hear a faint scuttling sound in the distance.";
            display_text(desc);
        }
        
    }

    private void change_option_selected(int newOptionIndex){
        Text tempOption = options[optionSelected];
        tempOption.fontStyle = FontStyle.Normal;

        tempOption = options[newOptionIndex];
        tempOption.fontStyle = FontStyle.Bold;

        optionSelected = newOptionIndex;
    }

    private void display_option(string desc){
        //Debug.Log("display_option()");
        
        Text tempOption = Instantiate( uiOption ) as Text;

        tempOption.transform.SetParent(canvas.transform, false);

        Vector3 pos = tempOption.transform.position;

        //tempOption.transform.position.y -= 86*optionsDisplayed;
        pos.y += -86*optionsDisplayed;

        tempOption.transform.position = pos;

        if(optionsDisplayed == 0) {tempOption.fontStyle = FontStyle.Bold;}
        // When in doubt, try to print it
        //print(tempOption.fontStyle);

        tempOption.text = desc;

        //options[optionsDisplayed] = tempOption; // Don't do this. Doesn't work. Do it proper.
        options.Add(tempOption);

        optionsDisplayed++;
    }

    private void clear_options() {
        //Debug.Log("clear_options()");
        for (int i = 0; i < optionsDisplayed; i++ ) {
            Destroy(options[i]); //memory leak in real time smh
        }
        //options = null;
        options = new List<Text>();
        optionsDisplayed = 0;
        optionSelected = 0;
        return;
    }

    private void display_room(Room currRoom) {
        currentRoom = currRoom;

        currentRoom.LOOK_AT();
        
        uiDesc.text = currRoom.activeScene.sceneDesc;
        
        clear_options();
        //optionsDisplayed = 0;

        for (int i = 0; i < currRoom.activeScene.options.Count; i++) {
            display_option(currRoom.activeScene.options[optionsDisplayed].desc);
        }
        display_option("Wait");
        display_option("Check Inventory");
    }

    private void display_inventory() {
        // I need a variant of display_room that just works for inventories. I'm sorry for the spaghetti code.
        //Debug.Log("AAAAAAAGGGGGGGGHHHHHHHHH");
        //Debug.Log(player.inventory.Count);
        uiDesc.text = "This is your inventory. You have " + player.inventory.Count.ToString() + " items.";
        //Debug.Log("AAAAAAAGGGGGGGGHHHHHHHHH");
        
        display_mode = "inventory";
        clear_options();
        //optionsDisplayed = 0;

        for (int i = 0; i < player.inventory.Count; i++) {
            display_option(player.inventory[optionsDisplayed].desc);
        }
        display_option("Go Back");
        //display_option("Wait");
        //display_option("Check Inventory");
    }
    
    // At this point I need just one display function that can take an array of options as the argument. Oh well. We'll save that for the refactor.
    // The problem is that the "option" construct is define in the "room" class. I need to include a utils document or the like with these definitions. 
    // either way here's another niche display function variant.
    private void display_item_useless(){
        uiDesc.text = "That item will not work here.";
        
        display_mode = "item_failed"; // I want to be able to put the player right back in their inventory after they fail to use an item.
        clear_options();
        //optionsDisplayed = 0;

        display_option("Go Back");
        //display_option("Wait");
        //display_option("Check Inventory");
    }
    // Alright here's another stupid display function. This one, hopefully will be more versatile.
    // It will take a temporary description to print to the screen to alert the player to information.
    // It should accept a string for the desc. And then the only option will be "Go Back"
    private void display_text(string desc) {
        uiDesc.text = desc;

        display_mode = "display_text";
        clear_options();

        display_option("Go Back");
    }

    private void visited() {
        roomsVisited += 1;
        Debug.Log("roomsVisited " + roomsVisited.ToString() );
    }


    private void pass_time(int step = 1) {
        time_passed += step;
        uiTime.text = "Time: " + time_passed.ToString();
        return;
    }

    static public void DISPLAY_ROOM(Room currRoom) {
        S.display_room(currRoom);
        /*
        S.uiDesc.text = Desc;
        S.optionsDisplayed = 0;
        */
        /*
        for (int i = 0; i < currentRoom.options.Count; i++) {

        }
        */
    }

    
    static public void DISPLAY_OPTION(string desc) {
        S.display_option(desc);
    }
    
    static public void PASS_TIME(int step = 1){
        S.pass_time(step);
    }

    static public void DISPLAY_ITEM_USELESS() {
        S.display_item_useless();
    }
    static public void DISPLAY_TEXT(string desc) {
        S.display_text(desc);
    }
    static public void VISITED() {
        S.visited();
    }
}
