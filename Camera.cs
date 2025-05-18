using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Mortens_Komeback_3
{
    public class Camera
    {

        #region Fields

        private static Camera instance;
        private Vector2 position;
        private readonly GraphicsDevice _graphicsDevice;

        #endregion
        #region Properties

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera(GameWorld.Instance.GraphicsDevice);

                return instance;
            }
        }

        /// <summary>
        /// Property to access the posi
        /// </summary>
        public Vector2 Position
        {

            get => position;

            set
            {

                position = value;

            }

        }

        /// <summary>
        /// Used to set the zoomlevel of the viewport (scaled float)
        /// </summary>
        public float Zoom { get; set; } = 0.2f;

        /// <summary>
        /// Used to rotate the camera
        /// </summary>
        public float Rotation { get; set; } = 0f;

        #endregion
        #region Constuctor

        /// <summary>
        /// Constructor for the camera viewport
        /// </summary>
        /// <param name="graphicsDevice">Defines which graphicsdevice to get viewport parameters from</param>
        private Camera(GraphicsDevice graphicsDevice)
        {

            _graphicsDevice = graphicsDevice;

        }

        #endregion
        #region Methods

        /// <summary>
        /// Transforms the cameraviewport to return value
        /// </summary>
        /// <returns>Center of screen location</returns>
        public Matrix GetTransformation()
        {
            var screenCenter = new Vector3(_graphicsDevice.Viewport.Width / 2f, _graphicsDevice.Viewport.Height / 2f, 0);

            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(screenCenter);
        }

        /// <summary>
        /// Reverts transformation to get position compared to viewport
        /// </summary>
        /// <returns>Inverted "GetTransformation" Matrix</returns>
        public Matrix InverseTransformation() => Matrix.Invert(GetTransformation());

        /// <summary>
        /// Converts a position on the viewport (mouse position as example) into ingame position (Screenspace -> Worldspace)
        /// </summary>
        /// <param name="position">The Vector2 to be transformed</param>
        /// <returns>Useable relative position</returns>
        public Vector2 RefactorPosition(Vector2 position) => Vector2.Transform(position, InverseTransformation());

        #endregion

    }
}
