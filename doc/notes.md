
next things to do 2024/7/31 TG:
    dialogue
    folder organize
    game saving and loading
    make some other pickupable objects to test the workflow
    clean up code as go and write docs

item spawning


dialogue

audio from http. 

folder organization:
    gameobjects (or prefab): all the tscn files for in game objects that have meshes

SceneManager stuff:
    a "level" can be thought of as a set of scenes for now
    Global: 
        holds static references to things that need to be widely accessible throughout the codebase and need to persist across scene changes. 

    some objects are instantiated right away in the scene (set in editor), some objects will be instantiated dynamically via code/events.
    objects that can be picked up can be either of those classes, therefore pickupables need to have a PackedScene assoc with them.
    the ones that are a part of the scene itself need to have their PackedScenes specified manually in the editor
        (OR is it possible to just read the .tscn files upon level start and get the info from there?)

    we need IDs/labels for every possible inventory item. but first, what are all the properties that invitems can have? and how do we want to make that stuff in game? 