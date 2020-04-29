using System.Collections;
using System.Security.Cryptography.X509Certificates;
using fc_manager_backend_da.Models;
using fc_manager_backend_abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace fc_manager_backend_repository
{
    public class MatchRecordRepository : IMatchRecordRepository
    {
        private FCMContext _context;
        public MatchRecordRepository(FCMContext context)
        {
            _context = context;
        }

        public void Add(MatchRecord matchRecord)
        {
            _context.MatchRecords.Add(matchRecord);
        }

        public void Remove(MatchRecord matchRecord)
        {
            _context.MatchRecords.Remove(matchRecord);
        }

        public async Task<MatchRecord> GetMatchRecord(int id)
        {
            return await _context.MatchRecords
                                    .Include(mr => mr.Match)
                                    .ThenInclude(mr => mr.MatchRecords)
                                    .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<MatchRecord>> GetMatchRecords(int matchId)
        {
            return await _context.MatchRecords
                            .Where(mr => mr.MatchId == matchId && mr.DeletedAt == null)
                            .Include(mr => mr.ScoreMember)
                                .ThenInclude(mrm => mrm.TeamMembers)
                            .Include(mr => mr.AssistMember)
                                .ThenInclude(mrm => mrm.TeamMembers)
                            .Include(mr => mr.Type)
                            .Include(mr => mr.Match)
                            .ToListAsync();
        }

        public async Task<List<MatchRecord>> GetMatchRecords(List<int> matchRecordIds)
        {
            return await _context.MatchRecords.Where(mr => matchRecordIds.Contains(mr.Id)).ToListAsync();
        }
    }
}
