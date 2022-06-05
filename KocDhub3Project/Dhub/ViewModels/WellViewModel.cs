﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KOC.DHUB3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTechIdea.Util;

namespace KOC.DHUB3.ViewModels
{
    public class WellViewModel : BaseViewModel
    {
        [ObservableProperty]
        WELL_LATEST_DATA well;
        [ICommand]
        public IErrorsInfo GetWell(string puwi)
        {
            try
            {
                DMEditor.ErrorObject.Ex = null;
                DMEditor.ErrorObject.Flag = Errors.Ok;
                well = Repo.GetWell(puwi);
            }
            catch (Exception ex)
            {
                DMEditor.ErrorObject.Ex = ex;
                DMEditor.ErrorObject.Message = $"Error in  {System.Reflection.MethodBase.GetCurrentMethod().Name} -  {ex.Message}";
                DMEditor.ErrorObject.Flag = Errors.Failed;
            }
            return DMEditor.ErrorObject;
        }
        [ICommand]
        public IErrorsInfo GetWell(int pwell_completion_id)
        {
            try
            {
                DMEditor.ErrorObject.Ex = null;
                DMEditor.ErrorObject.Flag = Errors.Ok;
                well = Repo.GetWell(pwell_completion_id);
            }
            catch (Exception ex)
            {
                DMEditor.ErrorObject.Ex = ex;
                DMEditor.ErrorObject.Message = $"Error in  {System.Reflection.MethodBase.GetCurrentMethod().Name} -  {ex.Message}";
                DMEditor.ErrorObject.Flag = Errors.Failed;
            }
            return DMEditor.ErrorObject;
        }
    }
}
