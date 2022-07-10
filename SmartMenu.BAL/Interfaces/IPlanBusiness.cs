using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IPlanBusiness
    {
        int AddUpdatePlan(PlanModel model);
        List<PlanModel> GetPlans(int planId);
    }
}
