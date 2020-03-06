using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Uplift.DataAccess.Data.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null);

        void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null);
        void ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null);
    }
}