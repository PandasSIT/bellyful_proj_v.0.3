using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bellyful_proj_v._0._3.ViewModels
{
    public class DashBoardViewModel
    {
        public string TotalMeal { get; set; }
        public string TotalOrder { get; set; }
        public string TotalRecipient { get; set; }
        public string TotalDeliveryHours { get; set; }
        public string TotalBatch { get; set; }
        public Dictionary<Month, int?> MealsReportMonthly { get; set; }
        public Dictionary<Month, int?> DeliveriesReportMonthly { get; set; }
        public Dictionary<Month, double?> DeliveryHrsReportMonthly { get; set; }



        public DashBoardViewModel()
        {
            MealsReportMonthly = new Dictionary<Month, int?>();
            for (Month i = Month.Jan; i <= Month.Dec; i++)
            {
                MealsReportMonthly.Add(i, null);
            }
            

            DeliveriesReportMonthly = new Dictionary<Month, int?>();
            for (Month i = Month.Jan; i <= Month.Dec; i++)
            {
                DeliveriesReportMonthly.Add(i, null);
            }

            DeliveryHrsReportMonthly = new Dictionary<Month, double?>();
            for (Month i = Month.Jan; i <= Month.Dec; i++)
            {
                DeliveryHrsReportMonthly.Add(i, null);
            }

        }
    }
    public enum Month
    {
        Jan=1,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec
    }
   
}
