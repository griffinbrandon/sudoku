using Api.Models;
using refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpPost]
        public List<ICell> Solve(List<ICell> grid)
        {
            var cracker = new Cracker(grid);            
            return cracker.Solve();
        }


    }
}
