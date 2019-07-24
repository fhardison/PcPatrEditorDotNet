using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using GuiLabs.Canvas.DrawStyle;
using GuiLabs.Canvas.Renderer;
using GuiLabs.Editor.Blocks;
using GuiLabs.Editor.Lists;
using GuiLabs.Utils;

namespace PCPATR.Blocks
{
  public class PcpatrRulesBlock : UniversalBlock
  {
    public PcpatrRulesBlock()
    {
      HMembers.Add(new PcpatrComment("Grammar rules"));
      VMembers.Add(new EmptyRuleBlock());
    }
  }

  public class EmptyRuleBlock : EmptyBlock
  {
    public EmptyRuleBlock()
    {
      Completion.AddReplaceBlocksItem<PcpatrRuleBlock, EmptyRuleBlock>("rule");
      Completion.AddReplaceBlocksItem<PcpatrComment, EmptyRuleBlock>("comment");
    }
  }

  public class PcpatrRuleBlock : UniversalBlock
  {
    public PcpatrRuleBlock()
    {
      HMembers.Add(MyUtils.CreateKeyword("rule"), MyUtils.CreateKeyword("{"), _identifier, MyUtils.CreateKeyword("}"));
      Psr = new PcpatrPsrBlock();
      VMembers.Add(Psr);
    }
    public PcpatrPsrBlock Psr { get; set; }
    private TextBoxBlock _identifier = new TextBoxBlock(25);
  }

  public class PcpatrPsrBlock : HContainerBlock
  {
    public PcpatrPsrBlock()
    {
      Add(_lhs, new LabelBlock("->"), new PcpatrPsrEmptyBlock());

    }

    public PcpatrPsrSymbolBlock _lhs = new PcpatrPsrSymbolBlock();
  }

  public interface IPsrContainerElement
  {
    IEnumerable<PcpatrPsrSymbolBlock> GetChildSymbols();
    IChildrenList Children { get; set; }

    ReadOnlyCollection<T> FindChildrenRecursive<T>() where T : class;
    
    bool TryToRemoveMe();
  }

  public class PcpatrPsrEmptyBlock : EmptyBlock
  {
    public PcpatrPsrEmptyBlock()
    {
      MyTextBox.MinWidth = 18;
      MyControl.Style = new ShapeStyle(){
        FillColor = Color.LightGray
      };
      
      FillCompletion();
      
    }
    
    protected void FillCompletion()
    {
      Completion.AddCreateBlocksItem<PcpatrPsrSymbolBlock, PcpatrPsrEmptyBlock>("symbol");

      var disjunction = Completion.AddCreateBlocksItem<PcpatrPsrDisjunctionBlock, PcpatrPsrEmptyBlock>("disjunction");
      disjunction.VisibilityConditions.Add(new DelegateCondition(() => !(Parent is PcpatrPsrDisjunctionBlock || Parent is PcpatrPsrDisjunctBlock)));

      var disjunct = Completion.AddCreateBlocksItem<PcpatrPsrDisjunctBlock, PcpatrPsrEmptyBlock>("disjunct");
      disjunct.VisibilityConditions.Add(new DelegateCondition(() => this.Parent.Prev is PcpatrPsrDisjunctBlock));

      Completion.AddCreateBlocksItem<PcpatrPsrOptionalSymbolsBlock, PcpatrPsrEmptyBlock>("optional symbols");
    }

    /// <summary>
    /// Handles
    /// </summary>
    protected override void OnKeyDown(object sender, KeyEventArgs e)
    {
      var parent = this.Parent;
      if (e.KeyCode == Keys.Delete)
      {
        if (!TryRemoveParent(this.Parent)){
          base.OnKeyDown(sender, e);
        }
      }
      else
      {
        base.OnKeyDown(sender, e);
      }
      
      
      
    }
    
    /// <summary>
    /// Tries to delete the parent of this block. Returns true if the delete key was handled correctly.
    /// </summary>
    /// <returns></returns>
    protected virtual bool TryRemoveParent(ContainerBlock parent)
    {
      bool handledCorrectly = false;
      
      if (parent != null)
      {
        if (this.Parent.GetType() == typeof(PcpatrPsrBlock))
        {
          handledCorrectly = true;
        }
        else
        {
          IPsrContainerElement container  = parent as IPsrContainerElement;
          if (container != null)
          {
            container.TryToRemoveMe();
          } else
          {
            var grandParent = parent.Parent;
            if (grandParent != null)
            {
              grandParent.Children.Delete(parent);
              handledCorrectly = true;
            }
          }
        }
      }
      
      return handledCorrectly;
    }
  }

