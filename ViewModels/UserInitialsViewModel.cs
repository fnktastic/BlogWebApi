using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.ViewModels
{
    public class UserInitialsViewModel
    {
        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }

        public UserInitialsViewModel(string firstName, string middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }
    }
}
