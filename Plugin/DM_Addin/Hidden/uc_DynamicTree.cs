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
using TheTechIdea.Beep.Vis;
using TheTechIdea.Logger;
using TheTechIdea.Util;
using TheTechIdea.Winforms.VIS;

namespace TheTechIdea.Hidden
{
    public partial class uc_DynamicTree : UserControl,IDM_Addin
    {
        public uc_DynamicTree()
        {
            InitializeComponent();
        }

        public string AddinName { get; set; } = "Dynamic Data View Tree";
        public string Description { get; set; } = "Dynamic Data View Tree";
        public string ObjectName { get; set; }
        public string ObjectType { get; set; } = "UserControl";
        public string DllPath { get; set; }
        public string DllName { get; set; }
        public string NameSpace { get; set; }
        public string ParentName { get; set; }
        public Boolean DefaultCreate { get; set; } = false;
        public DataSet Dset { get; set; }
        public IErrorsInfo ErrorObject { get; set; }
        public IDMLogger Logger { get; set; }
        public IDMEEditor DMEEditor { get; set; }
        public EntityStructure EntityStructure { get; set; }
        public string EntityName { get; set; }
        public IPassedArgs Passedarg { get; set; }
        public IVisUtil Visutil { get; set; }
        public  ITree TreeEditor { get; set; }
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
            try
            {
                TreeEditor = Visutil.treeEditor;
                TreeEditor.TreeStrucure = treeView1;
            }
            catch (Exception  ex)
            {
                DMEEditor.AddLogMessage("Failed", $"{ex.Message}", DateTime.Now, 0, "", Errors.Failed);
            }
           
            TreeEditor.CreateRootTree();
            TreeEditor.CreateGlobalMenu();
        }

        private void DataConnectionbutton_Click(object sender, EventArgs e)
        {
            Visutil.ShowUserControlInContainer("uc_DataConnection", Visutil.DisplayPanel, DMEEditor, null, null);
        }

       
    }
}
