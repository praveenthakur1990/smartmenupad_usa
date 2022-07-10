using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IPartnerBusiness
    {
        int AddUpdatePartner(PartnerModel model);   
        List<PartnerModel> GetAllPartners(int id);
    }
}
