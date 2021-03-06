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
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Logger;
using TheTechIdea.Util;
using TheTechIdea.Winforms.VIS;

namespace TheTechIdea.ETL
{
    public partial class uc_copydatasource : UserControl,IDM_Addin
    {
        public uc_copydatasource()
        {
            InitializeComponent();
        }

        public string AddinName { get; set; } = "Entity to Entity Link";
        public string Description { get; set; } = "Entity to Entity Link";
        public string ObjectName { get; set; }
        public string ObjectType { get; set; } = "UserControl";
      
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
        public string ParentName { get; set ; }
        public IVisUtil Visutil { get; set; }
        IBranch branch = null;
        IBranch Parentbranch = null;
        IDataSource Srcds;
        IDataSource destds;
        SyncDataSource scripHeader;
        public void Run(IPassedArgs pPassedarg)
        {
            throw new NotImplementedException();
        }

        public void SetConfig(IDMEEditor pbl, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs e, IErrorsInfo per)
        {
            Passedarg = e;
       
            DMEEditor = pbl;
          
            Visutil = (IVisUtil)e.Objects.Where(c => c.Name == "VISUTIL").FirstOrDefault().obj;
            branch = (IBranch)e.Objects.Where(c => c.Name == "Branch").FirstOrDefault().obj;
            if (e.Objects.Where(c => c.Name == "ParentBranch").Any())
            {
                Parentbranch = (IBranch)e.Objects.Where(c => c.Name == "ParentBranch").FirstOrDefault().obj;
            }
            scripHeader = new SyncDataSource();
            Srcds = DMEEditor.GetDataSource(branch.BranchText);
            if (Srcds != null)
            {
                Srcds.Openconnection();
                if (Srcds.ConnectionStatus== ConnectionState.Open)
                {

                }else
                {
                    MessageBox.Show($"Error Cannot Connect to Source  {branch.BranchText}");
                    DMEEditor.AddLogMessage("Fail", $"Error Cannot Connext to Source {branch.BranchText}", DateTime.Now, 0, null, Errors.Failed);
                }
            }
            else
            {
                DMEEditor.AddLogMessage("Fail", $"Error Cannot get Source {branch.BranchText}", DateTime.Now, 0, null, Errors.Failed);
                MessageBox.Show($"Error Cannot get Source  {branch.BranchText}");
            }
        }
        private void update()
        {

        }
        private void CreateScriptHeader()
        {
            int i=0;
            DMEEditor.ETL.script = new SyncDataSource();
            DMEEditor.ETL.script.scriptSource = Srcds.DatasourceName;
            List<EntityStructure> ls = new List<EntityStructure>();
            Srcds.GetEntitesList();
            foreach (string item in Srcds.EntitiesNames)
            {
                ls.Add(Srcds.GetEntityStructure(item, true));
            }
            var progress = new Progress<PassedArgs>(percent =>
            {
               
                update();
            });
           // DMEEditor.ETL.GetCreateEntityScript(Srcds, ls,progress,token);
            foreach (var item in ls)
            {
               
                SyncEntity upscript = new SyncEntity();
                upscript.sourcedatasourcename = item.DataSourceID;
                upscript.sourceentityname = item.EntityName;
                upscript.sourceDatasourceEntityName = item.DatasourceEntityName;

                upscript.destinationDatasourceEntityName = item.EntityName;
                upscript.destinationentityname = item.EntityName;
                upscript.destinationdatasourcename = Srcds.DatasourceName;
                upscript.scriptType = DDLScriptType.CopyData;
                DMEEditor.ETL.script.Entities.Add(upscript);
                i += 1;
            }
        }
    }
}
