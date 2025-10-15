#region header

// MouseJiggler - Helpers.cs
// 
// Created by: Alistair J R Young (avatar) at 2021/01/20 7:40 PM.
// Updates by: Dimitris Panokostas (midwan)

#endregion

#region using

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

// using Windows.Win32;
// using Windows.Win32.UI.Input.KeyboardAndMouse;

#endregion

namespace ArkaneSystems.MouseJiggler
{
  internal static class Helpers
  {
    #region Console management

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool AttachConsole(uint dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool FreeConsole();

    /// <summary>
    ///     Constant value signifying a request to attach to the console of the parent process.
    /// </summary>
    internal const uint AttachParentProcess = uint.MaxValue;

    public static void AttachConsole()
    {
      AttachConsole(AttachParentProcess);
    }

    #endregion Console management

    #region Jiggling

    [DllImport("User32.dll", SetLastError = true)]
    public static extern int SendInput(int nInputs, INPUT[] inputs, int cbSize);

    internal enum MOUSE_EVENT_FLAGS : uint
    {
      MOUSEEVENTF_ABSOLUTE = 0x00008000,
      MOUSEEVENTF_LEFTDOWN = 0x00000002,
      MOUSEEVENTF_LEFTUP = 0x00000004,
      MOUSEEVENTF_MIDDLEDOWN = 0x00000020,
      MOUSEEVENTF_MIDDLEUP = 0x00000040,
      MOUSEEVENTF_MOVE = 0x00000001,
      MOUSEEVENTF_RIGHTDOWN = 0x00000008,
      MOUSEEVENTF_RIGHTUP = 0x00000010,
      MOUSEEVENTF_WHEEL = 0x00000800,
      MOUSEEVENTF_XDOWN = 0x00000080,
      MOUSEEVENTF_XUP = 0x00000100,
      MOUSEEVENTF_HWHEEL = 0x00001000,
      MOUSEEVENTF_MOVE_NOCOALESCE = 0x00002000,
      MOUSEEVENTF_VIRTUALDESK = 0x00004000,
    }

    /// <summary>Contains information about a simulated mouse event.</summary>
    /// <remarks>
    /// <para>If the mouse has moved, indicated by **MOUSEEVENTF_MOVE**, **dx** and **dy** specify information about that movement. The information is specified as absolute or relative integer values. If **MOUSEEVENTF_ABSOLUTE** value is specified, **dx** and **dy** contain normalized absolute coordinates between 0 and 65,535. The event procedure maps these coordinates onto the display surface. Coordinate (0,0) maps onto the upper-left corner of the display surface; coordinate (65535,65535) maps onto the lower-right corner. In a multimonitor system, the coordinates map to the primary monitor. If **MOUSEEVENTF_VIRTUALDESK** is specified, the coordinates map to the entire virtual desktop. If the **MOUSEEVENTF_ABSOLUTE** value is not specified, **dx**and **dy** specify movement relative to the previous mouse event (the last reported position). Positive values mean the mouse moved right (or down); negative values mean the mouse moved left (or up). Relative mouse motion is subject to the effects of the mouse speed and the two-mouse threshold values. A user sets these three values with the **Pointer Speed** slider of the Control Panel's **Mouse Properties** sheet. You can obtain and set these values using the [SystemParametersInfo](/windows/desktop/api/winuser/nf-winuser-systemparametersinfoa) function. The system applies two tests to the specified relative mouse movement. If the specified distance along either the x or y axis is greater than the first mouse threshold value, and the mouse speed is not zero, the system doubles the distance. If the specified distance along either the x or y axis is greater than the second mouse threshold value, and the mouse speed is equal to two, the system doubles the distance that resulted from applying the first threshold test. It is thus possible for the system to multiply specified relative mouse movement along the x or y axis by up to four times.</para>
    /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-mouseinput#">Read more on docs.microsoft.com</see>.</para>
    /// </remarks>
    internal partial struct MOUSEINPUT
    {
      /// <summary>
      /// <para>Type: **LONG** The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the **dwFlags** member. Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels moved.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-mouseinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal int dx;

      /// <summary>
      /// <para>Type: **LONG** The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the **dwFlags** member. Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels moved.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-mouseinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal int dy;

      /// <summary>
      /// <para>Type: **DWORD** If **dwFlags** contains **MOUSEEVENTF_WHEEL**, then **mouseData** specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as **WHEEL_DELTA**, which is 120. **Windows Vista**: If *dwFlags* contains **MOUSEEVENTF_HWHEEL**, then *dwData* specifies the amount of wheel movement. A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left. One wheel click is defined as **WHEEL_DELTA**, which is 120. If **dwFlags** does not contain **MOUSEEVENTF_WHEEL**, **MOUSEEVENTF_XDOWN**, or **MOUSEEVENTF_XUP**, then **mouseData** should be zero. If **dwFlags** contains **MOUSEEVENTF_XDOWN** or **MOUSEEVENTF_XUP**, then **mouseData** specifies which X buttons were pressed or released. This value may be any combination of the following flags. | Value | Meaning | |-|-| | **XBUTTON1**<br>0x0001 | Set if the first X button is pressed or released. | | **XBUTTON2**<br>0x0002 | Set if the second X button is pressed or released. |</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-mouseinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal uint mouseData;

      /// <summary>Type: **DWORD**</summary>
      internal MOUSE_EVENT_FLAGS dwFlags;

      /// <summary>
      /// <para>Type: **DWORD** The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-mouseinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal uint time;

      /// <summary>
      /// <para>Type: **ULONG_PTR** An additional value associated with the mouse event. An application calls [GetMessageExtraInfo](/windows/desktop/api/winuser/nf-winuser-getmessageextrainfo) to obtain this extra information.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-mouseinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal UIntPtr dwExtraInfo;
    }

    internal enum VIRTUAL_KEY : ushort
    {
      VK_0 = 48,
      VK_1 = 49,
      VK_2 = 50,
      VK_3 = 51,
      VK_4 = 52,
      VK_5 = 53,
      VK_6 = 54,
      VK_7 = 55,
      VK_8 = 56,
      VK_9 = 57,
      VK_A = 65,
      VK_B = 66,
      VK_C = 67,
      VK_D = 68,
      VK_E = 69,
      VK_F = 70,
      VK_G = 71,
      VK_H = 72,
      VK_I = 73,
      VK_J = 74,
      VK_K = 75,
      VK_L = 76,
      VK_M = 77,
      VK_N = 78,
      VK_O = 79,
      VK_P = 80,
      VK_Q = 81,
      VK_R = 82,
      VK_S = 83,
      VK_T = 84,
      VK_U = 85,
      VK_V = 86,
      VK_W = 87,
      VK_X = 88,
      VK_Y = 89,
      VK_Z = 90,
      VK_ABNT_C1 = 193,
      VK_ABNT_C2 = 194,
      VK_DBE_ALPHANUMERIC = 240,
      VK_DBE_CODEINPUT = 250,
      VK_DBE_DBCSCHAR = 244,
      VK_DBE_DETERMINESTRING = 252,
      VK_DBE_ENTERDLGCONVERSIONMODE = 253,
      VK_DBE_ENTERIMECONFIGMODE = 248,
      VK_DBE_ENTERWORDREGISTERMODE = 247,
      VK_DBE_FLUSHSTRING = 249,
      VK_DBE_HIRAGANA = 242,
      VK_DBE_KATAKANA = 241,
      VK_DBE_NOCODEINPUT = 251,
      VK_DBE_NOROMAN = 246,
      VK_DBE_ROMAN = 245,
      VK_DBE_SBCSCHAR = 243,
      VK__none_ = 255,
      VK_LBUTTON = 1,
      VK_RBUTTON = 2,
      VK_CANCEL = 3,
      VK_MBUTTON = 4,
      VK_XBUTTON1 = 5,
      VK_XBUTTON2 = 6,
      VK_BACK = 8,
      VK_TAB = 9,
      VK_CLEAR = 12,
      VK_RETURN = 13,
      VK_SHIFT = 16,
      VK_CONTROL = 17,
      VK_MENU = 18,
      VK_PAUSE = 19,
      VK_CAPITAL = 20,
      VK_KANA = 21,
      VK_HANGEUL = 21,
      VK_HANGUL = 21,
      VK_IME_ON = 22,
      VK_JUNJA = 23,
      VK_FINAL = 24,
      VK_HANJA = 25,
      VK_KANJI = 25,
      VK_IME_OFF = 26,
      VK_ESCAPE = 27,
      VK_CONVERT = 28,
      VK_NONCONVERT = 29,
      VK_ACCEPT = 30,
      VK_MODECHANGE = 31,
      VK_SPACE = 32,
      VK_PRIOR = 33,
      VK_NEXT = 34,
      VK_END = 35,
      VK_HOME = 36,
      VK_LEFT = 37,
      VK_UP = 38,
      VK_RIGHT = 39,
      VK_DOWN = 40,
      VK_SELECT = 41,
      VK_PRINT = 42,
      VK_EXECUTE = 43,
      VK_SNAPSHOT = 44,
      VK_INSERT = 45,
      VK_DELETE = 46,
      VK_HELP = 47,
      VK_LWIN = 91,
      VK_RWIN = 92,
      VK_APPS = 93,
      VK_SLEEP = 95,
      VK_NUMPAD0 = 96,
      VK_NUMPAD1 = 97,
      VK_NUMPAD2 = 98,
      VK_NUMPAD3 = 99,
      VK_NUMPAD4 = 100,
      VK_NUMPAD5 = 101,
      VK_NUMPAD6 = 102,
      VK_NUMPAD7 = 103,
      VK_NUMPAD8 = 104,
      VK_NUMPAD9 = 105,
      VK_MULTIPLY = 106,
      VK_ADD = 107,
      VK_SEPARATOR = 108,
      VK_SUBTRACT = 109,
      VK_DECIMAL = 110,
      VK_DIVIDE = 111,
      VK_F1 = 112,
      VK_F2 = 113,
      VK_F3 = 114,
      VK_F4 = 115,
      VK_F5 = 116,
      VK_F6 = 117,
      VK_F7 = 118,
      VK_F8 = 119,
      VK_F9 = 120,
      VK_F10 = 121,
      VK_F11 = 122,
      VK_F12 = 123,
      VK_F13 = 124,
      VK_F14 = 125,
      VK_F15 = 126,
      VK_F16 = 127,
      VK_F17 = 128,
      VK_F18 = 129,
      VK_F19 = 130,
      VK_F20 = 131,
      VK_F21 = 132,
      VK_F22 = 133,
      VK_F23 = 134,
      VK_F24 = 135,
      VK_NAVIGATION_VIEW = 136,
      VK_NAVIGATION_MENU = 137,
      VK_NAVIGATION_UP = 138,
      VK_NAVIGATION_DOWN = 139,
      VK_NAVIGATION_LEFT = 140,
      VK_NAVIGATION_RIGHT = 141,
      VK_NAVIGATION_ACCEPT = 142,
      VK_NAVIGATION_CANCEL = 143,
      VK_NUMLOCK = 144,
      VK_SCROLL = 145,
      VK_OEM_NEC_EQUAL = 146,
      VK_OEM_FJ_JISHO = 146,
      VK_OEM_FJ_MASSHOU = 147,
      VK_OEM_FJ_TOUROKU = 148,
      VK_OEM_FJ_LOYA = 149,
      VK_OEM_FJ_ROYA = 150,
      VK_LSHIFT = 160,
      VK_RSHIFT = 161,
      VK_LCONTROL = 162,
      VK_RCONTROL = 163,
      VK_LMENU = 164,
      VK_RMENU = 165,
      VK_BROWSER_BACK = 166,
      VK_BROWSER_FORWARD = 167,
      VK_BROWSER_REFRESH = 168,
      VK_BROWSER_STOP = 169,
      VK_BROWSER_SEARCH = 170,
      VK_BROWSER_FAVORITES = 171,
      VK_BROWSER_HOME = 172,
      VK_VOLUME_MUTE = 173,
      VK_VOLUME_DOWN = 174,
      VK_VOLUME_UP = 175,
      VK_MEDIA_NEXT_TRACK = 176,
      VK_MEDIA_PREV_TRACK = 177,
      VK_MEDIA_STOP = 178,
      VK_MEDIA_PLAY_PAUSE = 179,
      VK_LAUNCH_MAIL = 180,
      VK_LAUNCH_MEDIA_SELECT = 181,
      VK_LAUNCH_APP1 = 182,
      VK_LAUNCH_APP2 = 183,
      VK_OEM_1 = 186,
      VK_OEM_PLUS = 187,
      VK_OEM_COMMA = 188,
      VK_OEM_MINUS = 189,
      VK_OEM_PERIOD = 190,
      VK_OEM_2 = 191,
      VK_OEM_3 = 192,
      VK_GAMEPAD_A = 195,
      VK_GAMEPAD_B = 196,
      VK_GAMEPAD_X = 197,
      VK_GAMEPAD_Y = 198,
      VK_GAMEPAD_RIGHT_SHOULDER = 199,
      VK_GAMEPAD_LEFT_SHOULDER = 200,
      VK_GAMEPAD_LEFT_TRIGGER = 201,
      VK_GAMEPAD_RIGHT_TRIGGER = 202,
      VK_GAMEPAD_DPAD_UP = 203,
      VK_GAMEPAD_DPAD_DOWN = 204,
      VK_GAMEPAD_DPAD_LEFT = 205,
      VK_GAMEPAD_DPAD_RIGHT = 206,
      VK_GAMEPAD_MENU = 207,
      VK_GAMEPAD_VIEW = 208,
      VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON = 209,
      VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON = 210,
      VK_GAMEPAD_LEFT_THUMBSTICK_UP = 211,
      VK_GAMEPAD_LEFT_THUMBSTICK_DOWN = 212,
      VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT = 213,
      VK_GAMEPAD_LEFT_THUMBSTICK_LEFT = 214,
      VK_GAMEPAD_RIGHT_THUMBSTICK_UP = 215,
      VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN = 216,
      VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT = 217,
      VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT = 218,
      VK_OEM_4 = 219,
      VK_OEM_5 = 220,
      VK_OEM_6 = 221,
      VK_OEM_7 = 222,
      VK_OEM_8 = 223,
      VK_OEM_AX = 225,
      VK_OEM_102 = 226,
      VK_ICO_HELP = 227,
      VK_ICO_00 = 228,
      VK_PROCESSKEY = 229,
      VK_ICO_CLEAR = 230,
      VK_PACKET = 231,
      VK_OEM_RESET = 233,
      VK_OEM_JUMP = 234,
      VK_OEM_PA1 = 235,
      VK_OEM_PA2 = 236,
      VK_OEM_PA3 = 237,
      VK_OEM_WSCTRL = 238,
      VK_OEM_CUSEL = 239,
      VK_OEM_ATTN = 240,
      VK_OEM_FINISH = 241,
      VK_OEM_COPY = 242,
      VK_OEM_AUTO = 243,
      VK_OEM_ENLW = 244,
      VK_OEM_BACKTAB = 245,
      VK_ATTN = 246,
      VK_CRSEL = 247,
      VK_EXSEL = 248,
      VK_EREOF = 249,
      VK_PLAY = 250,
      VK_ZOOM = 251,
      VK_NONAME = 252,
      VK_PA1 = 253,
      VK_OEM_CLEAR = 254,
    }

    [Flags]
    internal enum KEYBD_EVENT_FLAGS : uint
    {
      KEYEVENTF_EXTENDEDKEY = 0x00000001,
      KEYEVENTF_KEYUP = 0x00000002,
      KEYEVENTF_SCANCODE = 0x00000008,
      KEYEVENTF_UNICODE = 0x00000004,
    }

    /// <summary>Contains information about a simulated keyboard event.</summary>
    /// <remarks>
    /// <para><b> INPUT_KEYBOARD</b> supports nonkeyboard-input methods—such as handwriting recognition or voice recognition—as if it were text input by using the <b>KEYEVENTF_UNICODE</b> flag. If <b>KEYEVENTF_UNICODE</b> is specified, <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-sendinput">SendInput</a> sends a <a href="https://docs.microsoft.com/windows/desktop/inputdev/wm-keydown">WM_KEYDOWN</a> or <a href="https://docs.microsoft.com/windows/desktop/inputdev/wm-keyup">WM_KEYUP</a> message to the foreground thread's message queue with <i>wParam</i> equal to <b>VK_PACKET</b>. Once <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-getmessage">GetMessage</a> or <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-peekmessagea">PeekMessage</a> obtains this message, passing the message to <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-translatemessage">TranslateMessage</a> posts a <a href="https://docs.microsoft.com/windows/desktop/inputdev/wm-char">WM_CHAR</a> message with the Unicode character originally specified by <b>wScan</b>. This Unicode character will automatically be converted to the appropriate ANSI value if it is posted to an ANSI window. Set the <b>KEYEVENTF_SCANCODE</b> flag to define keyboard input in terms of the scan code. This is useful for simulating a physical keystroke regardless of which keyboard is currently being used. You can also pass the <b>KEYEVENTF_EXTENDEDKEY</b> flag if the scan code is an extended key. The virtual key value of a key can change depending on the current keyboard layout or what other keys were pressed, but the scan code will always be the same.</para>
    /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-keybdinput#">Read more on docs.microsoft.com</see>.</para>
    /// </remarks>
    internal partial struct KEYBDINPUT
    {
      /// <summary>
      /// <para>Type: <b>WORD</b> A <a href="https://docs.microsoft.com/windows/desktop/inputdev/virtual-key-codes">virtual-key code</a>. The code must be a value in the range 1 to 254. If the <b>dwFlags</b> member specifies <b>KEYEVENTF_UNICODE</b>, <b>wVk</b> must be 0.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-keybdinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal VIRTUAL_KEY wVk;

      /// <summary>
      /// <para>Type: <b>WORD</b> A hardware scan code for the key. If <b>dwFlags</b> specifies <b>KEYEVENTF_UNICODE</b>, <b>wScan</b> specifies a Unicode character which is to be sent to the foreground application.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-keybdinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal ushort wScan;

      /// <summary>Type: <b>DWORD</b></summary>
      internal KEYBD_EVENT_FLAGS dwFlags;

      /// <summary>
      /// <para>Type: <b>DWORD</b> The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-keybdinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal uint time;

      /// <summary>
      /// <para>Type: <b>ULONG_PTR</b> An additional value associated with the keystroke. Use the <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-getmessageextrainfo">GetMessageExtraInfo</a> function to obtain this information.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-keybdinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal UIntPtr dwExtraInfo;
    }

    internal partial struct HARDWAREINPUT
    {
      /// <summary>
      /// <para>Type: <b>DWORD</b> The message generated by the input hardware.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-hardwareinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal uint uMsg;

      /// <summary>
      /// <para>Type: <b>WORD</b> The low-order word of the <i>lParam </i> parameter for <b>uMsg</b>.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-hardwareinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal ushort wParamL;

      /// <summary>
      /// <para>Type: <b>WORD</b> The high-order word of the <i>lParam </i> parameter for <b>uMsg</b>.</para>
      /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-hardwareinput#members">Read more on docs.microsoft.com</see>.</para>
      /// </summary>
      internal ushort wParamH;
    }

    internal enum INPUT_TYPE : uint
    {
      INPUT_MOUSE = 0U,
      INPUT_KEYBOARD = 1U,
      INPUT_HARDWARE = 2U,
    }

    internal partial struct INPUT
    {
      /// <summary>Type: <b>DWORD</b></summary>
      internal INPUT_TYPE type;

      internal _Anonymous_e__Union Anonymous;

      [StructLayout(LayoutKind.Explicit)]
      internal partial struct _Anonymous_e__Union
      {
        [FieldOffset(0)]
        internal MOUSEINPUT mi;

        [FieldOffset(0)]
        internal KEYBDINPUT ki;

        [FieldOffset(0)]
        internal HARDWAREINPUT hi;
      }
    }


    /// <summary>
    ///     Jiggle the mouse; i.e., fake a mouse movement event.
    /// </summary>
    /// <param name="delta">The mouse will be moved by delta pixels along both X and Y.</param>
    internal static void Jiggle(int delta)
    {
      INPUT[] inp = new[] { new INPUT
      {
        type = INPUT_TYPE.INPUT_MOUSE,
        Anonymous = new INPUT._Anonymous_e__Union
        {
          mi = new MOUSEINPUT
          {
            dx = delta,
            dy = delta,
            mouseData = 0,
            dwFlags = MOUSE_EVENT_FLAGS.MOUSEEVENTF_MOVE,
            time = 0,
            dwExtraInfo = (UIntPtr)0
          }
        }
      }};

      var returnValue = SendInput(1, /*new ReadOnlySpan<INPUT>(*/inp/*)*/, Marshal.SizeOf<INPUT>());

      if (returnValue == 1) return;
      var errorCode = Marshal.GetLastWin32Error();

      Debugger.Log(1,
          "Jiggle",
          $"failed to insert event to input stream; retval={returnValue}, errcode=0x{errorCode:x8}\n");
    }

    #endregion Jiggling
  }
}