
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
    
public partial class TKB
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TKB()
    {

        this.CHITIETMONHOCs = new HashSet<CHITIETMONHOC>();

        this.FORMLUUTRUs = new HashSet<FORMLUUTRU>();

        this.DIEMDANHs = new HashSet<DIEMDANH>();

    }


    public int ID { get; set; }

    public Nullable<int> NHOM { get; set; }

    public string MAMH { get; set; }

    public string MAGIANGVIEN { get; set; }

    public string PHONG { get; set; }

    public string TENLOP { get; set; }

    public Nullable<System.DateTime> NGAYBATDAU { get; set; }

    public Nullable<System.DateTime> NGAYKETHUC { get; set; }

    public int CA { get; set; }

    public string THU { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CHITIETMONHOC> CHITIETMONHOCs { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FORMLUUTRU> FORMLUUTRUs { get; set; }

    public virtual GIANGVIEN GIANGVIEN { get; set; }

    public virtual LOP LOP { get; set; }

    public virtual MONHOC MONHOC { get; set; }

    public virtual NHOM NHOM1 { get; set; }

    public virtual PHONG PHONG1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<DIEMDANH> DIEMDANHs { get; set; }

}

}
