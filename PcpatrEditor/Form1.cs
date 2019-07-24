using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuiLabs.Editor.UI;
using PCPATR.Blocks;

namespace PcpatrEditor
{
  public partial class PcpatrEditor : Form
  {
    public PcpatrEditor()
    {
      InitializeComponent();
      Grammar = new PcpatrGrammar();
      _viewWindow.RootBlock = Grammar;
      Controls.Add(_viewWindow);
    }

    public PcpatrGrammar Grammar { get; set; }

    private ViewWindow _viewWindow = new ViewWindow{Dock = DockStyle.Fill};

  }
}
