using System;
using System.Collections.Generic;
using System.Text;

namespace GamerCompanion.Models {
    public class Recommendation {
        public string Message { get; set; }

        public RecommendationSeverity Severity { get; set; }
    }

    public enum RecommendationSeverity {
        Info,
        Warning,
        Critical
    }
}
