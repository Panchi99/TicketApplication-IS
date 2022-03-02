using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using TicketApp.Domain.DomainModels;
using TicketApp.Domain.DTO;
using TicketApp.Services.Interface;

namespace TicketApp.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketServicet)
        {
            _ticketService = ticketServicet;
        }

        // GET: tickets
        public IActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine(User.IsInRole("Administrator"));
            //System.Diagnostics.Debug.WriteLine(User.Roles.FirstOrDefault());
            var alltickets = this._ticketService.GetAllTickets();
            List<string> categories = alltickets.Select(t => t.Category).Distinct().ToList();

            TicketAndList dto = new TicketAndList
            {
                
                TicketList = alltickets,
                Categories = categories
            };

            return View(dto);
        }

        [HttpPost]
        public IActionResult FilterTicketsByCategory(TicketAndList id)
        {
            if(id.Category == null)
            {
                return Redirect("Index");
            }
            var alltickets = this._ticketService.GetAllTickets();
            List<string> categories = alltickets.Select(t => t.Category).Distinct().ToList();
            alltickets.RemoveAll(ticket => ticket.Category != id.Category);
            TicketAndList dto = new TicketAndList
            {
                TicketList = alltickets,
                Categories = categories
            };
            return ExportAllTickets(alltickets);
            
        }

            public FileContentResult ExportAllTickets(List<Ticket> ticks)
        {
            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Ticket Id";
                worksheet.Cell(1, 2).Value = "Movie Name";
                worksheet.Cell(1, 3).Value = "Date & Time";
                worksheet.Cell(1, 4).Value = "Category";
                worksheet.Cell(1, 5).Value = "Duration";
                worksheet.Cell(1, 6).Value = "TicketPrice";
                worksheet.Cell(1, 7).Value = "Director";
                worksheet.Cell(1, 8).Value = "Actors";

                List<Ticket> tickets;
                if (ticks == null)
                {
                    tickets = this._ticketService.GetAllTickets();
                }
                else
                {
                    tickets = ticks;
                }

                for (int i = 1; i <= tickets.Count(); i++)
                {
                    var item = tickets[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.MovieName;
                    worksheet.Cell(i + 1, 3).Value = item.DateTime.ToString();
                    worksheet.Cell(i + 1, 4).Value = item.Category;
                    worksheet.Cell(i + 1, 5).Value = item.Duration;
                    worksheet.Cell(i + 1, 6).Value = item.TicketPrice;
                    worksheet.Cell(i + 1, 7).Value = item.Director;
                    worksheet.Cell(i + 1, 8).Value = item.Actors;


                    //for (int p = 0; p < item.TicketInOrders.Count(); p++)
                    //{
                    //    worksheet.Cell(1, p + 3).Value = "Product-" + (p + 1);
                    //    worksheet.Cell(i + 1, p + 3).Value = item.TicketInOrders.ElementAt(p).OrderedProduct.ProductName;
                    //}
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }


        public IActionResult Sort()
        {
            var sortedTickets = this._ticketService.GetAllTickets();
            sortedTickets.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));

            List<string> categories = sortedTickets.Select(t => t.Category).Distinct().ToList();

            TicketAndList dto = new TicketAndList
            {

                TicketList = sortedTickets,
                Categories = categories
            };


            return View("Index", dto);
        }


        [HttpPost]
        public IActionResult Filter(string id)
        {
            var filteredTickets = this._ticketService.GetAllTickets();
            filteredTickets.RemoveAll(ticket =>
            {
               // System.Diagnostics.Debug.WriteLine(ticket.DateTime.ToString("yyyy-MM-dd").Split(" ")[0]);
                return ticket.DateTime.ToString("yyyy-MM-dd").Split(" ")[0].Trim().ToString() != id.Trim().ToString();
            });
            List<string> categories = filteredTickets.Select(t => t.Category).Distinct().ToList();
            TicketAndList dto = new TicketAndList
            {

                TicketList = filteredTickets,
                Categories = categories
            };
            return View("Index", dto);
        }



        public IActionResult AddTicketToCart(Guid? id)
        {
            var model = this._ticketService.GetShoppingCartInfo(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTicketToCart([Bind("TicketId", "Quantity")] AddToShoppingCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._ticketService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Tickets");
            }

            return View(item);
        }

        // GET: tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,MovieName,DateTime,Category,ImageUrl,Duration,TicketPrice,Director,Actors")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                this._ticketService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,MovieName,DateTime,Category,ImageUrl,Duration,TicketPrice,Director,Actors")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._ticketService.UpdeteExistingTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ticketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._ticketService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ticketExists(Guid id)
        {
            return this._ticketService.GetDetailsForTicket(id) != null;
        }
    }
}
