using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.DataService;

namespace Data.Repository.Authorization.Interface
{
    public interface ILoginRepository
    {
        Task<ExternalDataResultManager<int?>> Login(string? login, string? password);
    }
}
