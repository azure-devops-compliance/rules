using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NSubstitute;
using SecurePipelineScan.VstsService;
using Shouldly;
using System;
using Xunit;
using Xunit.Abstractions;
using Response = SecurePipelineScan.VstsService.Response;

namespace SecurePipelineScan.Rules.Tests
{
    public class SecurityReportScanTest : IClassFixture<TestConfig>
    {
        private readonly ITestOutputHelper _output;
        private readonly TestConfig _config;

        public SecurityReportScanTest(ITestOutputHelper output, TestConfig config)
        {
            _output = output;
            _config = config;
        }

        [Fact]
        [Trait("category", "integration")]
        public void IntegrationTestOnScan()
        {
            var organization = _config.Organization;
            var token = _config.Token;

            var client = new VstsRestClient(organization, token);
            var scan = new SecurityReportScan(client);
            var securityReport = scan.Execute(_config.Project, DateTime.Now);

            securityReport.ApplicationGroupContainsProductionEnvironmentOwner.ShouldBeTrue();
            securityReport.ProjectAdminGroupOnlyContainsRabobankProjectAdminGroup.ShouldBeTrue();

            securityReport.TeamRabobankProjectAdministrators.GlobalRightsIsSecure.ShouldBeTrue();

            securityReport.BuildRightsBuildAdmin.IsSecure.ShouldBeTrue();
            securityReport.BuildRightsProjectAdmin.IsSecure.ShouldBeTrue();
            securityReport.BuildRightsContributor.IsSecure.ShouldBeTrue();

            securityReport.BuildDefinitionsRightsContributor.IsSecure.ShouldBeTrue();
            securityReport.BuildDefinitionsRightsBuildAdmin.IsSecure.ShouldBeTrue();
            securityReport.BuildDefinitionsRightsProjectAdmin.IsSecure.ShouldBeTrue();

            securityReport.RepositoryRightsProjectAdmin.RepositoryRightsIsSecure.ShouldBeTrue();

            securityReport.ReleaseRightsContributor.IsSecure.ShouldBeTrue();
            securityReport.ReleaseRightsReleaseAdmin.IsSecure.ShouldBeTrue();
            securityReport.ReleaseRightsProjectAdmin.IsSecure.ShouldBeTrue();
            securityReport.ReleaseRightsProductionEnvOwner.IsSecure.ShouldBeTrue();
            securityReport.ReleaseRightsRaboProjectAdmin.IsSecure.ShouldBeTrue();

            securityReport.ReleaseDefinitionsRightsContributor.IsSecure.ShouldBeTrue();
            securityReport.ReleaseDefinitionsRightsReleaseAdmin.IsSecure.ShouldBeTrue();
            securityReport.ReleaseDefinitionsRightsProjectAdmin.IsSecure.ShouldBeTrue();
            securityReport.ReleaseDefinitionsRightsRaboProjectAdmin.IsSecure.ShouldBeTrue();
            securityReport.ReleaseDefinitionsRightsProductionEnvOwner.IsSecure.ShouldBeTrue();

            securityReport.ProjectIsSecure.ShouldBeTrue();
        }

        [Fact]
        public void SecurityReportScanExecuteWithApplicationsGroupsResponseShouldNotBeEmpty()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization());

            var applicationGroup1 = new Response.ApplicationGroup
            { DisplayName = "[dummy]\\Project Administrators", TeamFoundationId = "1", };
            var applicationGroup2 = new Response.ApplicationGroup
            { DisplayName = "[dummy]\\Rabobank Project Administrators", TeamFoundationId = "2" };
            var applicationGroup3 = new Response.ApplicationGroup
            { DisplayName = "[dummy]\\Build Administrators", TeamFoundationId = "3", };
            var applicationGroup4 = new Response.ApplicationGroup
            { DisplayName = "[dummy]\\Production Environment Owners", TeamFoundationId = "4", };
            var applicationGroup5 = new Response.ApplicationGroup
            { DisplayName = "[dummy]\\Release Administrators", TeamFoundationId = "5", };
            var applicationGroup6 = new Response.ApplicationGroup
            { DisplayName = "[dummy]\\Contributors", TeamFoundationId = "6", };
            var applicationGroups = new Response.ApplicationGroups
            {
                Identities = new[]
                {
                    applicationGroup1, applicationGroup2, applicationGroup3, applicationGroup4, applicationGroup5,
                    applicationGroup6
                }
            };

