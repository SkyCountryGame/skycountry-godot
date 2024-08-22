some random notes that are not yet categorized into specific doc files.

- establish sensible separation between PlayerModel and Player
- need to organize Global and SceneManager. should we have 1 autoload singleton Global, which holds things like SceneManager, AtmosphereManager, 
    - Global class holds references to globally-accessible data.
    - SceneManager
    - AtmosphereManager
    - each level registers itself with the SceneManager upon _Ready()
    - the SceneManager listens for some events broadcasted from anywhere within the gameplay logic, such as EnteredRoom, QuestReceived, DialogueInitiated, EntityAttacked, EntityDamaged, ItemPickedUp, TimeSet, WeatherBegin
    - for example, the AtmosphereManager might broadcast that a storm is beginning in some location, so it calls EventManager.Invoke(WeatherBegin, event parameters). then any object can listen for
    - 

- NPCNode can have a specific type of NodeModel depending on if it's an enemy, shopkeeper, random person, questgiver, etc. 

- what objects/systems in the game have "state" and need a "state machine" (constraints on attributes based on state, and things that need happen when transioning from state X to state Y)
    - player
    - enemy
    - NPC
    - level?

- state management thoughts. https://shaggydev.com/2023/10/08/godot-4-state-machines/
    - state_machine.process_input(event)  state machine shouldn't take the InputEvent object
    - for now we'll keep state management in the model class of the model of interest. if the state management for that object gets too complex, then move it into a seperate ModelStateMachine or ModelStateManager class, which handles transitions of between all states. only if that gets too complex then we would separate logic into single-state classes. 


