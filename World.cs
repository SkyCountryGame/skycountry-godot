using Godot;

namespace SkyCountry;

public class World
{
    public World()
    {
        //attempting to create some world object
        //create the godot scene nodes
        RigidBody3D chest = new RigidBody3D();
        CollisionShape3D colShape = new CollisionShape3D();
        //- init colShape
        //MeshInstance3D mesh = new MeshInstance3D();
        //mesh.Mesh = new Mesh();
        
        //assemble the world object by connecting the nodes
        chest.AddChild(colShape);
        //chest.AddChild(mesh);
        
    }
    
}