using System;

namespace Zombie.Components.General
{
    
    public interface IScreenFactory
    {
        
        GameScreen CreateScreen(Type screenType);
    }
}
