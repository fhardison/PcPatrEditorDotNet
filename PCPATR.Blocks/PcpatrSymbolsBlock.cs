using System.Collections.Generic;
using System.Linq;
using GuiLabs.Editor.Blocks;

namespace PCPATR.Blocks
{
  public class PcpatrSymbolsBlock : UniversalBlock
  {
    public PcpatrSymbolsBlock()
    {
      NonTerminals = new PcpatrNonTerminalsSymbolsBlock();
      Terminals = new PcpatrTerminalsSymbolsBlock();
      HMembers.Add(new PcpatrComment("Symbol definitions"));
      VMembers.Add(NonTerminals, Terminals);

    }

    public PcpatrNonTerminalsSymbolsBlock NonTerminals { get; private set; }
    public PcpatrTerminalsSymbolsBlock  Terminals { get; private set; }
  }
  public class PcpatrSymbolsList : UniversalBlock
  {
    public IEnumerable<string> GetSymbols()
    {
      var symbols = new List<string>();

      var symbolBlocks = FindChildren<PcpatrSymbolDef>();

      foreach (var symbol in symbolBlocks)
      {
        symbols.Add(symbol.Text);
      }

      return symbols;
    }



  }


  public class PcpatrTerminalsSymbolsBlock : PcpatrSymbolsList
  {
    public PcpatrTerminalsSymbolsBlock()
    {
      HMembers.Add(new PcpatrComment("Terminal Symbols"));
      VMembers.AddAcceptableBlockTypes<PcpatrTerminalSymbolBlock, EmptyTerminalSymbolBlock>();
      _empty = new EmptyTerminalSymbolBlock();
      VMembers.Add(_empty);
    }

    public override void SetDefaultFocus()
    {
      _empty.MyControl.SetFocus();
    }

    public IEnumerable<string> GetTerminalNames()
    {
      var list = new List<string>();
      var terminals = VMembers.FindChildrenRecursive<PcpatrTerminalSymbolBlock>();
      
      foreach (var terminal in terminals)
      {
        list.Add(terminal.Text);
      }
      return list;
    }

    private readonly EmptyTerminalSymbolBlock _empty;
  }

  public class PcpatrNonTerminalsSymbolsBlock : PcpatrSymbolsList
  {
    public PcpatrNonTerminalsSymbolsBlock()
    {
      HMembers.Add(new PcpatrComment("Non Terminal Symbols"));
      VMembers.AddAcceptableBlockTypes<PcpatrNonTerminalSymbolblock, EmptyNonTerminalSymbolBlock>();
      _empty = new EmptyNonTerminalSymbolBlock();
      VMembers.Add(_empty);
    }

    public override void SetDefaultFocus()
    {
      _empty.MyControl.SetFocus();
    }

    public IEnumerable<string> GetNonTerminalNames()
    {
      var list = new List<string>();
     var nonTerminals  = VMembers.FindChildrenRecursive<PcpatrNonTerminalSymbolblock>();


      foreach (var nonTerminal in nonTerminals)
      {
        list.Add(nonTerminal.Text);
      }
      return list;
    }

    private readonly EmptyNonTerminalSymbolBlock _empty;
  }

  public class PcpatrSymbolDef : TextBoxBlock
  {
    public PcpatrSymbolDef()
    {
      TextChanged += PcpatrSymbolDef_TextChanged;
    }

    void PcpatrSymbolDef_TextChanged(GuiLabs.Canvas.Controls.ITextProvider sender, string oldText, string newText)
    {
      var grammar = Root as PcpatrGrammar;
      if (grammar == null) return;
      if (newText == string.Empty) return;
      var symbolNames = grammar.GetSymbols();
      if (symbolNames.Count(f => f == newText) > 1)
      {
        if (ActionManager != null && ActionManager.ActionIsExecuting)
        {
          ActionManager.CurrentAction.UnExecute();
        }
      }
    }
  }

  public class PcpatrTerminalSymbolBlock : PcpatrSymbolDef
  {

  }


  public class PcpatrNonTerminalSymbolblock : PcpatrSymbolDef
  {

  }


  public class EmptySymbolBlock<T> : EmptyBlock
  {
    public EmptySymbolBlock(string name)
    {
      CanBeDeletedByUser = false;
      //Completion.AddCreateBlocksItem<T,EmptySymbolBlock<T>>(name);
      Completion.AddReplaceBlocksItem<T, EmptySymbolBlock<T>>(name);
    }
  }

  public class EmptyNonTerminalSymbolBlock : EmptyBlock
  {
    public EmptyNonTerminalSymbolBlock()
    {
      CanBeDeletedByUser = false;
      Completion.AddReplaceBlocksItem<PcpatrNonTerminalSymbolblock, EmptyNonTerminalSymbolBlock>("non terminal");
    }
  }

  public class EmptyTerminalSymbolBlock : EmptyBlock
  {
    public EmptyTerminalSymbolBlock()
    {
      CanBeDeletedByUser = false;
      Completion.AddReplaceBlocksItem<PcpatrTerminalSymbolBlock, EmptyTerminalSymbolBlock>("terminal");
    }
  }
}