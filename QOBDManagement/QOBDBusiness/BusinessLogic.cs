// FILE: D:/Just IT Training/BillManagment/Classes//BusinessLogic.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000B5D begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000B5D end

using QOBDCommon.Interfaces.BL;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDBusiness
{
    public class BusinessLogic : IActionManager
    {
        // Attributes

        public IAgentManager BlAgent { get; set; }
        public IOrderManager BlOrder { get; set; }
        public IClientManager BlClient { get; set; }
        public IItemManager BlItem { get; set; }
        public IReferentialManager BlReferential { get; set; }
        public ISecurityManager BlSecurity { get; set; }
        public IStatisticManager DALStatisitc { get; set; }

        public BusinessLogic(
                            IAgentManager inBlAgent,
                            IClientManager inBlClient,
                            IItemManager inBlItem,
                            IOrderManager inBlCommande,
                            ISecurityManager inBlSecurity,
                            IStatisticManager inDALStatisitc,
                            IReferentialManager inBlReferential)
                            
        {
            this.BlAgent = inBlAgent;
            this.BlClient = inBlClient;
            this.BlOrder = inBlCommande;
            this.BlItem = inBlItem;
            this.BlReferential = inBlReferential;
            this.BlSecurity = inBlSecurity;
            this.DALStatisitc = inDALStatisitc;
        }
        
    } /* end class BusinessLogic */
}