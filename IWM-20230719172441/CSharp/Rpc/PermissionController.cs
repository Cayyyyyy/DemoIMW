using IWM.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.PER;

namespace IWM.Rpc
{
    public class PermissionController : SimpleController
    {
        private ICurrentContext CurrentContext;
        public PermissionController(ICurrentContext CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }

        [HttpPost, Route("rpc/iwm/permission/list-path")]
        public async Task<List<string>> ListPath()
        {
            List<string> paths = await PermissionBuilder.ListPath(CurrentContext.UserId);
            return paths;
        }
    }
}
