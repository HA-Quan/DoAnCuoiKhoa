using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services.Models.Entities.DTO
{
    public class ProductModel
    {
        public Product Product { get; set; }
        public IFormCollection? ListImages { get; set; }
        public IFormFile? MainImage { get; set; }    

        public List<string>? ListImageView { get; set; }

        public List<Gift>? ListGifts { get; set; }
        
    }
}