            var securityNamespace1 = new Response.SecurityNamespace
            { DisplayName = "Git Repositories", NamespaceId = "123456" };
            var securityNamespace2 = new Response.SecurityNamespace { Name = "Build", NamespaceId = "54321" };
            var securityNamespace3 = new Response.SecurityNamespace
            {
                Name = "ReleaseManagement",
                NamespaceId = "54454321",
                Actions = new[] { new Response.NamespaceAction { Name = "ViewReleaseDefinition" } }
            };
            var securityNamespaces =
                new Response.Multiple<Response.SecurityNamespace>(securityNamespace1, securityNamespace2,
                    securityNamespace3);

            var client = Substitute.For<IVstsRestClient>();

            client.Get(Arg.Any<IVstsRestRequest<Response.ApplicationGroups>>()).Returns(applicationGroups);

            client.Get(Arg.Any<IVstsRestRequest<Response.Multiple<Response.SecurityNamespace>>>())
                .Returns(securityNamespaces);
            client.Get(Arg.Any<IVstsRestRequest<Response.ProjectProperties>>())
                .Returns(fixture.Create<Response.ProjectProperties>());

            client.Get(Arg.Any<IVstsRestRequest<Response.Multiple<Response.Repository>>>())
                .Returns(fixture.Create<Response.Multiple<Response.Repository>>());
            client.Get(Arg.Any<IVstsRestRequest<Response.Multiple<Response.BuildDefinition>>>())
                .Returns(fixture.Create<Response.Multiple<Response.BuildDefinition>>());
            client.Get(Arg.Any<IVstsRestRequest<Response.Multiple<Response.ReleaseDefinition>>>())
                .Returns(fixture.Create<Response.Multiple<Response.ReleaseDefinition>>());

            client.Get(Arg.Any<IVstsRestRequest<Response.PermissionsSetId>>())
                .Returns(fixture.Create<Response.PermissionsSetId>());

            client.Get(Arg.Any<IVstsRestRequest<Response.PermissionsProjectId>>())
                .Returns(fixture.Create<Response.PermissionsProjectId>());

            var scan = new SecurityReportScan(client);
            var securityReport = scan.Execute("dummy", DateTime.Now);

            securityReport.ShouldNotBeNull();
        }

        [Fact]
        public void ProjectArgumentNullThrowsException()
        {
            var client = Substitute.For<IVstsRestClient>();

            var scan = new SecurityReportScan(client);
            var ex = Assert.Throws<ArgumentNullException>(() => scan.Execute(null, DateTime.Now));
            Assert.Contains("Parameter name: project", ex.Message);
        }

        [Fact]
        public void ApplicationGroupsIsNullThrowsException()
        {
            var client = Substitute.For<IVstsRestClient>();

            var scan = new SecurityReportScan(client);
            var ex = Assert.Throws<ArgumentNullException>(() => scan.Execute("dummy", DateTime.Now));
            Assert.Contains("Parameter name: applicationGroups", ex.Message);
        }

        [Fact]
        public void SecurityNamespacesIsNullThrowsException()
        {
            var applicationGroup1 = new Response.ApplicationGroup
            { DisplayName = "[dummy]\\Project Administrators", TeamFoundationId = "1", };
            var applicationGroup2 = new Response.ApplicationGroup
            { DisplayName = "[dummy]\\Rabobank Project Administrators", TeamFoundationId = "2" };
            var applicationGroups = new Response.ApplicationGroups
            {
                Identities = new[]
                {
                    applicationGroup1, applicationGroup2
                }
            };
            var securityNamespaces = new Response.Multiple<Response.SecurityNamespace> { };

            var client = Substitute.For<IVstsRestClient>();
            client.Get(Arg.Any<IVstsRestRequest<Response.ApplicationGroups>>()).Returns(applicationGroups);
            client.Get(Arg.Any<IVstsRestRequest<Response.Multiple<Response.SecurityNamespace>>>())
                .Returns(securityNamespaces);

            var scan = new SecurityReportScan(client);
            var ex = Assert.Throws<ArgumentNullException>(() => scan.Execute("dummy", DateTime.Now));
            Assert.Contains("Parameter name: securityNamespaces", ex.Message);
        }
    }
}