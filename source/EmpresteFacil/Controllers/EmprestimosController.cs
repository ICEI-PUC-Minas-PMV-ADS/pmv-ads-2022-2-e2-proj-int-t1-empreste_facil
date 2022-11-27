﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmpresteFacil.Context;
using EmpresteFacil.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace EmpresteFacil.Controllers
{
    public class EmprestimosController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<Usuario> _userManager;

        public EmprestimosController(DatabaseContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Emprestimes
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.Emprestimos.Where(e => e.UserId == userId.ToString()).ToListAsync());
        }

        // GET: Emprestimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Emprestimos == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .FirstOrDefaultAsync(m => m.EmprestimoId == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // GET: Emprestimes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emprestimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ValorTotalEmprestimo,NumeroParcelas,TaxaJuros,DataInicioEmprestimo")] Emprestimo emprestimo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            emprestimo.UserId = userId.ToString();
            _context.Add(emprestimo);
            await _context.SaveChangesAsync();

            //var teste = emprestimo.CalculoDeParcelas(emprestimo.ValorTotalEmprestimo, emprestimo.NumeroParcelas, emprestimo.TaxaJuros);
            //ViewBag.CalcularParcela = teste;

            //return RedirectToAction("Create", "Parcelas", teste);
            return RedirectToRoute(new { controller = "Emprestimos", action = "Details", id = emprestimo.EmprestimoId });
            //return RedirectToAction(nameof(Index));
        }

        // GET: Emprestimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Emprestimos == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            return View(emprestimo);
        }

        // POST: Emprestimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmprestimoId,ValorTotalEmprestimo,NumeroParcelas,TaxaJuros,DataInicioEmprestimo")] Emprestimo emprestimo)
        {
            if (id != emprestimo.EmprestimoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprestimo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmprestimoExists(emprestimo.EmprestimoId))
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
            return View(emprestimo);
        }

        // GET: Emprestimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Emprestimos == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .FirstOrDefaultAsync(m => m.EmprestimoId == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // POST: Emprestimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Emprestimos == null)
            {
                return Problem("Entity set 'DatabaseContext.Emprestimos'  is null.");
            }
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo != null)
            {
                _context.Emprestimos.Remove(emprestimo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmprestimoExists(int id)
        {
            return _context.Emprestimos.Any(e => e.EmprestimoId == id);
        }

        public double CalculoDeParcelas(double ValorTotalEmprestimo, int NumeroParcelas, double TaxaJuros)
        {
            // Formula: PMT = [PV. (1 + i) ^ n] i / [(1 + i) ^ n - 1]
            return (ValorTotalEmprestimo * Math.Pow((1 + TaxaJuros), NumeroParcelas) * TaxaJuros) / (Math.Pow((1 + TaxaJuros), NumeroParcelas) - 1);
        }

        // Simulando um empréstimo
        //public IActionResult CalculoDeParcelas(int id)
        //{
        //    return View(Emprestimo.);
        //}

        //public IActionResult Simulacao()


    }
}
