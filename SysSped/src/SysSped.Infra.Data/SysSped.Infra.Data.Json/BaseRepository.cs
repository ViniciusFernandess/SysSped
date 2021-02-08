using Newtonsoft.Json;
using SysSped.Domain.Entities.Importacao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysSped.Infra.Data.Json
{
    public abstract class BaseRepository
    {
        protected readonly string path = System.IO.Directory.GetCurrentDirectory() + @"\BD";

    }
}
