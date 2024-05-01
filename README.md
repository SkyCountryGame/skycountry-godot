this is the sky country unity game converted to a godot game. currently i am considering 2 different approaches. 

1. keep as much of the game code decoupled from godot engine stuff as possible. this means making a "godot controller adapter interface thing" to attach godot nodes to the C# controller class, making an input manager with enums for the actions, and similar things with the other classes in the existing unity project. 
2. integrate the design into common godot engine design

since this is my first time using godot, i'm going to start with #2 just to get something running quicker. but #1 is better for handling the case where we would switch to another engine, and might actually be a reason to switch to monogame (the fact that i've kind of made my own engine).


-----

2024/4/2: 
move effects manager to project so we can keep track of what indicators and other stuff is temporarily displaying.
clean up folder.
maybe make a generalized script if there are enough things that need to be done in code that are common to every game object, then can just have a Controller object. 

https://www.reddit.com/r/godot/comments/16laocg/trouble_setting_up_godot_to_debug_c_code_in_vscode/