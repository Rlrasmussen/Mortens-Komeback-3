using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Menu
{
    class MenuOverlay
    {
        #region Fields
        private MenuType menuScreen;
        
        #endregion
        #region Constructor
        public MenuOverlay(MenuType type)
        {
            this.menuScreen = type;
        }
        #endregion
        #region Methods
        public void ShowMenu()
        {
            switch (menuScreen)
            {
                case MenuType.MainMenu:
                    
                    break;
                case MenuType.GameOver:
                    break;
                case MenuType.Pause:
                    break;
                case MenuType.Win:
                    break;
                case MenuType.Playing:
                    
                    break;
                default:
                    break;
            }
        }


        
        #endregion
    }
}
