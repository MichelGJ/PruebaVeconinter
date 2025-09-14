using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeconinterContacts.Models;
using VeconinterContacts.Repositories.Interfaces;

namespace VeconinterContacts.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IClienteRepository _repo;
        private readonly ISubClienteRepository _subRepo;

        public ClientesController(IClienteRepository repo, ISubClienteRepository subRepo)
        {
            _repo = repo; _subRepo = subRepo;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _repo.GetAllAsync(c => c.SubClientes);
            return View(data);
        }

        public IActionResult Create() => View(new Cliente());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente model)
        {
            if (!ModelState.IsValid) return View(model);
            await _repo.AddAsync(model);
            await _repo.SaveAsync();
            TempData["ok"] = "Cliente creado.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cliente model)
        {
            if (!ModelState.IsValid) return View(model);
            await _repo.UpdateAsync(model);
            await _repo.SaveAsync();
            TempData["ok"] = "Cliente actualizado.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var c = await _repo.GetWithSubsAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _repo.GetWithSubsAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c != null)
            {
                await _repo.DeleteAsync(c);
                await _repo.SaveAsync();
            }
            TempData["ok"] = "Cliente eliminado.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSub(int clienteId, SubCliente sub)
        {
            if (!ModelState.IsValid)
            {
                var cliente = await _repo.GetWithSubsAsync(clienteId);
                return View("Details", cliente);
            }

            sub.ClienteId = clienteId;
            await _subRepo.AddAsync(sub);
            await _subRepo.SaveAsync();
            TempData["ok"] = "Subcliente agregado correctamente.";

            return RedirectToAction(nameof(Details), new { id = clienteId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSub(int id, int clienteId)
        {
            var sc = await _subRepo.GetByIdAsync(id);
            if (sc != null)
            {
                await _subRepo.DeleteAsync(sc);
                await _subRepo.SaveAsync();
                TempData["ok"] = "Subcliente eliminado.";
            }
            return RedirectToAction(nameof(Details), new { id = clienteId });
        }
    }
}
