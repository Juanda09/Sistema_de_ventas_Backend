﻿using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contratos
{
    public interface IDashBoardService
    {
        Task<DashBoardDTO> Resumen();

    }
}
