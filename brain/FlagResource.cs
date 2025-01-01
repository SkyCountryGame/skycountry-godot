using Godot;
using System;
using System.Collections.Generic;
[GlobalClass]
public partial class FlagResource: Resource
{
    public Dictionary<string, Enum> dict;
    //Will need to make this toned down so every flag doesnt check every flag value
    [Export]
    public FunnyBucksProgressFlags funnyBucksFlagValue;

    public enum FunnyBucksProgressFlags{
        Default = 1<<0,
        TenBucks = 1<<1,
        TwentyBucks = 1<<2,
        ThirtyBucks = 1<<3
    }

    public bool compareFlags(){
        //did it this way because export is the last thing that gets populated and a constructor runs before that.  Dont want to populate it everytime like this though
        dict = Global.flagManager.populateDictionary(funnyBucksFlagValue);
        foreach(KeyValuePair<string,Enum> entry in dict){
            if(Global.flagManager.dict[entry.Key].CompareTo(dict[entry.Key])<0){
                return false;
            }
        }
        return true;
    }
}