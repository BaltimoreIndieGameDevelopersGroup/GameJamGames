using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zombie.Components.General
{
  
    public class InputAction
    {
        private readonly Buttons[] buttons;
        private readonly Keys[] keys;
        private readonly bool newPressOnly;

        // These delegate types map to the methods on InputState. We use these to simplify the evalute method
        // by allowing us to map the appropriate delegates and invoke them, rather than having two separate code paths.
        private delegate bool ButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex player);
        private delegate bool KeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex player);

        
        /// false if it occurs each frame one of the buttons/keys is down.</param>
        public InputAction(Buttons[] buttons, Keys[] keys, bool newPressOnly)
        {
            // Store the buttons and keys. If the arrays are null, we create a 0 length array so we don't
            // have to do null checks in the Evaluate method
            this.buttons = buttons != null ? buttons.Clone() as Buttons[] : new Buttons[0];
            this.keys = keys != null ? keys.Clone() as Keys[] : new Keys[0];

            this.newPressOnly = newPressOnly;
        }

      
        public bool Evaluate(InputState state, PlayerIndex? controllingPlayer, out PlayerIndex player)
        {
            // Figure out which delegate methods to map from the state which takes care of our "newPressOnly" logic
            ButtonPress buttonTest;
            KeyPress keyTest;
            if (newPressOnly)
            {
                buttonTest = state.IsNewButtonPress;
                keyTest = state.IsNewKeyPress;
            }
            else
            {
                buttonTest = state.IsButtonPressed;
                keyTest = state.IsKeyPressed;
            }

            // Now we simply need to invoke the appropriate methods for each button and key in our collections
            foreach (Buttons button in buttons)
            {
                if (buttonTest(button, controllingPlayer, out player))
                    return true;
            }
            foreach (Keys key in keys)
            {
                if (keyTest(key, controllingPlayer, out player))
                    return true;
            }

            // If we got here, the action is not matched
            player = PlayerIndex.One;
            return false;
        }
    }
}
