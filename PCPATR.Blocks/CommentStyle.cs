using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLabs.Canvas.DrawStyle;

namespace PCPATR.Blocks
{
  public static class CommentFactory
  {
    public static ShapeStyle CreateCommentStyle()
    {
      var s = new ShapeStyle(Color.LightGreen );      
      
      return s;
    }
  }
}
