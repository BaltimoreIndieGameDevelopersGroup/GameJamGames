
#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace Zombie.Engine
{

    class PlayerIndexEventArgs : EventArgs
    {

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }


        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        PlayerIndex playerIndex;
    }
}
