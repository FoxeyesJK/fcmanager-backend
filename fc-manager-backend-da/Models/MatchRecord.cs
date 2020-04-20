using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fc_manager_backend_da.Models
{
    public class MatchRecord : BaseEntity
    {
        public int? ScoreMemberId { get; set; }
        [ForeignKey("ScoreMemberId")]
        public virtual Member ScoreMember { get; set; }
        public int? AssistMemberId { get; set; }
        [ForeignKey("AssistMemberId")]
        public virtual Member AssistMember { get; set; }
        [ForeignKey("Code")]
        public int CodeId { get; set; }
        public virtual Code Type { get; set; }
        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }
        public bool isHomeTeam { get; set; }
    }
}