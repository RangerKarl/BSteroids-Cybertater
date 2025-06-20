using Godot;

namespace BSteroids.Scripts.Game
{
    public partial class PlayerMovement : CharacterBody2D
    {
        [Export]
        float MaxSpeed = 10;

        [Export]
        double RotationSpeed = 3.5;

        [Export]
        double VelocityDampingFactor = 0.5;

        [Export]
        double LinearVelocity = 200;


        [Export]
        Vector2 localVelocity;

        [Export]
        Vector2 InputVector;
        int RotationDirection;


        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // For C# modifying individual values in Vector2 we need to make an explicit reference.
            localVelocity = Velocity;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            InputVector.X = Input.GetActionStrength("rotate_left") - Input.GetActionStrength("rotate_right");
            InputVector.Y = Input.GetActionStrength("thrust");

            if (Input.IsActionPressed("rotate_left"))
                RotationDirection = -1;
            else if (Input.IsActionPressed("rotate_right"))
                RotationDirection = 1;
            else
                RotationDirection = 0;
        }

        // calculate before every phys-step. Fixed time intervals
        public override void _PhysicsProcess(double delta)
        {
            // use the previously retrieved rotation to rotate our sprite!
            Rotation = Rotation + (float)(RotationDirection * RotationSpeed * delta);

            if (InputVector.Y > 0)
                AccelerateForward(delta);
            else if (-0.01 <= InputVector.Y && InputVector.Y <= 0.01 && Velocity != Vector2.Zero)
                SlowDownAndStop(delta);

            MoveAndCollide(localVelocity * (float)delta);
        }

        private void AccelerateForward(double delta)
        {
            // Accelerate the whole thing by multiplying the scalar
            // we need to apply the rotation as we update the InputVector
            InputVector = (InputVector * ((float)LinearVelocity * (float)delta)).Rotated(Rotation);

            localVelocity += InputVector;

            // Cap the length of the resulting Vector2 projection to a ceiling Scalar.
            localVelocity.LimitLength(MaxSpeed);
        }


        private void SlowDownAndStop(double delta)
        {

            localVelocity.Lerp(Vector2.Zero, (float)(VelocityDampingFactor * delta));

            // stop with a deadzone
            if (-0.1 <= Velocity.Y && Velocity.Y <= 0.1)
            {
                localVelocity.X = 0;
            }
        }


    }

}
