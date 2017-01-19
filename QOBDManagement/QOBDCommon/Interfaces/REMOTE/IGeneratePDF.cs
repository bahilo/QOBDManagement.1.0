
using QOBDCommon.Entities;
using QOBDCommon.Structures;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IGeneratePDF
    {
        // Operations

        void GeneratePdfDelivery(ParamDeliveryToPdf paramDeliveryToPdf);

        void GeneratePdfQuote(ParamOrderToPdf paramOrderToPdf);

        void GeneratePdfOrder(ParamOrderToPdf paramOrderToPdf);

    } /* end interface IGeneratePDF */
}