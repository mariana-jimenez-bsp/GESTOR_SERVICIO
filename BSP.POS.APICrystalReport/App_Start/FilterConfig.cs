using System.Web;
using System.Web.Mvc;

namespace BSP.POS.APICrystalReport
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
