using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLabs.Canvas.Controls;
using GuiLabs.Editor.Blocks;
using GuiLabs.Editor.UI;


namespace PCPATR.Blocks
{
  public class PcpatrEmptyFeatureBlock : EmptyBlock
  {
    public PcpatrEmptyFeatureBlock()
    {
      FillItems();
      SetDefaultFocus();
      CanBeDeletedByUser = false;
    }

    protected virtual void FillItems()
    {
      AddItem<PcpatrFeature>("feature");
      AddItem<PcpatrComment>("comment");
    }

    protected virtual void AddItem<T>(string name)
    {
      Completion.AddReplaceBlocksItem<T, PcpatrEmptyFeatureBlock>(name);
    }

  }

  public class PcpatrFeatureSystemBlock : UniversalBlock
  {
    public PcpatrFeatureSystemBlock()
    {
      HMembers.Add(new PcpatrComment("Feature system definition"));
      VMembers.AddAcceptableBlockTypes(new Type[]{typeof(PcpatrEmptyFeatureBlock), typeof(PcpatrFeature), typeof(PcpatrComment)});
      _empty = new PcpatrEmptyFeatureBlock();
      VMembers.Add(_empty);
      
    }


    public override void SetDefaultFocus()
    {
      _empty.MyControl.SetFocus();
    }

    private PcpatrEmptyFeatureBlock _empty;
  }


  public class PcpatrFeature : UniversalBlock
  {
    public PcpatrFeature()
    {
      VMembers.AddAcceptableBlockTypes(new Type[]{typeof (PcpatrFeature),typeof (PcpatrEmptyFeatureBlock)});
      LabelBlock label = new LabelBlock("feature");
      label.MyControl.Box.Margins.Right = 4;
      HMembers.Add(label, _feaureNameBlock);
      VMembers.Add(new PcpatrEmptyFeatureBlock());
      SetDefaultFocus();
      //MyUniversalControl.CurlyType = UniversalControl.TypeOfCurlies.CSharp;
      _feaureNameBlock.TextChanged += FeaureNameBlockTextChanged;      
    }

    void FeaureNameBlockTextChanged(ITextProvider sender, string oldText, string newText)
    {
      var grammar = Root as PcpatrGrammar;
      if (grammar != null)
      {
        var features = grammar.GetFeatureNames();

        if (features.Count(f => f == this.FeatureName) > 1)
        {
          this.ActionManager.CurrentAction.UnExecute();              
        }
      }
    }

    public string FeatureName
    {
      get { return _feaureNameBlock.Text; } 
      set { _feaureNameBlock.Text = value; }
    }

    public override void SetDefaultFocus()
    {
      _feaureNameBlock.MyControl.SetFocus();
    }

    private readonly TextBoxBlock _feaureNameBlock = new TextBoxBlock(10);
  }
}
