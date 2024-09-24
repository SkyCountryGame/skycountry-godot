using System;
using Godot;

//this class generates a convex polygon for a collision shape, as secified by some properties
public partial class ConvexPolygonGenerator : CollisionShape3D {

    ConvexPolygonShape3D g; //the polygon shape (gon)

    public override void _Ready(){
        g = new ConvexPolygonShape3D();
        
    }
    
    public void GeneratePolygon(int numVertices, float radius){
        /*for (int l = 0; l < 3 ; l++){ //vertical layer
            for (int v = 0; v < 6; v++){ //vertexes
                GD.Print($"layer {l}, vertex {v}");

            }
        }*/
        float[] vertices = new float[]{
            0, 0, 0,
            1, 0, 0,
            1.4f, 0, 1,
            1, 0, 2,
            0, 0, 2,
            -0.4f, 0, 1,

            0, 0, 0,
            2, 0, 0,
            2.6f, 0, 2,
            2, 0, 4,
            0, 0, 4,
            -0.8f, 0, 2,

            0, 2, 0,
            1, 2, 0,
            1.4f, 2, 1,
            1, 2, 2,
            0, 2, 2,
            -0.4f, 2, 1,
        };
        g = new ConvexPolygonShape3D();
        g.Points = new Vector3[vertices.Length/3];
        for (int v = 0; v < vertices.Length; v+=3){
            g.Points[v*3] = new Vector3(vertices[v], vertices[v+1], vertices[v+2]);
        }
    }
}