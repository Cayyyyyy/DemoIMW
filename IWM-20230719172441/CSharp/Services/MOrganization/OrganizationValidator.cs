using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM;
using IWM.Common;
using IWM.Enums;
using IWM.Entities;
using IWM.Repositories;
using System.Text.RegularExpressions;

namespace IWM.Services.MOrganization
{
    public interface IOrganizationValidator : IServiceScoped
    {
        Task Get(Organization Organization);
        Task<bool> Import(List<Organization> Organizations);
    }

    public class OrganizationValidator : IOrganizationValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private OrganizationMessage OrganizationMessage;

        public OrganizationValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.OrganizationMessage = new OrganizationMessage();
        }

        public async Task Get(Organization Organization)
        {
        }


        public async Task<bool> Import(List<Organization> Organizations)
        {
            return true;
        }
        
    }
}
