using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeconinterContacts.Models;
using VeconinterContacts.Repositories.Interfaces;

namespace VeconinterContacts.Controllers
{
    [Authorize]
    public class SubClientesController : Controller
    {
        private readonly ISubClienteRepository _subRepo;
        private readonly IClienteRepository _clienteRepo;

        public SubClientesController(ISubClienteRepository subRepo, IClienteRepository clienteRepo)
        {
            _subRepo = subRepo;
            _clienteRepo = clienteRepo;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _subRepo.GetAllAsync(sc => sc.Cliente!);
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sub = await _subRepo.GetByIdAsync(id, s => s.Cliente!);
            if (sub is null) return NotFound();
            return View(sub);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var subCliente = await _subRepo.GetByIdAsync(id, s => s.Cliente!);
            if (subCliente == null)
                return NotFound();

            return View(subCliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCliente subCliente)
        {
            if (id != subCliente.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(subCliente);

            await _subRepo.UpdateAsync(subCliente);
            await _subRepo.SaveAsync();

            // 🔹 Redirige directo a la ficha del cliente dueño
            return RedirectToAction("Details", "Clientes", new { id = subCliente.ClienteId });
        }
    }
}
