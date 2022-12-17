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
using LemonUI.Elements;

using LemonUI.Menus;
using System;
using System.Drawing;

/// <summary>
/// Represents a menu checkbox item that is suitable for trainer operations.
/// </summary>
public class LozengeCheckboxItem : NativeItem
{
    #region Fields

    /// <summary>
    /// The image shown on the checkbox.
    /// </summary>
    internal protected ScaledTexture check = new(PointF.Empty, SizeF.Empty, "commonmenu", string.Empty);
    /// <summary>
    /// If this item is checked or not.
    /// </summary>
    private bool _checked = false;

    #endregion

    #region Properties

    /// <summary>
    /// If this item is checked or not.
    /// </summary>
    public bool Checked
    {
        get => _checked;
        set
        {
            if (_checked == value)
            {
                return;
            }
            _checked = value;
            UpdateTexture(lastSelected);
            CheckboxChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Event triggered when the checkbox changes.
    /// </summary>
    public event EventHandler CheckboxChanged;

    #endregion

    #region Constructor

    /// <summary>
    /// Creates a new <see cref="LozengeCheckboxItem"/>.
    /// </summary>
    /// <param name="title">The title used for the Item.</param>
    public LozengeCheckboxItem(string title) : this(title, string.Empty, false)
    {
    }
    /// <summary>
    /// Creates a new <see cref="LozengeCheckboxItem"/>.
    /// </summary>
    /// <param name="title">The title used for the Item.</param>
    /// <param name="check">If the checkbox should be enabled or not.</param>
    public LozengeCheckboxItem(string title, bool check) : this(title, string.Empty, check)
    {
    }
    /// <summary>
    /// Creates a new <see cref="LozengeCheckboxItem"/>.
    /// </summary>
    /// <param name="title">The title used for the Item.</param>
    /// <param name="description">The description of the Item.</param>
    public LozengeCheckboxItem(string title, string description) : this(title, description, false)
    {
    }
    /// <summary>
    /// Creates a new <see cref="LozengeCheckboxItem"/>.
    /// </summary>
    /// <param name="title">The title used for the Item.</param>
    /// <param name="description">The description of the Item.</param>
    /// <param name="check">If the checkbox should be enabled or not.</param>
    public LozengeCheckboxItem(string title, string description, bool check) : base(title, description)
    {
        Checked = check;
        Activated += Toggle;
        EnabledChanged += NativeCheckboxItem_EnabledChanged;
    }

    #endregion

    #region Local Events

    private void NativeCheckboxItem_EnabledChanged(object sender, EventArgs e) => UpdateTexture(lastSelected);

    #endregion

    #region Internal Functions

    /// <summary>
    /// Inverts the checkbox activation.
    /// </summary>
    private void Toggle(object sender, EventArgs e) => Checked = !Checked;
    /// <summary>
    /// Updates the texture of the sprite.
    /// </summary>
    internal protected void UpdateTexture(bool selected)
    {
        // If the item is not selected or is not enabled, use the white pictures
        if (!selected || !Enabled)
        {
            check.Texture = Checked ? "shop_box_tick" : "shop_box_blank";
        }
        // Otherwise, use the black ones
        else
        {
            check.Texture = Checked ? "shop_box_tickb" : "shop_box_blankb";
        }
    }

    #endregion

    #region Public Functions

    /// <summary>
    /// Recalculates the item positions and sizes with the specified values.
    /// </summary>
    /// <param name="pos">The position of the item.</param>
    /// <param name="size">The size of the item.</param>
    /// <param name="selected">If this item has been selected.</param>
    public override void Recalculate(PointF pos, SizeF size, bool selected)
    {
        base.Recalculate(pos, size, selected);
        // Set the correct texture
        UpdateTexture(selected);
        // And set the checkbox positions
        check.Position = new PointF(pos.X + size.Width - 50, pos.Y - 6);
        check.Size = new SizeF(50, 50);
    }

    /// <summary>
    /// Draws the Checkbox on the screen.
    /// </summary>
    public override void Draw()
    {
        title.Draw();
        badgeLeft?.Draw();
        check.Draw();
    }
    /// <inheritdoc/>
    public override void UpdateColors()
    {
        base.UpdateColors();

        if (!Enabled)
        {
            check.Color = Colors.BadgeRightDisabled;
        }
        else if (lastSelected)
        {
            check.Color = Colors.BadgeRightHovered;
        }
        else
        {
            check.Color = Colors.BadgeRightNormal;
        }
    }

    /// <summary>
    /// Sets the value of <see cref="Checked"/> without triggering the <see cref="CheckboxChanged"/> event.
    /// </summary>
    /// <param name="value">The value to set to.</param>
    public void SetCheckedSilent(bool value)
    {
        if (_checked == value)
        {
            return;
        }

        _checked = value;
        UpdateTexture(lastSelected);
    }

    #endregion
}