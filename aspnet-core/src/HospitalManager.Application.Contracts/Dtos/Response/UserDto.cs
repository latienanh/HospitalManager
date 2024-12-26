using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Response
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }
}
