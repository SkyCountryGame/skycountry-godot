for now, assuming that NPCs will just be created in the editor, placed into the level, and have their NPCModel set there. 
no dynamic loading of NPCs currently, so don't need to preemptively store NPCModels and PackedScenes for them. 

associating animations with NPC emotions and states:
    - NPCModel resource holds a Map<NPCStateOrEmotion, AnimationKey>
    - NPCNode can listen for AnimationChange 