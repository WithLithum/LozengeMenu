// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

// All portion of this code file are licensed under the MIT (Expat) licence.

/**
 * Copyright (c) 2020-2021 Hannele Ruiz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

 */

namespace LozengeMenu.Core.UI;

using LemonUI.Menus;

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
