using Godot;
using static StateManager;

//encapsulates motion-handling logic within a module that can be added to the player or any character
//and adds functionality such as acceleration
//uses a CharacterBody3D as a physics body to manipulate (might in RigidBody3D in future)

[GlobalClass]
public partial class MotionModule : Node3D  {
    [Export] private float jolt = 30; //change of acceleration wrg time. this is always constant (or infinity if -1)
    private Vector3 accel_goal;
    private Vector3 vel_goal; //the velocity that the character is currently trying to reach
    public Vector3 pos_goal; //the position that the character is currently trying to reach
    private Vector3 accel;
    [Export] private float gravity = -20f;
    private bool jump = false;
    [Export] private float max_accel = 40f;
    [Export] private float max_speed = 16f;
    //TODO 
    /* need to decrease acceleration when some distance from target. to avoid overshooting
    */

    public override void _Ready(){
        base._Ready();
    }

    public void Update(double dt, CharacterBody3D body){
        // Define a deceleration distance
        float deceleration_distance = .4f;//2.0f; // Adjust this value as needed

        // Calculate the distance to the target
        float distance_to_target = (pos_goal - body.GlobalPosition).Length();

        // Calculate the velocity goal
        vel_goal = (pos_goal - body.Position).Normalized() * max_speed;

        // If within the deceleration distance, reduce the velocity goal proportionally
        if (distance_to_target < deceleration_distance)
        {
            vel_goal *= distance_to_target / deceleration_distance;
        }

        // Calculate the acceleration goal
        accel_goal = (vel_goal - body.Velocity) * max_accel;

        // Apply the acceleration
        accel = accel_goal; // Don't do jolt for now

        // Clamp the acceleration to the maximum acceleration
        if (accel.Length() > max_accel)
        {
            accel = accel.Normalized() * max_accel;
        }

        // If the acceleration is very small, stop the body
        if (accel.Length() < .05f)
        {
            accel = Vector3.Zero;
            body.Velocity = vel_goal;
        }
        else
        {
            body.Velocity += accel * (float)dt;
            // Clamp the velocity to the maximum speed
            if (body.Velocity.Length() > max_speed)
            {
                body.Velocity = body.Velocity.Normalized() * max_speed;
            }
        }

        // If the velocity is very small, stop the body
        if (body.Velocity.Length() <= .08f)
        {
            body.Velocity = Vector3.Zero;
        }

        // TODO: Implement jumping

        // Move the body
        body.MoveAndSlide();
    }

    public void UpdateOld(double dt, CharacterBody3D body)
    {
        if ((pos_goal - ((Node3D)body.GetParent()).GlobalPosition).Length() < .05f){
            body.Velocity = Vector3.Zero;
            body.MoveAndSlide();
            return;
        }
        vel_goal = (pos_goal - ((Node3D)body.GetParent()).GlobalPosition).Normalized() * max_speed;
        accel_goal = (vel_goal - body.Velocity).Normalized() * max_accel;
        accel = accel_goal; //don't do jolt for now
        //accel = (accel_goal - accel).Normalized() * jolt * (float)dt;  //(vel_goal - Velocity).Normalized() * max_accel;
        if (accel.Length() > max_accel){
            accel = accel.Normalized() * max_accel;
        }
        if (accel.Length() < .05f){
            accel = Vector3.Zero;
            body.Velocity = vel_goal;
        } else {
            body.Velocity += accel * (float)dt;
            if (body.Velocity.Length() > max_speed){
                body.Velocity = body.Velocity.Normalized() * max_speed;
            }
        }
        
        if (  body.Velocity.Length()   /*Mathf.Abs((vel_goal - Velocity).Length())*/ <= .08f){
            body.Velocity = Vector3.Zero;
        }

        //TODO jumping

        body.MoveAndSlide();
    }

    public void Stop(CharacterBody3D body){
        vel_goal = Vector3.Zero;
    }

    //NOTE later might be OnStateChange, from listener interface
    public void HandleStateChange(State state){

    }


}