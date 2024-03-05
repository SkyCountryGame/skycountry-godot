using System;
using System.Collections.Generic;
using Godot;
using SkyCountry;


/**
* singleton. passes input events from game engine to listeners for game logic
*/
public partial class InputManager
{
	private Dictionary<ActionType, List<dynamic>> _ActionKeyMap; //associate each input action type with a set of hardware inputs

	private List<InputActionListener> _Listeners;

	private Dictionary<string, List<InputActionListener>> actionListenerMap; //which actionlisteners want to know about what actions
	
	private static InputManager _Instance;

	public InputManager()
	{
		_ActionKeyMap = new Dictionary<ActionType, List<dynamic>>();
		_ActionKeyMap.Add(ActionType.Forward, new List<dynamic>{Key.KEY_W, Key.KEY_UP });
/*        _ActionKeyMap.Add(ActionType.Back, new List<dynamic> { KeyCode.S, KeyCode.DownArrow, KeyCode.Keypad2 });
        _ActionKeyMap.Add(ActionType.Left, new List<dynamic> { KeyCode.A, KeyCode.LeftArrow, KeyCode.Keypad1 });
        _ActionKeyMap.Add(ActionType.Right, new List<dynamic> { KeyCode.D, KeyCode.RightArrow, KeyCode.Keypad3 });
        //_ActionKeyMap.Add(ActionType.Up, new List<dynamic> { KeyCode.Keypad8, KeyCode.R });
        //_ActionKeyMap.Add(ActionType.Down, new List<dynamic> { KeyCode.Keypad0, KeyCode.F });
        _ActionKeyMap.Add(ActionType.Clockwise, new List<dynamic> { KeyCode.Keypad6 });
        _ActionKeyMap.Add(ActionType.Counterclockwise, new List<dynamic> { KeyCode.Keypad4 });
        _ActionKeyMap.Add(ActionType.CamZoomOut, new List<dynamic> { scrollDown  });
        _ActionKeyMap.Add(ActionType.CamZoomIn, new List<dynamic> { scrollUp  });
        _ActionKeyMap.Add(ActionType.CamAdjust, new List<dynamic> { 2 }); //middle click to adjust camera angle

        _ActionKeyMap.Add(ActionType.PlayerEquip, new List<dynamic> { KeyCode.F });
        _ActionKeyMap.Add(ActionType.PlayerUse, new List<dynamic> { KeyCode.E,  });
        _ActionKeyMap.Add(ActionType.PlayerDequip, new List<dynamic> { KeyCode.C });
        _ActionKeyMap.Add(ActionType.PlayerHeal, new List<dynamic> { KeyCode.Q,  });
        _ActionKeyMap.Add(ActionType.PlayerOpenInv, new List<dynamic> { KeyCode.Tab,  });
        _ActionKeyMap.Add(ActionType.PlayerAction1, new List<dynamic> { 0,  });
        _ActionKeyMap.Add(ActionType.PlayerAction2, new List<dynamic> { 1,  });
        _ActionKeyMap.Add(ActionType.PlayerAction3, new List<dynamic> { 2 }); 
        _ActionKeyMap.Add(ActionType.PlayerDeflect, new List<dynamic> { KeyCode.Z });
        _ActionKeyMap.Add(ActionType.PlayerAlt, new List<dynamic> { KeyCode.LeftShift, });
        _ActionKeyMap.Add(ActionType.PlayerReload, new List<dynamic> { KeyCode.R,  });
        _ActionKeyMap.Add(ActionType.PlayerSpeedToggle, new List<dynamic> { KeyCode.LeftShift, KeyCode.RightShift});
        _ActionKeyMap.Add(ActionType.DebugToggle, new List<dynamic> { KeyCode.KeypadDivide, KeyCode.Tilde });
        _ActionKeyMap.Add(ActionType.DebugCycleSelectedKey, new List<dynamic> { KeyCode.Backslash });
        _ActionKeyMap.Add(ActionType.DebugDecreaseKfT, new List<dynamic> { KeyCode.LeftBracket });
        _ActionKeyMap.Add(ActionType.DebugIncreaseKfT, new List<dynamic> { KeyCode.RightBracket });
        _ActionKeyMap.Add(ActionType.DebugDecreaseKfV, new List<dynamic> { KeyCode.Semicolon });
        _ActionKeyMap.Add(ActionType.DebugIncreaseKfV, new List<dynamic> { KeyCode.Quote});

        _ActionKeyMap.Add(ActionType.F1, new List<dynamic> {  KeyCode.F1 });

        _ActionKeyMap.Add(ActionType.PauseMenu, new List<dynamic> {KeyCode.P, KeyCode.Escape });
        _ActionKeyMap.Add(ActionType.DebugMenu, new List<dynamic> { KeyCode.Slash });
*/
        _Listeners = new List<ActionListener>();

		GD.Print("InputManager created.");
	}
	
	public static InputManager getInstance()
    {
        if (_Instance == null)
        {
            _Instance = new InputManager();
        }
        return _Instance;
    }

	public void update(){

	}

	/**
	 * external class calls this and registers for input actions, passing a list of actions it cares about
	 */
	public void addActionListener(InputActionListener l, List<string> actionsOfConcern)
	{
		actionListeners.Add(l);
		foreach (string a in actionsOfConcern)
		{
			if (!actionListenerMap.ContainsKey(a))
			{
				actionListenerMap[a] = new List<InputActionListener>();
			}
			actionListenerMap[a].Add(l);
		}
		
	}
}


/*
 * possible user inputs and the option to include a payload
 * 
 * */
public class InputActionP {
    public enum ActionType{
        Left, Right, Forward, Back, Up, Down, Clockwise, Counterclockwise,
        CamZoomOut, CamZoomIn, CamAdjust,
        PlayerAlt, //racial ability: block, roll, etc.
        PlayerAction1, //probably left click
        PlayerAction2, //right
        PlayerAction3, //middle
        PlayerSpeedToggle,
        PlayerDeflect, //block & reflect bullet
        PlayerEquip,
        PlayerDequip,
        PlayerHeal,
        PlayerOpenInv,
        PlayerUse,
        PlayerReload,
        PlayerTargetEntity,
        PlayerSwitchTarget,
        Interact,
        LeftStick,//2d vector
        RightStick, //2d vector
        ArrowKeys, //2d vector, also includes awsd
        PauseMenu,
        DebugMenu,
        DebugToggle,
        DebugCycleSelectedKey,
        DebugDecreaseKfT,
        DebugIncreaseKfT,
        DebugDecreaseKfV,
        DebugIncreaseKfV,
        F1,
    };

    //some datastructs to use for various action payloads
    public Vector3 vec;
    public ActionType type;

    public InputActionP(ActionType type){
        this.type = type;
    }

}