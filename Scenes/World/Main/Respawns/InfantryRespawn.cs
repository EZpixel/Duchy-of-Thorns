using DuchyofThorns.Scenes.Globals;

namespace DuchyOfThorns;

/// <summary>
/// Class for spawning infantry units
/// </summary>
public partial class InfantryRespawn : Respawn
{
    Infantry aliveUnit;
    Vector2 nextBaseCord = Vector2.Zero;
    public override void _Ready()
    {
        base._Ready();
        nextBaseCord = GlobalPosition;
    }
    public override void SpawnUnit()
    {
        if (RespawnCount > 0 && Unit != null)
        {
            aliveUnit = Unit.Instantiate<Infantry>();
            AddChild(aliveUnit);
            aliveUnit.Connect("Died", new Callable(this, "HandleUnitDeath"));
            aliveUnit.NextBase = nextBaseCord;
            aliveUnit.SetState(TroopState.ADVANCE);
            RespawnCount--;
        }
        else
        {
            base.Clear();
            EmitSignal("OutOfTroops");
        }
    }
    public override void SetCapturableBase(Vector2 nextBaseCord)
    {
        this.nextBaseCord = nextBaseCord;
        if (aliveUnit is null)
        {
            return;
        }
        aliveUnit.NextBase = nextBaseCord;
        aliveUnit.SetState(TroopState.ADVANCE);
    }
    public override void Clear()
    {
        if (Unit is null)
        {
            return;
        }
        base.Clear();
        aliveUnit.QueueFree();
    }
}
