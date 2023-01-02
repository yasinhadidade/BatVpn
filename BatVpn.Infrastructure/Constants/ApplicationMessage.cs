using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Infrastructure.Constants
{
    public class ApplicationMessage
    {
        public const string YouAreHasentThis = "You already is following this page";
        public const string RecordNotFound = "Record Not Found";
        public const string DuplicatedRecord = "Operation Failed. Because It Can Create Duplication In DataBase";
        public const string UserNotFound = "User Not Found";
        public const string NotFound = " Not Found";
        public const string InvalidUser = "This User Is Not Valid";
        public const string BadRequest = "Bad Request Recieved";
        public const string OperationFailed = "OperationFailed";
        public const string YouAreNotOwner = "YouAreNotOwner";
    }
}
