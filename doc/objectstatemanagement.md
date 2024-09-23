HUDManager:
is always in one of the following states:
    - showing normal gameplay things (hp, ammo, minimap?, compass?, )
    - showing the player's inventory
    - showing the inventory of a chest that player has just opened up
    - showing a trading GUI when the player is trading with an NPC
    - showing a currently active dialogue, which includes a statement said by the NPC, along with one of the following:
        - nothing else, just the close/continue buttons
        - some response choices for the player to choose from
        -

Player:
is always in one of the following states:
    - default. normal gameplay. can walk around and interact with the world
    -

I now think that state change function (UpdateState()) should actually go in the node classes, because it needs to update animation

________________________________________________________________________________________________
state changes COULD have payloads associated with them.
