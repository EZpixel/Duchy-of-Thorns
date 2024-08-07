﻿namespace DuchyOfThorns;
public enum Team
{
    PLAYER,
    ENEMY,
    NEUTRAL
}

public enum TroopState
{
    NONE,

    PATROL,
    ENGAGE,
    ADVANCE,
    ATTACK
}

public enum BaseCaptureOrder
{
    FIRST,
    LAST
}

public enum LoadingForm
{
    New,
    Save,
    Load
}

public enum ProjectileType
{
    ARROW,
    FIRE_ARROW,
    FIRE_SPELL
}

public enum LootType
{
    GOLD,
    SILVER,
    BRONZE
}

public enum TroopType
{
    NONE,

    ALLY_ARCHER,
    ALLY_FOOTMAN,

    ENEMY_ARCHER,
    ENEMY_FOOTMAN,
    ENEMY_MAGE
}