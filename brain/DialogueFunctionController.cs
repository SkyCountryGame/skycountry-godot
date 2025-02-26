using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;

public partial class DialogueFunctionController: GodotObject //needs to be GodotObject in order to use #Call
{
    private enum Function{
        [Description("CheckAndRemoveInventory")] // can work without this description now, but this would be the name of the function
        CheckAndRemoveInventory, //can name whatever we want as long as we populate description, if its named the same as the function we dont need description
        ChangeFunnyBucksFlag,
    }

    public bool run(string methodName, List<object> args)
    {
        Function function = (Function)Enum.Parse(typeof(Function), methodName);
        Variant[] convertedArgs;
        int argsCount = 0;
        if(args == null){
            convertedArgs = new Variant[0];
        } else {
            convertedArgs = new Variant[args.Count];
            argsCount = args.Count;
        }
        for(int i = 0; i<argsCount; i++){
            if(int.TryParse((string)args[i], out int j)){
                convertedArgs[i]=j;
            }
            else {
                convertedArgs[i]=(string)args[i];
            }
        }
        if(HasMethod(function.GetDescription())){
            Variant result = Call(function.GetDescription(), convertedArgs);
            switch(result.VariantType){
                case Variant.Type.Bool:
                    return (bool)result;
                default:
                    return true;
            }
        }
        return false;
    }

    public bool CheckAndRemoveInventory(string itemName, int count){
		Dictionary<string,int> detailedItemList = Global.playerModel.inv.GetDetailedItemList();
		if(detailedItemList.ContainsKey(itemName) && detailedItemList[itemName]>=count){
			for(int i = 0; i<count; i++){
				bool temp = Global.playerModel.inv.RemoveItemByName(itemName);
				if(!temp){
					GD.PrintErr("uh oh something fucked up in DialogueFunctionController#CheckAndRemoveInventory alert alert");
					return false;
				} 
			}
			return true;
		}
		return false;	
	}

    public bool ChangeFunnyBucksFlag(){
        return Global.flagManager.ChangeFunnyBucksProgressFlagValue((int)Global.flagManager.funnyBucksFlagValue << 1);
    }
}