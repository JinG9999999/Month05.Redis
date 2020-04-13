using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Month05.Redis.Models
{
    public class ProductsModel
    {
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]

        [Display(Name = "品牌名称")]
        public string Pname { get; set; }
        [Display(Name = "品牌图片")]
        public string Photo { get; set; }
        [Display(Name = "品牌链接")]
        public string Purl { get; set; }
        [Display(Name = "备注信息")]
        public string BeiZhu { get; set; }
        [Display(Name = "更新时间")]
        public DateTime UpdTime { get; set; }
    }
}