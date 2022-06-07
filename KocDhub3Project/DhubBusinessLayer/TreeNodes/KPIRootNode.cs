﻿using BeepEnterprize.Vis.Module;
using System;
using System.Collections.Generic;
using System.Text;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Util;

namespace KOC.DHUB3.TreeNodes
{
    public class KPIRootNode : IBranch
    {
        public int ID { get  ; set  ; }
        public IDMEEditor DMEEditor { get  ; set  ; }
        public IDataSource DataSource { get  ; set  ; }
        public string DataSourceName { get  ; set  ; }
        public List<IBranch> ChildBranchs { get  ; set  ; }
        public ITree TreeEditor { get  ; set  ; }
        public IVisManager Visutil { get  ; set  ; }
        public List<string> BranchActions { get  ; set  ; }
        public EntityStructure EntityStructure { get  ; set  ; }
        public int MiscID { get  ; set  ; }
        public string Name { get  ; set  ; }
        public string BranchText { get; set; } = "KPI";
        public int Level { get; set; } = 0;
        public EnumPointType BranchType { get; set; } = EnumPointType.Root;
        public int BranchID { get; set; }
        public string IconImageName { get; set; }
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Root node for KPI Data Management";
        public string BranchClass { get; set; } = "Dhub3.Kpi";

        public IErrorsInfo CreateChildNodes()
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

        public IErrorsInfo SetConfig(ITree pTreeEditor, IDMEEditor pDMEEditor, IBranch pParentNode, string pBranchText, int pID, EnumPointType pBranchType, string pimagename)
        {
            throw new NotImplementedException();
        }
    }
}