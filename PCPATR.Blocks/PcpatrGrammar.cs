using System;
using System.Collections.Generic;
using System.Linq;
using GuiLabs.Editor.Blocks;

namespace PCPATR.Blocks
{
  public class PcpatrGrammar : RootBlock
  {
    public PcpatrGrammar()
    {
      AddAcceptableBlockTypes(new Type[] { typeof(PcpatrFeatureSystemBlock), typeof(PcpatrSymbolsBlock), typeof(PcpatrRulesBlock) });
      AddSeparatorType<PcpatrEmptyTopBlock>();
      SetDefaultFocus();
      Clear();
    }

    public override void Clear()
    {
      base.Clear();
      Add(new PcpatrComment("pcpatr grammar"));
      Add(new PcpatrEmptyTopBlock());
    }

    /// <summary>
    /// Gets the ids from all elements that must have unique identifiers
    /// </summary>
    public IEnumerable<string> GetUniqueIds()
    {
      var symbolsTuple = GetNonTerminalAndNonTerminalSymbols();
      return GetFeatureNames().Concat(symbolsTuple.Item1).Concat(symbolsTuple.Item2);
    }

    /// <summary>
    /// Gets all the feature names in the feature system
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetFeatureNames()
    {
      var found = FindChildrenRecursive<PcpatrFeatureSystemBlock>();
      var featuresList = new List<string>();

      if (found.Count > 0)
      {
        var featureSystem = found[0];
        var features = featureSystem.FindChildrenRecursive<PcpatrFeature>();

        foreach (var feature in features)
        {
          featuresList.Add(feature.FeatureName);
        }
      }

      return featuresList;
    }

    /// <summary>
    /// Gets a tuple of all non terminal and terminal symbols in the grammar.
    /// </summary>
    /// <returns></returns>
    public Tuple<IEnumerable<string>, IEnumerable<string>> GetNonTerminalAndNonTerminalSymbols()
    {
      var found = FindChildrenRecursive<PcpatrSymbolsBlock>();
      var terminalList = new List<string>();
      var nonTermnialList = new List<string>();

      if (found.Count > 0)
      {
        var symbols = found[0];

        var terminalsBlock = symbols.Terminals;
        terminalList.AddRange(terminalsBlock.GetTerminalNames());

        var nonTerminalsBlock = symbols.NonTerminals;
        nonTermnialList.AddRange(nonTerminalsBlock.GetNonTerminalNames());
      }

      return Tuple.Create<IEnumerable<string>, IEnumerable<string>>(nonTermnialList, terminalList);
    }

    public IEnumerable<string> GetSymbols()
    {
      var symbolsTuple = GetNonTerminalAndNonTerminalSymbols();
      return symbolsTuple.Item1.Concat(symbolsTuple.Item2);
    }
  }

  public class PcpatrComment : HContainerBlock
  {
    public PcpatrComment()
      : this(string.Empty)
    {

    }

    public PcpatrComment(string comment)
    {
      _commentBlock = new TextBoxBlock(comment);
      var commentMarker = new LabelBlock("|");
      commentMarker.MyControl.Box.Margins.Right = 4;
      Add(commentMarker, _commentBlock);
      CanBeSelected = true;
      MyControl.Style = CommentFactory.CreateCommentStyle();
    }

    public override void SetDefaultFocus()
    {
      _commentBlock.MyControl.SetFocus();
    }

    private readonly TextBoxBlock _commentBlock;
  }
}