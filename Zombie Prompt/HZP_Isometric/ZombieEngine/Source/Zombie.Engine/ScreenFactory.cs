using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zombie.Components;
using Zombie.Components.General;

namespace Zombie.Engine
{

    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            // All of the screens have empty constructors so just use Activator
            return Activator.CreateInstance(screenType) as GameScreen;

            
        }
    }
}
