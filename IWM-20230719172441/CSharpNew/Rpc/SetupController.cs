using IWM.Common;
using IWM.Entities;
using IWM.Handlers;
using IWM.Models;
using IWM.Repositories;
using IWM.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TrueSight;
using TrueSight.Common;
using TrueSight.PER.Entities;
using TrueSight.RabbitMQ.Configuration;
using SubSystem = TrueSight.Entities.SubSystem;
using TrueSight.Entities;
using System.Reflection;

namespace IWM.Rpc
{
    public partial class SetupController : ControllerBase
    {
        private DataContext DataContext;
        private IRabbitManager RabbitManager;
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        public SetupController(
            DataContext DataContext,
            IRabbitManager RabbitManager,
            IUOW UOW,
            ICurrentContext CurrentContext
            )
        {
            this.DataContext = DataContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }
        [HttpGet, Route("rpc/iwm/setup/init")]
        public async Task<ActionResult> Init()
        {
            await InitEnum();
            SendMenu();
            MasterEntityRegister();
            await ApprovalFlowRegister();
            return Ok();
        }

        private void SendMenu()
        {
            Site Site = new Site
            {
                Code = "/iwm/",
                Name = "IWM",
                IsDisplay = true
            };
            List<Type> routeTypes = typeof(SetupController).Assembly.GetTypes()
                .Where(x => typeof(Root).IsAssignableFrom(x) && x.IsClass && x.Name != "Root")
                .ToList();

            List<Menu> Menus = CurrentContext.GenerateMenu(routeTypes);
            Site.Menus = Menus;
            RabbitManager.PublishList(CurrentContext, new List<Site> { Site }, MessageRoutingKey.MenuSend);
        }

        private void MasterEntityRegister()
        {
            List<MasterEntity> MasterEntities = new List<MasterEntity>();

            MasterEntities.ForEach(x => x.CreatedAt = DateTime.Now);
            MasterEntities.ForEach(x => x.UpdatedAt = DateTime.Now);
            MasterEntities.ForEach(x => x.StatusId = Entities.Status.ACTIVE.Id);
            RabbitManager.PublishList(CurrentContext, MasterEntities, MessageRoutingKey.MasterEntityRegister);
        }

        private async Task ApprovalFlowRegister()
        {
            var SubSystems = await UOW.SubSystemRepository.List(new Entities.SubSystemFilter { Skip = 0, Take = 1, Code = new StringFilter { Equal = StaticParams.SubSystemCode }, Selects = Entities.SubSystemSelect.ALL });
            SubSystem SubSystem = SubSystems.FirstOrDefault();

            List<ApprovalType> ApprovalTypes = new List<ApprovalType>();
            List<Type> routeTypes = typeof(SetupController).Assembly.GetTypes()
                .Where(x => typeof(Root).IsAssignableFrom(x) && x.IsClass && x.Name != "Root")
                .ToList();
            foreach (Type type in routeTypes)
            {
                ApprovalType ApprovalType = ApprovalTypes.Where(x => x.Code == type.Name.Remove(type.Name.Length - 5)).FirstOrDefault();
                if (ApprovalType == null) continue;

                ApprovalType.ApprovalConditionalParameters = new List<ApprovalConditionalParameter>();
                ApprovalType.ApprovalDataParameters = new List<ApprovalDataParameter>();

                var conditionalParameters = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => !fi.IsInitOnly && fi.FieldType == typeof(List<ApprovalConditionalParameter>))
                .Select(x => (List<ApprovalConditionalParameter>)x.GetValue(x))
                .FirstOrDefault();
                if (conditionalParameters != null)
                {
                    foreach (var parameter in conditionalParameters)
                    {
                        ApprovalConditionalParameter ApprovalConditionalParameter = new ApprovalConditionalParameter
                        {
                            Code = parameter.Code,
                            Name = parameter.Name,
                            ApprovalParameterTypeId = parameter.ApprovalParameterTypeId,
                            ApprovalTypeId = ApprovalType.Id
                        };
                        ApprovalType.ApprovalConditionalParameters.Add(ApprovalConditionalParameter);
                    }
                }

                var dataParameters = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => !fi.IsInitOnly && fi.FieldType == typeof(List<ApprovalDataParameter>))
                .Select(x => (List<ApprovalDataParameter>)x.GetValue(x))
                .FirstOrDefault();
                if (dataParameters != null)
                {
                    foreach (var parameter in dataParameters)
                    {
                        ApprovalDataParameter ApprovalDataParameter = new ApprovalDataParameter
                        {
                            Code = parameter.Code,
                            Name = parameter.Name,
                            ApprovalParameterTypeId = parameter.ApprovalParameterTypeId,
                            ApprovalTypeId = ApprovalType.Id
                        };
                        ApprovalType.ApprovalDataParameters.Add(ApprovalDataParameter);
                    }
                }
            }
            RabbitManager.PublishList(CurrentContext, ApprovalTypes, MessageRoutingKey.ApprovalTypeRegister);
        }

        private async Task InitEnum()
        {
            List<Type> enumServiceTypes = typeof(BaseService).Assembly.GetTypes()
                    .Where(x => typeof(IEnumServiceScoped).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                    .ToList();
            foreach (Type type in enumServiceTypes)
            {
                IEnumServiceScoped service = (IEnumServiceScoped)Activator.CreateInstance(type, UOW, CurrentContext, RabbitManager);
                await service.Initialize();
            }
        }
    }
}
