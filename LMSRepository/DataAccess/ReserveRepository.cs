﻿using LMSLibrary.Data;
using LMSLibrary.Models;
using LMSRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class ReserveRepository : IReserveRepository
    {
        private readonly string reserved = "Reserved";
        private readonly DataContext _context;

        public ReserveRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ReserveAsset>> GetAllReserves()
        {
            var reserves = await _context.ReserveAssets
                .Include(s => s.Status)
                .Where(u => u.Status.Name == reserved).ToListAsync();

            return reserves;
        }

        public async Task<int> GetMemberCurrentReserveAmount(int cardId)
        {
            var count = await _context.ReserveAssets
                .Where(l => l.LibraryCard.Id == cardId)
                .Where(l => l.Status.Name == reserved)
                .CountAsync();

            return count;
        }

        public async Task<ReserveAsset> GetReserve(int id)
        {
            var reserve = await _context.ReserveAssets
                .Include(s => s.Status)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reserve;
        }

        public async Task<IEnumerable<ReserveAsset>> GetReservesForMember(int id)
        {
            var reserve = await _context.ReserveAssets
                .Where(l => l.LibraryCard.Id == id)
                .ToListAsync();

            return reserve;
        }
    }
}
