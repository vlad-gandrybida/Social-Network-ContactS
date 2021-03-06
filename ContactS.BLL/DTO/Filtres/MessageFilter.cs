﻿using System;

namespace ContactS.BLL.DTO.Filtres
{
    public class MessageFilter
    {
        public DialogDTO Chat { get; set; }

        public string Text { get; set; }

        public UserDTO Sender { get; set; }

        public DateTime Time { get; set; }
    }
}
