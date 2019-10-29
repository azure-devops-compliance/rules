using SecurePipelineScan.VstsService;
using SecurePipelineScan.VstsService.Response;
using System;
using System.Threading.Tasks;

namespace SecurePipelineScan.Rules.Security
{
    public class ArtifactIsStoredSecure : PipelineHasTaskRuleBase, IBuildPipelineRule
    {
        public ArtifactIsStoredSecure(IVstsRestClient client) : base(client)
        {
            //nothing
        }

        protected override string TaskId => "2ff763a7-ce83-4e1f-bc89-0ae63477cebe";
        protected override string TaskName => "PublishBuildArtifacts@1";

        string IRule.Description => "Artifact is stored in secure artifactory";
        string IRule.Link => "https://confluence.dev.rabobank.nl/x/TI8AD";
        bool IRule.IsSox => true;

        public Task<bool?> EvaluateAsync(string projectId, BuildDefinition buildPipeline)
        {
            if (buildPipeline == null)
                throw new ArgumentNullException(nameof(buildPipeline));

            return base.EvaluateAsync(buildPipeline);
        }
    }
}