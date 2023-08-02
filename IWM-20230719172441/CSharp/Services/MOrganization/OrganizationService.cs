using TrueSight.Common;
using IWM.Handlers;
using IWM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using IWM.Repositories;
using IWM.Entities;
using IWM.Enums;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Services.MOrganization
{
    public interface IOrganizationService :  IServiceScoped
    {
        Task<int> Count(OrganizationFilter OrganizationFilter);
        Task<List<Organization>> List(OrganizationFilter OrganizationFilter);
        Task<Organization> Get(long Id);
        Task<List<Organization>> BulkMerge(List<Organization> Organizations);
    }

    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly IOrganizationValidator OrganizationValidator;

        public OrganizationService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IOrganizationValidator OrganizationValidator,
            IRabbitManager RabbitManager

        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.OrganizationValidator = OrganizationValidator;
        }

        public async Task<int> Count(OrganizationFilter OrganizationFilter)
        {
            try
            {
                int result = await UOW.OrganizationRepository.Count(OrganizationFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(OrganizationService));
            }
        }

        public async Task<List<Organization>> List(OrganizationFilter OrganizationFilter)
        {
            try
            {
                List<Organization> Organizations = await UOW.OrganizationRepository.List(OrganizationFilter);
                return Organizations;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(OrganizationService));
            }
        }

        public async Task<Organization> Get(long Id)
        {
            Organization Organization = await UOW.OrganizationRepository.Get(Id);
            if (Organization == null)
                return null;
            await OrganizationValidator.Get(Organization);
            return Organization;
        }
        

        public async Task<List<Organization>> BulkMerge(List<Organization> Organizations)
        {
            if (!await OrganizationValidator.Import(Organizations))
                return Organizations;
            try
            {
                var Ids = await UOW.OrganizationRepository.BulkMerge(Organizations);
                Organizations = await UOW.OrganizationRepository.List(Ids);
                return Organizations;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(OrganizationService));
            }
        }     
        
    }
}
