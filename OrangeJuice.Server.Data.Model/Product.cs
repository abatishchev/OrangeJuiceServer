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
    
    public partial class Product
    {
        public Product()
        {
            this.Ratings = new HashSet<Rating>();
        }
    
        public System.Guid ProductId { get; internal set; }
        public string Barcode { get; set; }
        public byte BarcodeType { get; set; }
    
        public virtual ICollection<Rating> Ratings { get; internal set; }
    }
}