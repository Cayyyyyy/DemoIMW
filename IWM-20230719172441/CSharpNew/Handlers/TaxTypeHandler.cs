using DotNetCore.CAP;
using TrueSight.Common;
using IWM.Common;
using IWM.Entities;
using IWM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IWM.Services.MTaxType;
using TrueSight;
using TrueSight.RabbitMQ.Configuration;

namespace IWM.Handlers
{
    public class TaxTypeHandler : Handler
    {
        private readonly IUOW UOW;
        private readonly ITaxTypeService TaxTypeService;
        public TaxTypeHandler(ICurrentContext CurrentContext, IRabbitManager RabbitManager, IUOW UOW, ITaxTypeService TaxTypeService)
        {
            this.CurrentContext = CurrentContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.TaxTypeService = TaxTypeService;
        }
        protected override void Initialize(CapHeader headers, object content)
        {
            Preload(headers, content);
            CurrentContext.LoadConfiguration(StaticParams.ConnectionManager);
            UOW.LoadConfiguration();
        }
        
        [CapSubscribe(MessageRoutingKey.TaxTypeSync)]
        public async Task Sync([FromCap] CapHeader Headers, List<TaxType> TaxTypes)
        {
            try
            {
                Initialize(Headers, TaxTypes);
                if (TaxTypes != null && TaxTypes.Count > 0)
                    await TaxTypeService.BulkMerge(TaxTypes);
            }
            catch (Exception ex)
            {
                Log(ex, nameof(TaxTypeHandler));
            }
        }

    }
}