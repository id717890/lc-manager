﻿using System;

namespace LCManager.Infrastructure.Request
{
    public class AddPhoneRequest
    {
        /// <summary>
        /// идентификатор Участника
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
    }
}
