using Application;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PdfExportController : ControllerBase
    {
        private readonly IPdfExportService _pdfExportService;
        private readonly IClaimService _claimService;
        private readonly IUnitOfWork _unitOfWork; // Thêm instance này

        public PdfExportController(
            IPdfExportService pdfExportService,
            IClaimService claimService,
            IUnitOfWork unitOfWork) // Thêm parameter trong constructor
        {
            _pdfExportService = pdfExportService;
            _claimService = claimService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("subscription-report")]
        public async Task<IActionResult> ExportUserSubscriptions()
        {
            var userClaim = _claimService.GetUserClaim();
            var pdfData = await _pdfExportService.GenerateUserSubscriptionPdf(userClaim.Id);

            return File(pdfData, "application/pdf", $"subscription-report-{userClaim.Id}.pdf");
        }

        [HttpGet("health-report/{childId}")]
        public async Task<IActionResult> ExportHealthMetrics(int childId)
        {
            var userClaim = _claimService.GetUserClaim();

            // Sử dụng instance _unitOfWork thay vì truy cập tĩnh
            var child = await _unitOfWork.Childrens.GetAsync(c => c.Id == childId && c.AccountId == userClaim.Id);
            if (child == null)
                return NotFound("Child not found or you don't have access to this resource");

            var pdfData = await _pdfExportService.GenerateHealthMetricReportPdf(childId);

            return File(pdfData, "application/pdf", $"health-report-{childId}.pdf");
        }

        [HttpGet("invoice/{orderId}")]
        public async Task<IActionResult> ExportInvoice(int orderId)
        {
            var userClaim = _claimService.GetUserClaim();

            // Sử dụng instance _unitOfWork thay vì truy cập tĩnh
            var order = await _unitOfWork.Orders.GetAsync(o => o.Id == orderId && o.AccountId == userClaim.Id);
            if (order == null)
                return NotFound("Order not found or you don't have access to this resource");

            var pdfData = await _pdfExportService.GenerateInvoicePdf(orderId);

            return File(pdfData, "application/pdf", $"invoice-{orderId}.pdf");
        }

        // For admin use - requires Manager role
        [Authorize(Roles = "Manager")]
        [HttpGet("admin/subscription-report/{userId}")]
        public async Task<IActionResult> ExportUserSubscriptionsAdmin(int userId)
        {
            var pdfData = await _pdfExportService.GenerateUserSubscriptionPdf(userId);

            return File(pdfData, "application/pdf", $"subscription-report-{userId}.pdf");
        }
    }
}
