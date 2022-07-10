using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class AdminDashBoardViewModel
    {
        public List<OrderViewModel> LatestOrderList { get; set; }
        public DashboardStatDataVM StatsData { get; set; }

    }
}
