using System.Linq;
using SecurePipelineScan.Rules.Security;
using Shouldly;
using Xunit;

namespace SecurePipelineScan.Rules.Tests.Security
{
    public class RulesProviderTests
    {
        [Fact]
        public void GlobalPermissions()
        {
            var rules = new RulesProvider().GlobalPermissions(null);
            rules.OfType<NobodyCanDeleteTheTeamProject>().ShouldNotBeEmpty();
        }

        [Fact]
        public void RepositoryRules()
        {
            var rules = new RulesProvider().RepositoryRules(null);
            rules
                .OfType<NobodyCanDeleteTheRepository>()
                .ShouldNotBeEmpty();
            
            rules
                .OfType<ReleaseBranchesProtectedByPolicies>()
                .ShouldNotBeEmpty();
        }

        [Fact]
        public void BuildRules()
        {
            var rules = new RulesProvider().BuildRules(null);
            rules
                .OfType<NobodyCanDeleteBuilds>()
                .ShouldNotBeEmpty();
        }

        [Fact]
        public void ReleaseRules()
        {
            var rules = new RulesProvider().ReleaseRules(null);
            rules
                .OfType<NobodyCanDeleteReleases>()
                .ShouldNotBeEmpty();
        }

        [Fact]
        public void AllRulesShouldBeInProvider()
        {
            var types = typeof(RulesProvider).Assembly.GetTypes().Where(t => typeof(IRule).IsAssignableFrom(t) && !t.IsInterface);
            types.ShouldNotBeEmpty();
            
            var provider = new RulesProvider();
            var rules = provider.BuildRules(null).Concat(
                provider.ReleaseRules(null));
            
            types.ShouldAllBe(t => rules.Select(r => r.GetType()).Contains(t));
        }

        [Fact]
        public void AllRepositoryRulesShouldBeInProvider()
        {
            var types = typeof(RulesProvider).Assembly.GetTypes().Where(t => typeof(IRepositoryRule).IsAssignableFrom(t) && !t.IsInterface);
            types.ShouldNotBeEmpty();

            var provider = new RulesProvider();
            var rules = provider.RepositoryRules(null);

            types.ShouldAllBe(t => rules.Select(r => r.GetType()).Contains(t));
        }

        [Fact]
        public void AllProjectRulesShouldBeInProvider()
        {
            var types = typeof(RulesProvider).Assembly.GetTypes().Where(t => typeof(IProjectRule).IsAssignableFrom(t) && !t.IsInterface);
            types.ShouldNotBeEmpty();
            
            var provider = new RulesProvider();
            var rules = provider.GlobalPermissions(null);
            
            types.ShouldAllBe(t => rules.Select(r => r.GetType()).Contains(t));
        }
    }
}