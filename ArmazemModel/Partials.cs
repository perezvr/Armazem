﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemModel
{
    public partial class Produto
    {

    }

    public partial class Requisicao
    {
        public List<Item_Requisicao> Itens
        {
            get
            {
                return Item_Requisicao.ToList();
            }
        }
    }
}
