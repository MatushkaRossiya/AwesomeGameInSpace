using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Gamepad : MonoSingleton<Gamepad> {

    private bool playerIndexSet = false;
    private PlayerIndex playerIndex;
    private GamePadState state;
    private GamePadState prevState;
    private bool init = false;

    public override void Init()
    {
        if (init)
            return;

        for (int i = 0; i < 4; ++i)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                playerIndex = testPlayerIndex;
                playerIndexSet = true;
            }
        }

        state = GamePad.GetState(playerIndex);
        prevState = state;
        init = true;
    }
	
    void Update()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }

    public bool isConnected()
    {
        return state.IsConnected;
    }

    public float leftTrigger()
    {
        return state.Triggers.Left;
    }

    public float rightTrigger()
    {
        return state.Triggers.Right;
    }

    public Vector2 leftStick()
    {
        Vector2 value;
        value.x = state.ThumbSticks.Left.X;
        value.y = state.ThumbSticks.Left.Y;
        return value;
    }

    public Vector2 rightStick()
    {
        Vector2 value;
        value.x = state.ThumbSticks.Right.X;
        value.y = state.ThumbSticks.Right.Y;
        return value;
    }

    public bool pressedBack()
    {
        return state.Buttons.Back == ButtonState.Pressed;
    }

    public bool pressedStart()
    {
        return state.Buttons.Start == ButtonState.Pressed;
    }

    public bool pressedDPadDown()
    {
        return state.DPad.Down == ButtonState.Pressed;
    }

    public bool justPressedDPadDown()
    {
        return state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released;
    }

    public bool pressedDPadLeft()
    {
        return state.DPad.Left == ButtonState.Pressed;
    }

    public bool justPressedDPadLeft()
    {
        return state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released;
    }

    public bool pressedDPadRight()
    {
        return state.DPad.Right == ButtonState.Pressed;
    }

    public bool justPressedDPadRight()
    {
        return state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released;
    }

    public bool pressedDPadUp()
    {
        return state.DPad.Up == ButtonState.Pressed;
    }

    public bool justPressedDPadUp()
    {
        return state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released;
    }

    public bool pressedA()
    {
        return state.Buttons.A == ButtonState.Pressed;
    }

    public bool justPressedA()
    {
        return state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released;
    }

    public bool releasedA()
    {
        return state.Buttons.A == ButtonState.Released;
    }
    
    public bool justReleasedA()
    {
        return state.Buttons.A == ButtonState.Released && prevState.Buttons.A == ButtonState.Pressed;
    }

    public bool pressedB()
    {
        return state.Buttons.B == ButtonState.Pressed;
    }
    
    public bool justPressedB()
    {
        return state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released;
    }
    
    public bool releasedB()
    {
        return state.Buttons.B == ButtonState.Released;
    }
    
    public bool justReleasedB()
    {
        return state.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed;
    }

    public bool pressedX()
    {
        return state.Buttons.X == ButtonState.Pressed;
    }
    
    public bool justPressedX()
    {
        return state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released;
    }
    
    public bool releasedX()
    {
        return state.Buttons.X == ButtonState.Released;
    }
    
    public bool justReleasedX()
    {
        return state.Buttons.X == ButtonState.Released && prevState.Buttons.X == ButtonState.Pressed;
    }

    public bool pressedY()
    {
        return state.Buttons.Y == ButtonState.Pressed;
    }
    
    public bool justPressedY()
    {
        return state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released;
    }
    
    public bool releasedY()
    {
        return state.Buttons.Y == ButtonState.Released;
    }
    
    public bool justReleasedY()
    {
        return state.Buttons.Y == ButtonState.Released && prevState.Buttons.Y == ButtonState.Pressed;
    }

    public bool pressedLeftStick()
    {
        return state.Buttons.LeftStick == ButtonState.Pressed;
    }
    
    public bool justPressedLeftStick()
    {
        return state.Buttons.LeftStick == ButtonState.Pressed && prevState.Buttons.LeftStick == ButtonState.Released;
    }
    
    public bool releasedLeftStick()
    {
        return state.Buttons.LeftStick == ButtonState.Released;
    }
    
    public bool justReleasedLeftStick()
    {
        return state.Buttons.LeftStick == ButtonState.Released && prevState.Buttons.LeftStick == ButtonState.Pressed;
    }

    public bool pressedRightStick()
    {
        return state.Buttons.RightStick == ButtonState.Pressed;
    }
    
    public bool justPressedRightStick()
    {
        return state.Buttons.RightStick == ButtonState.Pressed && prevState.Buttons.RightStick == ButtonState.Released;
    }
    
    public bool releasedRightStick()
    {
        return state.Buttons.RightStick == ButtonState.Released;
    }
    
    public bool justReleasedRightStick()
    {
        return state.Buttons.RightStick == ButtonState.Released && prevState.Buttons.RightStick == ButtonState.Pressed;
    }

    public bool pressedLeftShoulder()
    {
        return state.Buttons.LeftShoulder == ButtonState.Pressed;
    }
    
    public bool justPressedLeftShoulder()
    {
        return state.Buttons.LeftShoulder == ButtonState.Pressed && prevState.Buttons.LeftShoulder == ButtonState.Released;
    }
    
    public bool releasedLeftShoulder()
    {
        return state.Buttons.LeftShoulder == ButtonState.Released;
    }
    
    public bool justReleasedLeftShoulder()
    {
        return state.Buttons.LeftShoulder == ButtonState.Released && prevState.Buttons.LeftShoulder == ButtonState.Pressed;
    }

    public bool pressedRightShoulder()
    {
        return state.Buttons.RightShoulder == ButtonState.Pressed;
    }
    
    public bool justPressedRightShoulder()
    {
        return state.Buttons.RightShoulder == ButtonState.Pressed && prevState.Buttons.RightShoulder == ButtonState.Released;
    }
    
    public bool releasedRightShoulder()
    {
        return state.Buttons.RightShoulder == ButtonState.Released;
    }
    
    public bool justReleasedRightShoulder()
    {
        return state.Buttons.RightShoulder == ButtonState.Released && prevState.Buttons.RightShoulder == ButtonState.Pressed;
    }
}
