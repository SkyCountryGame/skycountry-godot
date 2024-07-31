

item spawning

main menu

level switchin system
    on level start:
    GD.Load<PackedScene>(path) all the relevant scenes that can be access from this scene.

    when item picked up and added to inv, mark it to not be disposed of.
    OR dispose the Node3D and keep track of the object type, so that it can be respawned. it can be the same mesh+texture, but the datamodel will have to be saved and reapplied. the datamodel should just stay in mem.
    for items that have Node3D changes based on modelstate, use a scene that automatically deals with that.

    with this, we could use ChangePackedScene(). but we should still avoid that because of performance of repeated disk usage and reloading the same stuff onto mem that we just freed.



dialogue

audio from http. 

folder organization:
    gameobjects (or prefab): all the tscn files for in game objects that have meshes

thinking on GameObjectManager, LevelManager, Global. want to make sure these abstractions make sense. 
    a "level" can be thought of as a set of scenes for now
    Global: 
        holds static references to things that need to be widely accessible throughout the codebase and need to persist across scene changes. 


    some objects are instantiated right away in the scene (set in editor)
    some objects will be instantiated dynamically via code/events
    objects that can be picked up can be either of those classes, therefore pickupables need to have a PackedScene assoc with them
    the ones that are a part of the scene itself need to have their PackedScenes specified manually in the editor
        OR just read the .tscn files upon level start. 

    we need IDs/labels for every possible inventory item. but first, what are all the properties that invitems can have? and how do we want to make that stuff in game? 
    remember to clean up the commented-out code in scenemanager
    need a way to clear out the active gameobjects when a node is unloaded
    i really feel like SceneManager stuff should be static
    remember to move RegisterGameObject into SceneManager. also what is all the purpose of this function?     

random ideas:
    voice-over: it's the interface, the form of the interface, the interface to what is known.
