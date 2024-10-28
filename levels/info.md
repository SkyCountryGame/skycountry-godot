level description json file format: 
    * each level has id. can we just generate that in code though? and in the json they just reference each other by key name?
    * sublevels: this is a list of places in the level that are separate scenes, like the inside of a building, that when visited, the "containing" level nodes should not be disposed of, because the player will usually be exiting the building right after
    * accessible_levels: a list of the ids (or keys?) of the levels that can be accessed from this level
        * though maybe this information should be stored in a graph in a separate file. 