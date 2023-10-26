
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    /// <summary>
    /// BaseEntity các trường tái sử dụng
    /// </summary>
    /// Author: HAQUAN (13/09/2023)
    public class BaseEntity
    {
        #region Property

        /// <summary>
        /// Người tạo
        /// </summary>
        [Column("createdBy")]
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [Column("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        [Column("modifiedBy")]
        public Guid? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        [Column("modifiedDate")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        #endregion
    }
}