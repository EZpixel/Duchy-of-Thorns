using DuchyofThorns.Scenes.Globals;

namespace DuchyOfThorns;

/// <summary>
/// Intermediate class for all infantry units
/// </summary>
public partial class Infantry : Troop
{
    public TroopState CurrentState { get; set; }
    public Vector2 AdvancePosition{ get; set; }

    protected Timer patrolTimer;
    protected Area2D detectionZone;
    protected Area2D attackZone;
    protected Timer attackTimer;
    protected NavigationAgent2D navAgent;

    private Actor enemy = null;
    private Melee weapon = null;

    public override void _Ready()
	{
		base._Ready();
        patrolTimer = GetNode<Timer>("PatrolTimer");
        detectionZone = GetNode<Area2D>("DetectionZone");
        attackZone = GetNode<Area2D>("AttackZone");
        attackTimer = GetNode<Timer>("AttackTimer");
        navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

        navAgent.MaxSpeed = Stats.Speed;
        navAgent.SetNavigationMap(GetNode<TileMap>("/root/World/TileMap").GetNavigationMap(0));
        navAgent.TargetPosition = GlobalPosition;
        navAgent.Connect("velocity_computed", new Callable(this, "Move"));
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        switch (CurrentState)
        {
            case TroopState.PATROL:
                return;

            case TroopState.ADVANCE:
                if (navAgent.IsTargetReached())
                {
                    SetState(TroopState.PATROL);
                    return;
                }
                Direction = GlobalPosition.DirectionTo(navAgent.GetNextPathPosition());
                Velocity += Direction * Stats.Speed;
                navAgent.SetVelocity(Velocity);
                break;

            case TroopState.ENGAGE:
                navAgent.TargetPosition = enemy.GlobalPosition;
                Direction = GlobalPosition.DirectionTo(navAgent.GetNextPathPosition());
                Velocity += Direction * Stats.Speed;
                navAgent.SetVelocity(Velocity);
                break;

            case TroopState.ATTACK:
                RotateToward(enemy.GlobalPosition);
                Attack();
                break;

            default:
                GD.PushError("Invalid TroopState");
                break;
        }
    }

    public void SetState(TroopState newState)
    {
        if (newState == CurrentState)
        {
            return;
        }
        switch (newState)
        {
            case TroopState.ADVANCE:
                patrolTimer.Stop();
                navAgent.TargetPosition = AdvancePosition;

                break;
            case TroopState.PATROL:
                Velocity = Vector2.Zero;
                patrolTimer.Start();
                break;

            case TroopState.ENGAGE:
                patrolTimer.Stop();
                break;

            case TroopState.ATTACK:
                patrolTimer.Stop();
                Velocity = Vector2.Zero;
                break;
        }
        CurrentState = newState;
    }

    private void Move(Vector2 velocity)
    {
        Velocity = velocity;
        RotateToward(velocity.Angle());
        MoveAndSlide();
    }

    private void DetectionZoneBodyEntered(PhysicsBody2D body)
    {
        if (body is Actor actorBody && actorBody.GetTeam() != Team &&
            CurrentState != TroopState.ENGAGE && CurrentState != TroopState.ATTACK)
        {
            enemy = actorBody;
            SetState(TroopState.ENGAGE);
        }
    }

    private void DetectionZoneBodyExited(PhysicsBody2D body)
    {
        if (body == enemy && enemy != null && !IsQueuedForDeletion())
        {
            SetState(TroopState.ADVANCE);
            enemy = null;
        }
    }

    private void AttackZoneBodyEntered(PhysicsBody2D body)
    {
        if (body is Actor actorBody && actorBody.GetTeam() != Team)
        {
            enemy = actorBody;
            SetState(TroopState.ATTACK);
        }
    }

    private void AttackZoneBodyExited(PhysicsBody2D body)
    {
        if (enemy is not null)
        {
            SetState(TroopState.ENGAGE);
        }
    }

    private void PatrolTimerTimeout()
    {
        float patrolRange = 75f;
        float randomX = Globals.GetRandomFloat(-patrolRange, patrolRange);
        float randomY = Globals.GetRandomFloat(-patrolRange, patrolRange);
        AdvancePosition = new Vector2(randomX, randomY) + AdvancePosition;
        SetState(TroopState.ADVANCE);
    }

    protected virtual void AttackTimerTimeout() => GD.PrintErr("Calling AttackTimerTimeout from Infantry class");
    public virtual void Attack() => GD.PrintErr("Calling Attack from Infantry class");
}
