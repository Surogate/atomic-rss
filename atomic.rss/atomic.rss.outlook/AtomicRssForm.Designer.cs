namespace atomic.rss.outlook
{
    [System.ComponentModel.ToolboxItemAttribute(false)]
    partial class AtomicRssForm : Microsoft.Office.Tools.Outlook.FormRegionBase
    {
        public AtomicRssForm(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            : base(Globals.Factory, formRegion)
        {
            this.InitializeComponent();
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
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
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.mainView1 = new atomic.rss.outlook.MainView();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(3, 3);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(818, 472);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.mainView1;
            // 
            // AtomicRssForm
            // 
            this.AccessibleDescription = "AtomicRssForm";
            this.AccessibleName = "AtomicRssForm";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost1);
            this.Name = "AtomicRssForm";
            this.Size = new System.Drawing.Size(824, 478);
            this.FormRegionShowing += new System.EventHandler(this.AtomicRssForm_FormRegionShowing);
            this.FormRegionClosed += new System.EventHandler(this.AtomicRssForm_FormRegionClosed);
            this.ResumeLayout(false);

        }

        #endregion

        #region Form Region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private static void InitializeManifest(Microsoft.Office.Tools.Outlook.FormRegionManifest manifest, Microsoft.Office.Tools.Outlook.Factory factory)
        {
            manifest.Description = "AtomicRssForm";
            manifest.FormRegionName = "AtomicRssForm";
            manifest.FormRegionType = Microsoft.Office.Tools.Outlook.FormRegionType.ReplaceAll;
            manifest.Title = "AtomicRssForm";

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private MainView mainView1;

        public partial class AtomicRssFormFactory : Microsoft.Office.Tools.Outlook.IFormRegionFactory
        {
            public event Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler FormRegionInitializing;

            private Microsoft.Office.Tools.Outlook.FormRegionManifest _Manifest;

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public AtomicRssFormFactory()
            {
                this._Manifest = Globals.Factory.CreateFormRegionManifest();
                AtomicRssForm.InitializeManifest(this._Manifest, Globals.Factory);
                this.FormRegionInitializing += new Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler(this.AtomicRssFormFactory_FormRegionInitializing);
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public Microsoft.Office.Tools.Outlook.FormRegionManifest Manifest
            {
                get
                {
                    return this._Manifest;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.IFormRegion Microsoft.Office.Tools.Outlook.IFormRegionFactory.CreateFormRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            {
                AtomicRssForm form = new AtomicRssForm(formRegion);
                form.Factory = this;
                return form;
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            byte[] Microsoft.Office.Tools.Outlook.IFormRegionFactory.GetFormRegionStorage(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                throw new System.NotSupportedException();
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            bool Microsoft.Office.Tools.Outlook.IFormRegionFactory.IsDisplayedForItem(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                if (this.FormRegionInitializing != null)
                {
                    Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs cancelArgs = Globals.Factory.CreateFormRegionInitializingEventArgs(outlookItem, formRegionMode, formRegionSize, false);
                    this.FormRegionInitializing(this, cancelArgs);
                    return !cancelArgs.Cancel;
                }
                else
                {
                    return true;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.FormRegionKindConstants Microsoft.Office.Tools.Outlook.IFormRegionFactory.Kind
            {
                get
                {
                    return Microsoft.Office.Tools.Outlook.FormRegionKindConstants.WindowsForms;
                }
            }
        }
    }

    partial class WindowFormRegionCollection
    {
        internal AtomicRssForm AtomicRssForm
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.GetType() == typeof(AtomicRssForm))
                        return (AtomicRssForm)item;
                }
                return null;
            }
        }
    }
}
