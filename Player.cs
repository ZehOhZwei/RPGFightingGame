using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	
	Vector2 direction;
	Sprite2D sprite2D;
	AnimationPlayer animationPlayer;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
		sprite2D = (Sprite2D)GetNode("Sprite2D");
		animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		direction.X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		//Direction of the sprite
		if(direction.X >= 0)
		{
			sprite2D.FlipH = false;
		}
		else
		{
			sprite2D.FlipH = true;
		}

		if(IsOnFloor() && direction.X == 0)
		{
			animationPlayer.Play("Idle");
		}
		if(IsOnFloor() && direction.X != 0)
		{
			animationPlayer.Play("Run");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
