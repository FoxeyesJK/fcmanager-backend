using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fc_manager_backend_abstraction;
using fc_manager_backend_api.Controllers.Resources;
using fc_manager_backend_da.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fc_manager_backend_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchRecordController : ControllerBase
    {
        private readonly ILogger<MatchRecordController> _logger;
        private IMatchRecordRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MatchRecordController(ILogger<MatchRecordController> logger, IMatchRecordRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            //_repoWrapper = repoWrapper;
            _repository = repository;
        }

        [HttpPost]
        public async Task<List<MatchRecordResource>> UpsertRecords([FromBody] List<SaveMatchRecordResource> matchRecordsResource)
        {
        //Find way to validate list input
        // if (!ModelState.IsValid)
        //     return BadRequest(ModelState);
        var matchRecordIds = new List<int>();
        var matchRecord = new MatchRecord();

//Need to fix by using match query
#region ResetMatchScore
        var currentMatchRecordId = matchRecordsResource.FirstOrDefault().Id;
        var currentMatchRecord = await _repository.GetMatchRecord(currentMatchRecordId);
        if (currentMatchRecord != null) {
            currentMatchRecord.Match.HomeScore = 0;
            currentMatchRecord.Match.AwayScore = 0;
        }
#endregion

        foreach(var mr in matchRecordsResource)
        {
            if(mr.Id == 0)
            {
                var newMatchRecord = _mapper.Map<SaveMatchRecordResource, MatchRecord>(mr);
                _repository.Add(newMatchRecord);
                //matchrecord생성전
                await _unitOfWork.CompleteAsync();
                matchRecord = await _repository.GetMatchRecord(newMatchRecord.Id);
            }else {
                matchRecord = await _repository.GetMatchRecord(mr.Id);
            }

            if (matchRecord == null)
                return new List<MatchRecordResource>();

            _mapper.Map<SaveMatchRecordResource, MatchRecord>(mr, matchRecord);

            matchRecordIds.Add(matchRecord.Id);      
        }

        //Delete all unselected matchRecords
        foreach(var mr in currentMatchRecord.Match.MatchRecords.Where(m => !matchRecordIds.Contains(m.Id)))
        {
            mr.DeletedAt = DateTime.Now;
        }

       await _unitOfWork.CompleteAsync(); 
    
        var matchRecords = await _repository.GetMatchRecords(matchRecordIds);

        var result = _mapper.Map<List<MatchRecord>, List<MatchRecordResource>>(matchRecords);

        return result;
        }


        // [HttpPost]
        // public async Task<IActionResult> CreateRecord([FromBody] MatchRecordResource recordResource)
        // {
        // if (!ModelState.IsValid)
        //     return BadRequest(ModelState);

        // var record = _mapper.Map<MatchRecordResource, MatchRecord>(recordResource);

        // _repository.Add(record);
        // await _unitOfWork.CompleteAsync();

        // record = await _repository.GetMatchRecord(record.Id);

        // var result = _mapper.Map<MatchRecord, MatchRecordResource>(record);

        // return Ok(result);
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecord(int id, [FromBody] SaveMatchRecordResource matchRecordResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var matchRecord = await _repository.GetMatchRecord(id);

            if (matchRecord == null)
                return NotFound();

            _mapper.Map<SaveMatchRecordResource, MatchRecord>(matchRecordResource, matchRecord);

            await _unitOfWork.CompleteAsync();

            matchRecord = await _repository.GetMatchRecord(matchRecord.Id);
            var result = _mapper.Map<MatchRecord, MatchRecordResource>(matchRecord);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatchRecord(int id)
        {
            var matchRecord = await _repository.GetMatchRecord(id);

            if (matchRecord == null)
                return NotFound();

            matchRecord.DeletedAt = DateTime.Now;
            //_repository.Remove(matchRecord);
            await _unitOfWork.CompleteAsync();

            return Ok(id);
        }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetMatchRecord(int id)
        // {
        //     var record = await _repository.GetMatchRecord(id);

        //     if (record == null)
        //         return NotFound();

        //     var recordResource = _mapper.Map<MatchRecord, MatchRecordResource>(record);

        //     return Ok(recordResource);
        // }

        [HttpGet("{matchId}")]
        public async Task<IEnumerable<MatchRecordResource>> GetMatchRecordResources(int matchId)
        {
            var matchRecords = await _repository.GetMatchRecords(matchId);

            var result = _mapper.Map<IEnumerable<MatchRecord>, IEnumerable<MatchRecordResource>>(matchRecords);
            return result;
        }
    }
}
