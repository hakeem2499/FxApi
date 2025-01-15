using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;

        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            if (portfolio == null)
                throw new ArgumentNullException(nameof(portfolio));

            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol)
        {
            if (appUser == null)
                throw new ArgumentNullException(nameof(appUser));
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be null or empty", nameof(symbol));

            var portfolio = await _context
                .Portfolios.Include(p => p.Stock)
                .SingleOrDefaultAsync(p =>
                    p.AppUserId == appUser.Id
                    && p.Stock.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase)
                );

            if (portfolio == null)
                return null;

            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Only load necessary stock fields to reduce memory usage.
            return await _context
                .Portfolios.Where(p => p.AppUserId == user.Id)
                .Select(p => p.Stock) // Load only the related Stock entity
                .ToListAsync();
        }
    }
}
