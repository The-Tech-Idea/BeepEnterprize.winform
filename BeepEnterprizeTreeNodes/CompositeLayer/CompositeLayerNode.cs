using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BeepEnterprize.Winform.Vis;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.CompositeLayer;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Util;

namespace  BeepEnterprize.Vis.Module
{
    [AddinAttribute(Caption = "Composite Layer", Name = "CompositeLayer.Beep", misc = "Beep", iconimage = "clayerroot.ico", menu = "Beep", ObjectType = "Beep")]
    public class CompositeLayerNode : IBranch 
    {
        public CompositeLayerNode()
        {

        }
        public CompositeLayerNode(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, CompositeLayer pClayer)
        {
            TreeEditor = pTreeEditor;
            DMEEditor = pDMEEditor;
            ParentBranchID = pParentNode.ID;
            BranchText = pBranchText;
            DataSourceName = pBranchText;
            BranchType = pBranchType;
            CLayer = pClayer;
         
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
             //   IconImageName = pimagename;
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
        public string BranchText { get; set; }
        public IDMEEditor DMEEditor { get; set; }
        public IDataSource DataSource { get; set; }
        public string DataSourceName { get; set; }
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.DataPoint;
        public int BranchID { get; set; }
        public string IconImageName { get; set; } = "clayer.ico";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; }
        public string BranchClass { get; set; } = "CLAYER";
        public List<IBranch> ChildBranchs { get; set; } = new List<IBranch>();
        public ITree TreeEditor { get; set; }
        public List<string> BranchActions { get; set; }
        public List<Delegate> Delegates { get; set; }
        public int ID { get; set; }
        public object TreeStrucure { get ; set ; }
        public IVisManager  Visutil { get ; set ; }
       
