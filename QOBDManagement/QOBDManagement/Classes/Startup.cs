using QOBDBusiness;
using QOBDBusiness.Core;
using QOBDCommon.Entities;
using QOBDDAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Classes
{
    public class Startup
    {
        public BusinessLogic Bl { get; set; }
        public DataAccess Dal { get; set; }

        public Startup() {
            Dal = new DataAccess(
                                new DALAgent(),
                                new DALClient(),
                                new DALItem(),
                                new DALOrder(),
                                new DALSecurity(),
                                new DALStatisitc(),
                                new DALReferential(),
                                new DALNotification());

            BlSecurity BlSecurity = new BlSecurity(Dal);

            //Agent authenticatedUser = BlSecurity.AuthenticateUser("codsimex212", "e6299fbfe0ebe192cf9acf3975a3d087");
            //Dal.SetUserCredential(authenticatedUser);

                Bl = new BusinessLogic(
                                                new BLAgent(Dal),
                                                new BlCLient(Dal),
                                                new BLItem(Dal),
                                                new BLOrder(Dal),
                                                BlSecurity,
                                                new BLStatisitc(Dal),
                                                new BlReferential(Dal),
                                                new BlNotification(Dal));
        }

    }



}
