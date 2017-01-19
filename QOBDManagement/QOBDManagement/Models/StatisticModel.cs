using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class StatisticModel : BindBase
    {
        private Statistic _statistic;

        public StatisticModel()
        {
            _statistic = new Statistic();
        }

        public Statistic Statistic
        {
            get { return _statistic; }
            set { setProperty(ref _statistic, value); }
        }
    }
}
