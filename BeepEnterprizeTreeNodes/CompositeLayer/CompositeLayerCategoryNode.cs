using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeepEnterprize.Winform.Vis;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.CompositeLayer;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Util;

namespace  BeepEnterprize.Vis.Module
{
    [AddinAttribute(Caption = "Composite Layer", Name = "CompositeLayerCategoryNode.Beep", misc = "Beep", iconimage = "clayerroot.ico", menu = "Beep", ObjectType = "Beep")]
    public class CompositeLayerCategoryNode : IBranch 
    {
        public CompositeLayerCategoryNode()
        {

        }
        public CompositeLayerCategoryNode(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, string pimagename)
        {
            TreeEditor = pTreeEditor;
            DMEEditor = pDMEEditor;
            ParentBranchID = pParentNode.ID;
           
            BranchText = pBranchText;
            BranchType = pBranchType;
            IconImageName = pimagename;
            if (pID != 0)
            {
                ID = pID;
                BranchID = ID;
            }
        }
        public IErrorsInfo SetConfig(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, string pimagename)
        {

            try
            {
                TreeEditor = pTreeEditor;
                DMEEditor = pDMEEditor;
                ParentBranchID = pParentNode.ID;
                BranchText = pBranchText;
                BranchType = pBranchType;
                IconImageName = pimagename;
                if (pID != 0)
                {
                    ID = pID;
                    BranchID = ID;
                }

                DMEEditor.AddLogMessage("Success", "Set Config OK", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Set Config";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;

        }
        public object ParentBranch { get; set; }
        public string ObjectType { get; set; } = "Beep";
        public string Name { get; set; }
        public EntityStructure EntityStructure { get; set; }
        public int ID { get; set; }
        public string BranchText { get; set; }
        public IDMEEditor DMEEditor { get; set; }
        public IDataSource DataSource { get; set; }
        public string DataSourceName { get; set; }
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Category;
        public int BranchID { get; set; }
        public string IconImageName { get; set; }
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; }
        public string BranchClass { get; set; } = "CLAYER";
        public List<IBranch> ChildBranchs { get; set; } = new List<IBranch>();
        public ITree TreeEditor { get; set; }
        public List<string> BranchActions { get; set; }
        public List<Delegate> Delegates { get; set; }
        public object TreeStrucure { get ; set ; }
        public IVisManager  Visutil { get ; set ; }
        public int MiscID { get; set; }
      
        CompositeLayer clayer = new CompositeLayer();
       // public event EventHandler<PassedArgs> BranchSelected;
       // public event EventHandler<PassedArgs> BranchDragEnter;
       // public event EventHandler<PassedArgs> BranchDragDrop;
       // public event EventHandler<PassedArgs> BranchDragLeave;
       // public event EventHandler<PassedArgs> BranchDragClick;
       // public event EventHandler<PassedArgs> BranchDragDoubleClick;
       // public event EventHandler<PassedArgs> ActionNeeded;

        public IErrorsInfo CreateChildNodes()
        {


            try
            {
                foreach (CategoryFolder p in DMEEditor.ConfigEditor.CategoryFolders.Where(x => x.RootName == "CLAYER" && x.FolderName == BranchText))
                {
                    foreach (string item in p.items)
                    {
                        CompositeLayer i = DMEEditor.ConfigEditor.CompositeQueryLayers.Where(x => x.LayerName == item).FirstOrDefault();

                        clayer = i;
                        CreateLayerNode(i.ID,  i.LayerName); //Path.Combine(i.FilePath, i.FileName)

                    }



                }

                //DMEEditor.AddLogMessage("Success", "Added Database Connection", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Add Database Connection";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }

        public IErrorsInfo CreateDelegateMenu()
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo ExecuteBranchAction(string ActionName)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo MenuItemClicked(string ActionNam)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo RemoveChildNodes()
        {
            throw new NotImplementedException();
        }
        #region "Exposed Interface"
        [CommandAttribute(Caption = "Remove")]
        public IErrorsInfo RemoveCategory()
        {

            try
            {
                TreeEditor.treeBranchHandler.RemoveCategoryBranch(BranchID);
            }
            catch (Exception ex)
            {
                string mes = "Could not Added View ";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
      
        #endregion Exposed Interface"
        #region "other"
        private IErrorsInfo CreateLayerNode(string id, string ConnectionName)
        {

            try
            {
                if (!ChildBranchs.Any(x => x.BranchText == ConnectionName))
                {
                    CompositeLayerNode layer = new CompositeLayerNode(TreeEditor, DMEEditor, this, ConnectionName, TreeEditor.SeqID, EnumPointType.DataPoint, clayer);
                    TreeEditor.treeBranchHandler.AddBranch(this, layer);
                    ChildBranchs.Add(layer);
                }

             //   DMEEditor.AddLogMessage("Success", "Added Database Connection", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Add Database Connection";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };

            return DMEEditor.ErrorObject;
        }
        #endregion
    }
}
