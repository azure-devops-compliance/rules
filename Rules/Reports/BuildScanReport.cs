using System;

namespace SecurePipelineScan.Rules.Reports
{
    public class BuildScanReport
    {
        public string Id { get; set; }
        public string Pipeline { get; set; }
        public string Project { get; set; }
        public bool ArtifactsStoredSecure { get; set; }
        public bool UsesFortify { get; internal set; }
        public bool UsesSonarQube { get; internal set; }
        public DateTime CreatedDate { get; set; }
    }
}