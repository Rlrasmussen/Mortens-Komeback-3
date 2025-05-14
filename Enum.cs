using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public enum PlayerType
    {
        Morten,
        MortenAngriber
    }

    public enum EnemyType
    {
        WalkingGoose,
        AggroGoose,
        Goosifer
    }

    public enum DoorType
    {
        Open,
        Closed,
        Locked,
        Unlocked
    }

    public enum WeaponType
    {
        Melee,
        Ranged
    }

    public enum AttackType
    {
        Swing,
        Egg
    }

    public enum SurfaceType
    {
    }

    public enum MenuType
    {
        Start,
        GameOver,
        Pause,
        Inventory,
        Win,
        Cursor
    }

    public enum OverlayObjects
    {
        Heart,
        Dialog
    }

    public enum Sound
    {
        GooseSound,
        EggSmash,
        PlayerDamage,
        PlayerHeal,
        PlayerShoot,
        PlayerWalk1,
        PlayerWalk2
    }

    public enum MusicTrack
    {
        Battle,
        Background
    }

    public enum ItemType
    {
        Key,
        GeesusBlood,
        Sling,
        Sword,
        WallTurkey
    }

    public enum Roomtype
    {
        Single,
        Square
    }

    public enum NPCType
    {
        CanadaGoose,
        GreyGoose,
        Pope,
        Monk,
        Nun
    }

    public enum PuzzleType
    {
    }

    public enum DecorationType
    {
        Torch,
        Spikes,
        Pillars,
        Stairs,
        Chest,
        Candles,
        Coffin
    }

}
