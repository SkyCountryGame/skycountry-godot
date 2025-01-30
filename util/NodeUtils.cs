using Godot;
using Godot.Collections;

public class NodeUtils {

    public static Array<Node> GetChildrenRec(Node n){
        Array<Node> res = n.GetChildren(true);
        foreach (Node child in n.GetChildren()){
            res.AddRange(GetChildrenRec(child));
        }
        return res;
    }

    public static void AddToGroup(Node n, string group){
        if (!n.IsInGroup(group)){
            n.AddToGroup(group);
        }
        foreach (Node cn in GetChildrenRec(n)){
            cn.AddToGroup(group);
        }
    }

}