using System;
using System.Linq;
using System.Threading.Tasks;
using SecurePipelineScan.VstsService.Response;

namespace SecurePipelineScan.Rules.Security
{
    public class PipelineHasAtLeastOneStageWithApproval : IReleasePipelineRule
    {
        string IRule.Description => "Release pipeline contains 4-eyes approval";
        string IRule.Link => "https://confluence.dev.rabobank.nl/x/DGjlCw";
        bool IRule.IsSox => true;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<bool> EvaluateAsync(string projectId, string stageId, ReleaseDefinition releasePipeline)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (releasePipeline == null)
                throw new ArgumentNullException(nameof(releasePipeline));

            return releasePipeline
                .Environments
                .Select(p => p.PreDeployApprovals)
                .Any(p =>
                    p.ApprovalOptions != null
                    && !p.ApprovalOptions.ReleaseCreatorCanBeApprover
                    && p.Approvals.Any(a => a.Approver != null));
        }
    }
}