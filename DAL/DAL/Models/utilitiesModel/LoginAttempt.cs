using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.utilitiesModel
{
    public class LoginAttempt
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public DateTime AttemptedAt { get; set; }
        public bool IsSucceeded { get; set; }
        public string? RemoteIpAddress { get; set; }



    }
}
