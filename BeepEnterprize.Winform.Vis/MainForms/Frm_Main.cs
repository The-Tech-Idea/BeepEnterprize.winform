﻿using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BeepEnterprize.Winform.Vis.Controls;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Logger;
using TheTechIdea.Util;

namespace BeepEnterprize.Winform.Vis.MainForms
{
    [AddinAttribute(Caption ="Beep", Name ="MainForm", misc ="MainForm")]
    public partial class Frm_Main : Form, IDM_Addin
    {
        public Frm_Main()
        {
           
            InitializeComponent();
           
        }

        public string ParentName { get ; set ; }
        public string ObjectName { get ; set ; }
        public string ObjectType { get ; set ; }
        public string AddinName { get ; set ; }
        public string Description { get ; set ; }
        public bool DefaultCreate { get ; set ; }
        public string DllPath { get ; set ; }
        public string DllName { get ; set ; }
        public string NameSpace { get ; set ; }
        public IErrorsInfo ErrorObject { get ; set ; }
        public IDMLogger Logger { get ; set ; }
        public IDMEEditor DMEEditor { get ; set ; }
        public EntityStructure EntityStructure { get ; set ; }
        public string EntityName { get ; set ; }
        public IPassedArgs Passedarg { get ; set ; }
     
        public VisManager visManager { get; set; }
        public bool startLoggin { get; set; } = false;
        public void Run(IPassedArgs pPassedarg)
        {
           
        }
        ToolbarControl toolbar;
        MenuControl menu;
        TreeControl Datatree;
        TreeControl Apptree;
        bool IsTreeSideOpen = true;
        bool IsSidePanelsOpen = true;
        bool IsLogPanelOpen = true;
        int LogPanelHeight;
       // private System.Windows.Forms.Button MinMaxButton;
        Image CollapseLeft;
        Image Collapseright;
        Image CollapseUp;
        Image CollapseDown;
        Image ListSearch;
        bool IsDevModeOn = true;
       // bool IsAddingControl = false;
        
