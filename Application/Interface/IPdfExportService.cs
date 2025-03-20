using Application.Response;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IPdfExportService
    {
        Task<byte[]> GenerateUserSubscriptionPdf(int userId);
        Task<byte[]> GenerateHealthMetricReportPdf(int childId);
        Task<byte[]> GenerateInvoicePdf(int orderId);
    }
}