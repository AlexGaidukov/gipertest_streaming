using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Utils.Controls.Ribbon
{
    /// <summary>
    /// Provides a Ribbon toolbar
    /// </summary>
    [Designer(typeof(RibbonDesigner))]
    public class Ribbon : Control
    {
        #region Fields
        internal bool ForceOrbMenu;
        private Size _lastSizeMeasured;
        private RibbonContextCollection _contexts;
        private RibbonRenderer _renderer;
        private RibbonTab _activeTab;
        private RibbonTab _lastSelectedTab;
        private RibbonSensor _sensor;
        private float _tabSum;
        private bool _updatingSuspended;
        private bool _orbPressed;
        private bool _orbVisible;
        private Image _orbImage;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the Orb is clicked
        /// </summary>
        public event EventHandler OrbClicked;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new Ribbon control
        /// </summary>
        public Ribbon()
        {
            
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Tabs = new RibbonTabCollection(this);
            _contexts = new RibbonContextCollection(this);

            TabsMargin = new Padding(12, 24 + 2, 20, 0);
            TabTextMargin = new Padding(4, 2, 4, 2);
            TabsPadding = new Padding(8, 5, 8, 3);
            TabContentMargin = new Padding(1, 0, 1, 2);
            PanelPadding = new Padding(3);
            PanelMargin = new Padding(3, 2, 3, 15);
            PanelSpacing = 3;
            ItemPadding = new Padding(1, 0, 1, 0);
            ItemMargin = new Padding(4, 2, 4, 2);
            TabSpacing = 6;
            DropDownMargin = new Padding(2);
            _renderer = new RibbonProfessionalRenderer();
            _orbVisible = true;
            OrbDropDown = new RibbonOrbDropDown(this);
            QuickAcessToolbar = new RibbonQuickAccessToolbar(this);
            QuickAccessVisible = true;
        }

        ~Ribbon()
        {
            //if(hHook != 0)
            //InstallHook();
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the Orb's DropDown
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
        public RibbonOrbDropDown OrbDropDown { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if the QuickAcess toolbar should be visible
        /// </summary>
        [DefaultValue(true)]
        [Description("Shows or hides the QuickAccess toolbar")]
        public bool QuickAccessVisible { get; set; }

        /// <summary>
        /// Gets  the QuickAcessToolbar
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonQuickAccessToolbar QuickAcessToolbar { get; private set; }

        /// <summary>
        /// Gets or sets the Image of the orb
        /// </summary>
        public Image OrbImage
        {
            get { return _orbImage; }
            set { _orbImage = value; Invalidate(OrbBounds); }
        }

        /// <summary>
        /// Gets or sets if the Ribbon should show an orb on the corner
        /// </summary>
        [DefaultValue(true)]
        public bool OrbVisible
        {
            get { return _orbVisible; }
            set { _orbVisible = value; OnRegionsChanged(); }
        }

        /// <summary>
        /// Gets or sets if the Orb is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OrbSelected { get; set; }

        /// <summary>
        /// Gets or sets if the Orb is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OrbPressed
        {
            get { return _orbPressed; }
            set { _orbPressed = value; Invalidate(OrbBounds); }
        }

        /// <summary>
        /// Gets the Height of the caption bar
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CaptionBarSize
        {
            get { return 24; }
        }

        /// <summary>
        /// Gets the bounds of the orb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle OrbBounds
        {
            get
            {
                if (OrbVisible)
                {
                    return new Rectangle(4, 4, 36, 36);
                }

                return new Rectangle(4, 4, 0, 0);
            }
        }


        /// <summary>
        /// Gets the next tab to be activated
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab NextTab
        {
            get {

                if (ActiveTab == null || Tabs.Count == 0)
                {
                    if (Tabs.Count == 0)
                        return null;

                    return Tabs[0];
                }

                int index = Tabs.IndexOf(ActiveTab);

                if (index == Tabs.Count - 1)
                {
                    return ActiveTab;
                }

                return Tabs[index +1];
            }
        }

        /// <summary>
        /// Gets the next tab to be activated
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab PreviousTab
        {
            get
            {

                if (ActiveTab == null || Tabs.Count == 0)
                {
                    if (Tabs.Count == 0)
                        return null;

                    return Tabs[0];
                }

                int index = Tabs.IndexOf(ActiveTab);

                if (index == 0)
                {
                    return ActiveTab;
                }

                return Tabs[index - 1];
            }
        }

        /// <summary>
        /// Gets or sets the internal spacing between the tab and its text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabTextMargin { get; set; }

        /// <summary> 
        /// Gets or sets the margis of the DropDowns shown by the Ribbon
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding DropDownMargin { get; set; }

        /// <summary>
        /// Gets or sets the external spacing of items on panels
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding ItemPadding { get; set; }

        /// <summary>
        /// Gets or sets the internal spacing of items
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding ItemMargin { get; set; }

        /// <summary>
        /// Gets or sets the tab that is currently active
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab ActiveTab
        {
            get { return _activeTab; }
            set 
            {
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab != value)
                    {
                        tab.SetActive(false);
                    }
                    else
                    {
                        tab.SetActive(true);
                    }
                }

                _activeTab = value;

                RemoveHelperControls();
                
                value.UpdatePanelsRegions();

                Invalidate();

                if (Sensor != null) Sensor.Dispose();

                _sensor = new RibbonSensor(this, this, value);
            }
        }

        /// <summary>
        /// Gets or sets the spacing leaded between panels
        /// </summary>
        [DefaultValue(2)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PanelSpacing { get; set; }

        /// <summary>
        /// Gets or sets the external spacing of panels inside of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelPadding { get; set; }

        /// <summary>
        /// Gets or sets the internal spacing of panels inside of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelMargin { get; set; }

        /// <summary>
        /// Gets or sets the spacing between tabs
        /// </summary>
        [DefaultValue(7)]
        public int TabSpacing { get; set; }

        /// <summary>
        /// Gets the collection of RibbonTab tabs
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonTabCollection Tabs { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if the Ribbon is currently minimized
        /// </summary>
        public bool Minimized { get; set; }

        /// <summary>
        /// Gets the collection of Contexts of this Ribbon
        /// </summary>
        public RibbonContextCollection Contexts
        {
            get
            {
                return _contexts;
            }
        }

        /// <summary>
        /// Gets or sets the Renderer for this Ribbon control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonRenderer Renderer
        {
            get
            {
                return _renderer;
            }
            set
            {
                if (value == null) throw new ApplicationException("Null renderer!");
                _renderer = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the internal spacing of the tab content pane
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabContentMargin { get; set; }

        /// <summary>
        /// Gets or sets the external spacing of the tabs content pane
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabContentPadding { get; set; }

        /// <summary>
        /// Gets a value indicating the external spacing of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabsMargin { get; set; }

        /// <summary>
        /// Gets a value indicating the internal spacing of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabsPadding { get; set; }

        /// <summary>
        /// Overriden. The maximum size is fixed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MaximumSize
        {
            get
            {
                return new Size(0, 138); //115 was the old one
            }
            set
            {
                //Ignored.
            }
        }

        /// <summary>
        /// Overriden. The minimum size is fixed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize
        {
            get
            {
                return new Size(0, 138); //115);
            }
            set
            {
                //Ignored.
            }
        }

        /// <summary>
        /// Overriden. The default dock of the ribbon is top
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(DockStyle.Top)]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
            }
        }


        /// <summary>
        /// Gets or sets the current panel sensor for this ribbon
        /// </summary>
        [Browsable(false)]
        public RibbonSensor Sensor
        {
            get
            {
                return _sensor;
            }
        }

        #endregion

        #region Handler Methods

        /// <summary>
        /// Raises the OrbClicked event
        /// </summary>
        /// <param name="e">event data</param>
        protected void OnOrbClicked(EventArgs e)
        {
            if (OrbClicked != null)
            {
                OrbClicked(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Suspends any drawing/regions update operation
        /// </summary>
        public void SuspendUpdating()
        {
            _updatingSuspended = true;
        }

        /// <summary>
        /// Resumes any drawing/regions update operation
        /// </summary>
        public void ResumeUpdating()
        {
            ResumeUpdating(true);
        }

        /// <summary>
        /// Resumes any drawing/regions update operation
        /// </summary>
        /// <param name="update"></param>
        public void ResumeUpdating(bool update)
        {
            _updatingSuspended = false;

            if (update)
            {
                OnRegionsChanged();
            }
        }

        /// <summary>
        /// Removes all helper controls placed by any reason.
        /// Contol's visibility is set to false before removed.
        /// </summary>
        private void RemoveHelperControls()
        {
            while (Controls.Count > 0)
            {
                Control ctl = Controls[0];

                ctl.Visible = false;

                Controls.Remove(ctl);
            }
        }

        /// <summary>
        /// Hittest on tab
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if a tab has been clicked</returns>
        internal bool TabHitTest(int x, int y)
        {
            
            //look for mouse on tabs
            foreach (RibbonTab tab in Tabs)
            {
                if (tab.TabBounds.Contains(x, y))
                {
                    ActiveTab = tab;
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Updates the regions of the tabs and the tab content bounds of the active tab
        /// </summary>
        internal void UpdateRegions()
        {
            UpdateRegions(null);
        }

        /// <summary>
        /// Updates the regions of the tabs and the tab content bounds of the active tab
        /// </summary>
        internal void UpdateRegions(Graphics g)
        {
            bool graphicsCreated = false;

            if (IsDisposed || _updatingSuspended) return;

            ///Graphics for measurement
            if (g == null)
            {
                g = CreateGraphics();
                graphicsCreated = true;
            }

            ///X coordinate reminder
            int curLeft = TabsMargin.Left + OrbBounds.Width;

            ///Saves the width of the larger tab
            int maxWidth = 0;

            ///Saves the bottom of the tabs
            int tabsBottom = 0;

            #region Assign default tab bounds (best case)
            foreach (RibbonTab tab in Tabs)
            {
                Size tabSize = tab.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));
                Rectangle bounds = new Rectangle(curLeft, TabsMargin.Top,
                                                 TabsPadding.Left + tabSize.Width + TabsPadding.Right,
                                                 TabsPadding.Top + tabSize.Height + TabsPadding.Bottom);

                tab.SetTabBounds(bounds);

                curLeft = bounds.Right + TabSpacing;

                maxWidth = Math.Max(bounds.Width, maxWidth);
                tabsBottom = Math.Max(bounds.Bottom, tabsBottom);

                tab.SetTabContentBounds(Rectangle.FromLTRB(
                                            TabContentMargin.Left, tabsBottom + TabContentMargin.Top,
                                            ClientSize.Width - 1 - TabContentMargin.Right, ClientSize.Height - 1 - TabContentMargin.Bottom));

                if (tab.Active)
                {
                    tab.UpdatePanelsRegions();
                }
            }

            #endregion

            #region Reduce bounds of tabs if needed

            while (curLeft > ClientRectangle.Right && maxWidth > 0)
            {

                curLeft = TabsMargin.Left + OrbBounds.Width;
                maxWidth--;

                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.TabBounds.Width >= maxWidth)
                    {
                        tab.SetTabBounds(new Rectangle(curLeft, TabsMargin.Top,
                                                       maxWidth, tab.TabBounds.Height));
                    }
                    else
                    {
                        tab.SetTabBounds(new Rectangle(
                                             new Point(curLeft, TabsMargin.Top), 
                                             tab.TabBounds.Size));
                    }

                    curLeft = tab.TabBounds.Right + TabSpacing;
                }
            }

            #endregion

            #region Update QuickAccess bounds

            QuickAcessToolbar.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.Compact));
            QuickAcessToolbar.SetBounds(new Rectangle(new Point(OrbBounds.Right + QuickAcessToolbar.Margin.Left, OrbBounds.Top - 2), QuickAcessToolbar.LastMeasuredSize));

            #endregion

            if (graphicsCreated)
                g.Dispose();

            _lastSizeMeasured = Size;
        }
        
        /// <summary>
        /// Called when the tabs collection has changed (Tabs has been added or removed)
        /// and region re-measuring is necessary
        /// </summary>
        internal void OnRegionsChanged()
        {
            if (_updatingSuspended) return;

            if (Tabs.Count == 1)
            {
                ActiveTab = Tabs[0];
            }

            _lastSizeMeasured = Size.Empty;
            Refresh();
        }

        /// <summary>
        /// Redraws the specified tab
        /// </summary>
        /// <param name="tab"></param>
        private void RedrawTab(RibbonTab tab)
        {
            using (Graphics g = CreateGraphics())
            {
                Rectangle clip = Rectangle.FromLTRB(
                    tab.TabBounds.Left,
                    tab.TabBounds.Top,
                    tab.TabBounds.Right,
                    tab.TabBounds.Bottom);

                g.SetClip(clip);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                tab.OnPaint(this, new RibbonElementPaintEventArgs(tab.TabBounds, g, RibbonElementSizeMode.None));
            }
        }

        /// <summary>
        /// Sets the currently selected tab
        /// </summary>
        /// <param name="tab"></param>
        private void SetSelectedTab(RibbonTab tab)
        {
            if (tab == _lastSelectedTab) return;

            if (_lastSelectedTab != null)
            {
                _lastSelectedTab.SetSelected(false);
                RedrawTab(_lastSelectedTab);
            }

            if (tab != null)
            {
                tab.SetSelected(true);
                RedrawTab(tab);
            }
            
            _lastSelectedTab = tab; 
            
        }

        /// <summary>
        /// Suspends the sensor activity
        /// </summary>
        internal void SuspendSensor()
        {
            if (Sensor != null)
                Sensor.Suspend();
        }

        /// <summary>
        /// Resumes the sensor activity
        /// </summary>
        internal void ResumeSensor()
        {
            Sensor.Resume();
        }

        /// <summary>
        /// Redraws the specified area on the sensor's control
        /// </summary>
        /// <param name="area"></param>
        public void RedrawArea(Rectangle area)
        {
            Sensor.Control.Invalidate(area);
        }

        /// <summary>
        /// Activates the next tab available
        /// </summary>
        public void ActivateNextTab()
        {
            RibbonTab tab = NextTab;

            if (tab != null)
            {
                ActiveTab = tab;
            }
        }

        /// <summary>
        /// Activates the previous tab available
        /// </summary>
        public void ActivatePreviousTab()
        {
            RibbonTab tab = PreviousTab;

            if (tab != null)
            {
                ActiveTab = tab;
            }
        }

        /// <summary>
        /// Handles the mouse down on the orb area
        /// </summary>
        internal void OrbMouseDown()
        {
            if (OrbPressed)
            {
                OrbDropDown.Close();
            }
            else
            {
                ShowOrbDropDown();
            }
            OnOrbClicked(EventArgs.Empty);
        }

        #endregion

        #region Event Overrides
        /// <summary>
        /// Overriden. Raises the Paint event and draws all the Ribbon content
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if ( _updatingSuspended) return;
            
            if (Size != _lastSizeMeasured)
                UpdateRegions(e.Graphics);
             
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Renderer.OnRenderRibbonBackground(new RibbonRenderEventArgs(this, e.Graphics, e.ClipRectangle));

            //Caption Bar
            Renderer.OnRenderRibbonCaptionBar(new RibbonRenderEventArgs(this, e.Graphics, e.ClipRectangle));

            //Orb
            Renderer.OnRenderRibbonOrb(new RibbonRenderEventArgs(this, e.Graphics, e.ClipRectangle));

            //QuickAcess toolbar
            QuickAcessToolbar.OnPaint(this, new RibbonElementPaintEventArgs(e.ClipRectangle, e.Graphics, RibbonElementSizeMode.Compact));

            //Render Tabs
            foreach (RibbonTab tab in Tabs)
            {
                tab.OnPaint(this, new RibbonElementPaintEventArgs(tab.TabBounds, e.Graphics, RibbonElementSizeMode.None));
            }
        }

        /// <summary>
        /// Overriden. Raises the MouseMove event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (ActiveTab == null) return;

            bool someTabHitted = false;

            //Check if mouse on tab
            if (ActiveTab.TabContentBounds.Contains(e.X, e.Y))
            {
                //Do nothing, everything is on the sensor
            }
                //Check if mouse on orb
            else if (OrbVisible && OrbBounds.Contains(e.Location) && !OrbSelected)
            {
                OrbSelected = true;
                Invalidate(OrbBounds);
            }
                //Check if mouse on QuickAccess toolbar
            else if (QuickAccessVisible && QuickAcessToolbar.Bounds.Contains(e.Location))
            {

            }
            else
            {
                //look for mouse on tabs
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.TabBounds.Contains(e.X, e.Y))
                    {
                        SetSelectedTab(tab);
                        someTabHitted = true;
                    }
                }
            }

            if (!someTabHitted)
                SetSelectedTab(null);

            if (OrbSelected && !OrbBounds.Contains(e.Location))
            {
                OrbSelected = false;
                Invalidate(OrbBounds);
            } 
        }

        /// <summary>
        /// Overriden. Raises the MouseDown event and tunnels the message to child elements
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (OrbBounds.Contains(e.Location))
            {
                OrbMouseDown();
            }
            else
            {
                TabHitTest(e.X, e.Y);
            }

        }

        /// <summary>
        /// Shows the Orb's dropdown
        /// </summary>
        public void ShowOrbDropDown()
        {
            OrbPressed = true;
            OrbDropDown.Show(PointToScreen(new Point(OrbBounds.X - 4, OrbBounds.Bottom - OrbDropDown.ContentMargin.Top + 2)));
        }

        /// <summary>
        /// Handles the mouse wheel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Tabs.Count == 0 || ActiveTab == null) return;

            int index = Tabs.IndexOf(ActiveTab);

            if (e.Delta < 0)
            {
                _tabSum += 0.4f;
            }
            else
            {
                _tabSum -= 0.4f;
            }

            int tabRounded = Convert.ToInt16(Math.Round(_tabSum));

            if (tabRounded != 0)
            {
                index += tabRounded;

                if (index < 0)
                {
                    index = 0;
                }
                else if (index >= Tabs.Count - 1)
                {
                    index = Tabs.Count - 1;
                }

                ActiveTab = Tabs[index];
                _tabSum = 0f;
            }
        }

        /// <summary>
        /// Overriden. Raises the OnSizeChanged event and performs layout calculations
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateRegions();

            RemoveHelperControls();

            base.OnSizeChanged(e);
        }

        #endregion
    }
}