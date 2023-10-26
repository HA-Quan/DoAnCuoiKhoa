﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    /// <summary>
    /// Dữ liệu trả về khi lọc và phân trang
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng trong mảng trả về</typeparam>
    public class PagingData<T>
    {
        /// <summary>
        /// Mảng đối tượng thỏa mãn điều kiện lọc và phân trang 
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        public List<CriteriaModel> ListCriteriaModel { get; set; } = new List<CriteriaModel>();

        /// <summary>
        /// Tổng số bản ghi thỏa mãn điều kiện
        /// </summary>
        public long TotalCount { get; set; }
    }
}
