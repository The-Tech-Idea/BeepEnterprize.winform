using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeepEnterprize.Vis.Module;
using BeepEnterprize.Winform.Vis.Controls.TreeControls;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Logger;
using TheTechIdea.Util;
using static TheTechIdea.Beep.Util;

namespace BeepEnterprize.Winform.Vis.Controls
{
    [AddinAttribute(Caption = "Beep", Name = "TreeControl", misc = "Control")]
    public class TreeControl : IDM_Addin,ITree
    {
        public TreeControl(IDMEEditor pDMEEditor,IVisManager pVismanager)
        {
            DMEEditor = pDMEEditor;
            Vismanager = (VisManager)pVismanager;
            VisManager = pVismanager;
         


        }
        #region "Addin Properties"
        public string ParentName { get; set; }
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public string AddinName { get; set; }
        public string Description { get; set; }
        public bool DefaultCreate { get; set; }
        public string DllPath { get; set; }
        public string DllName { get; set; }
        public string NameSpace { get; set; }
        public IErrorsInfo ErrorObject { get; set; }
        public IDMLogger Logger { get; set; }
        public IDMEEditor DMEEditor { get; set; }
        public EntityStructure EntityStructure { get; set; }
        public string EntityName { get; set; }
        public IPassedArgs Passedarg { get; set; }
        #endregion
        public string TreeType { get; set; }
        public VisManager Vismanager { get; set; }
        public IVisManager VisManager { get; set; }
        public System.Windows.Forms.TreeView TreeV { get; set; }
        public string CategoryIcon { get; set; } = "Category.ico";
        public string SelectIcon { get; set; } = "select.ico";
        public IBranch CurrentBranch { get ; set ; }
        public List<ContextMenu> menus { get; set; }=new List<ContextMenu>();
        public PassedArgs args { get; set; }
        static int pSeqID = 0;
        public int SeqID {
            get {
                pSeqID += 1;
                return pSeqID;
            }
        }
        public List<IBranch> Branches { get; set; } = new List<IBranch>();
        public int SelectedBranchID { get ; set ; }
        public string TreeEvent { get; set; }
        public  string TreeOP { get; set; }
        public TreeNode LastSelectedNode { get; set; }
        public TreeNode SelectedNode { get; set; }
        public int StartselectBranchID { get; set; }
        public Color SelectBackColor { get; set; }
        public List<MenuList> Menus { get; set; } = new List<MenuList>();
        public List<int> SelectedBranchs { get; set; } = new List<int>();
        public TreeNodeDragandDropHandler treeNodeDragandDropHandler { get ; set ; }
        public ITreeBranchHandler treeBranchHandler { get ; set ; }
        private Size _iconsize = new Size(20,20);
        private bool IsNewSizeSet=false;
        public Size IconSize { get { return _iconsize; } set { _iconsize = value; IsNewSizeSet = true;  } }
        public IErrorsInfo CreateFunctionExtensions(MethodsClass item)
        {
            ContextMenuStrip nodemenu = new ContextMenuStrip();
            try
            {
                nodemenu.ImageList = Vismanager.Images;
                ToolStripItem st = nodemenu.Items.Add(item.Caption);
                foreach  (IBranch br in Branches)
                {
                    if(br.BranchType== item.PointType)
                    {
                        nodemenu.Name = br.ToString();
                        if (item.iconimage != null)
                        {
                            st.ImageIndex = Vismanager.visHelper.GetImageIndex(item.iconimage);
                        }
                        nodemenu.ItemClicked += Nodemenu_ItemClicked;
                        nodemenu.Tag = br;
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = $"Could not add method from Extension {item.Name} to menu " ;
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;

        }
        private bool IsMenuCreated(IBranch br)
        {
            if (br.ObjectType != null)
            {
                return Menus.Where(p =>  p.ObjectType!=null && p.BranchClass.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase)
                && p.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase) 
                && p.PointType == br.BranchType).Any();
            }return
                false;
            
         
            
            
        }
        private MenuList GetMenuList(IBranch br)
        {
            return Menus.Where(p => p.ObjectType != null &&  p.BranchClass.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase)
                && p.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase)
                && p.PointType == br.BranchType).FirstOrDefault();
        }
        public T CreateInstance<T>(params object[] paramArray)
        {
            return (T)Activator.CreateInstance(typeof(T), args: paramArray);
        }
        public TreeNode CurrentNode { get; set; }
        public TreeNode ParentNode { get; set; }
        public IErrorsInfo CreateRootTree()
        {
           
            try
            {
                //bool HasConstructor=false;
                SetupTreeView();
                treeNodeDragandDropHandler = new TreeNodeDragandDropHandler(DMEEditor, this);
                treeBranchHandler = new TreeBranchHandler(DMEEditor, this);
                Branches = new List<IBranch>();
                foreach (AssemblyClassDefinition cls in DMEEditor.ConfigEditor.BranchesClasses.Where(p=>p.classProperties != null).OrderBy(x => x.Order))
                {
                    Type adc = DMEEditor.assemblyHandler.GetType(cls.PackageName);
                    ConstructorInfo ctor = adc.GetConstructors().Where(o => o.GetParameters().Length == 0).FirstOrDefault();
                  
                    if (ctor != null)
                    {
                        ObjectActivator<IBranch> createdActivator = GetActivator<IBranch>(ctor);
                        try
                        {
                            IBranch br = createdActivator();
                            if (br.BranchType == EnumPointType.Root)
                            {
                                int id = SeqID;
                                br.Name = cls.PackageName;
                                if (TreeType != null)
                                {
                                    if (cls.classProperties.ObjectType != null)
                                    {
                                        if (cls.classProperties.ObjectType.Equals(TreeType, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            CreateNode(id, br, TreeV);
                                        }
                                    }
                                }
                                else CreateNode(id, br, TreeV);
                            }
                        }
                        catch (Exception ex)
                        {
                            DMEEditor.AddLogMessage("Error", $"Creating Tree Root Node {cls.PackageName} {ex.Message} ", DateTime.Now, 0, null, Errors.Failed);
                        }
                     }
                }
            }
            catch (Exception ex)
            {
                DMEEditor.ErrorObject.Ex = ex;
                DMEEditor.ErrorObject.Flag = Errors.Failed;
               
            };
            return DMEEditor.ErrorObject;
        }
        private void CreateNode(int id,IBranch br,TreeView tree)
        {
            TreeNode n = TreeV.Nodes.Add(br.BranchText);
            n.Tag = br;
         
            br.TreeEditor = this;
            br.Visutil = VisManager;
            br.BranchID = id;
            br.ID = id;
            n.Name = br.ID.ToString();
            ParentNode = null;
            CurrentNode = n;
            //br.ParentBranch = n;
            if (Vismanager.visHelper.GetImageIndex(br.IconImageName) == -1)
            {
                n.ImageIndex = Vismanager.visHelper.GetImageIndexFromConnectioName(br.BranchText);
                n.SelectedImageIndex = Vismanager.visHelper.GetImageIndexFromConnectioName(br.BranchText);
            }
            else
            {
                n.ImageKey = br.IconImageName;
                n.SelectedImageKey = br.IconImageName;
            }
            //n.ContextMenuStrip = 
            CreateMenuMethods(br);
            if ( br.ObjectType!=null && br.BranchClass != null)
            {
                CreateGlobalMenu(br, n);
            }
           
            br.DMEEditor = DMEEditor;
            if (!DMEEditor.ConfigEditor.objectTypes.Any(i => i.ObjectType == br.BranchClass && i.ObjectName == br.BranchType.ToString() + "_" + br.BranchClass))
            {
                DMEEditor.ConfigEditor.objectTypes.Add(new TheTechIdea.Beep.Workflow.ObjectTypes { ObjectType = br.BranchClass, ObjectName = br.BranchType.ToString() + "_" + br.BranchClass });
            }
            Branches.Add(br);
            br.CreateChildNodes();
        }
        public IErrorsInfo CreateGlobalMenu(IBranch br, TreeNode n)
        {
            try
            {
                MenuList menuList = new MenuList();
                if (!IsMenuCreated(br))
                {
                    menuList = new MenuList(br.ObjectType, br.BranchClass, br.BranchType);
                    menuList.branchname = br.BranchText;
                    Menus.Add(menuList);
                    ContextMenuStrip nodemenu = new ContextMenuStrip();
                    menuList.Menu = nodemenu;
                    menuList.ObjectType = br.ObjectType;
                    menuList.BranchClass = br.BranchClass;
                    menuList.Menu.ImageList = Vismanager.Images;
                    menuList.Menu.ItemClicked -= Nodemenu_ItemClicked;
                    menuList.Menu.ItemClicked += Nodemenu_ItemClicked;
                    menuList.Menu.Items.Clear();

                }
                else
                    menuList = GetMenuList(br);
                List<AssemblyClassDefinition> extentions = DMEEditor.ConfigEditor.GlobalFunctions.Where(o => o.classProperties != null && o.classProperties.ObjectType != null && o.classProperties.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase)).OrderBy(p => p.Order).ToList(); //&&  o.classProperties.menu.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase)
                foreach (AssemblyClassDefinition cls in extentions)
                {
                    if (!menuList.classDefinitions.Any(p => p.PackageName.Equals(cls.PackageName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        menuList.classDefinitions.Add(cls);
                        foreach (var item in cls.Methods)
                        {
                            if (item.PointType == br.BranchType)
                            {
                                ToolStripItem st = menuList.Menu.Items.Add(item.Caption);
                                menuList.Menu.Name = br.ToString();
                                if (item.iconimage != null)
                                {
                                    st.ImageIndex = Vismanager.visHelper.GetImageIndex(item.iconimage);
                                }
                                st.Tag = cls;
                            }
                        }
                      
                    }
                    
                }

                return DMEEditor.ErrorObject;
            }
            catch (Exception ex)
            {
                return DMEEditor.ErrorObject;
            }
        }
        public ContextMenuStrip CreateMenuMethods(IBranch branch)
        {
            MenuList menuList = new MenuList();
            if (!IsMenuCreated(branch))
            {
                menuList = new MenuList(branch.ObjectType, branch.BranchClass, branch.BranchType);
                menuList.branchname = branch.BranchText;
                Menus.Add(menuList);
                ContextMenuStrip nodemenu = new ContextMenuStrip();
                menuList.Menu = nodemenu;
                menuList.ObjectType = branch.ObjectType;
                menuList.BranchClass = branch.BranchClass;
                menuList.Menu.Items.Clear();
                menuList.Menu.ImageList = Vismanager.Images;
                menuList.Menu.ItemClicked -= Nodemenu_ItemClicked;
                menuList.Menu.ItemClicked += Nodemenu_ItemClicked;

            }
            else
                menuList = GetMenuList(branch);
          //  ContextMenuStrip nodemenu = new ContextMenuStrip();
            try
            {
               
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.BranchesClasses.Where(x => x.PackageName == branch.ToString() ).FirstOrDefault();
                if (!menuList.classDefinitions.Any(p => p.PackageName.Equals(cls.PackageName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    menuList.classDefinitions.Add(cls);
                    foreach (var item in cls.Methods.Where(y => y.Hidden == false))
                    {

                        ToolStripItem st = menuList.Menu.Items.Add(item.Caption);
                        menuList.Menu.Name = branch.ToString();
                        if (item.iconimage != null)
                        {
                            st.ImageIndex = Vismanager.visHelper.GetImageIndex(item.iconimage);
                        }
                        st.Tag = cls;

                    }
                  
                }
               

            }
            catch (Exception ex)
            {
                string mes = "Could not add method to menu " + branch.BranchText;
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return menuList.Menu;
        }
        public void Run(IPassedArgs pPassedarg)
        {
            throw new NotImplementedException();
        }
        public void RunFunction(object sender, EventArgs e)
        {
            IBranch br = null;
            ToolStripItem item = (ToolStripItem)sender;
            if (TreeV.SelectedNode != null)
            {
                TreeNode n = TreeV.SelectedNode;
                br =(IBranch)n.Tag;
            }
            if (br != null)
            {
                if (DMEEditor.Passedarguments == null)
                {
                    DMEEditor.Passedarguments = new PassedArgs();
                }
             //  
                DMEEditor.Passedarguments.ObjectName = br.BranchText;
                DMEEditor.Passedarguments.DatasourceName = br.DataSourceName;
                DMEEditor.Passedarguments.Id = br.BranchID;
                DMEEditor.Passedarguments.ParameterInt1 = br.BranchID;
                AssemblyClassDefinition assemblydef = (AssemblyClassDefinition)item.Tag;
                dynamic fc = DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.type.ToString(), new object[] { DMEEditor, Vismanager, this });
                Type t = ((IFunctionExtension)fc).GetType();
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.GlobalFunctions.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                if (!IsMethodApplicabletoNode(cls, br)) return;
                MethodsClass methodsClass;
                try
                {
                    if (br.BranchType != EnumPointType.Global)
                    {
                        methodsClass = cls.Methods.Where(x => x.Caption == item.Text).FirstOrDefault();
                    }
                    else
                    {
                        methodsClass = cls.Methods.Where(x => x.Caption == item.Text && x.PointType == br.BranchType).FirstOrDefault();
                    }

                }
                catch (Exception)
                {

                    methodsClass = null;
                }
                if (methodsClass != null)
                {
                    method = methodsClass.Info;
                    if (method.GetParameters().Length > 0)
                    {
                        method.Invoke(fc, new object[] { DMEEditor.Passedarguments });
                    }
                    else
                        method.Invoke(fc, null);
                }
            }


        }
        public void RunFunction(IBranch br, ToolStripItem item)
        {
          
         
            if (TreeV.SelectedNode != null)
            {
                TreeNode n = TreeV.SelectedNode;
                br = (IBranch)n.Tag;
            }
            if (br != null)
            {
                if (DMEEditor.Passedarguments == null)
                {
                    DMEEditor.Passedarguments = new PassedArgs();
                }
                DMEEditor.Passedarguments.ObjectName = br.BranchText;
                DMEEditor.Passedarguments.DatasourceName = br.DataSourceName;
                DMEEditor.Passedarguments.Id = br.BranchID;
                DMEEditor.Passedarguments.ParameterInt1 = br.BranchID;
                AssemblyClassDefinition assemblydef = (AssemblyClassDefinition)item.Tag;
                dynamic fc = DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.dllname,assemblydef.type.ToString(), new object[] { DMEEditor, Vismanager, this });
                Type t = ((IFunctionExtension)fc).GetType();
                //dynamic fc = Activator.CreateInstance(assemblydef.type, new object[] { DMEEditor, Vismanager, this });
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.GlobalFunctions.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                MethodsClass methodsClass;
                if (!IsMethodApplicabletoNode(cls, br)) return;
                try
                {
                    if (br.BranchType != EnumPointType.Global)
                    {
                        methodsClass = cls.Methods.Where(x => x.Caption == item.Text).FirstOrDefault();
                    }
                    else
                    {
                        methodsClass = cls.Methods.Where(x => x.Caption == item.Text && x.PointType == br.BranchType).FirstOrDefault();
                    }

                }
                catch (Exception)
                {

                    methodsClass = null;
                }
                if (methodsClass != null)
                {
                    method = methodsClass.Info;
                    if (method.GetParameters().Length > 0)
                    {
                        method.Invoke(fc, new object[] { DMEEditor.Passedarguments });
                    }
                    else
                        method.Invoke(fc, null);
                }
            }


        }
        public IErrorsInfo RunMethod(Object branch, string MethodName)
        {

            try
            {
                Type t = branch.GetType();
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.BranchesClasses.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                MethodsClass methodsClass;
                try
                {
                    methodsClass = cls.Methods.Where(x => x.Caption == MethodName).FirstOrDefault();
                }
                catch (Exception)
                {

                    methodsClass = null;
                }
                if (methodsClass != null)
                {
                    if (!IsMethodApplicabletoNode(cls, (IBranch)branch)) return DMEEditor.ErrorObject;
                    method = methodsClass.Info;
                    if (method.GetParameters().Length > 0)
                    {
                        method.Invoke(branch, new object[] { DMEEditor.Passedarguments.Objects[0].obj });
                    }
                    else
                        method.Invoke(branch, null);


                    //  DMEEditor.AddLogMessage("Success", "Running method", DateTime.Now, 0, null, Errors.Ok);
                }

            }
            catch (Exception ex)
            {
                string mes = "Could not Run Method " + MethodName;
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
        public void SetConfig(IDMEEditor pbl, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs e, IErrorsInfo per)
        {
            return;
        }
        private void SetupTreeView()
        {

            Vismanager.Images = new ImageList();
           
                Vismanager.Images.ImageSize = IconSize;
          
            
            Vismanager.Images.ColorDepth = ColorDepth.Depth32Bit;
            List<string> paths = Directory.GetFiles(DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath, "*.ico", SearchOption.AllDirectories).ToList();
            foreach (string filename_w_path in paths)
            {
                try
                {
                    string filename = Path.GetFileName(filename_w_path);

                    Vismanager.Images.Images.Add(filename, Image.FromFile(filename_w_path));


                }
                catch (FileLoadException ex)
                {
                    DMEEditor.ErrorObject.Flag = Errors.Failed;
                    DMEEditor.ErrorObject.Ex = ex;
                    DMEEditor.Logger.WriteLog($"Error Loading icons ({ex.Message})");
                }

            }
            TreeV.CheckBoxes = false;
            TreeV.ImageList = Vismanager.Images;
           // TreeV.ItemHeight = 24;
            TreeV.SelectedImageKey = SelectIcon;
            //TreeV.Dock = DockStyle.Fill;
            //TreeV.SendToBack();
           
        }
        public IErrorsInfo TurnonOffCheckBox(IPassedArgs Passedarguments)
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;
            try
            {

                //TreeView trv = (TreeView)TreeEditor.TreeStrucure;
                TreeV.CheckBoxes = !TreeV.CheckBoxes;
                SelectedBranchs.Clear();

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Fail", $"Could not select entities {ex.Message}", DateTime.Now, 0, Passedarguments.DatasourceName, Errors.Failed);
            }
            return DMEEditor.ErrorObject;

        }
        #region "TreeNode Handling"
        public TreeNode GetTreeNodeByID(int id, TreeNodeCollection p_Nodes)
        {
            try
            {
            //    foreach (TreeNode node in p_Nodes)
            //    {
            //        IBranch br = (IBranch)node.Tag;
            //        if (br.ID == id)
            //        {
            //            return node;
            //        }

            //        if (node.Nodes.Count > 0)
            //        {
            //            var result = GetTreeNodeByID(id, node.Nodes);
            //            if (result != null)
            //            {
            //                return result;
            //            }
            //        }
            //    }
            return p_Nodes.Find(id.ToString(), true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;


            }


            return null;// TreeV.Nodes.Cast<TreeNode>().Where(n => n.Tag.ToString() == tag).FirstOrDefault();
        }
        public TreeNode GetTreeNodeByCaption(string Caption, TreeNodeCollection p_Nodes)
        {
            foreach (TreeNode node in p_Nodes)
            {
                if (node.Text == Caption)
                {
                    return node;
                }

                if (node.Nodes.Count > 0)
                {
                    var result = GetTreeNodeByCaption(Caption, node.Nodes);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
        #endregion
        #region "Method Calling"
        private bool IsMethodApplicabletoNode(AssemblyClassDefinition cls, IBranch br)
        {
            if (cls.classProperties == null)
            {
                return true;
            }
            if (cls.classProperties.ObjectType != null)
            {
                if (!cls.classProperties.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false ;
                }
            }
            return true;


        }
        public void Nodemenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = (ContextMenuStrip)sender;
            ToolStripItem item = e.ClickedItem;
            TreeNode n = TreeV.SelectedNode;
            menu.Hide();
            IBranch br = (IBranch)n.Tag;
            AssemblyClassDefinition cls = (AssemblyClassDefinition)item.Tag;
           
            if (cls != null)
            {
                if (!IsMethodApplicabletoNode(cls, br)) return;
                if (cls.RootName != "IFunctionExtension")
                {
                    RunMethod(br, item.Text);
                }else
                {
                    
                    RunFunction(br, item);
                };

            }
        }
        public void Nodemenu_MouseClick(TreeNodeMouseClickEventArgs e)
        {
            // ContextMenuStrip n = (ContextMenuStrip)e.;
            TreeNode n = TreeV.SelectedNode;
            IBranch br =(IBranch)n.Tag;
            if(br != null)
            {
                string clicks = "";
                if(e.Button== MouseButtons.Right)
                {
                    if (IsMenuCreated(br))
                    {
                        MenuList menuList = GetMenuList(br);
                        menuList.Menu.Show(Cursor.Position);
                    }
                }
                else
                {
                    switch (e.Clicks)
                    {
                        case 1:
                            clicks = "SingleClick";
                            break;
                        case 2:
                            clicks = "DoubleClick";
                            break;

                        default:
                            break;
                    }
                    AssemblyClassDefinition cls = DMEEditor.ConfigEditor.BranchesClasses.Where(x => x.PackageName == br.Name && x.Methods.Where(y => y.DoubleClick == true || y.Click == true).Any()).FirstOrDefault();
                    if (cls != null)
                    {
                        if (!IsMethodApplicabletoNode(cls, br)) return;
                        RunMethod(br, clicks);
                    }
                }
              
            }
            
        }

        #endregion
        #region "Filter Nodes"
        public string Filterstring { set { FilterString_TextChanged(value); } }
        private TreeView TreeCache = new TreeView();
        private bool IsFiltering=false;
        public void FilterString_TextChanged(string value)
        {
            if (IsFiltering==false)
            {
                TreeCache.Nodes.Clear();
                foreach (TreeNode _node in TreeV.Nodes)
                {
                    TreeCache.Nodes.Add((TreeNode)_node.Clone());
                }
                IsFiltering = true;
            }
          
            //blocks repainting tree till all objects loaded
            
            this.TreeV.BeginUpdate();
            this.TreeV.Nodes.Clear();
            if (value != string.Empty)
            {
                foreach (TreeNode _parentNode in TreeCache.Nodes)
                {
                    ScanNodes(_parentNode, value);
                }
            }
            else
            {
                foreach (TreeNode _node in TreeCache.Nodes)
                {
                    TreeV.Nodes.Add((TreeNode)_node.Clone());
                }
                IsFiltering = false;
            }
            //enables redrawing tree after all objects have been added
            this.TreeV.EndUpdate();
        }
        private void ScanNodes(TreeNode _parentNode, string value)
        {
            foreach (TreeNode _childNode in _parentNode.Nodes)
            {
                if (_childNode.Text.StartsWith(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.TreeV.Nodes.Add((TreeNode)_childNode.Clone());
                }
                if(_childNode.Nodes.Count > 0)
                {
                    ScanNodes(_childNode, value);
                }
            }

        }
        #endregion
    }
}
