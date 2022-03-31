﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheTechIdea.Beep;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Logger;
using TheTechIdea.Util;
using TheTechIdea.Beep.Vis;
using BeepEnterprize.Vis.Module;

using TheTechIdea;

namespace BeepEnterprize.Winform.Vis
{
    [AddinAttribute(Caption = "WebApi Configuration", Name = "uc_webapiHeaders", misc = "Config", menu = "Configuration", addinType = AddinType.Control, displayType = DisplayType.Popup)]
    public partial class uc_webapiHeaders : UserControl,IDM_Addin

    {
        public uc_webapiHeaders()
        {
            InitializeComponent();
            
        }

        public string ParentName { get; set; }
        public string AddinName { get; set; } = "WebApi Headers";
        public string Description { get; set; } = "WebApi Headers";
        public string ObjectName { get; set; }
        public string ObjectType { get; set; } = "UserControl";
        public Boolean DefaultCreate { get; set; } = true;
        public string DllPath { get ; set ; }
        public string DllName { get ; set ; }
        public string NameSpace { get ; set ; }
        public DataSet Dset { get ; set ; }
        public IErrorsInfo ErrorObject { get ; set ; }
        public IDMLogger Logger { get ; set ; }
        public IDMEEditor DMEEditor { get ; set ; }
        public EntityStructure EntityStructure { get ; set ; }
        public string EntityName { get ; set ; }
        public IPassedArgs Passedarg { get ; set ; }

       // public event EventHandler<PassedArgs> OnObjectSelected;
        public IVisManager Visutil { get; set; }
        public void RaiseObjectSelected()
        {
            throw new NotImplementedException();
        }

        public void Run(IPassedArgs pPassedarg)
        {
            throw new NotImplementedException();
        }

        public void SetConfig(IDMEEditor pbl, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs e, IErrorsInfo per)
        {
            Passedarg = e;
            Logger = plogger;
            ErrorObject = per;
            DMEEditor = pbl;
            Visutil = (IVisManager)e.Objects.Where(c => c.Name == "VISUTIL").FirstOrDefault().obj;
            EntityName = e.DatasourceName;
            //this.headersBindingNavigatorSaveItem.Click += HeadersBindingNavigatorSaveItem_Click;
            this.headersBindingSource.AddingNew += HeadersBindingSource_AddingNew;
            this.headersBindingSource.DataSource = DMEEditor.ConfigEditor.DataConnections[DMEEditor.ConfigEditor.DataConnections.FindIndex(x => x.ConnectionName == EntityName)].Headers;
            uc_bindingNavigator1.bindingSource = headersBindingSource;
            uc_bindingNavigator1.SaveCalled += Uc_bindingNavigator1_SaveCalled;
            uc_bindingNavigator1.SetConfig(DMEEditor, DMEEditor.Logger, DMEEditor.Utilfunction, new string[] { }, e, DMEEditor.ErrorObject);
            uc_bindingNavigator1.HightlightColor = Color.Yellow;

        }

        private void Uc_bindingNavigator1_SaveCalled(object sender, BindingSource e)
        {
            try

            {
                DMEEditor.ConfigEditor.DataConnections[DMEEditor.ConfigEditor.DataConnections.FindIndex(x => x.ConnectionName == EntityName)].Headers = (List<WebApiHeader>)this.headersBindingSource.List;
                DMEEditor.ConfigEditor.SaveDataconnectionsValues();
                MessageBox.Show("Saved Successfully");

            }
            catch (Exception ex)
            {
                string errmsg = "Error in saving headesrs";
                DMEEditor.AddLogMessage("Fail", $"{errmsg}:{ex.Message}", DateTime.Now, 0, null, Errors.Failed);
                MessageBox.Show($"{errmsg}:{ex.Message}");
            }
        }

        private void HeadersBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
           
            WebApiHeader apiHeader = new WebApiHeader();
           
            e.NewObject = apiHeader;
        }

    }
}
