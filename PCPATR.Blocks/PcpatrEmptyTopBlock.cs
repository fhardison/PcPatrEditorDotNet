using System.Linq;
using GuiLabs.Editor.Blocks;
using GuiLabs.Utils;

namespace PCPATR.Blocks
{
  public class PcpatrEmptyTopBlock : EmptyBlock
  {
    public PcpatrEmptyTopBlock()
    {
      FillItems();
      CanBeDeletedByUser = false;
    }

    protected virtual void FillItems()
    {
      AddUniqueItem<PcpatrFeatureSystemBlock>("features system");
      AddUniqueItem<PcpatrSymbolsBlock>("symbols");
      AddUniqueItem<PcpatrRulesBlock>("rules");


    }
    protected virtual void AddUniqueItem<T>(string name)
    {
      var  item = Completion.AddReplaceBlocksItem<T, PcpatrEmptyTopBlock>(name);
      item.VisibilityConditions.Add(new DelegateCondition(
                                      () => this.Parent == null || this.Parent.Children.FirstOrDefault(f => f.GetType() == typeof(T)) == null)
        );
    }
    protected virtual void AddItem<T>(string name)
    {
      Completion.AddCreateBlocksItem<T, PcpatrEmptyTopBlock>(name);
    }
  }
}