  public class PsrContainerElement : HContainerBlock
  {
    
    protected override void OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete){
        HandleDeleteKey();
      } else {
        base.OnKeyDown(sender, e);
      }
    }
    
    protected virtual void HandleDeleteKey()
    {
      var parent = this.Parent;
      var next = this.Next;
      
      if (parent == null) return;
      if (next != null )
      {
        if (next.GetType() == typeof(PcpatrPsrEmptyBlock))
        {
          parent.Children.Delete(next);
          
        }
      }
    }
    
  }


  public class PcpatrPsrDisjunctionBlock : PsrContainerElement, IPsrContainerElement
  {

    public PcpatrPsrDisjunctionBlock()
    {
      InitDisjuncts();
      Add(new LabelBlock("{"), FirstDisjunct, SecondDisjunct, new LabelBlock("}"));
    }


    public IEnumerable<PcpatrPsrSymbolBlock> GetChildSymbols()
    {
      return MyUtils.GetSymbolsFromContainer(this);
    }

    protected void InitDisjuncts()
    {
      FirstDisjunct = new PcpatrPsrDisjunctBlock(false);
      SecondDisjunct = new PcpatrPsrDisjunctBlock(true);
    }

    public PcpatrPsrDisjunctBlock FirstDisjunct { get; protected set; }
    public PcpatrPsrDisjunctBlock SecondDisjunct { get; protected set; }
    
    public bool TryToRemoveMe()
    {
      var parent = this.Parent;
      if (parent != null)
      {
        parent.Children.Delete(this);
        
        return true;
      }
      
      return false;
    }
    
  }

  public class PcpatrPsrDisjunctBlock : PsrContainerElement, IPsrContainerElement
  {
    public PcpatrPsrDisjunctBlock(bool showDisjuctMarker)
    {
      if (showDisjuctMarker)
      {
        Add(new LabelBlock("\\"));
      }
      Add(_empty);
      
    }

    public PcpatrPsrDisjunctBlock() : this(true) { }


    public IEnumerable<PcpatrPsrSymbolBlock> GetChildSymbols()
    {
      return MyUtils.GetSymbolsFromContainer(this);
    }

    public override void SetDefaultFocus()
    {
      _empty.MyControl.SetFocus();
    }
    
    public bool TryToRemoveMe()
    {
      var parent = this.Parent;
      if (parent != null)
      {
        var parentAsDisjunct = parent as PcpatrPsrDisjunctionBlock;
        if (parentAsDisjunct != null)
        {
          if (parentAsDisjunct.SecondDisjunct != this)
          {
            parent.Children.Delete(this);
            
            return true;
          }
        }
      }
      
      return false;
    }
    
    private readonly PcpatrPsrEmptyBlock _empty = new PcpatrPsrEmptyBlock();
  }



  public class PcpatrPsrOptionalSymbolsBlock : PsrContainerElement, IPsrContainerElement
  {
    public PcpatrPsrOptionalSymbolsBlock()
    {
    }

    public IEnumerable<PcpatrPsrSymbolBlock> GetChildSymbols()
    {
      return MyUtils.GetSymbolsFromContainer(this);
    }
    
    public bool TryToRemoveMe()
    {
      var parent = this.Parent;
      
      if (parent == null) return false;
      
      parent.Children.Delete(this);
      
      return true;
    }
  }


  public class PcpatrPsrSymbolBlock : TextBoxBlockWithCompletion
  {
    public PcpatrPsrSymbolBlock()
    {
      MyControl.Box.Margins.SetLeftAndRight(4);
      MyTextBox.MinWidth = 18;
    }
    
    protected override void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      var key = e.KeyCode;
      if (key ==  Keys.F11)
      {
      }
      else if (key == Keys.Delete)
      {
        HandleDeleteKey();
      }
      else
      {
        base.OnKeyDown(sender, e);
      }
    }
    
    protected virtual void HandleDeleteKey()
    {
      var parent = this.Parent;
      var next = this.Next;
      
      if (parent == null) return;
      if (next != null )
      {
        if (next.GetType() == typeof(PcpatrPsrEmptyBlock))
        {
          parent.Children.Delete(next);
          
        }
      }
      
      parent.Children.Delete(this);
      
      
    }
  }
}