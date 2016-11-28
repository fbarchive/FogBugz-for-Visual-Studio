namespace FogBugzForVisualStudio
{
    partial class FogBugzCtl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FogBugzCtl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid = new System.Windows.Forms.DataGridView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolNewCase = new System.Windows.Forms.ToolStripButton();
            this.toolSendEmail = new System.Windows.Forms.ToolStripButton();
            this.ddFilters = new System.Windows.Forms.ToolStripDropDownButton();
            this.loadingListOfFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblFilter = new System.Windows.Forms.ToolStripLabel();
            this.Settings = new System.Windows.Forms.ToolStripDropDownButton();
            this.reportErrorsAutomaticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLogOff = new System.Windows.Forms.ToolStripButton();
            this.ddWorkingOn = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuWorkOnNothing = new System.Windows.Forms.ToolStripMenuItem();
            this.recentlyWorkedOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNoRecentWorkedOn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewCurrentCase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditTimesheet = new System.Windows.Forms.ToolStripMenuItem();
            this.panelLogOn = new System.Windows.Forms.Panel();
            this.lblNotLoggedOn = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.timUpdateWorkingOn = new System.Windows.Forms.Timer(this.components);
            this.ixCategory = new System.Windows.Forms.DataGridViewImageColumn();
            this.ixBug = new System.Windows.Forms.DataGridViewLinkColumn();
            this.sTitle = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ixPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixPersonAssignedTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hrsRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hrsOrigEst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hrsCurrEst = new System.Windows.Forms.DataGridViewLinkColumn();
            this.hrsElapsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixFixFor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixPersonClosedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixPersonOpenedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixPersonResolvedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixPersonLastEditedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ixProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtOpened = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtResolved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtClosed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtDue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtLastUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iBacklog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.panelLogOn.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToOrderColumns = true;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.ColumnHeadersHeight = 25;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ixCategory,
            this.ixBug,
            this.sTitle,
            this.ixPriority,
            this.ixPersonAssignedTo,
            this.ixStatus,
            this.ixArea,
            this.hrsRemaining,
            this.hrsOrigEst,
            this.hrsCurrEst,
            this.hrsElapsed,
            this.ixFixFor,
            this.ixPersonClosedBy,
            this.ixPersonOpenedBy,
            this.ixPersonResolvedBy,
            this.ixPersonLastEditedBy,
            this.ixProject,
            this.dtOpened,
            this.dtResolved,
            this.dtClosed,
            this.dtDue,
            this.dtLastUpdated,
            this.iBacklog});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid.DefaultCellStyle = dataGridViewCellStyle3;
            this.grid.GridColor = System.Drawing.SystemColors.ControlLight;
            this.grid.Location = new System.Drawing.Point(0, 21);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grid.RowHeadersVisible = false;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(715, 210);
            this.grid.TabIndex = 0;
            this.grid.VirtualMode = true;
            this.grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellContentClick);
            this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_CellFormatting);
            this.grid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseClick);
            this.grid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseDown);
            this.grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_CellPainting);
            this.grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.grid_CellValueNeeded);
            this.grid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_ColumnHeaderMouseClick);
            this.grid.ColumnStateChanged += new System.Windows.Forms.DataGridViewColumnStateChangedEventHandler(this.grid_ColumnStateChanged);
            this.grid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grid_ColumnWidthChanged);
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonStop,
            this.toolRefresh,
            this.toolNewCase,
            this.toolSendEmail,
            this.ddFilters,
            this.lblFilter,
            this.Settings,
            this.btnLogOff,
            this.ddWorkingOn});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(716, 25);
            this.toolStrip.TabIndex = 3;
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.AutoSize = false;
            this.toolStripButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStop.Image")));
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(64, 22);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.Visible = false;
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // toolRefresh
            // 
            this.toolRefresh.AutoSize = false;
            this.toolRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolRefresh.Image")));
            this.toolRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRefresh.Name = "toolRefresh";
            this.toolRefresh.Size = new System.Drawing.Size(64, 22);
            this.toolRefresh.Text = "Reload";
            this.toolRefresh.ToolTipText = "Refresh";
            this.toolRefresh.Click += new System.EventHandler(this.toolRefresh_Click);
            // 
            // toolNewCase
            // 
            this.toolNewCase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolNewCase.Name = "toolNewCase";
            this.toolNewCase.Size = new System.Drawing.Size(63, 22);
            this.toolNewCase.Text = "New Case";
            this.toolNewCase.Click += new System.EventHandler(this.toolNewCase_Click);
            // 
            // toolSendEmail
            // 
            this.toolSendEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolSendEmail.Name = "toolSendEmail";
            this.toolSendEmail.Size = new System.Drawing.Size(69, 22);
            this.toolSendEmail.Text = "Send Email";
            this.toolSendEmail.Click += new System.EventHandler(this.toolSendEmail_Click);
            // 
            // ddFilters
            // 
            this.ddFilters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddFilters.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadingListOfFiltersToolStripMenuItem});
            this.ddFilters.Image = ((System.Drawing.Image)(resources.GetObject("ddFilters.Image")));
            this.ddFilters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddFilters.Name = "ddFilters";
            this.ddFilters.Size = new System.Drawing.Size(51, 22);
            this.ddFilters.Text = "Filters";
            this.ddFilters.DropDownOpening += new System.EventHandler(this.ddFilters_DropDownOpening);
            this.ddFilters.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ddFilters_DropDownItemClicked);
            // 
            // loadingListOfFiltersToolStripMenuItem
            // 
            this.loadingListOfFiltersToolStripMenuItem.Enabled = false;
            this.loadingListOfFiltersToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.loadingListOfFiltersToolStripMenuItem.Name = "loadingListOfFiltersToolStripMenuItem";
            this.loadingListOfFiltersToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.loadingListOfFiltersToolStripMenuItem.Tag = "LoadingDoNotList";
            this.loadingListOfFiltersToolStripMenuItem.Text = "Loading list of filters...";
            // 
            // lblFilter
            // 
            this.lblFilter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(0, 22);
            // 
            // Settings
            // 
            this.Settings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Settings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportErrorsAutomaticallyToolStripMenuItem});
            this.Settings.Image = ((System.Drawing.Image)(resources.GetObject("Settings.Image")));
            this.Settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(29, 22);
            this.Settings.Text = "Settings";
            // 
            // reportErrorsAutomaticallyToolStripMenuItem
            // 
            this.reportErrorsAutomaticallyToolStripMenuItem.CheckOnClick = true;
            this.reportErrorsAutomaticallyToolStripMenuItem.Name = "reportErrorsAutomaticallyToolStripMenuItem";
            this.reportErrorsAutomaticallyToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.reportErrorsAutomaticallyToolStripMenuItem.Text = "Report Errors Automatically";
            this.reportErrorsAutomaticallyToolStripMenuItem.CheckedChanged += new System.EventHandler(this.reportErrorsAutomaticallyToolStripMenuItem_CheckedChanged);
            // 
            // btnLogOff
            // 
            this.btnLogOff.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnLogOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLogOff.Image = ((System.Drawing.Image)(resources.GetObject("btnLogOff.Image")));
            this.btnLogOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogOff.Name = "btnLogOff";
            this.btnLogOff.Size = new System.Drawing.Size(51, 22);
            this.btnLogOff.Text = "Log Off";
            this.btnLogOff.Click += new System.EventHandler(this.btnLogOff_Click);
            // 
            // ddWorkingOn
            // 
            this.ddWorkingOn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ddWorkingOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddWorkingOn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWorkOnNothing,
            this.recentlyWorkedOnToolStripMenuItem,
            this.toolStripSeparator2,
            this.menuViewCurrentCase,
            this.toolStripSeparator1,
            this.menuEditTimesheet});
            this.ddWorkingOn.Image = ((System.Drawing.Image)(resources.GetObject("ddWorkingOn.Image")));
            this.ddWorkingOn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddWorkingOn.Name = "ddWorkingOn";
            this.ddWorkingOn.Size = new System.Drawing.Size(84, 22);
            this.ddWorkingOn.Text = "Working On";
            // 
            // menuWorkOnNothing
            // 
            this.menuWorkOnNothing.Name = "menuWorkOnNothing";
            this.menuWorkOnNothing.Size = new System.Drawing.Size(182, 22);
            this.menuWorkOnNothing.Text = "Nothing";
            this.menuWorkOnNothing.Click += new System.EventHandler(this.menuWorkOnNothing_Click);
            // 
            // recentlyWorkedOnToolStripMenuItem
            // 
            this.recentlyWorkedOnToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNoRecentWorkedOn});
            this.recentlyWorkedOnToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recentlyWorkedOnToolStripMenuItem.Name = "recentlyWorkedOnToolStripMenuItem";
            this.recentlyWorkedOnToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.recentlyWorkedOnToolStripMenuItem.Text = "Recently Worked On";
            // 
            // menuNoRecentWorkedOn
            // 
            this.menuNoRecentWorkedOn.Enabled = false;
            this.menuNoRecentWorkedOn.Name = "menuNoRecentWorkedOn";
            this.menuNoRecentWorkedOn.Size = new System.Drawing.Size(127, 22);
            this.menuNoRecentWorkedOn.Text = "(no cases)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(179, 6);
            // 
            // menuViewCurrentCase
            // 
            this.menuViewCurrentCase.Name = "menuViewCurrentCase";
            this.menuViewCurrentCase.Size = new System.Drawing.Size(182, 22);
            this.menuViewCurrentCase.Text = "View Current Case";
            this.menuViewCurrentCase.Click += new System.EventHandler(this.menuViewCurrentCase_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // menuEditTimesheet
            // 
            this.menuEditTimesheet.Name = "menuEditTimesheet";
            this.menuEditTimesheet.Size = new System.Drawing.Size(182, 22);
            this.menuEditTimesheet.Text = "Edit Timesheet";
            this.menuEditTimesheet.Click += new System.EventHandler(this.menuEditTimesheet_Click);
            // 
            // panelLogOn
            // 
            this.panelLogOn.BackColor = System.Drawing.SystemColors.Control;
            this.panelLogOn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLogOn.Controls.Add(this.grid);
            this.panelLogOn.Controls.Add(this.lblNotLoggedOn);
            this.panelLogOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLogOn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.panelLogOn.Location = new System.Drawing.Point(0, 0);
            this.panelLogOn.Name = "panelLogOn";
            this.panelLogOn.Size = new System.Drawing.Size(716, 232);
            this.panelLogOn.TabIndex = 4;
            // 
            // lblNotLoggedOn
            // 
            this.lblNotLoggedOn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNotLoggedOn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotLoggedOn.Location = new System.Drawing.Point(-4, 24);
            this.lblNotLoggedOn.Name = "lblNotLoggedOn";
            this.lblNotLoggedOn.Size = new System.Drawing.Size(719, 207);
            this.lblNotLoggedOn.TabIndex = 0;
            this.lblNotLoggedOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNotLoggedOn.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // timUpdateWorkingOn
            // 
            this.timUpdateWorkingOn.Interval = 300000;
            this.timUpdateWorkingOn.Tick += new System.EventHandler(this.timUpdateWorkingOn_Tick);
            // 
            // ixCategory
            // 
            this.ixCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ixCategory.DataPropertyName = "categoryImage";
            this.ixCategory.HeaderText = "";
            this.ixCategory.MinimumWidth = 18;
            this.ixCategory.Name = "ixCategory";
            this.ixCategory.ReadOnly = true;
            this.ixCategory.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ixCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixCategory.Width = 18;
            // 
            // ixBug
            // 
            this.ixBug.DataPropertyName = "ixBug";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ixBug.DefaultCellStyle = dataGridViewCellStyle2;
            this.ixBug.HeaderText = "Case";
            this.ixBug.Name = "ixBug";
            this.ixBug.ReadOnly = true;
            this.ixBug.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixBug.TrackVisitedState = false;
            this.ixBug.Width = 57;
            // 
            // sTitle
            // 
            this.sTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sTitle.DataPropertyName = "sTitle";
            this.sTitle.FillWeight = 400F;
            this.sTitle.HeaderText = "Title";
            this.sTitle.MinimumWidth = 150;
            this.sTitle.Name = "sTitle";
            this.sTitle.ReadOnly = true;
            this.sTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.sTitle.TrackVisitedState = false;
            // 
            // ixPriority
            // 
            this.ixPriority.DataPropertyName = "priority";
            this.ixPriority.HeaderText = "Priority";
            this.ixPriority.Name = "ixPriority";
            this.ixPriority.ReadOnly = true;
            this.ixPriority.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixPriority.Width = 70;
            // 
            // ixPersonAssignedTo
            // 
            this.ixPersonAssignedTo.DataPropertyName = "assignedTo";
            this.ixPersonAssignedTo.HeaderText = "Assigned To";
            this.ixPersonAssignedTo.Name = "ixPersonAssignedTo";
            this.ixPersonAssignedTo.ReadOnly = true;
            this.ixPersonAssignedTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixPersonAssignedTo.Width = 97;
            // 
            // ixStatus
            // 
            this.ixStatus.DataPropertyName = "status";
            this.ixStatus.HeaderText = "Status";
            this.ixStatus.Name = "ixStatus";
            this.ixStatus.ReadOnly = true;
            this.ixStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixStatus.Width = 64;
            // 
            // ixArea
            // 
            this.ixArea.DataPropertyName = "area";
            this.ixArea.HeaderText = "Area";
            this.ixArea.Name = "ixArea";
            this.ixArea.ReadOnly = true;
            this.ixArea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixArea.Visible = false;
            this.ixArea.Width = 56;
            // 
            // hrsRemaining
            // 
            this.hrsRemaining.DataPropertyName = "hrsRemaining";
            this.hrsRemaining.HeaderText = "Remaining Time";
            this.hrsRemaining.Name = "hrsRemaining";
            this.hrsRemaining.ReadOnly = true;
            this.hrsRemaining.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.hrsRemaining.Visible = false;
            this.hrsRemaining.Width = 119;
            // 
            // hrsOrigEst
            // 
            this.hrsOrigEst.DataPropertyName = "hrsOrigEst";
            this.hrsOrigEst.HeaderText = "Estimate (original)";
            this.hrsOrigEst.Name = "hrsOrigEst";
            this.hrsOrigEst.ReadOnly = true;
            this.hrsOrigEst.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.hrsOrigEst.Visible = false;
            this.hrsOrigEst.Width = 128;
            // 
            // hrsCurrEst
            // 
            this.hrsCurrEst.DataPropertyName = "hrsCurrEst";
            this.hrsCurrEst.HeaderText = "Estimate (current)";
            this.hrsCurrEst.Name = "hrsCurrEst";
            this.hrsCurrEst.ReadOnly = true;
            this.hrsCurrEst.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.hrsCurrEst.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.hrsCurrEst.TrackVisitedState = false;
            this.hrsCurrEst.Width = 126;
            // 
            // hrsElapsed
            // 
            this.hrsElapsed.DataPropertyName = "hrsElapsed";
            this.hrsElapsed.HeaderText = "Elapsed Time";
            this.hrsElapsed.Name = "hrsElapsed";
            this.hrsElapsed.ReadOnly = true;
            this.hrsElapsed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.hrsElapsed.Visible = false;
            this.hrsElapsed.Width = 102;
            // 
            // ixFixFor
            // 
            this.ixFixFor.DataPropertyName = "fixfor";
            this.ixFixFor.HeaderText = "Milestone";
            this.ixFixFor.Name = "ixFixFor";
            this.ixFixFor.ReadOnly = true;
            this.ixFixFor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixFixFor.Visible = false;
            this.ixFixFor.Width = 84;
            // 
            // ixPersonClosedBy
            // 
            this.ixPersonClosedBy.DataPropertyName = "closedBy";
            this.ixPersonClosedBy.HeaderText = "Closed By";
            this.ixPersonClosedBy.Name = "ixPersonClosedBy";
            this.ixPersonClosedBy.ReadOnly = true;
            this.ixPersonClosedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixPersonClosedBy.Visible = false;
            this.ixPersonClosedBy.Width = 84;
            // 
            // ixPersonOpenedBy
            // 
            this.ixPersonOpenedBy.DataPropertyName = "openedBy";
            this.ixPersonOpenedBy.HeaderText = "Opened By";
            this.ixPersonOpenedBy.Name = "ixPersonOpenedBy";
            this.ixPersonOpenedBy.ReadOnly = true;
            this.ixPersonOpenedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixPersonOpenedBy.Visible = false;
            this.ixPersonOpenedBy.Width = 90;
            // 
            // ixPersonResolvedBy
            // 
            this.ixPersonResolvedBy.DataPropertyName = "resolvedBy";
            this.ixPersonResolvedBy.HeaderText = "Resolved By";
            this.ixPersonResolvedBy.Name = "ixPersonResolvedBy";
            this.ixPersonResolvedBy.ReadOnly = true;
            this.ixPersonResolvedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixPersonResolvedBy.Visible = false;
            this.ixPersonResolvedBy.Width = 95;
            // 
            // ixPersonLastEditedBy
            // 
            this.ixPersonLastEditedBy.DataPropertyName = "lastEditedBy";
            this.ixPersonLastEditedBy.HeaderText = "Last Edited By";
            this.ixPersonLastEditedBy.Name = "ixPersonLastEditedBy";
            this.ixPersonLastEditedBy.ReadOnly = true;
            this.ixPersonLastEditedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixPersonLastEditedBy.Visible = false;
            this.ixPersonLastEditedBy.Width = 105;
            // 
            // ixProject
            // 
            this.ixProject.DataPropertyName = "project";
            this.ixProject.HeaderText = "Project";
            this.ixProject.Name = "ixProject";
            this.ixProject.ReadOnly = true;
            this.ixProject.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ixProject.Visible = false;
            this.ixProject.Width = 69;
            // 
            // dtOpened
            // 
            this.dtOpened.DataPropertyName = "dtOpened";
            this.dtOpened.HeaderText = "Date Opened";
            this.dtOpened.Name = "dtOpened";
            this.dtOpened.ReadOnly = true;
            this.dtOpened.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dtOpened.Visible = false;
            this.dtOpened.Width = 101;
            // 
            // dtResolved
            // 
            this.dtResolved.DataPropertyName = "dtResolved";
            this.dtResolved.HeaderText = "Date Resolved";
            this.dtResolved.Name = "dtResolved";
            this.dtResolved.ReadOnly = true;
            this.dtResolved.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dtResolved.Visible = false;
            this.dtResolved.Width = 106;
            // 
            // dtClosed
            // 
            this.dtClosed.DataPropertyName = "dtClosed";
            this.dtClosed.HeaderText = "Date Closed";
            this.dtClosed.Name = "dtClosed";
            this.dtClosed.ReadOnly = true;
            this.dtClosed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dtClosed.Visible = false;
            this.dtClosed.Width = 95;
            // 
            // dtDue
            // 
            this.dtDue.DataPropertyName = "dtDue";
            this.dtDue.HeaderText = "Date Due";
            this.dtDue.Name = "dtDue";
            this.dtDue.ReadOnly = true;
            this.dtDue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dtDue.Visible = false;
            this.dtDue.Width = 80;
            // 
            // dtLastUpdated
            // 
            this.dtLastUpdated.DataPropertyName = "dtLastUpdated";
            this.dtLastUpdated.HeaderText = "Last Updated";
            this.dtLastUpdated.Name = "dtLastUpdated";
            this.dtLastUpdated.ReadOnly = true;
            this.dtLastUpdated.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dtLastUpdated.Visible = false;
            this.dtLastUpdated.Width = 101;
            // 
            // iBacklog
            // 
            this.iBacklog.DataPropertyName = "iBacklog";
            this.iBacklog.HeaderText = "Backlog";
            this.iBacklog.Name = "iBacklog";
            this.iBacklog.ReadOnly = true;
            this.iBacklog.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.iBacklog.Visible = false;
            this.iBacklog.Width = 74;
            // 
            // FogBugzCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.panelLogOn);
            this.Name = "FogBugzCtl";
            this.Size = new System.Drawing.Size(716, 232);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelLogOn.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolRefresh;
        private System.Windows.Forms.ToolStripDropDownButton ddFilters;
        private System.Windows.Forms.ToolStripMenuItem loadingListOfFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel lblFilter;
        private System.Windows.Forms.ToolStripButton btnLogOff;
        private System.Windows.Forms.ToolStripButton toolNewCase;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.Panel panelLogOn;
        private System.Windows.Forms.ToolStripDropDownButton ddWorkingOn;
        private System.Windows.Forms.ToolStripButton toolSendEmail;
        private System.Windows.Forms.ToolStripMenuItem menuWorkOnNothing;
        private System.Windows.Forms.ToolStripMenuItem recentlyWorkedOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuViewCurrentCase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuEditTimesheet;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuNoRecentWorkedOn;
        private System.Windows.Forms.Timer timUpdateWorkingOn;
        private System.Windows.Forms.Label lblNotLoggedOn;
        private System.Windows.Forms.ToolStripDropDownButton Settings;
        private System.Windows.Forms.ToolStripMenuItem reportErrorsAutomaticallyToolStripMenuItem;
        private System.Windows.Forms.DataGridViewImageColumn ixCategory;
        private System.Windows.Forms.DataGridViewLinkColumn ixBug;
        private System.Windows.Forms.DataGridViewLinkColumn sTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixPersonAssignedTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn hrsRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn hrsOrigEst;
        private System.Windows.Forms.DataGridViewLinkColumn hrsCurrEst;
        private System.Windows.Forms.DataGridViewTextBoxColumn hrsElapsed;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixFixFor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixPersonClosedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixPersonOpenedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixPersonResolvedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixPersonLastEditedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ixProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtOpened;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtResolved;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtClosed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtDue;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtLastUpdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn iBacklog;
    }
}
