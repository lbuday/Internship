using System;
using System.Collections.Generic;

namespace AHP.Model.Common
{
    public interface IUserModel
    {
        ICollection<IChoiceModel> Choices { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
        string Password { get; set; }
        Guid UserID { get; set; }
        string Username { get; set; }
    }
}