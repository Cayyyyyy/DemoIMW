using TrueSight;
using TrueSight.Common;
using IWM.Handlers;
using IWM.Common;
using IWM.Repositories;
using IWM.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Services.MCategory
{
    public interface ICategoryService : IServiceScoped
    {
        Task<int> Count(CategoryFilter CategoryFilter);
        Task<List<Category>> List(CategoryFilter CategoryFilter);
        Task<Category> Get(long Id);
        Task<List<Category>> BulkMerge(List<Category> Categories);
    }

    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUOW UOW;
        private readonly IRabbitManager RabbitManager;
        private readonly ICurrentContext CurrentContext;
        private readonly ICategoryValidator CategoryValidator;
        public CategoryService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            ICategoryValidator CategoryValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.CategoryValidator = CategoryValidator;
        }

        public async Task<int> Count(CategoryFilter CategoryFilter)
        {
            try
            {
                int result = await UOW.CategoryRepository.Count(CategoryFilter);
                return result;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(CategoryService));
            }
        }

        public async Task<List<Category>> List(CategoryFilter CategoryFilter)
        {
            try
            {
                List<Category> Categories = await UOW.CategoryRepository.List(CategoryFilter);
                return Categories;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(CategoryService));
            }
        }

        public async Task<Category> Get(long Id)
        {
            Category Category = await UOW.CategoryRepository.Get(Id);
            if (Category == null)
                return null;
            await CategoryValidator.Get(Category);
            return Category;
        }
        

        public async Task<List<Category>> BulkMerge(List<Category> Categories)
        {
            if (!await CategoryValidator.Import(Categories))
                return Categories;
            try
            {
                var Ids = await UOW.CategoryRepository.BulkMerge(Categories);
                Categories = await UOW.CategoryRepository.List(Ids);
                return Categories;
            }
            catch (Exception ex)
            {
                throw new MessageException(ex, nameof(CategoryService));
            }
        }
        
    }
}
