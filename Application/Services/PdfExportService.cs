using Application.Interface;
using Application.Response;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PdfExportService : IPdfExportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimService _claimService;

        public PdfExportService(IUnitOfWork unitOfWork, IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _claimService = claimService;

            // Configure QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateUserSubscriptionPdf(int userId)
        {
            // Get user data and subscriptions
            var user = await _unitOfWork.UserAccounts.GetAsync(u => u.Id == userId);
            if (user == null)
                throw new Application.CustomExceptions.NotFoundException($"User with ID {userId} not found");

            var subscriptions = await _unitOfWork.Subscriptions.GetSubscriptionHistory(userId);

            // Generate the PDF using QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeSubscriptionContent(x, user, subscriptions));
                    page.Footer().Element(ComposeFooter);
                });
            });

            // Return PDF as byte array
            using (var stream = new MemoryStream())
            {
                document.GeneratePdf(stream);
                return stream.ToArray();
            }
        }

        public async Task<byte[]> GenerateHealthMetricReportPdf(int childId)
        {
            // Get child data
            var child = await _unitOfWork.Childrens.GetAsync(
                c => c.Id == childId,
                include => include.Include(c => c.HealthMetrics)
            );

            if (child == null)
                throw new Application.CustomExceptions.NotFoundException($"Child with ID {childId} not found");

            // Generate the PDF using QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeHealthMetricContent(x, child));
                    page.Footer().Element(ComposeFooter);
                });
            });

            using (var stream = new MemoryStream())
            {
                document.GeneratePdf(stream);
                return stream.ToArray();
            }
        }

        public async Task<byte[]> GenerateInvoicePdf(int orderId)
        {
            // Get order data
            var order = await _unitOfWork.Orders.GetAsync(
                o => o.Id == orderId,
                include => include
                    .Include(o => o.UserAccount)
                    .Include(o => o.Subscription)
                    .Include(o => o.Subscription.SubscriptionPlans)
            );

            if (order == null)
                throw new Application.CustomExceptions.NotFoundException($"Order with ID {orderId} not found");

            // Generate the PDF using QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeInvoiceContent(x, order));
                    page.Footer().Element(ComposeFooter);
                });
            });

            using (var stream = new MemoryStream())
            {
                document.GeneratePdf(stream);
                return stream.ToArray();
            }
        }

        // Helper methods for composing different sections of PDF documents
        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("MomTracking").Bold().FontSize(20).FontColor(Colors.Blue.Medium);
                    column.Item().Text("Health Tracking and Support for Mothers").FontSize(12);
                });

                // Chỉ tải logo nếu file tồn tại
                try
                {
                    string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");
                    if (File.Exists(logoPath))
                    {
                        row.ConstantItem(100).Image(logoPath);
                    }
                }
                catch
                {
                    // Bỏ qua nếu không thể tải logo
                    row.ConstantItem(100).Height(50);
                }
            });
        }
        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Page ");
                text.CurrentPageNumber();
                text.Span(" of ");
                text.TotalPages();
            });
        }

        private void ComposeSubscriptionContent(IContainer container, UserAccount user, List<Subscription> subscriptions)
        {
            container.Column(column =>
            {
                column.Item().Text($"Subscription Report for {user.FirstName} {user.LastName}").FontSize(14).Bold();
                column.Item().Text($"Email: {user.Email}").FontSize(12);
                column.Item().Text($"Report Generated: {DateTime.Now:g}").FontSize(12);

                column.Item().Height(20);

                column.Item().Text("Subscription History").FontSize(14).Bold();

                column.Item().Table(table =>
                {
                    // Define columns
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    // Header row
                    table.Header(header =>
                    {
                        header.Cell().Text("Plan").Bold();
                        header.Cell().Text("Start Date").Bold();
                        header.Cell().Text("End Date").Bold();
                        header.Cell().Text("Price").Bold();
                        header.Cell().Text("Status").Bold();
                    });

                    // Data rows
                    foreach (var subscription in subscriptions)
                    {
                        table.Cell().Text(subscription.SubscriptionPlans?.Name.ToString() ?? "Unknown");
                        table.Cell().Text(subscription.StartDate.ToString("d"));
                        table.Cell().Text(subscription.EndDate.ToString("d"));
                        table.Cell().Text($"${subscription.Price}");
                        table.Cell().Text(subscription.Status.ToString());
                    }
                });
            });
        }

        private void ComposeHealthMetricContent(IContainer container, Children child)
        {
            container.Column(column =>
            {
                column.Item().Text($"Health Report for {child.FullName} ({child.NickName})").FontSize(14).Bold();
                column.Item().Text($"Date of Birth: {child.Birth:d}").FontSize(12);
                column.Item().Text($"Gender: {child.Gender}").FontSize(12);
                column.Item().Text($"Report Generated: {DateTime.Now:g}").FontSize(12);

                column.Item().Height(20);

                column.Item().Text("Health Metrics").FontSize(14).Bold();

                if (child.HealthMetrics != null && child.HealthMetrics.Any())
                {
                    column.Item().Table(table =>
                    {
                        // Define columns
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        // Header row
                        table.Header(header =>
                        {
                            header.Cell().Text("Date").Bold();
                            header.Cell().Text("Weight (kg)").Bold();
                            header.Cell().Text("Length (cm)").Bold();
                            header.Cell().Text("Head (cm)").Bold();
                            header.Cell().Text("Status").Bold();
                        });

                        // Data rows
                        foreach (var metric in child.HealthMetrics.OrderByDescending(m => m.CreatedDate))
                        {
                            table.Cell().Text(metric.CreatedDate?.ToString("d") ?? "N/A");
                            table.Cell().Text(metric.Weight?.ToString() ?? "N/A");
                            table.Cell().Text(metric.Lenght?.ToString() ?? "N/A");
                            table.Cell().Text(metric.HeadCircumference?.ToString() ?? "N/A");
                            table.Cell().Text(metric.IsAlert ? "Alert" : "Normal");
                        }
                    });
                }
                else
                {
                    column.Item().Text("No health metrics recorded").Italic();
                }
            });
        }

        private void ComposeInvoiceContent(IContainer container, Order order)
        {
            var user = order.UserAccount;
            var subscription = order.Subscription;
            var plan = subscription?.SubscriptionPlans;

            container.Column(column =>
            {
                column.Item().Text("INVOICE").FontSize(20).Bold();
                column.Item().Text($"Invoice #: {order.Id:D6}").FontSize(12);
                column.Item().Text($"Date: {DateTime.Now:d}").FontSize(12);

                column.Item().Height(20);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text("Billed To:").Bold();
                        column.Item().Text($"{user?.FirstName} {user?.LastName}");
                        column.Item().Text(user?.Email ?? "N/A");
                    });

                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text("Payment Details:").Bold();
                        column.Item().Text($"Payment Status: {subscription?.PaymentStatus}");
                        column.Item().Text($"Subscription Period: {subscription?.StartDate:d} - {subscription?.EndDate:d}");
                    });
                });

                column.Item().Height(20);

                column.Item().Text("Order Details").FontSize(14).Bold();

                column.Item().Table(table =>
                {
                    // Define columns
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    // Header row
                    table.Header(header =>
                    {
                        header.Cell().Text("Description").Bold();
                        header.Cell().AlignRight().Text("Duration").Bold();
                        header.Cell().AlignRight().Text("Amount").Bold();
                    });

                    // Data row
                    table.Cell().Text($"Subscription Plan: {plan?.Name}");
                    table.Cell().AlignRight().Text($"{plan?.DurationMonth} months");
                    table.Cell().AlignRight().Text($"${order.Price}");

                    // Total row
                    table.Cell().ColumnSpan(2).AlignRight().Text("Total:").Bold();
                    table.Cell().AlignRight().Text($"${order.Price}").Bold();
                });

                column.Item().Height(40);

                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Text("Thank you for your subscription to MomTracking. For questions about this invoice, please contact support@momtracking.com");
            });
        }
    }
}