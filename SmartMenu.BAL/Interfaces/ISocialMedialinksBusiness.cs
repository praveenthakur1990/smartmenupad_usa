using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface ISocialMedialinksBusiness
    {
        List<SocialMediaModel> GetSocialMediaLinks(string connectionStr);
        int AddUpdateSocialMediaLinks(string SocialMediaLinkJsonStr, string createdBy, string connectionStr);

    }
}
