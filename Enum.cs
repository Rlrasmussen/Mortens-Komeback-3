
namespace Mortens_Komeback_3
{

    /// <summary>
    /// Enum for identifying animations different of the player
    /// </summary>
    public enum PlayerType
    {
        Morten,
        MortenAngriber
    }

    /// <summary>
    /// Enum for identifying different types of enemies
    /// </summary>
    public enum EnemyType
    {
        WalkingGoose,
        AggroGoose,
        Goosifer //Goosifer needs to be last
    }

    /// <summary>
    /// Enum for identifying state of doors
    /// </summary>
    public enum DoorType
    {
        Open,
        Closed,
        Locked,
        Unlocked,
        Stairs,
        StairsLocked
    }

    /// <summary>
    /// Enum for identifying direction of doors
    /// </summary>
    public enum DoorDirection
    { 
        Top,
        Right,
        Bottom,
        Left

    }

    /// <summary>
    /// Enum for identifying weapons
    /// </summary>
    public enum WeaponType
    {
        Melee,
        Ranged
    }

    /// <summary>
    /// Enum for identifying attacks
    /// </summary>
    public enum AttackType
    {
        Swing,
        Egg
    }

    /// <summary>
    /// Enum for identifying surfaces
    /// </summary>
    public enum SurfaceType
    {
        AvSurface
    }

    /// <summary>
    /// Enum for identifying menu-items
    /// </summary>
    public enum MenuType
    {
        Start,
        GameOver,
        Pause,
        Inventory,
        Win,
        Cursor
    }

    /// <summary>
    /// Enum for identifying objects on the overlay
    /// </summary>
    public enum OverlayObjects
    {
        Heart,
        Dialog,
        InteractBubble
    }

    /// <summary>
    /// Enum for identifying sounds
    /// </summary>
    public enum Sound
    {
        GooseSound,
        CanadaGoose,
        Goosifer,
        EggSmash,
        PlayerDamage,
        PlayerHeal,
        PlayerShoot,
        PlayerWalk1,
        PlayerWalk2,
        PlayerSwordAttack,
        PlayerChange,
        Fire,
        Click,
        CatacombDoor
    }

    /// <summary>
    /// Enum for identifying music
    /// </summary>
    public enum MusicTrack
    {
        Battle,
        Background,
        Death,
        Win,
        Pope,
        GoosiferFigth,
        TrapRoom,
        Menu
    }

    /// <summary>
    /// Enum for identifying items
    /// </summary>
    public enum ItemType
    {
        Key,
        GeesusBlood,
        Sling,
        Sword,
        WallTurkey
    }

    /// <summary>
    /// Enum for identifying rooms
    /// </summary>
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

    /// <summary>
    /// Enum for identifying NPCs
    /// </summary>
    public enum NPCType
    {
        CanadaGoose,
        GreyGoose,
        Pope,
        Monk,
        Nun
    }

    /// <summary>
    /// Enum for identifying puzzles
    /// </summary>
    public enum PuzzleType
    {
        OrderPuzzle,
        OrderPuzzlePlaque,
        PathfindingPuzzle,
        ShootPuzzle,
    }

    /// <summary>
    /// Enum for identifying decorations
    /// </summary>
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

    /// <summary>
    /// Enum for identifying collisionpixel
    /// </summary>
    public enum DebugEnum
    {
        Pixel
    }

    /// <summary>
    /// Enum for identifying Vector2 positions
    /// </summary>
    public enum Location
    {
        Spawn,
        Test
    }

}
