using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;

public class FlagManager{
    string flagFilePath = "res://assets/flags.json";
    //Can prob remove this variable and just use the dictionary, and also using hard coded strings is freaky for me and also maintaining a list of Enums that is parallel to another class is freaky to me.  Thomas you got any ideas?
    public FlagResource.FunnyBucksProgressFlags funnyBucksFlagValue = FlagResource.FunnyBucksProgressFlags.Default;
    public Dictionary<string, Enum> dict = new Dictionary<string, Enum>();

    public FlagManager(){
        string flagStr = FileAccess.GetFileAsString(flagFilePath);
        JsonDocument jsonDocument = JsonDocument.Parse(flagStr);
        foreach(JsonProperty flagCategories in jsonDocument.RootElement.EnumerateObject()){
            foreach(JsonProperty jsonProperty in flagCategories.Value.EnumerateObject()){
                if(jsonProperty.Value.GetType() == typeof(int)){

                }
                else if(jsonProperty.Value.GetBoolean()){
                    if(flagCategories.NameEquals(FlagResource.FunnyBucksProgressFlags.TenBucks.GetType().Name)){
                        ChangeFunnyBucksProgressFlagValue((int)Enum.Parse(typeof(FlagResource.FunnyBucksProgressFlags), jsonProperty.Name));
                    }
                }
            }
        }
        dict = populateDictionary(funnyBucksFlagValue);
    }

    public FlagResource.FunnyBucksProgressFlags GetProgressFlag(){
        return funnyBucksFlagValue;
    }

    public bool ChangeFunnyBucksProgressFlagValue(int inputValue){
        if(Enum.IsDefined((FlagResource.FunnyBucksProgressFlags)((int)funnyBucksFlagValue << 1))){
            while((int)funnyBucksFlagValue < inputValue){
                funnyBucksFlagValue = (FlagResource.FunnyBucksProgressFlags)((int)funnyBucksFlagValue << 1);
            }
            dict["FunnyBucksProgressFlags"]=funnyBucksFlagValue;
            return true;
        }
        return false;
    }


    public Dictionary<string, Enum> populateDictionary(FlagResource.FunnyBucksProgressFlags funnyBucksFlagValue){
        Dictionary <string,Enum> dictionary = new Dictionary<string, Enum>
        {
            { "FunnyBucksProgressFlags", funnyBucksFlagValue }
        };
        return dictionary;
    }
}