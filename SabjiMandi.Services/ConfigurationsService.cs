using SabjiMandi.Database;
using SabjiMandi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabjiMandi.Services
{
    public class ConfigurationsService
    {
        #region Singleton
        private static ConfigurationsService instance {get; set;}
        public static ConfigurationsService Instance
        {
            get
            {
                if (instance == null) instance = new ConfigurationsService();
                return instance;
            }
        }

        private ConfigurationsService() { }
        #endregion
        public Config GetConfig(string key)
        {
            using(var context = new CBContext())
            {
                return context.Configurations.FirstOrDefault(x => x.Key == key);
            }
        }

        public int GetPageSize()
        {
            using (var context = new CBContext())
            {
                var pageSizeConfig = context.Configurations.Find("PageSize");
                return pageSizeConfig != null ? int.Parse(pageSizeConfig.Value) : 5;
            }
        }
    }
}
