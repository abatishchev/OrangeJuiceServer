//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrangeJuice.Server.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    internal partial class User
    {
        public User()
        {
            this.Ratings = new HashSet<Rating>();
        }
    
        internal int UserId { get; private set; }
        public System.Guid UserGuid { get; private set; }
        public string Email { get; internal set; }
    
        internal virtual ICollection<Rating> Ratings { get; set; }
    }
}
