using System;
using Rules.Reports;
using SecurePipelineScan.Rules.Reports;
using Shouldly;
using Xunit;

namespace SecurePipelineScan.Rules.Tests.Checks
{
    public class SecurityReportTests
    {
        [Theory]

        [InlineData(true, true, true, true, true, true, true, true, true, true, true, true, true, true, true)]
        [InlineData(false, true, true, true, true, true, true, true, true, true, true, true, true, true, false)]
        [InlineData(true, false, true, true, true, true, true, true, true, true, true, true, true, true, false)]
        [InlineData(true, true, false, true, true, true, true, true, true, true, true, true, true, true, false)]
        [InlineData(true, true, true, false, true, true, true, true, true, true, true, true, true, true, false)]
        [InlineData(true, true, true, true, false, true, true, true, true, true, true, true, true, true, false)]
        [InlineData(true, true, true, true, true, false, true, true, true, true, true, true, true, true, false)]
        [InlineData(true, true, true, true, true, true, false, true, true, true, true, true, true, true, false)]
        [InlineData(true, true, true, true, true, true, true, false, true, true, true, true, true, true, false)]
        [InlineData(true, true, true, true, true, true, true, true, false, true, true, true, true, true, false)]
        [InlineData(true, true, true, true, true, true, true, true, true, false, true, true, true, true, false)]
        [InlineData(true, true, true, true, true, true, true, true, true, true, false, true, true, true, false)]
        [InlineData(true, true, true, true, true, true, true, true, true, true, true, false, true, true, false)]
        [InlineData(true, true, true, true, true, true, true, true, true, true, true, true, false, true, false)]
        [InlineData(true, true, true, true, true, true, true, true, true, true, true, true, true, false, false)]
        public void CheckSecurityReport(
            bool a, bool b, bool c, bool d, bool e, bool f,bool g,bool h, bool i,  bool j, bool k, bool l, bool m, bool n, bool expected)
        {
            var securityReport = new SecurityReport(DateTime.Now)
            {
                ApplicationGroupContainsProductionEnvironmentOwner = a,
                ProjectAdminGroupOnlyContainsRabobankProjectAdminGroup = b,

                BuildRightsBuildAdmin = new BuildRights
                {
                    HasNoPermissionsToDeleteBuilds = c,
                    HasNoPermissionsToDeDestroyBuilds = c,
                    HasNoPermissionsToDeleteBuildDefinition = c,
                    HasNoPermissionsToAdministerBuildPermissions = c
                },
                BuildRightsProjectAdmin = new BuildRights
                {
                    HasNoPermissionsToDeleteBuilds = d,
                    HasNoPermissionsToDeDestroyBuilds = d,
                    HasNoPermissionsToDeleteBuildDefinition = d,
                    HasNoPermissionsToAdministerBuildPermissions = d
                },

                RepositoryRightsProjectAdmin = new RepositoryRights
                {
                    HasNotSetToManagePermissionsRepositories = e,
                    HasNotSetToManagePermissionsRepositorySet = e,
                    HasNoPermissionToDeleteRepositorySet = e,
                    HasNoPermissionToDeleteRepositories = e
                },
                
                BuildDefinitionsRightsBuildAdmin = new BuildRights
                {
                    HasNoPermissionsToDeleteBuilds = f,
                    HasNoPermissionsToDeDestroyBuilds = f,
                    HasNoPermissionsToDeleteBuildDefinition = f,
                    HasNoPermissionsToAdministerBuildPermissions = f
                },
                
                BuildDefinitionsRightsProjectAdmin = new BuildRights
                {
                    HasNoPermissionsToDeleteBuilds = g,
                    HasNoPermissionsToDeDestroyBuilds = g,
                    HasNoPermissionsToDeleteBuildDefinition = g,
                    HasNoPermissionsToAdministerBuildPermissions = g
                },
                
                ReleaseRightsContributor = new ContributorsReleaseRights
                {
                    HasNoPermissionToCreateReleases = h,
                    HasNoPermissionToDeleteReleases = h,
                    HasNoPermissionToManageReleaseApprovers = h,
                    HasNoPermissionToDeleteReleasePipeline = h,
                    HasNoPermissionsToAdministerReleasePermissions = h,
                    HasNotSetToManageReleaseApprovers = h,
                    HasPermissionToCreateReleases = h,
                    HasPermissionToManageReleaseApprovers = h,
                },
               
                ReleaseRightsRaboProjectAdmin = new RaboAdminReleaseRights
                {
                    HasNoPermissionToCreateReleases = i,
                    HasNoPermissionToDeleteReleases = i,
                    HasNoPermissionToManageReleaseApprovers = i,
                    HasNoPermissionToDeleteReleasePipeline = i,
                    HasNoPermissionsToAdministerReleasePermissions = i,
                    HasPermissionToManageReleaseApprovers = i,
                    HasPermissionToCreateReleases = i,
                    HasNotSetToManageReleaseApprovers = i,
                    
                },
                
                ReleaseRightsProductionEnvOwner = new ProductionEnvOwnerReleaseRights
                {
                    HasNoPermissionToCreateReleases = j,
                    HasPermissionToManageReleaseApprovers = j,
                    HasPermissionToCreateReleases = j,
                    HasNoPermissionToDeleteReleases = j,
                    HasNotSetToManageReleaseApprovers = j,
                    HasNoPermissionToDeleteReleasePipeline = j,
                    HasNoPermissionToManageReleaseApprovers = j,
                    HasNoPermissionsToAdministerReleasePermissions = j,
                },
                
                ReleaseDefintionsRightsContributor = new ContributorsReleaseRights()
                {
                    HasNoPermissionToCreateReleases = k,
                    HasNoPermissionToDeleteReleases = k,
                    HasNoPermissionToDeleteReleasePipeline = k,
                    HasNoPermissionToManageReleaseApprovers = k,
                    HasNoPermissionsToAdministerReleasePermissions = k,
                    HasPermissionToCreateReleases = k,
                    HasPermissionToManageReleaseApprovers = k,
                    HasNotSetToManageReleaseApprovers = k,
                },
                
                ReleaseDefinitionsRightsRaboProjectAdmin = new RaboAdminReleaseRights()
                {
                    HasNoPermissionToCreateReleases = l,
                    HasNoPermissionToDeleteReleases = l,
                    HasNoPermissionToDeleteReleasePipeline = l,
                    HasNoPermissionToManageReleaseApprovers = l,
                    HasNoPermissionsToAdministerReleasePermissions = l,
                    HasPermissionToCreateReleases = l,
                    HasPermissionToManageReleaseApprovers = l,
                    HasNotSetToManageReleaseApprovers = l,
                },
                
                ReleaseDefintionsRightsProductionEnvOwner = new ProductionEnvOwnerReleaseRights()
                {
                    HasNoPermissionToCreateReleases = m,
                    HasPermissionToManageReleaseApprovers = m,
                    HasPermissionToCreateReleases = m,
                    HasNoPermissionToDeleteReleases = m,
                    HasNotSetToManageReleaseApprovers = m,
                    HasNoPermissionToDeleteReleasePipeline = m,
                    HasNoPermissionToManageReleaseApprovers = m,
                    HasNoPermissionsToAdministerReleasePermissions = m,
                },

                TeamRabobankProjectAdministrators = new GlobalRights
                {
                    HasNoPermissionToDeleteTeamProject = n,
                    HasNoPermissionToPermanentlyDeleteWorkitems = n,
                    HasNoPermissionToManageProjectProperties = n
                }

            };
            
            securityReport.ProjectIsSecure.ShouldBe(expected);
        }

    }
}