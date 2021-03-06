using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheTechIdea.Beep;
using TheTechIdea.Beep.AppBuilder;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.DataView;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Logger;
using TheTechIdea.Util;
using TheTechIdea.Winforms.VIS;

namespace TheTechIdea.ETL
{
    public partial class uc_ViewEditor : UserControl,IDM_Addin
    {
        public uc_ViewEditor()
        {
            InitializeComponent();
        }

        public string ParentName { get; set; }
        public string AddinName { get; set; } = "View Editor";
        public string Description { get; set; } = "View Editor";
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
       
        public IPassedArgs Passedarg { get ; set ; }
      //  private IDMDataView MyDataView;
        public IVisUtil Visutil { get; set; }
        string IDM_Addin.EntityName { get ; set ; }

        DataViewDataSource ds;
        IBranch RootAppBranch;
        IBranch branch;
        EntityStructure entity;
      //  App app;
       // public event EventHandler<PassedArgs> OnObjectSelected;

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
            Visutil = (IVisUtil)e.Objects.Where(c => c.Name == "VISUTIL").FirstOrDefault().obj;

            branch = (IBranch)e.Objects.Where(c => c.Name == "Branch").FirstOrDefault().obj;
            RootAppBranch = (IBranch)e.Objects.Where(c => c.Name == "RootAppBranch").FirstOrDefault().obj;
           
            dataViewDataSourceBindingSource.AddingNew += DataViewDataSourceBindingSource_AddingNew;
           
            entitiesBindingSource.AddingNew += EntitiesBindingSource_AddingNew;
            dataViewDataSourceBindingNavigatorSaveItem.Click += DataViewDataSourceBindingNavigatorSaveItem_Click;
            foreach (var item in Enum.GetValues(typeof(ViewType)))
            {
                this.ViewtypeComboBox.Items.Add(item);

            }
            
                ds = (DataViewDataSource)DMEEditor.GetDataSource(e.CurrentEntity);
                ds.LoadView();
                dataViewDataSourceBindingSource.DataSource = ds.DataView;
                this.viewNameTextBox.Enabled = false;
            this.dataSourcesBindingSource.DataSource = DMEEditor.DataSources;
            this.dataConnectionsBindingSource.DataSource = DMEEditor.ConfigEditor.DataConnections;
            entitiesBindingSource.DataSource = dataViewDataSourceBindingSource;
            this.entitiesDataGridView.DataError += EntitiesDataGridView_DataError;
            this.ChangeDatasourceButton.Click += ChangeDatasourceButton_Click;
        }

        private void ChangeDatasourceButton_Click(object sender, EventArgs e)
        {
            entitiesBindingSource.MoveFirst();
            for (int i = 0; i <= entitiesBindingSource.Count; i++)
            {
                entity = (EntityStructure)entitiesBindingSource.Current;
                entity.DataSourceID = this.NewDatasourcecomboBox1.Text;
                entitiesBindingSource.MoveNext();


            }
            this.entitiesDataGridView.Refresh();
        }

        private void EntitiesDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
        }

        private void DataViewDataSourceBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try

            {
                this.entitiesBindingSource.EndEdit();
                this.dataViewDataSourceBindingSource.EndEdit();
                DMEEditor.ConfigEditor.SaveDataconnectionsValues();
                ds.DataView = (DMDataView)dataViewDataSourceBindingSource.Current;
                ds.WriteDataViewFile(ds.DataView.DataViewDataSourceID);
                DMEEditor.AddLogMessage("Success", $"Saving View Data", DateTime.Now, 0, null, Errors.Ok);
               
            }
            catch (Exception ex)
            {
                string errmsg = "Error in Saving View Data";
                DMEEditor.AddLogMessage("Fail", $"{errmsg}:{ex.Message}", DateTime.Now, 0, null, Errors.Failed);
                
            }
           
            
        }

        private void EntitiesBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            EntityStructure en = new EntityStructure();
            DMDataView dv =(DMDataView) dataViewDataSourceBindingSource.Current;
            en.ViewID = dv.ViewID;
            e.NewObject = en;
        }

        private void DataViewDataSourceBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            DMDataView dv = new DMDataView();
            dv.VID = Guid.NewGuid().ToString();
            e.NewObject = dv;

        }
    }
}
