let's think through some different interaction situations. 

Pickup item: 

Mine a rock with a pickaxe:

Talk to a NPC:

need to think about if the SkyCountry GameObject abstraction is really necessary. it's possible to check if a node is interactable without having had called RegisterGameObject. what other purposes would using GameObject serve? 
if decide to go with checking type rather than GameObjectType enum, remove RegisterGameObject calls