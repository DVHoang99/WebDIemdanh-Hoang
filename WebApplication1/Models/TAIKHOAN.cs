
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace WebApplication1.Models
{

using System;
    using System.Collections.Generic;
    
public partial class TAIKHOAN
{

    public string USERNAME { get; set; }

    public string PASSWORD { get; set; }

    public string Name { get; set; }

    public Nullable<int> ROLE1 { get; set; }

    public string EMAIL { get; set; }



    public virtual ROLE ROLE { get; set; }

}

}
