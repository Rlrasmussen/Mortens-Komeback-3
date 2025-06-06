using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Command
{
    public class ChangeWeaponCommand : ICommand
    {

        private Player player;
        private WeaponType weaponType;


        public ChangeWeaponCommand(Player instance, WeaponType weapon)
        {

            player = instance;
            weaponType = weapon;

        }

        /// <summary>
        /// Changes players weapon
        /// Simon
        /// </summary>
        public void Execute()
        {

            player.ChangeWeapon(weaponType);

        }

    }
}
