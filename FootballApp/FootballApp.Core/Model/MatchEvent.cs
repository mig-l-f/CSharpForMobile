using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballApp.Core.Model
{
    public class MatchEvent
    {
        public string event_id { get; set; }
        public string event_match_id { get; set; }
        public string event_type { get; set; }
        public string event_minute { get; set; }
        public string event_team { get; set; }
        public string event_player { get; set; }
        public string event_player_id { get; set; }
        public string event_result { get; set; }
    }
}
