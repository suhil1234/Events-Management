using Event_managment.BL;
using Event_managment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Event_managment.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AccessListController : Controller
    {
        private readonly IAccessListService _accessListService;

        // Constructor accepting IAccessListService via Dependency Injection
        public AccessListController(IAccessListService accessListService)
        {
            _accessListService = accessListService;
        }

        // GET: Admin/AccessList/List
        public IActionResult List()
        {
            List<TbAccessList> lstAccess = _accessListService.GetAll(); // Fetch all IP access records
            return View(lstAccess);
        }

        // GET: Admin/AccessList/Edit/{id}
        public IActionResult Edit(int? id)
        {
            TbAccessList ipAddress = id != null ? _accessListService.GetByID(id.Value) : new TbAccessList();
            return View(ipAddress); // Return the view for editing
        }

        // POST: Admin/AccessList/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(TbAccessList ip)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", ip); // Return view with validation errors
            }

            // Check for uniqueness of the IP address
            if (_accessListService.IsEventUnique(ip.IpAddress, ip.Id))
            {
                ModelState.AddModelError("IpAddress", "This IP Address is already in use.");
                return View("Edit", ip); // Return view with error message
            }

            bool success = _accessListService.Save(ip); // Save IP access record
            if (!success)
            {
                ModelState.AddModelError("", "An error occurred while saving the IP address.");
                return View("Edit", ip); // Return view with error message
            }

            return RedirectToAction("List"); // Redirect to the list of IPs
        }

        // GET: Admin/AccessList/Delete/{id}
        public IActionResult Delete(int id)
        {
            bool success = _accessListService.Delete(id); // Delete the IP record
            if (!success)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the IP address.";
            }

            return RedirectToAction("List"); // Redirect to the list
        }
    }
}