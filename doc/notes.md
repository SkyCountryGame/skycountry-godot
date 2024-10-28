some random notes that are not yet categorized into specific doc files.

- do we need a "world time" that cycles to represent the repeating daily happenings

- NPCNode can have a specific type of NodeModel depending on if it's an enemy, shopkeeper, random person, questgiver, etc. 

- what objects/systems in the game have "state" and need a "state machine" (constraints on attributes based on state, and things that need happen when transioning from state X to state Y)
    - player
    - enemy
    - NPC
    - level?

- state management thoughts. https://shaggydev.com/2023/10/08/godot-4-state-machines/
    - state_machine.process_input(event)  state machine shouldn't take the InputEvent object
    - for now we'll keep state management in the model class of the model of interest. if the state management for that object gets too complex, then move it into a seperate ModelStateMachine or ModelStateManager class, which handles transitions of between all states. only if that gets too complex then we would separate logic into single-state classes. 


- camera
    there's a possibility we would have multiple cameras, and switch between them based on events and gameplay happenings, so would need something like a CameraManager to set the correct camera. OR, just have a bunch of "camera locations" (similar to spawnpoints) that the camera would jump to upon the appropriate events. 

- idea for generating floors https://docs.godotengine.org/en/3.1/classes/class_surfacetool.html#class-surfacetool
                https://docs.godotengine.org/en/stable/tutorials/3d/procedural_geometry/arraymesh.html