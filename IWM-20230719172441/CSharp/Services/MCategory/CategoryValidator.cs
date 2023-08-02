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

namespace IWM.Services.MCategory
{
    public interface ICategoryValidator : IServiceScoped
    {
        Task Get(Category Category);
        Task<bool> Import(List<Category> Categories);
    }

    public class CategoryValidator : ICategoryValidator
    {
        private readonly IUOW UOW;
        private readonly ICurrentContext CurrentContext;
        private CategoryMessage CategoryMessage;

        public CategoryValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.CategoryMessage = new CategoryMessage();
        }

        public async Task Get(Category Category)
        {
        }


        public async Task<bool> Import(List<Category> Categories)
        {
            return true;
        }
        
    }
}
