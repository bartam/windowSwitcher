using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace UI
{
    class ShowWindow
    {
        // Define API functions.
        //[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        //static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPlacement(IntPtr hWnd,
           [In] ref WINDOWPLACEMENT lpwndpl);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public int Length;
            public int Flags;
            public ShowWindowCommands ShowCmd;
            public POINT MinPosition;
            public POINT MaxPosition;
            public RECT NormalPosition;
            public static WINDOWPLACEMENT Default
            {
                get {
                    WINDOWPLACEMENT result = new WINDOWPLACEMENT();
                    result.Length = Marshal.SizeOf(result);
                    return result;
                }
            }
        }

        internal enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maximize = 3, // is this the right value?
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            private int _Left;
            private int _Top;
            private int _Right;
            private int _Bottom;
        }

        // Set the target's placement.
        static public void setWindow(IntPtr handler, int status)
        {
            // Get the target window's handle.
            //IntPtr target_hwnd = FindWindowByCaption(IntPtr.Zero, txtAppTitle.Text);
            //if (target_hwnd == IntPtr.Zero) {
            //    MessageBox.Show("Could not find a window with the title \"" +
            //        txtAppTitle.Text + "\"");
            //    return;
            //}

            // Prepare the WINDOWPLACEMENT structure.
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.Length = Marshal.SizeOf(placement);

            // Get the window's current placement.
            GetWindowPlacement(handler, out placement);

            // Set the placement's action.
            //if (radMaximized.Checked)
            //    placement.ShowCmd = ShowWindowCommands.ShowMaximized;
            //else if (radMinimized.Checked)
            //    placement.ShowCmd = ShowWindowCommands.ShowMinimized;
            //else
            //    placement.ShowCmd = ShowWindowCommands.Normal;

            placement.ShowCmd = (ShowWindowCommands)status;

            // Perform the action.
            SetWindowPlacement(handler, ref placement);
        }

        static public void minimize(IntPtr handle)
        {
            setWindow(handle, 6);
        }

        static public void maximize(IntPtr handle)
        {
            setWindow(handle, 3);
        }

        static public void restore (IntPtr handle)
        {
            setWindow(handle, 9);
        }
    }
}
