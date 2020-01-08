﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawTool
{
    public class ElementBase
    {
        private static IntPtr m_WinHandle;
        public static IntPtr WinHandle
        {
            get { return m_WinHandle; }
        }
        public static bool LoadWindow(System.Windows.Forms.Form window)
        {
            if (window == null)
                return false;
            m_WinHandle = window.Handle;
            return true;
        }
    }
}
