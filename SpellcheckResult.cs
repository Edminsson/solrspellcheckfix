using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolrUnitTest
{
    public class SpellCheckResult
    {
        /// <summary>
        /// Original query term
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Result count for original term
        /// </summary>
        public int NumFound { get; set; }

        /// <summary>
        /// Start offset
        /// </summary>
        public int StartOffset { get; set; }

        /// <summary>
        /// End offset
        /// </summary>
        public int EndOffset { get; set; }

        /// <summary>
        /// Spelling suggestions
        /// </summary>
        public ICollection<string> Suggestions { get; set; }
    }
}