        public void SetConfig(IDMEEditor pbl, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs e, IErrorsInfo per)
        {
            DMEEditor = pbl;
            ErrorObject = pbl.ErrorObject;
            Logger = pbl.Logger;
            Passedarg = e;
            if (e.Objects.Where(c => c.Name == "VISUTIL").Any())
            {
                visManager = (VisManager)e.Objects.Where(c => c.Name == "VISUTIL").FirstOrDefault().obj;
            }
            Logger.Onevent += Logger_Onevent;
            if (Passedarg.ParameterInt1 > 0)
            {
                if(Passedarg.ParameterInt1 > 1)
                {
                    IsDevModeOn=true;
                }
                
               
            }
            DevModeOn();
            toolbar = (ToolbarControl)visManager.ToolStrip;
             menu = (MenuControl)visManager.MenuStrip;
             Datatree = (TreeControl)visManager.Tree;
             Apptree = (TreeControl)visManager.SecondaryTree;

            if (Passedarg.Objects.Where(i => i.Name == "TreeControl").Any())
            {
                Passedarg.Objects.Remove(Passedarg.Objects.Where(i => i.Name == "TreeControl").FirstOrDefault());
            }
            if (Passedarg.Objects.Where(i => i.Name == "ControlManager").Any())
            {
                Passedarg.Objects.Remove(Passedarg.Objects.Where(i => i.Name == "ControlManager").FirstOrDefault());
            }
            if (Passedarg.Objects.Where(i => i.Name == "MenuControl").Any())
            {
                Passedarg.Objects.Remove(Passedarg.Objects.Where(i => i.Name == "MenuControl").FirstOrDefault());
            }
            if (Passedarg.Objects.Where(i => i.Name == "ToolbarControl").Any())
            {
                Passedarg.Objects.Remove(Passedarg.Objects.Where(i => i.Name == "ToolbarControl").FirstOrDefault());
            }
            Passedarg.Objects.Add(new ObjectItem() { Name = "TreeControl", obj = Datatree });
            Passedarg.Objects.Add(new ObjectItem() { Name = "AppTreeControl", obj = Apptree });
            Passedarg.Objects.Add(new ObjectItem() { Name = "ControlManager", obj = visManager._controlManager });
            Passedarg.Objects.Add(new ObjectItem() { Name = "MenuControl", obj = menu });
            Passedarg.Objects.Add(new ObjectItem() { Name = "ToolbarControl", obj = toolbar });
            if (!visManager.WaitFormShown)
            {
                visManager.ShowWaitForm((PassedArgs)Passedarg);
            }
            DMEEditor.Passedarguments = Passedarg;
            visManager.Container = ContainerPanel;

            Datatree.TreeType = "Beep";
            Datatree.TreeV = DatatreeView;

            Apptree.TreeType = "dhub";
            Apptree.TreeV = PlugintreeView;

            toolbar.TreeV = Datatree.TreeV;
            menu.TreeV = Datatree.TreeV;

            toolbar.TreeV = Apptree.TreeV;
            menu.TreeV = Apptree.TreeV;

            menu.vismanager = visManager;
            toolbar.vismanager = visManager;
            Passedarg.ParameterString1 = "Loading Beep Data Management Functions and Tree";
            visManager.PasstoWaitForm((PassedArgs)Passedarg);
            Datatree.CreateRootTree();
          //  Apptree.IconSize = new Size(24, 24);
            Passedarg.ParameterString1 = "Loading DHUB Functions and Tree";
            visManager.PasstoWaitForm((PassedArgs)Passedarg);
            Apptree.CreateRootTree();

            menu.menustrip = MainMenuStrip;
            Passedarg.ParameterString1 = "Loading Function Extensions and Menu ";
            visManager.PasstoWaitForm((PassedArgs)Passedarg);
            menu.CreateGlobalMenu();
            toolbar.toolbarstrip = MaintoolStrip1;
            Passedarg.ParameterString1 = "Loading Toobar Functions ";
            visManager.PasstoWaitForm((PassedArgs)Passedarg);
            toolbar.CreateToolbar();
            this.Shown += Frm_Main_Shown;
            startLoggin = true;
            visManager.CloseWaitForm();
            this.Filterbutton.Click += Filterbutton_Click;
            this.SidePanelCollapsebutton.Click += SidePanelCollapsebutton_Click;
            this.MinMaxButton.Click += MinMaxButton_Click;
            this.LogPanelCollapsebutton.Click += LogPanelCollapsebutton_Click;
            LogPanelHeight = LogPanel.Height;
          
            MainSplitContainer1.Panel1MinSize = 40;

            ListSearch = (Bitmap)Properties.Resources.ResourceManager.GetObject("ListBoxSearch");
            CollapseLeft = (Bitmap)Properties.Resources.ResourceManager.GetObject("CollapseLeft");
            Collapseright = (Bitmap)Properties.Resources.ResourceManager.GetObject("Collapseright");
            CollapseUp = (Bitmap)Properties.Resources.ResourceManager.GetObject("CollapseUp");
            CollapseDown = (Bitmap)Properties.Resources.ResourceManager.GetObject("CollapseDown");

            this.SidePanelCollapsebutton.Image = CollapseDown;
            this.MinMaxButton.Image= CollapseLeft;
            this.LogPanelCollapsebutton.Image = CollapseDown;
            Filterbutton.Image = ListSearch;
           
        }
        public void DevModeOn()
        {

            if (!IsDevModeOn)
            {
                LogPanelCollapsebutton.Visible = false;
                MainViewsplitContainer.Panel2Collapsed = true;
                SidePanelCollapsebutton.Visible = false;
                SidePanelContainer.Panel2Collapsed = true;
            }
            else
            {
                LogPanelCollapsebutton.Visible = true;
                MainViewsplitContainer.Panel2Collapsed = false;
                SidePanelCollapsebutton.Visible = true;
                SidePanelContainer.Panel2Collapsed = false;
            }
        }
        private void LogPanelCollapsebutton_Click(object sender, EventArgs e)
        {
            //IsLogPanelOpen
            if (IsLogPanelOpen)
            {
                MainViewsplitContainer.Panel2Collapsed = true;
                LogPanelCollapsebutton.Image = CollapseUp;
                IsLogPanelOpen = false;
            }
            else
            {
                LogPanelCollapsebutton.Image = CollapseDown;
                MainViewsplitContainer.Panel2Collapsed = false;
                IsLogPanelOpen = true;
            }
        }
        private void SidePanelCollapsebutton_Click(object sender, EventArgs e)
        {
            //IsSidePanelsOpen
            if (IsSidePanelsOpen)
            {
                SidePanelContainer.Panel2Collapsed = true;
                SidePanelCollapsebutton.Image = CollapseUp;
                IsSidePanelsOpen = false;
            }
            else
            {
                SidePanelContainer.Panel2Collapsed = false;
                SidePanelCollapsebutton.Image = CollapseDown;
                IsSidePanelsOpen = true;
            }
        }
        private void MinMaxButton_Click(object sender, EventArgs e)
        {
            if (IsTreeSideOpen)
            {
                MainSplitContainer1.Panel1Collapsed = true;
                MinMaxButton.Image = Collapseright;
                IsTreeSideOpen =false;
            }
            else
            {
                MainSplitContainer1.Panel1Collapsed = false;
                MinMaxButton.Image = CollapseLeft;
                IsTreeSideOpen = true;
            }
        }
        private void Filterbutton_Click(object sender, EventArgs e)
        {
            Apptree.Filterstring=TreeFilterTextBox.Text;
        }
        private void Frm_Main_Shown(object sender, EventArgs e)
        {
           // this.WindowState = FormWindowState.Maximized;
            this.Text = "Beep - The Plugable Integrated Platform";
        }
        private void Logger_Onevent(object sender, string e)
        {
            if (startLoggin)
            {
                    this.LogPanel.BeginInvoke(new Action(() => {
                    this.LogPanel.AppendText(e + Environment.NewLine);
                    LogPanel.SelectionStart = LogPanel.Text.Length;
                    LogPanel.ScrollToCaret();
                }));
            }

        }
     
    }
}
