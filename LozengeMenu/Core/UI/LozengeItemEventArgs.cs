namespace LozengeMenu.Core.UI;

using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct LozengeItemEventArgs<T>
{
    /// <summary>
    /// The new object.
    /// </summary>
    public T Object { get; set; }
    /// <summary>
    /// The index of the object.
    /// </summary>
    public int Index { get; }
    /// <summary>
    /// The direction of the Item Changed event.
    /// </summary>
    public Direction Direction { get; }

    public LozengeItemEventArgs(T obj, int index, Direction direction)
    {
        Object = obj;
        Index = index;
        Direction = direction;
    }
}
