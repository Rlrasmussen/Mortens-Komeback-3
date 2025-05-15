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
        Goosifer //Goosifer needs to be last
    }

    public enum DoorType
    {
        Open,
        Closed,
        Locked,
        Unlocked
    }

    public enum DoorDirection
    { 
        Top,
        Right,
        Bottom,
        Left

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
        PlayerWalk2,
        PlayerSwordAttack
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

    public enum RoomType
    {
        PopeRoom,
        Stairs,
        CatacombesA,
        CatacombesB,
        CatacombesC,
        CatacombesD,
        CatacombesE,
        CatacombesF,
        CatacombesG,
        CatacombesH,
        TrapRoom

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
        OrderPuzzle,
        OrderPuzzlePlaque,
        PathfindingPuzzle,
        ShootPuzzle,
    }

    public enum DecorationType
    {
        Torch,
        Spikes,
        Pillars,
        Stairs,
        Chest,
        Candles,
        Coffin,
    }

    public enum DebugEnum
    {
        Pixel
    }

    public enum Location
    {
        Spawn
    }

}
