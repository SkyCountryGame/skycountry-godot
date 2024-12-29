using System;
using System.Text.Json;
using Godot;

public class FlagManager{
    string flagFilePath = "res://assets/flags.json";
    public FunnyBucksProgressFlags funnyBucksFlagValue = FunnyBucksProgressFlags.NoBucks;
    [Flags]
    public enum FunnyBucksProgressFlags{
        NoBucks = 1<<0,
        TenBucks = 1<<1,
        TwentyBucks = 1<<2,
        ThirtyBucks = 1<<3
    }

    public FlagManager(){
        string flagStr = FileAccess.GetFileAsString(flagFilePath);
        JsonDocument jsonDocument = JsonDocument.Parse(flagStr);
        foreach(JsonProperty flagCategories in jsonDocument.RootElement.EnumerateObject()){
            foreach(JsonProperty jsonProperty in flagCategories.Value.EnumerateObject()){
                if(jsonProperty.Value.GetBoolean()){
                    if(flagCategories.NameEquals(FunnyBucksProgressFlags.TenBucks.GetType().Name)){
                        ChangeFunnyBucksProgressFlagValue((int)Enum.Parse(typeof(FunnyBucksProgressFlags), jsonProperty.Name));
                    }
                }
            }
        }
    }

    public FunnyBucksProgressFlags GetProgressFlag(){
        return funnyBucksFlagValue;
    }

    public bool ChangeFunnyBucksProgressFlagValue(int inputValue){
        if(Enum.IsDefined((FunnyBucksProgressFlags)((int)funnyBucksFlagValue << 1))){
            while((int)funnyBucksFlagValue < inputValue){
                funnyBucksFlagValue = (FunnyBucksProgressFlags)((int)funnyBucksFlagValue << 1);
            }
            return true;
        }
        return false;
    }
}