
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
    
public partial class DIEMDANH
{

    public int ID { get; set; }

    public string MASINHVIEN { get; set; }

    public string TENSINHVIEN { get; set; }

    public string MAGIANGVIEN { get; set; }

    public string MAMON { get; set; }

    public Nullable<System.DateTime> NGAYDIENDANH { get; set; }



    public virtual GIANGVIEN GIANGVIEN { get; set; }

    public virtual MONHOC MONHOC { get; set; }

    public virtual SINHVIEN SINHVIEN { get; set; }

}

}
