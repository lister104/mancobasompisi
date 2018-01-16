using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Data.Models
{
    public sealed class ModelSent : ModelCommonBase, IModelCommon
    {
        public List<ModelApplication> Applications { get; set; }
    }
}
