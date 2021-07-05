using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Service.Interface;
using TicketShop.Web.Data;

namespace TicketShop.Web.Controllers {
    public class TicketsController: Controller {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService) {
            _ticketService = ticketService;
        }

        // GET: Tickets
        public IActionResult Index() {
            return View(this._ticketService.GetAllTickets());
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null) {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ValidTo,Price,Quantity,MovieName,MovieGenre,Image,Id")] Ticket ticket) {
            if (ModelState.IsValid) {
                this._ticketService.CreateNewTicket(ticket);

                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);
            if (ticket == null) {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("ValidTo,Price,Quantity,MovieName,MovieGenre,Image,Id")] Ticket ticket) {
            if (id != ticket.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    this._ticketService.UpdateExistingTicket(ticket);
                }
                catch (DbUpdateConcurrencyException) {
                    if (!TicketExists(ticket.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);
            if (ticket == null) {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id) {
            this._ticketService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id) {
            return this._ticketService.GetDetailsForTicket(id) != null;
        }

        // GET: Tickets/AddTicketToCart
        public IActionResult AddTicketToCart(Guid? id) {
            var model = this._ticketService.GetShoppingCartInfo(id);

            return View(model);
        }

        // POST: Tickets/AddTicketToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTicketToCart([Bind("TicketId", "Quantity")] AddToShoppingCartDTO item) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._ticketService.AddToShoppingCart(item, userId);

            if (result) {
                return RedirectToAction("Index", "Tickets");
            }

            return View(item);
        }


        [HttpPost]
        public FileContentResult ExportAllTickets(string movieGenre) {
            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook()) {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Tickets");

                worksheet.Cell(1, 1).Value = "Ticket Id";
                worksheet.Cell(1, 2).Value = "Movie";
                worksheet.Cell(1, 3).Value = "Date and Time";
                worksheet.Cell(1, 4).Value = "Price";
                worksheet.Cell(1, 5).Value = "Movie Genre";


                var result = _ticketService.GetAllTicketsByGenre(movieGenre);

                for (int i = 1; i <= result.Count(); i++) {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.MovieName;
                    worksheet.Cell(i + 1, 3).Value = item.ValidTo.ToString();
                    worksheet.Cell(i + 1, 4).Value = item.Price + "$";
                    worksheet.Cell(i + 1, 5).Value = movieGenre;
                }

                using (var stream = new MemoryStream()) {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }

            }
        }
    }
}
