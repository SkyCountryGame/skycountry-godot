using System.Collections;
using System.Collections.Generic;
using Godot;

/**
this is so that in game entities (enemies, NPCs, etc.) can behave differently based on what collider zone is triggered. unity doesn't allow that by default for Colliders attached to GameObjects
*/

public enum ColliderZone
{ 
    Awareness0, Awareness1, Body
}

public interface Collideable
{
    void HandleCollide(ColliderZone zone, Node other);
    void HandleDecollide(ColliderZone zone, Node other);
}
