using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace Utility
{
    public static class FormHelper
    {
        public static void SetFormStartPositionAtCentral(Form childForm,Form parentForm)
        {
            if (childForm == null || parentForm == null)
                return;

            System.Drawing.Point point = parentForm.Location;
            if(parentForm.Size.Width > childForm.Size.Width)
                point.X += (parentForm.Size.Width - childForm.Size.Width) / 2;
            
            if(parentForm.Size.Height > childForm.Size.Height)
                point.Y += (parentForm.Size.Height - childForm.Size.Height) / 2;

            childForm.StartPosition = FormStartPosition.Manual;
            childForm.Location = point;
        }
    }
}
