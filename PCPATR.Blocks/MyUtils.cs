using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLabs.Editor.Blocks;

namespace PCPATR.Blocks
{
  internal static class MyUtils
  {
    internal static LabelBlock CreateKeyword(string name)
    {
      var labelBlock = new LabelBlock(name);
      labelBlock.MyControl.Box.Margins.Right = 4;

      return labelBlock;
    }

    internal static IEnumerable<PcpatrPsrSymbolBlock> GetSymbolsFromContainer(IPsrContainerElement container)
    {
      var list = new List<PcpatrPsrSymbolBlock>();

      foreach (var block in container.Children)
      {
        if (block is IPsrContainerElement)
        {
          list.AddRange((block as IPsrContainerElement).GetChildSymbols());
        } else if (block is PcpatrPsrSymbolBlock)
        {
          list.Add(block as PcpatrPsrSymbolBlock);
        }
      }

      return list;
    }
    
    
  }
}
