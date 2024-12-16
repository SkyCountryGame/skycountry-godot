using System;
using System.Text.Json;
using Godot;

public class FlagManager{
    string flagFilePath = "res://assets/flags.json";
    public RockProgressFlags rockFlagValue = RockProgressFlags.NoRocks;
    [Flags]
    public enum RockProgressFlags{
        NoRocks = 1<<0,
        TenRocks = 1<<1,
        TwentyRocks = 1<<2,
        ThirtyRocks = 1<<3
    }

    public FlagManager(){
        string flagStr = FileAccess.GetFileAsString(flagFilePath);
        JsonDocument jsonDocument = JsonDocument.Parse(flagStr);
        foreach(JsonProperty flagCategories in jsonDocument.RootElement.EnumerateObject()){
            foreach(JsonProperty jsonProperty in flagCategories.Value.EnumerateObject()){
                if(jsonProperty.Value.GetBoolean()){
                    if(flagCategories.NameEquals(RockProgressFlags.TenRocks.GetType().Name)){
                        ChangeRockProgressFlagValue((int)Enum.Parse(typeof(RockProgressFlags), jsonProperty.Name));
                    }
                }
            }
        }
    }

    public RockProgressFlags GetProgressFlag(){
        return rockFlagValue;
    }

    public bool ChangeRockProgressFlagValue(int inputValue){
        if(Enum.IsDefined((RockProgressFlags)((int)rockFlagValue << 1))){
            while((int)rockFlagValue < inputValue){
                rockFlagValue = (RockProgressFlags)((int)rockFlagValue << 1);
            }
            return true;
        }
        return false;
    }
}