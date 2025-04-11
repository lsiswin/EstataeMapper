using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary.Models;
using Prism.Events;

namespace EstateMapperClient.Events
{
    public class UserLoggedInEvent : PubSubEvent<UserDto> { }
}