        public int MiscID { get; set; }
        public CompositeLayerDataSource compositeLayerDataSource { get; set; }
        CompositeLayer CLayer = new CompositeLayer();
        CancellationTokenSource tokenSource;
        CancellationToken token;
        public IErrorsInfo CreateChildNodes()
        {
           return GetEntites();
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
        private void CheckCreatedentities()
        {
            List<EntityStructure> ls = new List<EntityStructure>();
            ls = compositeLayerDataSource.LayerInfo.Entities.Where(x => x.Created == false).ToList();
            foreach (EntityStructure item in ls)
            {
                compositeLayerDataSource.LayerInfo.Entities[compositeLayerDataSource.LayerInfo.Entities.FindIndex(x => x.EntityName.ToLower() == item.EntityName.ToLower())].Created = compositeLayerDataSource.CheckEntityExist(item.EntityName);

                if (compositeLayerDataSource.LayerInfo.Entities[compositeLayerDataSource.LayerInfo.Entities.FindIndex(x => x.EntityName.ToLower() == item.EntityName.ToLower())].Created)
                {
                    compositeLayerDataSource.LayerInfo.Entities[compositeLayerDataSource.LayerInfo.Entities.FindIndex(x => x.EntityName.ToLower() == item.EntityName.ToLower())].DataSourceID = compositeLayerDataSource.DatasourceName;
                }

            }
            DMEEditor.ConfigEditor.SaveCompositeLayersValues();
        } 
        #region "Exposed Methods"
        [CommandAttribute(Caption = "Get Entities",iconimage = "get_childs.ico")]
        public IErrorsInfo GetEntites()
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;
            string iconimage;
            try
            {
                OpenCompositeDataSource();
                if (compositeLayerDataSource != null)
                {
                    if (compositeLayerDataSource.ConnectionStatus == ConnectionState.Open)
                    {
                        CheckCreatedentities();
                        
                        TreeEditor.treeBranchHandler.RemoveChildBranchs(this);
                    
                        foreach (EntityStructure tb in compositeLayerDataSource.LayerInfo.Entities)
                        {
                            if (tb.Created == false)
                            {
                                iconimage = "entitynotcreated.ico";
                            }
                            else
                            {
                                iconimage = "databaseentities.ico";
                            }
                            CompositeLayerEntitesNode dbent = new CompositeLayerEntitesNode(TreeEditor, DMEEditor, this, tb, TreeEditor.SeqID, EnumPointType.Entity, iconimage, compositeLayerDataSource);
                            TreeEditor.treeBranchHandler.AddBranch(this, dbent);
                            dbent.DataSourceName = compositeLayerDataSource.DatasourceName;
                            dbent.compositeLayerDataSource = compositeLayerDataSource;
                            dbent.DataSource = compositeLayerDataSource;
                            ChildBranchs.Add(dbent);
                         
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DMEEditor.Logger.WriteLog($"Error in Filling Database Entites ({ex.Message}) ");
                DMEEditor.ErrorObject.Flag = Errors.Failed;
                DMEEditor.ErrorObject.Ex = ex;
            }
            return DMEEditor.ErrorObject;

        }
        [CommandAttribute(Caption = "Sync. Layer w. View",iconimage = "createentity.ico")]
        public IErrorsInfo SyncMissingEntites()
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;
            try
            {
                OpenCompositeDataSource();
                if (compositeLayerDataSource != null)
                {
                    if (compositeLayerDataSource.ConnectionStatus == System.Data.ConnectionState.Open)
                    {
                       
                        compositeLayerDataSource.GetAllEntitiesFromDataView();
                        DMEEditor.ErrorObject = GetEntites();
                    }
                }
            }
            catch (Exception ex)
            {
                DMEEditor.Logger.WriteLog($"Error in Filling Database Entites ({ex.Message}) ");
                DMEEditor.ErrorObject.Flag = Errors.Failed;
                DMEEditor.ErrorObject.Ex = ex;
            }
            return DMEEditor.ErrorObject;

        }
        [CommandAttribute(Caption = "Create Entites",iconimage = "createnewentities.ico")]
        public IErrorsInfo CreateMissingEntites()
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;
            DMEEditor.Logger.WriteLog($"Filling Database Entites ) ");
            try
            {
                OpenCompositeDataSource();
                if (compositeLayerDataSource != null)
                {

                    if (compositeLayerDataSource.ConnectionStatus == System.Data.ConnectionState.Open)
                    {
                        CheckCreatedentities();

                            string[] args = { "New Query Entity", null, null };
                            List<ObjectItem> ob = new List<ObjectItem>(); ;
                            ObjectItem it = new ObjectItem();
                            it.obj = this;
                            it.Name = "Branch";
                            ob.Add(it);
                            IBranch RootBranch = TreeEditor.Branches[TreeEditor.Branches.FindIndex(x => x.BranchClass == "CLAYER" && x.BranchType == EnumPointType.Root)];
                            it = new ObjectItem();
                            it.obj = RootBranch;
                            it.Name = "RootBranch";
                            ob.Add(it);
                            it = new ObjectItem();
                            it.obj = DMEEditor;
                            it.Name = "DMEEDITOR";
                            ob.Add(it);
                             it = new ObjectItem();
                             it.obj = compositeLayerDataSource;
                             it.Name = "Compositeds";
                             ob.Add(it);
                        PassedArgs Passedarguments = new PassedArgs
                            {
                                Addin = null,
                                AddinName = null,
                                AddinType = "",
                                DMView = null,
                                CurrentEntity = null,

                                ObjectType = "SCRIPT",
                                DataSource = compositeLayerDataSource,
                                ObjectName = compositeLayerDataSource.DatasourceName,

                                Objects = ob,

                                DatasourceName = null,
                                EventType = "RUNSCRIPT"

                            };
                        DMEEditor.ETL.Script= CreateScript();
                        Visutil.ShowPage("uc_CopyEntities",   Passedarguments);
                    }
                }
            }
            catch (Exception ex)
            {
                DMEEditor.Logger.WriteLog($"Error in Filling Database Entites ({ex.Message}) ");
                DMEEditor.ErrorObject.Flag = Errors.Failed;
                DMEEditor.ErrorObject.Ex = ex;
            }
            return DMEEditor.ErrorObject;

        }
        //[CommandAttribute(Caption = "Paste Entities")]
        //public IErrorsInfo PasteEntities()
        //{

        //    try
        //    {
        //        OpenCompositeDataSource();
        //        if(compositeLayerDataSource.ConnectionStatus== ConnectionState.Open)
        //        {
        //            if (TreeEditor.args != null)
        //            {
        //                if (TreeEditor.args.EventType == "COPYENTITIES")
        //                {
        //                    if (TreeEditor.args.Objects != null)
        //                    {
        //                        foreach (var item in TreeEditor.args.EntitiesNames)
        //                        {
        //                            IDataSource ds = DMEEditor.GetDataSource(TreeEditor.args.DatasourceName);
        //                            IBranch pbr = (IBranch)TreeEditor.args.Objects.Where(x => x.Name == "ParentBranch").FirstOrDefault().obj;
        //                            EntityStructure entity = (EntityStructure)ds.GetEntityStructure(item, true); ;
        //                            if (compositeLayerDataSource.CheckEntityExist(entity.EntityName))
        //                            {
        //                                DMEEditor.AddLogMessage("Fail", $"Could Not Paste Entity {entity.EntityName}, it already exist", DateTime.Now, -1, null, Errors.Failed);
        //                            }
        //                            else
        //                            {
        //                                compositeLayerDataSource.ConnectionStatus = DMEEditor.OpenDataSource(compositeLayerDataSource.DatasourceName);
        //                                entity.Created = false;
        //                                compositeLayerDataSource.LayerInfo.Entities.Add(entity);
        //                                DMEEditor.ConfigEditor.SaveCompositeLayersValues();
        //                                GetEntites();
        //                                DMEEditor.AddLogMessage("Success", $"Pasted Entity {entity.EntityName}", DateTime.Now, -1, null, Errors.Ok);
        //                            }
        //                        }


        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string mes = "Could not Copy Entites";
        //        DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
        //    };
        //    return DMEEditor.ErrorObject;
        //}
        //[BranchDelegate(Caption = "Paste Entity")]
        //public IErrorsInfo PastEntity()
        //{

        //    try
        //    {
        //        // IBranch pbr = TreeEditor.Branches.Where(x => x.BranchType == EnumBranchType.Root && x.BranchClass == "VIEW").FirstOrDefault();
        //        //PassedArgs args = new PassedArgs
        //        //{
        //        //    ObjectName = "DATABASE",
        //        //    ObjectType = "TABLE",
        //        //    EventType = "COPYENTITY",
        //        //    Parameter = "COPYENTITY",
        //        //    DataSource = DataSource,
        //        //    DatasourceName = DataSource.DatasourceName,
        //        //    CurrentEntity = BranchText,
        //        //    Objects = new List<ObjectItem> { new ObjectItem { Name = "Branch", obj = this } }
        //        //};
        //        if (TreeEditor.args != null)
        //        {
        //            if (TreeEditor.args.EventType == "COPYENTITY")
        //            {
        //                if (TreeEditor.args.Objects != null)
        //                {
        //                    IBranch pbr = (IBranch)TreeEditor.args.Objects.Where(x => x.Name == "Branch").FirstOrDefault().obj;
        //                    EntityStructure entity = (EntityStructure)TreeEditor.args.Objects.Where(x => x.Name == "Entity").FirstOrDefault().obj;
        //                    if (compositeLayerDataSource.CheckEntityExist(entity.EntityName) || compositeLayerDataSource.LayerInfo.Entities.Where(i=>i.EntityName.ToLower()==entity.EntityName.ToLower()).Any())
        //                    {
        //                        DMEEditor.AddLogMessage("Fail" , $"Could Not Paste Entity {entity.EntityName}, it already exist", DateTime.Now, -1, null, Errors.Failed);
        //                    }
        //                    else
        //                    {
        //                        entity.Created = false;
        //                        compositeLayerDataSource.LayerInfo.Entities.Add(entity);
        //                        DMEEditor.ConfigEditor.SaveCompositeLayersValues();
        //                        GetEntites();
        //                        DMEEditor.AddLogMessage("Success", $"Pasted Entity {entity.EntityName}", DateTime.Now, -1, null, Errors.Ok);
        //                    }
                           
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string mes = "Could not Added Entity ";
        //        DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
        //    };
        //    return DMEEditor.ErrorObject;
        //}
        
        [CommandAttribute(Caption = "Remove Layer",iconimage ="remove.ico")]
        public IErrorsInfo RemoveLayer()
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;


            try
            {
                if (Visutil.Controlmanager.InputBoxYesNo("Remove Layer", "Area you Sure ? you want to remove Layer???") == BeepEnterprize.Vis.Module.DialogResult.Yes)
                {
                    ConnectionProperties cn = DMEEditor.ConfigEditor.DataConnections.Where(x => string.Equals(Path.GetFileName(BranchText), x.ConnectionName, StringComparison.OrdinalIgnoreCase) ).FirstOrDefault();
                    try
                    {
                        if (cn != null)
                        {
                            string file = Path.Combine(cn.FilePath, cn.FileName);
                            bool ok = false;
                            if (DMEEditor.ConfigEditor.CompositeQueryLayers.Any(p=> string.Equals(p.LayerName, cn.ConnectionName, StringComparison.OrdinalIgnoreCase) ))
                            {
                                 ok = DMEEditor.ConfigEditor.RemoveLayerByName(cn.ConnectionName);
                            }
                            if (DMEEditor.DataSources.Any(p => p.DatasourceName.ToLower() == cn.ConnectionName.ToLower()))
                            {
                                ok = DMEEditor.RemoveDataDource(cn.ConnectionName);
                            }
                            if (ok)
                                {
                                    if (Visutil.Controlmanager.InputBoxYesNo("Remove Layer", "Do you want to Delete the Layer File ???") == BeepEnterprize.Vis.Module.DialogResult.Yes)
                                    {

                                        if (compositeLayerDataSource.DropDatabase())
                                        {
                                            DMEEditor.ConfigEditor.SaveCompositeLayersValues();
                                            if (DMEEditor.ConfigEditor.DataConnections.Any(p => p.ConnectionName.ToLower() == cn.ConnectionName.ToLower()))
                                            {
                                                ok = DMEEditor.ConfigEditor.RemoveDataConnection(cn.ConnectionName);
                                                DMEEditor.ConfigEditor.SaveDataconnectionsValues();
                                            }
                                      
                                            TreeEditor.treeBranchHandler.RemoveBranch(this);
                                            IBranch v=TreeEditor.Branches.Where(p => p.BranchType == EnumPointType.DataPoint && p.BranchText.Equals(this.BranchText) && p.BranchClass == "RDBMS").FirstOrDefault();
                                            if (v != null)
                                            {
                                                TreeEditor.treeBranchHandler.RemoveBranch(v);
                                            }
                                            DMEEditor.AddLogMessage("Success", "Removed Data file", DateTime.Now, 0, null, Errors.Ok);
                                        }else
                                        {
                                            DMEEditor.AddLogMessage("Fail", "Couldnot Remove Data File", DateTime.Now, 0, null, Errors.Failed);
                                        }
                                    }
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        string mes = "Could not Remove View from Layer List";
                        DMEEditor.AddLogMessage("Fail",ex.Message+ mes, DateTime.Now, -1, mes, Errors.Failed);
                    };

                }
              
            }
            catch (Exception ex)
            {
                string mes = "Could not Remove Layer";
                DMEEditor.AddLogMessage("Fail",ex.Message+ mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;

        }
        //[CommandAttribute(Caption = "Defaults")]
        //public IErrorsInfo EditDefaults()
        //{
        //    DMEEditor.ErrorObject.Flag = Errors.Ok;
        //    //   DMEEditor.Logger.WriteLog($"Filling Database Entites ) ");
        //    try
        //    {
        //        string[] args = { "New Query Entity", null, null };
        //        List<ObjectItem> ob = new List<ObjectItem>(); ;
        //        ObjectItem it = new ObjectItem();
        //        it.obj = this;
        //        it.Name = "Branch";
        //        ob.Add(it);
        //        PassedArgs Passedarguments = new PassedArgs
        //        {
        //            Addin = null,
        //            AddinName = null,
        //            AddinType = "",
        //            DMView = null,
        //            CurrentEntity = BranchText,
        //            Id = 0,
        //            ObjectType = "DEFAULTS",
        //            DataSource = null,
        //            ObjectName = BranchText,

        //            Objects = ob,

        //            DatasourceName = BranchText,
        //            EventType = "DEFAULTS"

        //        };
             
        //        Visutil.ShowPage("uc_datasourceDefaults", Passedarguments);
        //    }
        //    catch (Exception ex)
        //    {
        //        DMEEditor.Logger.WriteLog($"Error in Filling Database Entites ({ex.Message}) ");
        //        DMEEditor.ErrorObject.Flag = Errors.Failed;
        //        DMEEditor.ErrorObject.Ex = ex;
        //    }
        //    return DMEEditor.ErrorObject;

        //}
        //[CommandAttribute(Caption = "Copy Defaults")]
        //public IErrorsInfo CopyDefaults()
        //{
        //    DMEEditor.ErrorObject.Flag = Errors.Ok;
        //    //  DMEEditor.Logger.WriteLog($"Filling Database Entites ) ");
        //    try
        //    {
        //        List<DefaultValue> defaults = DMEEditor.ConfigEditor.DataConnections[DMEEditor.ConfigEditor.DataConnections.FindIndex(i => i.ConnectionName == BranchText)].DatasourceDefaults;
        //        if (defaults != null)
        //        {
        //            string[] args = { "CopyDefaults", null, null };
        //            List<ObjectItem> ob = new List<ObjectItem>(); ;
        //            ObjectItem it = new ObjectItem();
        //            it.obj = defaults;
        //            it.Name = "Defaults";
        //            ob.Add(it);
        //            PassedArgs Passedarguments = new PassedArgs
        //            {
        //                Addin = null,
        //                AddinName = null,
        //                AddinType = "",
        //                DMView = null,
        //                CurrentEntity = BranchText,
        //                Id = 0,
        //                ObjectType = "COPYDEFAULTS",
        //                DataSource = null,
        //                ObjectName = BranchText,

        //                Objects = ob,

        //                DatasourceName = BranchText,
        //                EventType = "COPYDEFAULTS"

        //            };
        //            TreeEditor.args = Passedarguments;
        //        }
        //        else
        //        {
        //            string mes = "Could not get Defaults";
        //            DMEEditor.AddLogMessage("Failed", mes, DateTime.Now, -1, mes, Errors.Failed);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DMEEditor.Logger.WriteLog($"Error getting defaults ({ex.Message}) ");
        //        DMEEditor.ErrorObject.Flag = Errors.Failed;
        //        DMEEditor.ErrorObject.Ex = ex;
        //    }
        //    return DMEEditor.ErrorObject;

        //}
        //[CommandAttribute(Caption = "Paste Defaults")]
        //public IErrorsInfo PasteDefaults()
        //{
        //    DMEEditor.ErrorObject.Flag = Errors.Ok;
        //    //  DMEEditor.Logger.WriteLog($"Filling Database Entites ) ");
        //    try
        //    {
        //        if (TreeEditor.args != null)
        //        {
        //            if (TreeEditor.args.ObjectType == "COPYDEFAULTS")
        //            {
        //                  compositeLayerDataSource.ConnectionStatus= DMEEditor.OpenDataSource(compositeLayerDataSource.DatasourceName);
        //                List<DefaultValue> defaults = (List<DefaultValue>)TreeEditor.args.Objects[0].obj;
        //                DMEEditor.ConfigEditor.DataConnections[DMEEditor.ConfigEditor.DataConnections.FindIndex(i => i.ConnectionName == BranchText)].DatasourceDefaults = defaults;
        //                DMEEditor.ConfigEditor.SaveDataconnectionsValues();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DMEEditor.Logger.WriteLog($"Error in  pasting Defaults ({ex.Message}) ");
        //        DMEEditor.ErrorObject.Flag = Errors.Failed;
        //        DMEEditor.ErrorObject.Ex = ex;
        //    }
        //    return DMEEditor.ErrorObject;

        //}
      
        #endregion "Exposed Methods"
        #region "Util"
        private ConnectionState OpenCompositeDataSource()
        {
            ConnectionProperties cn = DMEEditor.ConfigEditor.DataConnections.Where(x => x.ConnectionName == CLayer.DataSourceName).FirstOrDefault();
            if (cn != null)
            {
                if (compositeLayerDataSource == null)
                {
                    ConnectionDriversConfig driversConfig = DMEEditor.Utilfunction.LinkConnection2Drivers(cn);
                    compositeLayerDataSource = new CompositeLayerDataSource(cn.ConnectionName, DMEEditor.Logger, DMEEditor, cn.DatabaseType, DMEEditor.ErrorObject);
                }
                if (compositeLayerDataSource.ConnectionStatus != ConnectionState.Open)
                {
                    if (cn != null)
                    {
                        //
                        try
                        {

                            IDataSource localdb = (IDataSource)DMEEditor.GetDataSource(cn.ConnectionName);

                            compositeLayerDataSource.Dataconnection = localdb.Dataconnection;
                            compositeLayerDataSource.LocalDB = (ILocalDB)localdb;
                            compositeLayerDataSource.Openconnection();
                            return compositeLayerDataSource.ConnectionStatus;
                        }
                        catch (Exception ex)
                        {
                            DMEEditor.Logger.WriteLog($"Error in  Open Composite Database ({ex.Message}) ");
                            DMEEditor.ErrorObject.Flag = Errors.Failed;
                            DMEEditor.ErrorObject.Ex = ex;
                            return compositeLayerDataSource.ConnectionStatus;
                        }
                    }
                    else
                    {
                        DMEEditor.Logger.WriteLog($"Error: Could not Find Local Database for Composite Layer");
                        DMEEditor.ErrorObject.Flag = Errors.Failed;
                    }
                }
                return compositeLayerDataSource.ConnectionStatus;
            }
            else
                return ConnectionState.Broken;
           
        }
        private ETLScriptHDR CreateScript()
        {
            if (OpenCompositeDataSource() == ConnectionState.Open)
            {
                List<EntityStructure> ls = new List<EntityStructure>();
              //  TreeEditor.ShowWaiting();
               // TreeEditor.ChangeWaitingCaption($"Generating Scripts for  Entities Total:{compositeLayerDataSource.LayerInfo.Entities.Count}");
                ls = compositeLayerDataSource.LayerInfo.Entities.Where(x => x.Created == false).ToList();
                int i = 0;
                var progress = new Progress<PassedArgs>(percent =>
                {
                  
                    update();
                });
                if (ls.Count > 0)
                {
                    tokenSource = new CancellationTokenSource();
                    token = tokenSource.Token;
                    DMEEditor.ETL.Script = new ETLScriptHDR();
                    DMEEditor.ETL.Script.scriptSource = compositeLayerDataSource.DatasourceName;
                    DMEEditor.ETL.GetCreateEntityScript(compositeLayerDataSource, ls, progress,token);
                    foreach (var item in ls)
                    {
                     //   TreeEditor.AddCommentsWaiting($"{i} - Creating script for Entity {item.EntityName} ");
                        ETLScriptDet upscript = new ETLScriptDet();
                        upscript.sourcedatasourcename = item.DataSourceID;
                        upscript.sourceentityname = item.EntityName;
                        upscript.sourceDatasourceEntityName = item.DatasourceEntityName;

                        upscript.destinationDatasourceEntityName= item.EntityName;
                        upscript.destinationentityname = item.EntityName;
                        upscript.destinationdatasourcename = compositeLayerDataSource.DatasourceName;
                        upscript.scriptType = DDLScriptType.CopyData;
                        DMEEditor.ETL.Script.ScriptDTL.Add(upscript);
                        i += 1;
                    }
                }
               // TreeEditor.HideWaiting();
                return DMEEditor.ETL.Script;
            }
            else
                return null;
           
            
        }
        private void update()
        {

        }
        #endregion
    }
}
