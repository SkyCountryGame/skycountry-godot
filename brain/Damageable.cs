using System.Collections;
using System.Collections.Generic;
using Godot;

/**
things that try to do damage to other entities check if an encountered thing is a Damageable
*/
public interface Damageable
{
    void ApplyDamage(int d /* TODO damagetype (fire, magic, slash)*/);
}
