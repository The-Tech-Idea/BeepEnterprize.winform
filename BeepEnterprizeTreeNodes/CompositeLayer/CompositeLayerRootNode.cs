using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeepEnterprize.Winform.Vis;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.CompositeLayer;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Util;

namespace  BeepEnterprize.Vis.Module
{
    [AddinAttribute(Caption = "Composite Layer", Name = "CompositeLayerRootNode.Beep", misc = "Beep", iconimage = "clayerroot.ico", menu = "Beep", ObjectType = "Beep")]
    public class CompositeLayerRootNode : IBranch ,IOrder,IBranchRootCategory
    {

        public CompositeLayerRootNode()
        {

        }
        public CompositeLayerRootNode(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, string pimagename)
        {
            TreeEditor = pTreeEditor;
            DMEEditor = pDMEEditor;
            ParentBranchID = pParentNode.ID;
            BranchText = pBranchText;
            BranchType = pBranchType;
         //   IconImageName = pimagename;
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
              //  ParentBranchID = pParentNode.ID;
              //  BranchText = pBranchText;
              //  BranchType = pBranchType;
              ////  IconImageName = pimagename;
              //  if (pID != 0)
              //  {
              //      ID = pID;
              //      BranchID = ID;
              //  }

             //   DMEEditor.AddLogMessage("Success", "Set Config OK", DateTime.Now, 0, null, Errors.Ok);
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
        public string BranchText { get; set; } = "Composite Layer";
        public IDMEEditor DMEEditor { get ; set ; }
        public IDataSource DataSource { get ; set ; }
        public string DataSourceName { get; set; }
        public int Level { get; set; } = 1;
        public  EnumPointType BranchType { get; set; } = EnumPointType.Root;
        public int BranchID { get ; set ; }
        public string IconImageName { get ; set ; }= "clayerroot.ico";
        public string BranchStatus { get ; set ; }
        public int ParentBranchID { get ; set ; }
        public string BranchDescription { get ; set ; }
      
        public string BranchClass { get; set; } = "CLAYER";
        public List<IBranch> ChildBranchs { get ; set ; } = new List<IBranch>();
        public ITree TreeEditor { get ; set ; }
        public List<string> BranchActions { get ; set ; }
        public List<Delegate> Delegates { get ; set ; }
        public int ID { get ; set ; }
        public int Order { get; set; } = 3;
        public int MiscID { get; set; }
        public  IVisManager  Visutil  { get; set; }
        PassedArgs Passedarguments { get; set; } = new PassedArgs();
        public object TreeStrucure { get ; set ; }
        CompositeLayer clayer = new CompositeLayer();
       // public event EventHandler<PassedArgs> BranchSelected;
       // public event EventHandler<PassedArgs> BranchDragEnter;
       // public event EventHandler<PassedArgs> BranchDragDrop;
       // public event EventHandler<PassedArgs> BranchDragLeave;
       // public event EventHandler<PassedArgs> BranchDragClick;
       // public event EventHandler<PassedArgs> BranchDragDoubleClick;
       // public event EventHandler<PassedArgs> ActionNeeded;
       // public event EventHandler<PassedArgs> OnObjectSelected;

       
        public IErrorsInfo CreateChildNodes()
        {
            try
            {
                TreeEditor.treeBranchHandler.RemoveChildBranchs(this);
                foreach (CompositeLayer i in DMEEditor.ConfigEditor.CompositeQueryLayers)
                {
                    if (TreeEditor.treeBranchHandler.CheckifBranchExistinCategory(i.LayerName, "CLAYER") ==null)
                    {
                         clayer = i;
                        CreateLayerNode(i.ID, i.LayerName);
                     
                    }


                }
                foreach (CategoryFolder i in DMEEditor.ConfigEditor.CategoryFolders.Where(x => x.RootName == "CLAYER"))
                {

                    CreateCategoryNode(i);


                }

            //    DMEEditor.AddLogMessage("Success", "Added Database Connection", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Add Database Connection";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
            
        }

        private IErrorsInfo CreateLayerNode(string id, string ConnectionName)
        {

            try
            {

                if (!ChildBranchs.Any(x => x.BranchText == ConnectionName))
                {
                  
                     CompositeLayerNode layer = new CompositeLayerNode(TreeEditor, DMEEditor, this, ConnectionName, TreeEditor.SeqID, EnumPointType.DataPoint, clayer);
               
                    TreeEditor.treeBranchHandler.AddBranch(this, layer);
                    ChildBranchs.Add(layer);
                    layer.CreateChildNodes();
                }
                
               
            //    DMEEditor.AddLogMessage("Success", "Added Database Connection", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Add Database Connection";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
           
            return DMEEditor.ErrorObject;
        }
        public IErrorsInfo CreateCategoryNode(CategoryFolder p)
        {
            try
            {
                if (!ChildBranchs.Any(x => x.BranchText == p.FolderName))
                {
                    CompositeLayerCategoryNode Category = new CompositeLayerCategoryNode(TreeEditor, DMEEditor, this, p.FolderName, TreeEditor.SeqID, EnumPointType.Category, TreeEditor.CategoryIcon);
                    TreeEditor.treeBranchHandler.AddBranch(this, Category);
                    ChildBranchs.Add(Category);
                    Category.CreateChildNodes();
                }

                
               
            }
            catch (Exception ex)
            {
                DMEEditor.Logger.WriteLog($"Error Creating Category Node File Node ({ex.Message}) ");
                DMEEditor.ErrorObject.Flag = Errors.Failed;
                DMEEditor.ErrorObject.Ex = ex;
            }
            return DMEEditor.ErrorObject;

        }
        public IErrorsInfo ExecuteBranchAction(string ActionName)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo MenuItemClicked(string ActionNam)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo OnBranchSelected(PassedArgs ActionDef)
        {
            throw new NotImplementedException();
        }

        public IErrorsInfo RemoveChildNodes()
        {
            throw new NotImplementedException();
        }
        #region "Delegate for Database Root Node"
        [CommandAttribute(Caption = "Create Composite Layer Database", iconimage = "localdb.ico")]
        public IErrorsInfo CreateNewLocalDatabase()
        {

            try
            {
                string[] args = { "New Query Entity", null, null };
                List<ObjectItem> ob = new List<ObjectItem>(); ;
                ObjectItem it = new ObjectItem();
                it.obj = this;
                it.Name = "Branch";
                ob.Add(it);
                IBranch RootCompositeLayerBranch = TreeEditor.Branches[TreeEditor.Branches.FindIndex(x => x.BranchClass == "CLAYER" && x.BranchType == EnumPointType.Root)];
                it = new ObjectItem();
                it.obj = RootCompositeLayerBranch;
                it.Name = "RootCompositeLayerBranch";
                ob.Add(it);
                PassedArgs Passedarguments = new PassedArgs
                {
                    Addin = null,
                    AddinName = null,
                    AddinType = "",
                    DMView = null,
                    CurrentEntity = null,
                    Id = 0,
                    ObjectType = "COMPOSITELAYER",
                    DataSource = null,
                    ObjectName = null,

                    Objects = ob,

                    DatasourceName = null,
                    EventType = "NEW"

                };
                // ActionNeeded?.Invoke(this, Passedarguments);
                Visutil.ShowPage("uc_ComposedLayer", Passedarguments);

           
                DMEEditor.AddLogMessage("Success", "create Composite Layer", DateTime.Now, 0, null, Errors.Failed);
            }
            catch (Exception ex)
            {
                string mes = "Could not create Composite layer";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }

        [CommandAttribute(Caption = "New/Edit Data Source", iconimage = "addconnection.ico")]
        public  IErrorsInfo AddDataBaseConnection()
        {

            try
            {
                string[] args = { "New DataSource", "", null };
                Visutil.ShowPage("uc_DataConnection",  null);

             //   DMEEditor.AddLogMessage("Success", "Added Database Connection", DateTime.Now, 0,null, Errors.Failed);
            }
            catch (Exception ex)
            {
                string mes = "Could not Add Database Connection";
                DMEEditor.AddLogMessage(ex.Message,  mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
        [CommandAttribute(Caption = "Add Category", iconimage = "category.ico")]
        public  IErrorsInfo AddCategory()
        {

            try
            {
                TreeEditor.treeBranchHandler.AddCategory(this);
            }
            catch (Exception ex)
            {
                string mes = "Could not Add Category";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
        //[BranchDelegate(Caption = "Add Layer",Hidden =true)]
        //public IErrorsInfo AddLayer()
        //{

        //    try
        //    {
        //        Passedarguments = new PassedArgs();
        //        string foldername = "";
        //        Visutil.Controlmanager.InputBox("Enter Category Name", "What Category you want to Add", ref foldername);
        //        if (foldername != null)
        //        {
        //            if (foldername.Length > 0)
        //            {
        //                CategoryFolder x = DMEEditor.ConfigEditor.AddFolderCategory(foldername, "CLAYER", BranchText);
        //                CreateCategoryNode(x);
        //                DMEEditor.ConfigEditor.SaveCategoryFoldersValues();

        //            }
        //        }
        //        DMEEditor.AddLogMessage("Success", "Added Category", DateTime.Now, 0, null, Errors.Failed);
        //    }
        //    catch (Exception ex)
        //    {
        //        string mes = "Could not Add Category";
        //        DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
        //    };
        //    return DMEEditor.ErrorObject;
        //}
        #endregion
    }
}
