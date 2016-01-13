using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Utils.Controls.Ribbon
{
    [Designer(typeof(RibbonButtonDesigner))]
    public class RibbonButton : RibbonItem, IContainsRibbonComponents
    {
        #region Fields

        private const int arrowWidth = 5;
        private RibbonButtonStyle _style;
        private Rectangle _dropDownBounds;
        private Rectangle _buttonFaceBounds;
        private RibbonItemCollection _dropDownItems;
        private bool _dropDownPressed;
        private bool _dropDownSelected;
        private Image _smallImage; 
        private Size _dropDownArrowSize;
        private Padding _dropDownMargin;
        private RibbonDropDown _dropDown;
        private Point _lastMousePos;
        private RibbonArrowDirection _DropDownArrowDirection;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the dropdown is about to be displayed
        /// </summary>
        public event EventHandler DropDownShowing; 

        #endregion

        #region Ctors

        /// <summary>
        /// Creates a new button
        /// </summary>

        public RibbonButton()
        {
            _dropDownItems = new RibbonItemCollection();
            _dropDownArrowSize = new Size(5, 3);
            _dropDownMargin = new Padding(6);
            _DropDownArrowDirection = RibbonArrowDirection.Down;
            Image = CreateImage(32);
            SmallImage = CreateImage(16);
        }

        public RibbonButton(string text)
            : this()
        {
            Text = text;
        }

        public RibbonButton(Image smallImage)
        {
            SmallImage = smallImage;
        }

        public RibbonButton(string text, Image smallImage)
            : this()
        {
            Text = text;
            SmallImage = smallImage;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the DropDown of the button
        /// </summary>
        internal RibbonDropDown DropDown
        {
            get { return _dropDown; }
        }

        [DefaultValue(false)]
        [Description("Toggles the Checked property of the button when clicked")]
        public bool CheckOnClick { get; set; }


        /// <summary>
        /// Gets or sets a value indicating if the DropDown should be resizable
        /// </summary>
        [DefaultValue(false)]
        [Description("Makes the DropDown resizable with a grip on the corner")]
        public bool DropDownResizable { get; set; }

        /// <summary>
        /// Gets the bounds where the <see cref="Image"/> or <see cref="SmallImage"/> will be drawn.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ImageBounds { get; private set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle TextBounds { get; private set; }


        /// <summary>
        /// Gets if the DropDown is currently visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownVisible { get; private set; }

        /// <summary>
        /// Gets or sets the size of the dropdown arrow
        /// </summary>
        public Size DropDownArrowSize
        {
            get { return _dropDownArrowSize; }
            set { _dropDownArrowSize = value; NotifyOwnerRegionsChanged(); }
        }

        /// <summary>
        /// Gets or sets the direction where drop down's arrow should be pointing to
        /// </summary>
        public RibbonArrowDirection DropDownArrowDirection
        {
            get { return _DropDownArrowDirection; }
            set { _DropDownArrowDirection = value; NotifyOwnerRegionsChanged(); }
        }


        /// <summary>
        /// Gets the style of the button
        /// </summary>
        public RibbonButtonStyle Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;

                if (Canvas is RibbonPopup
                    || (OwnerItem != null && OwnerItem.Canvas is RibbonPopup))
                {
                    DropDownArrowDirection = RibbonArrowDirection.Left;
                }

                NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets the collection of items shown on the dropdown pop-up when Style allows it
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content)]
        public RibbonItemCollection DropDownItems
        {
            get
            {
                return _dropDownItems;
            }
        }

        /// <summary>
        /// Gets the bounds of the button face
        /// </summary>
        /// <remarks>When Style is different from SplitDropDown and SplitBottomDropDown, ButtonFaceBounds is the same area than DropDownBounds</remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Rectangle ButtonFaceBounds
        {
            get
            {
                return _buttonFaceBounds;
            }
        }

        /// <summary>
        /// Gets the bounds of the dropdown button
        /// </summary>
        /// <remarks>When Style is different from SplitDropDown and SplitBottomDropDown, ButtonFaceBounds is the same area than DropDownBounds</remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Rectangle DropDownBounds
        {
            get
            {
                return _dropDownBounds;
            }
        }

        /// <summary>
        /// Gets if the dropdown part of the button is selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownSelected
        {
            get
            {
                return _dropDownSelected;
            }
        }

        /// <summary>
        /// Gets if the dropdown part of the button is pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownPressed
        {
            get
            {
                return _dropDownPressed;
            }
        }

        /// <summary>
        /// Gets or sets the image of the button when in compact of medium size mode
        /// </summary>
        [DefaultValue(null)]
        public virtual Image SmallImage
        {
            get
            {
                return _smallImage;
            }
            set 
            {
                _smallImage = value;

                NotifyOwnerRegionsChanged();
            }
        } 

        #endregion

        #region Methods

        protected void SetDropDownMargin(Padding p)
        {
            _dropDownMargin = p;
        }

        /// <summary>
        /// Performs a click on the button
        /// </summary>
        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new Transparent, empty image
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private Image CreateImage(int size)
        {
            var bmp = new Bitmap(size, size);

            return bmp;
        }

        /// <summary>
        /// Creates the DropDown menu
        /// </summary>
        protected virtual void CreateDropDown()
        {
            _dropDown = new RibbonDropDown(this, DropDownItems, Owner);
        }

        internal override void SetOwner(Controls.Ribbon.Ribbon owner)
        {
            base.SetOwner(owner);

            if (_dropDownItems != null) _dropDownItems.SetOwner(owner);
        }

        internal override void SetOwnerPanel(RibbonPanel ownerPanel)
        {
            base.SetOwnerPanel(ownerPanel);

            if (_dropDownItems != null) _dropDownItems.SetOwnerPanel(ownerPanel);
        }

        internal override void SetOwnerTab(RibbonTab ownerTab)
        {
            base.SetOwnerTab(ownerTab);

            if (_dropDownItems != null) _dropDownItems.SetOwnerTab(ownerTab);
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (Owner == null) return;
            RibbonElementSizeMode theSize = GetNearestSize(e.Mode);

            Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(Owner, e.Graphics, e.Clip, this));

            Rectangle img = Rectangle.Empty;

            #region Image
            if (theSize == RibbonElementSizeMode.Large)
            {
                if (Image != null)
                {
                    img = new Rectangle(
                        Bounds.Left + ((Bounds.Width - Image.Width) / 2),
                        Bounds.Top + Owner.ItemMargin.Top,
                        Image.Width,
                        Image.Height);

                    Owner.Renderer.OnRenderRibbonItemImage(
                        new RibbonItemBoundsEventArgs(Owner, e.Graphics, e.Clip, this, img));
                }
            }
            else
            {


                if (SmallImage != null)
                {
                    img = new Rectangle(
                        Bounds.Left + Owner.ItemMargin.Left,
                        Bounds.Top + ((Bounds.Height - SmallImage.Height) / 2),
                        SmallImage.Width,
                        SmallImage.Height);

                    Owner.Renderer.OnRenderRibbonItemImage(
                        new RibbonItemBoundsEventArgs(Owner, e.Graphics, e.Clip, this, img));
                }
            }
            #endregion

            if (SizeMode != RibbonElementSizeMode.Compact)
                Owner.Renderer.OnRenderRibbonItemText(new RibbonItemBoundsEventArgs(Owner, e.Graphics, e.Clip, this, TextBounds));
        }

        /// <summary>
        /// Sets the bounds of the button
        /// </summary>
        /// <param name="bounds"></param>
        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            RibbonElementSizeMode sMode = GetNearestSize(SizeMode);

            ImageBounds = OnGetImageBounds(sMode, bounds);

            TextBounds = OnGetTextBounds(sMode, bounds);

            if (Style == RibbonButtonStyle.SplitDropDown)
            {
                _dropDownBounds = OnGetDropDownBounds(sMode, bounds);
                _buttonFaceBounds = OnGetButtonFaceBounds(sMode, bounds);
            }
        }

        /// <summary>
        /// Sets the bounds of the image of the button when SetBounds is called.
        /// Override this method to change image bounds
        /// </summary>
        /// <param name="sMode">Mode which is being measured</param>
        /// <param name="bounds">Bounds of the button</param>
        /// <remarks>
        /// The measuring occours in the following order:
        /// <list type="">
        /// <item>OnSetImageBounds</item>
        /// <item>OnSetTextBounds</item>
        /// <item>OnSetDropDownBounds</item>
        /// <item>OnSetButtonFaceBounds</item>
        /// </list>
        /// </remarks>
        internal virtual Rectangle OnGetImageBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            if (sMode == RibbonElementSizeMode.Large || this is RibbonOrbMenuItem)
            {
                if (Image != null)
                {
                    return new Rectangle(
                        Bounds.Left + ((Bounds.Width - Image.Width) / 2),
                        Bounds.Top + Owner.ItemMargin.Top,
                        Image.Width,
                        Image.Height);
                }

                return new Rectangle(ContentBounds.Location, new Size(32, 32));
            }
            else
            {
                if (SmallImage != null)
                {
                    return new Rectangle(
                        Bounds.Left + Owner.ItemMargin.Left,
                        Bounds.Top + ((Bounds.Height - SmallImage.Height) / 2),
                        SmallImage.Width,
                        SmallImage.Height);
                }

                return new Rectangle(ContentBounds.Location, new Size(16, 16));
            }
        }

        /// <summary>
        /// Sets the bounds of the text of the button when SetBounds is called.
        /// Override this method to change image bounds
        /// </summary>
        /// <param name="sMode">Mode which is being measured</param>
        /// <param name="bounds">Bounds of the button</param>
        /// <remarks>
        /// The measuring occours in the following order:
        /// <list type="">
        /// <item>OnSetImageBounds</item>
        /// <item>OnSetTextBounds</item>
        /// <item>OnSetDropDownBounds</item>
        /// <item>OnSetButtonFaceBounds</item>
        /// </list>
        /// </remarks>
        internal virtual Rectangle OnGetTextBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            int imgw = ImageBounds.Width;
            int imgh = ImageBounds.Height;

            if (sMode == RibbonElementSizeMode.Large)
            {
                return Rectangle.FromLTRB(
                    Bounds.Left + Owner.ItemMargin.Left,
                    Bounds.Top + Owner.ItemMargin.Top + imgh + Owner.ItemMargin.Vertical,
                    Bounds.Right - Owner.ItemMargin.Right,
                    Bounds.Bottom - Owner.ItemMargin.Bottom);
            }
            else
            {
                int ddw = Style != RibbonButtonStyle.Normal ? _dropDownMargin.Horizontal : 0;
                return Rectangle.FromLTRB(
                    Bounds.Left + imgw + Owner.ItemMargin.Horizontal + Owner.ItemMargin.Left,
                    Bounds.Top + Owner.ItemMargin.Top,
                    Bounds.Right - ddw,
                    Bounds.Bottom - Owner.ItemMargin.Bottom);

            } 
        }

        /// <summary>
        /// Sets the bounds of the dropdown part of the button when SetBounds is called.
        /// Override this method to change image bounds
        /// </summary>
        /// <param name="sMode">Mode which is being measured</param>
        /// <param name="bounds">Bounds of the button</param>
        /// <remarks>
        /// The measuring occours in the following order:
        /// <list type="">
        /// <item>OnSetImageBounds</item>
        /// <item>OnSetTextBounds</item>
        /// <item>OnSetDropDownBounds</item>
        /// <item>OnSetButtonFaceBounds</item>
        /// </list>
        /// </remarks>
        internal virtual Rectangle OnGetDropDownBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            Rectangle sideBounds = Rectangle.FromLTRB(
                bounds.Right - _dropDownMargin.Horizontal - 2,
                bounds.Top, bounds.Right, bounds.Bottom);

            switch (SizeMode)
            {
                case RibbonElementSizeMode.Large:
                case RibbonElementSizeMode.Overflow:
                    return Rectangle.FromLTRB(bounds.Left,
                                              bounds.Top + Image.Height + Owner.ItemMargin.Vertical,
                                              bounds.Right, bounds.Bottom);

                case RibbonElementSizeMode.DropDown:
                case RibbonElementSizeMode.Medium:
                case RibbonElementSizeMode.Compact:
                    return sideBounds;
            }
            

            return Rectangle.Empty;
        }

        /// <summary>
        /// Sets the bounds of the button face part of the button when SetBounds is called.
        /// Override this method to change image bounds
        /// </summary>
        /// <param name="sMode">Mode which is being measured</param>
        /// <param name="bounds">Bounds of the button</param>
        /// <remarks>
        /// The measuring occours in the following order:
        /// <list type="">
        /// <item>OnSetImageBounds</item>
        /// <item>OnSetTextBounds</item>
        /// <item>OnSetDropDownBounds</item>
        /// <item>OnSetButtonFaceBounds</item>
        /// </list>
        /// </remarks>
        internal virtual Rectangle OnGetButtonFaceBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            switch (SizeMode)
            {
                case RibbonElementSizeMode.Large:
                case RibbonElementSizeMode.Overflow:
                    return Rectangle.FromLTRB(bounds.Left,
                                              bounds.Top, bounds.Right, _dropDownBounds.Top);

                case RibbonElementSizeMode.DropDown:
                case RibbonElementSizeMode.Medium:
                case RibbonElementSizeMode.Compact:
                    return Rectangle.FromLTRB(bounds.Left, bounds.Top,
                                              _dropDownBounds.Left, bounds.Bottom);

            }

            return Rectangle.Empty;
        }

        /// <summary>
        /// Measures the string for the large size
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Size MeasureStringLargeSize(Graphics g, string text, Font font)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Size.Empty;
            }

            Size sz = g.MeasureString(text, font).ToSize();
            string[] words = text.Split(' ');
            string longestWord = string.Empty;
            int width = sz.Width;

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > longestWord.Length)
                {
                    longestWord = words[i];
                }
            }

            if (words.Length > 1)
            {
                width = Math.Max(sz.Width / 2, g.MeasureString(longestWord, font).ToSize().Width) + 1;
            }
            else
            {
                return g.MeasureString(text, font).ToSize();
            }

            Size rs = g.MeasureString(text, font, width).ToSize();

            return new Size(rs.Width, rs.Height);
        }

        /// <summary>
        /// Measures the size of the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            RibbonElementSizeMode theSize = GetNearestSize(e.SizeMode);
            int widthSum = Owner.ItemMargin.Horizontal;
            int heightSum = Owner.ItemMargin.Vertical;
            int largeHeight = OwnerPanel == null ? 0 : OwnerPanel.ContentBounds.Height - Owner.ItemPadding.Vertical;// -Owner.ItemMargin.Vertical; //58;

            Size simg = SmallImage != null ? SmallImage.Size : Size.Empty;
            Size img = Image != null ? Image.Size : Size.Empty;
            Size sz = Size.Empty;

            switch (theSize)
            {
                case RibbonElementSizeMode.Large:
                case RibbonElementSizeMode.Overflow:
                    sz = MeasureStringLargeSize(e.Graphics, Text, Owner.Font);
                    if (!string.IsNullOrEmpty(Text))
                    {
                        widthSum += Math.Max(sz.Width + 1, img.Width);
                        heightSum = largeHeight;
                    }
                    else
                    {
                        widthSum += img.Width;
                        heightSum += img.Height;
                    }
                    
                    break;
                case RibbonElementSizeMode.DropDown:
                case RibbonElementSizeMode.Medium:
                    sz = e.Graphics.MeasureString(Text, Owner.Font).ToSize();
                    if(!string.IsNullOrEmpty(Text)) widthSum += sz.Width + 1;
                    widthSum += simg.Width + Owner.ItemMargin.Horizontal;
                    heightSum += Math.Max(sz.Height, simg.Height);
                    break;
                case RibbonElementSizeMode.Compact:
                    widthSum += simg.Width;
                    heightSum += simg.Height;
                    break;
                default:
                    throw new ApplicationException("SizeMode not supported: " + e.SizeMode);
            }

            if (theSize == RibbonElementSizeMode.DropDown)
            {
                heightSum += 2;
            }

            if (Style == RibbonButtonStyle.DropDown)
            {
                widthSum += arrowWidth + Owner.ItemMargin.Right;
            }
            else if (Style == RibbonButtonStyle.SplitDropDown)
            {
                widthSum += arrowWidth + Owner.ItemMargin.Horizontal;
            }

            SetLastMeasuredSize(new Size(widthSum, heightSum));

            return LastMeasuredSize;
        }

        /// <summary>
        /// Shows the drop down items of the button, as if the dropdown part has been clicked
        /// </summary>
        public void ShowDropDown()
        {
            OnDropDownShowing(EventArgs.Empty);
            
            if (Style == RibbonButtonStyle.Normal || DropDownItems.Count == 0)
            {
                CloseDropDown();
                return;
            }

            if (Style == RibbonButtonStyle.DropDown)
            {
                SetPressed(true);
            }
            else
            {
                _dropDownPressed = true;
            }

            CreateDropDown();
            DropDown.Closed += _dropDown_Closed;
            DropDown.ShowSizingGrip = DropDownResizable;
            
            RibbonPopup canvasdd = Canvas as RibbonPopup;
            Point location = OnGetDropDownMenuLocation();
            Size minsize = OnGetDropDownMenuSize();

            if (canvasdd != null)
            {
                canvasdd.NextPopup = DropDown;
                DropDown.PreviousPopup = canvasdd;
            }

            if (!minsize.IsEmpty) DropDown.MinimumSize = minsize;

            SetDropDownVisible(true);
            DropDown.SelectionService = GetService(typeof(ISelectionService)) as ISelectionService;
            DropDown.Show(location);
        }

        /// <summary>
        /// Gets the location where the dropdown menu will be shown
        /// </summary>
        /// <returns></returns>
        internal virtual Point OnGetDropDownMenuLocation()
        {
            
            var location = Point.Empty;

            if (Canvas is RibbonDropDown)
            {
                location = Canvas.PointToScreen(new Point(Bounds.Right, Bounds.Top));
            }
            else
            {
                location = Canvas.PointToScreen(new Point(Bounds.Left, Bounds.Bottom));
            }

            return location;
        }

        /// <summary>
        /// Gets the size of the dropdown. If Size.Empty is returned, 
        /// size will be measured automatically
        /// </summary>
        /// <returns></returns>
        internal virtual Size OnGetDropDownMenuSize()
        {
            return Size.Empty;
        }

        /// <summary>
        /// Handles the closing of the dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _dropDown_Closed(object sender, EventArgs e)
        {
            SetPressed(false);

            _dropDownPressed = false;

            SetDropDownVisible(false);

            SetSelected(false);

            RedrawItem();
        }

        /// <summary>
        /// Closes the DropDown if opened
        /// </summary>
        public void CloseDropDown()
        {
            if (DropDown != null)
            {
                DropDown.Close();
            }

            SetDropDownVisible(false);
        }

        /// <summary>
        /// Overriden. Informs the button text
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{1}: {0}", Text, GetType().Name);
        }

        /// <summary>
        /// Sets the value of DropDownVisible
        /// </summary>
        /// <param name="visible"></param>
        internal void SetDropDownVisible(bool visible)
        {
            DropDownVisible = visible;
        }

        /// <summary>
        /// Raises the DropDownShowing event
        /// </summary>
        /// <param name="e"></param>
        public void OnDropDownShowing(EventArgs e)
        {
            if (DropDownShowing != null)
            {
                DropDownShowing(this, e);
            }
        }

        #endregion

        #region Overrides

        public override void OnCanvasChanged(EventArgs e)
        {
            base.OnCanvasChanged(e);

            if (Canvas is RibbonDropDown)
            {
                DropDownArrowDirection = RibbonArrowDirection.Left;
            }
        }

        public override bool ClosesDropDownAt(Point p)
        {
            if (Style == RibbonButtonStyle.DropDown)
            {
                return false;
            }

            if (Style == RibbonButtonStyle.SplitDropDown)
            {
                return ButtonFaceBounds.Contains(p);
            }

            return true;
        }

        internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            

            if (sizeMode == RibbonElementSizeMode.Overflow)
            {
                base.SetSizeMode(RibbonElementSizeMode.Large);
            }
            else
            {
                base.SetSizeMode(sizeMode);
            }
        }

        internal override void SetSelected(bool selected)
        {
            base.SetSelected(selected);

            SetPressed(false);
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!Enabled) return;

            if ((DropDownSelected || Style == RibbonButtonStyle.DropDown)/* && DropDownItems.Count > 0*/)
            {
                if (DropDownItems.Count > 0)
                {
                    _dropDownPressed = true;
                }

                ShowDropDown();
            }

            base.OnMouseDown(e);
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!Enabled) return;

            //Detect mouse on dropdwon
            if (Style == RibbonButtonStyle.SplitDropDown)
            {
                bool lastState = _dropDownSelected;

                _dropDownSelected = DropDownBounds.Contains(e.X, e.Y);

                if (lastState != _dropDownSelected) 
                    RedrawItem();
            }

            _lastMousePos = new Point(e.X, e.Y);

            base.OnMouseMove(e);
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            _dropDownSelected = false;
            
        }

        public override void OnClick(EventArgs e)
        {
            if (Style != RibbonButtonStyle.Normal && !ButtonFaceBounds.Contains(_lastMousePos))
            {
                //Clicked the dropdown area, don't raise Click()
                return;
            }

            if(CheckOnClick)
                Checked = !Checked;

            base.OnClick(e);
        }

        #endregion

        #region IContainsRibbonItems Members

        public IEnumerable<RibbonItem> GetItems()
        {
            return DropDownItems;
        }

        #endregion

        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return DropDownItems.ToArray();
        }

        #endregion
    }
}