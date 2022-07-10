using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IContactUsBusiness
    {
        int SaveContactUs(string emailAddress, string connectionStr);
        List<ContactUsVM> GetContactUsList(PaginationModel model, string connectionStr);

        int SaveRequestForDemo(RequestDemoModel model);
        int SaveTalkToExpertRequest(TalkToExpertModel model);
        int SaveNewsLetterSubscriber(string emailAddress);

        List<RequestDemoModel> RequestForDemoList();
        List<TalkToExpertModel> TalkToExpertRequestList();
        List<NewsLetterSubscriberModel> NewsLetterSubscriberList();
    }
}
