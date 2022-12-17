// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

// All portion of this code file, excluding methods ending with "Silent",
// are licensed under the MIT (Expat) licence.

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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Represents a customised <see cref="NativeListItem{T}"/> suitable for trainer operations.
/// </summary>
public class LozengeListItem<T> : NativeListItem, IEnumerable<T>
{
    #region Fields

    private int _index = 0;
    private List<T> _items;

    #endregion

    #region Properties

    /// <summary>
    /// The index of the currently selected index.
    /// </summary>
    public int SelectedIndex
    {
        get
        {
            if (Items.Count == 0)
            {
                return -1;
            }
            return _index;
        }
        set
        {
            if (Items.Count == 0)
            {
                throw new InvalidOperationException("There are no available items.");
            }
            if (value < 0)
            {
                throw new InvalidOperationException("The index is under zero.");
            }
            if (value >= Items.Count)
            {
                throw new InvalidOperationException($"The index is over the limit of {Items.Count - 1}");
            }
            if (_index == value)
            {
                return;
            }
            _index = value;
            TriggerEvent(value, Direction.Unknown);
            UpdateIndex();
        }
    }
    /// <summary>
    /// The currently selected item.
    /// </summary>
    public T SelectedItem
    {
        get
        {
            if (Items.Count == 0)
            {
                return default;
            }

            return Items[SelectedIndex];
        }
        set
        {
            if (Items.Count == 0)
            {
                throw new InvalidOperationException("There are no available items.");
            }

            int newIndex = Items.IndexOf(value);

            if (newIndex == -1)
            {
                throw new InvalidOperationException("The object is not the list of Items.");
            }

            SelectedIndex = newIndex;
        }
    }
    /// <summary>
    /// The objects used by this item.
    /// </summary>
    public List<T> Items
    {
        get => _items;
        set
        {
            _items = value;
            UpdateIndex();
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Event triggered when the selected item is changed.
    /// </summary>
    public event EventHandler<LozengeItemEventArgs<T>> ItemChanged;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new <see cref="NativeListItem"/>.
    /// </summary>
    /// <param name="title">The title of the Item.</param>
    /// <param name="objs">The objects that are available on the Item.</param>
    public LozengeListItem(string title, params T[] objs) : this(title, string.Empty, objs)
    {
    }
    /// <summary>
    /// Creates a new <see cref="NativeListItem"/>.
    /// </summary>
    /// <param name="title">The title of the Item.</param>
    /// <param name="subtitle">The subtitle of the Item.</param>
    /// <param name="objs">The objects that are available on the Item.</param>
    public LozengeListItem(string title, string subtitle, params T[] objs) : base(title, subtitle)
    {
        _items = new List<T>();
        _items.AddRange(objs);
        UpdateIndex();
    }

    #endregion

    #region Tools

    /// <summary>
    /// Triggers the <seealso cref="ItemChangedEventHandler{T}"/> event.
    /// </summary>
    private void TriggerEvent(int index, Direction direction)
    {
        ItemChanged?.Invoke(this, new LozengeItemEventArgs<T>(_items[index], index, direction));
    }
    private void FixIndexIfRequired()
    {
        if (_index >= _items.Count)
        {
            _index = _items.Count - 1;
            UpdateIndex();
        }
    }
    /// <summary>
    /// Updates the currently selected item based on the index.
    /// </summary>
    private void UpdateIndex()
    {
        text.Text = SelectedIndex != -1 ? SelectedItem.ToString() : string.Empty;

        text.Position = new PointF(RightArrow.Position.X - text.Width + 3, text.Position.Y);
        LeftArrow.Position = new PointF(text.Position.X - LeftArrow.Size.Width, LeftArrow.Position.Y);
    }

    #endregion

    #region Functions

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    /// <summary>
    /// Adds a <typeparamref name="T" /> into this item.
    /// </summary>
    /// <param name="item">The <typeparamref name="T" /> to add.</param>
    public void Add(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (_items.Contains(item))
        {
            throw new InvalidOperationException("Item is already part of this NativeListItem.");
        }

        _items.Add(item);

        if (_items.Count == 1)
        {
            UpdateIndex();
        }
    }
    /// <summary>
    /// Adds a <typeparamref name="T" /> in a specific location.
    /// </summary>
    /// <param name="position">The position where the item should be added.</param>
    /// <param name="item">The <typeparamref name="T" /> to add.</param>
    public void Add(int position, T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (position < 0 || position > Items.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "The position is out of the range of items.");
        }

        Items.Insert(position, item);

        FixIndexIfRequired();
    }

    /// <summary>
    /// Removes a specific <typeparamref name="T" />.
    /// </summary>
    /// <param name="item">The <typeparamref name="T" /> to remove.</param>
    public void Remove(T item)
    {
        if (_items.Remove(item))
        {
            FixIndexIfRequired();
        }
    }

    /// <summary>
    /// Removes all of the items that match the <paramref name="pred"/>.
    /// </summary>
    /// <param name="pred">The function to use as a check.</param>
    public void Remove(Func<T, bool> pred)
    {
        if (_items.RemoveAll(pred.Invoke) > 0)
        {
            FixIndexIfRequired();
        }
    }

    /// <summary>
    /// Removes a <typeparamref name="T" /> at a specific location.
    /// </summary>
    /// <param name="position">The position of the <typeparamref name="T" />.</param>
    public void RemoveAt(int position)
    {
        if (position >= _items.Count)
        {
            return;
        }

        _items.RemoveAt(position);
        FixIndexIfRequired();
    }
    /// <summary>
    /// Removes all of the <typeparamref name="T" /> from this item.
    /// </summary>
    public void Clear()
    {
        _items.Clear();

        UpdateIndex();
    }

    /// <summary>
    /// Sets the value of <see cref="SelectedIndex"/> without triggering <see cref="ItemChanged"/> event.
    /// </summary>
    /// <param name="value">The index to select.</param>
    /// <exception cref="InvalidOperationException">The index is over the limit of items.</exception>
    public void SelectIndexSilent(int value)
    {
        if (Items.Count == 0)
        {
            throw new InvalidOperationException("There are no available items.");
        }
        if (value < 0)
        {
            throw new InvalidOperationException("The index is under zero.");
        }
        if (value >= Items.Count)
        {
            throw new InvalidOperationException($"The index is over the limit of {Items.Count - 1}");
        }
        if (_index == value)
        {
            return;
        }
        _index = value;
        UpdateIndex();
    }

    /// <summary>
    /// Sets the value of <see cref="SelectedItem"/> without triggering <see cref="ItemChanged"/> event.
    /// </summary>
    /// <param name="value">The item to select.</param>
    /// <exception cref="InvalidOperationException">The object is not in the list of Items.</exception>
    public void SelectItemSilent(T value)
    {
        if (Items.Count == 0)
        {
            throw new InvalidOperationException("There are no available items.");
        }

        int newIndex = Items.IndexOf(value);

        if (newIndex == -1)
        {
            throw new InvalidOperationException("The object is not in the list of Items.");
        }

        SelectIndexSilent(newIndex);
    }

    /// <summary>
    /// Recalculates the item positions and sizes with the specified values.
    /// </summary>
    /// <param name="pos">The position of the item.</param>
    /// <param name="size">The size of the item.</param>
    /// <param name="selected">If this item has been selected.</param>
    public override void Recalculate(PointF pos, SizeF size, bool selected)
    {
        base.Recalculate(pos, size, selected);

        text.Position = new PointF(pos.X + size.Width - RightArrow.Size.Width - 1 - text.Width, pos.Y + 3);
        LeftArrow.Position = new PointF(text.Position.X - LeftArrow.Size.Width, pos.Y + 4);
    }

    /// <summary>
    /// Moves to the previous item.
    /// </summary>
    public override void GoLeft()
    {
        if (Items.Count == 0)
        {
            return;
        }

        if (_index == 0)
        {
            _index = Items.Count - 1;
        }
        else
        {
            _index--;
        }

        TriggerEvent(_index, Direction.Left);
        UpdateIndex();
    }
    /// <summary>
    /// Moves to the next item.
    /// </summary>
    public override void GoRight()
    {
        if (Items.Count == 0)
        {
            return;
        }

        if (_index == Items.Count - 1)
        {
            _index = 0;
        }
        else
        {
            _index++;
        }

        TriggerEvent(_index, Direction.Right);
        UpdateIndex();
    }
    /// <summary>
    /// Draws the List on the screen.
    /// </summary>
    public override void Draw()
    {
        base.Draw(); // Arrows, Title and Left Badge
        text.Draw();
    }
    /// <inheritdoc/>
    public override void UpdateColors()
    {
        base.UpdateColors();

        if (!Enabled)
        {
            text.Color = Colors.TitleDisabled;
        }
        else if (lastSelected)
        {
            text.Color = Colors.TitleHovered;
        }
        else
        {
            text.Color = Colors.TitleNormal;
        }
    }

    #endregion
}
