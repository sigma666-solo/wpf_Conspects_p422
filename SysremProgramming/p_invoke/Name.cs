using System;
using System.Runtime.InteropServices;

class Programm
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]

    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    static void Main()
    {
        MessageBox(IntPtr.Zero, "Гришин Сергей Витальевич", "ФИО", 0);    
    }
    
}