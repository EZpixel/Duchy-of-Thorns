namespace DuchyOfThorns;

/// <summary>
/// Intermediate class for all cavalry units
/// </summary>
public partial class Cavalry : Actor
{
    PackedScene damagePopup;
    public override void _Ready()
    {
        base._Ready();
    }
    public override void HandleHit(float baseDamage, Vector2 impactPosition)
    {
        return;
    }
    public override void Die()
    {
        return;
    }
}
