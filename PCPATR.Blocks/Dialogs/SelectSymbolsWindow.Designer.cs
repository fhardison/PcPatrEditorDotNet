﻿/*
 * Created by SharpDevelop.
 * User: TFHardison
 * Date: 5/30/2013
 * Time: 3:35 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace PCPATR.Blocks.Dialogs
{
  partial class SelectSymbolsWindow
  {
    /// <summary>
    /// Designer variable used to keep track of non-visual components.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    
    /// <summary>
    /// Disposes resources used by the form.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing) {
        if (components != null) {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    
    /// <summary>
    /// This method is required for Windows Forms designer support.
    /// Do not change the method contents inside the source code editor. The Forms designer might
    /// not be able to load this method if it was changed manually.
    /// </summary>
    private void InitializeComponent()
    {
      // 
      // SelectSymbolsWindow
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Text = "SelectSymbolsWindow";
      this.Name = "SelectSymbolsWindow";
    }
  }
}
