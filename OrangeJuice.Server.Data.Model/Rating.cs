//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrangeJuice.Server.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rating
    {
        internal System.Guid UserId { get; set; }
        internal System.Guid ProductId { get; set; }
        public byte Value { get; set; }
        public string Comment { get; set; }
    
        public virtual Product Product { get; internal set; }
        public virtual User User { get; internal set; }
    }
}
