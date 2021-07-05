using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TicketShop.Domain.Identity;
using TicketShop.Repository.Interface;
using TicketShop.Service.Interface;

namespace TicketShop.Web.Controllers {
    public class OrderController: Controller {
        private readonly IOrderService _orderService;
        private readonly UserManager<TicketShopUser> _userManager;
        private readonly IUserRepository _userRepository;

        public OrderController(IOrderService orderService, UserManager<TicketShopUser> userManager, IUserRepository userRepository) {
            _orderService = orderService;
            _userManager = userManager;
            _userRepository = userRepository;
        }
        [HttpGet]
        public IActionResult Index() {
            var user = _userRepository.Get(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = _orderService.getAllOrders(user);

            return View(orders);
        }


        [HttpGet]
        public IActionResult Details(Guid? id) {
            var result = _orderService.getOrderDetails(id);
            return View(result);
        }
        [HttpGet]
        public async Task<FileContentResult> ExportAllOrders() {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook()) {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Costumer Email";

                var user = await _userManager.GetUserAsync(HttpContext.User);

                var result = _orderService.getAllOrders(user);

                for (int i = 1; i <= result.Count(); i++) {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;

                    for (int p = 0; p < item.TicketsInOrder.Count(); p++) {
                        worksheet.Cell(1, p + 3).Value = "Ticket-" + (p + 1);
                        worksheet.Cell(i + 1, p + 3).Value = "For the movie - " + item.TicketsInOrder.ElementAt(p).OrderedTicket.MovieName;
                    }
                }

                using (var stream = new MemoryStream()) {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }

        public FileContentResult CreateInvoice(Guid id) {
            var result = _orderService.getOrderDetails(id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);


            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            foreach (var item in result.TicketsInOrder) {
                totalPrice += item.OrderedTicket.Price;
                sb.AppendLine("Ticket for the movie " + item.OrderedTicket.MovieName + " and price of: " + item.OrderedTicket.Price + "$");
            }


            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");


            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
