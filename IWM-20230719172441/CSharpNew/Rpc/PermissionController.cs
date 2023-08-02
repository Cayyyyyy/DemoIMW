using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight;

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
            List<string> paths = await CurrentContext.ListPath();
            return paths;
        }
    }
}
