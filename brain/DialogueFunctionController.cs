using System;
using System.Collections.Generic;
using Godot;

public partial class DialogueFunctionController: GodotObject //needs to be GodotObject in order to use #Call
{
    public bool run(string methodName, List<object> args)
    {
        Variant[] convertedArgs = new Variant[args.Count];
        for(int i = 0; i<args.Count; i++){
            if(int.TryParse((string)args[i], out int j)){
                convertedArgs[i]=j;
            }
            else {
                convertedArgs[i]=(string)args[i];
            }
        }
        Variant result = Call(methodName,convertedArgs);
        switch(result.VariantType){
            case Variant.Type.Bool:
                return (bool)result;
            default:
                return true;
        }
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
}