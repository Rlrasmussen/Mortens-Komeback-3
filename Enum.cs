﻿
namespace Mortens_Komeback_3
{

    /// <summary>
    /// Enum for identifying animations different of the player
    /// </summary>
    public enum PlayerType
    {
        Morten,
        MortenMunk,
        MortenAngriber,
        MortenSling
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
        StairsLocked,
        StairsUp
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
        AvSurface,
        Fireball,
        Spikes,
        BigSpikes
        
    }

    /// <summary>
    /// Enum for identifying menu-items
    /// </summary>
    public enum MenuType
    {
        MainMenu,
        GameOver,
        Pause,
        Inventory,
        Win,
        Cursor,
        Playing
    }
    public enum ButtonAction
    {
        StartGame,
        QuitGame,
        TryAgain,
        ResumeGame,
        ToggleMusic,
        ToggleSound,
        Reload
    }

    public enum ButtonSpriteType
    {
        Button,
        ButtonPressed,
        ButtonSquare,
        ButtonSquareChecked
        
    }

    /// <summary>
    /// Enum for identifying objects on the overlay
    /// </summary>
    public enum OverlayObjects
    {
        Heart,
        Dialog,
        InteractBubble,
        DialogBox,
        WeaponBox,
        Bibel,
        Rosary
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
        CatacombDoor,
        PuzzleFail,
        PuzzleSucces,
        Ghost
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
        WallTurkey,
        Bible,
        Rosary,
        Grail
    }

    /// <summary>
    /// Enum for identifying rooms
    /// </summary>
    public enum RoomType
    {
        PopeRoom,
        Stairs,
        CatacombesA,
        CatacombesA1,
        CatacombesB,
        CatacombesC,
        CatacombesD,
        CatacombesD1,
        CatacombesE,
        CatacombesF,
        CatacombesG,
        CatacombesH,
        TrapRoom,
        Cutscene

    }

    /// <summary>
    /// Enum for identifying NPCs
    /// </summary>
    public enum NPCType
    {
        CanadaGoose,
        Pope,
        Monk,
        Nun,
        Hole0,
        Coffin,
        Empty,
        Ghost,
        Chest
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
        PuzzleObstacle
    }

    public enum EnvironmentType
    {
        Lever,
        Plaque,
        Spikes,
        Pillars,
        Stairs,
        Chest,
        ChestOpen,
        Coffin,

    }

    /// <summary>
    /// Enum for identifying decorations
    /// </summary>
    public enum DecorationType
    {
        Torch,
        Candle,
        Cobweb,
        Light,
        Cross,
        Coffin,
        Splash,
        Hole1,
        Hole2,
        Tomb,
        Barrel,
        Painting,
        Pentagram,
        Spejlegg

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
        Test,
        PuzzleOne,
        PuzzleTwo,
        PuzzleThree
    }
    /// <summary>
    /// Enum for identifying tile type, used for Astar algorithm.
    /// </summary>
    public enum TileEnum
    {
        Tile
    }

    public enum StatusType
    {
        EnemiesKilled,
        WeaponMelee,
        WeaponRanged,
        Health,
        PlayerDead,
        Bible,
        Rosary,
        Delivered,
        GoosiferFigth,
        BackGroundMusic,
        Win
    }

    public enum CutSceneRoom
    {
        CutsceneMovie
    }
}
