using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fc_manager_backend_da.Models;
using System.Collections.Generic;



namespace fc_manager_backend_api.Controllers.Resources
{
    public class SaveMatchRecordResource : BaseResource
    {        
        public int? ScoreMemberId { get; set; }
        public int ScoreTeamId { get; set; }
        public int? AssistMemberId { get; set; }
        public int AssistTeamId { get; set; }
        public int CodeId { get; set; }
        public int MatchId { get; set; }
        public bool IsHomeTeam { get; set; }

    }
}