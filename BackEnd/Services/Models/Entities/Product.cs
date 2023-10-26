using Services.Models.Entities.DTO;
using Services.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities
{
    [Table("Product")]
    public class Product : BaseEntity
    {
        [Key]
        [Column("productID")]
        public Guid ProductID { get; set; }

        [Column("productName")]
        public string ProductName { get; set; }

        [Column("mainImage")]
        public string? MainImage { get; set; }

        [Column("listImage")]
        public string? ListImageString { get; set; }

        [ForeignKey("Chip")]
        [Column("chipID")]
        public Guid ChipID { get; set; }

        [Column("chipDetail")]
        public string ChipDetail { get; set; }

        [ForeignKey("Memory")]
        [Column("memoryID")]
        public Guid MemoryID { get; set; }

        [Column("memoryDetail")]
        public string MemoryDetail { get; set; }

        [ForeignKey("Storage")]
        [Column("storageID")]
        public Guid StorageID { get; set; }

        [Column("storageDetail")]
        public string StorageDetail { get; set; }

        [ForeignKey("Display")]
        [Column("displayID")]
        public Guid DisplayID { get; set; }

        [Column("displayDetail")]
        public string DisplayDetail { get; set; }

        [Column("cardType")]
        public bool CardType { get; set; }

        [Column("cardDetail")]
        public string CardDetail { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; } = 0;

        [Column("inventory")]
        public int Inventory { get; set; } = 0;

        [Column("price")]
        public long Price { get; set; } = 0;

        [Column("demandType")]
        public string DemandTypeString { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("condition")]
        public byte Condition { get; set; }

        [Column("discount")]
        public byte Discount { get; set; } = 0;

        [Column("status")]
        public byte Status { get; set; } = (byte)EnumType.StatusProduct.PrepairCome;

        [Column("numberView")]
        public int NumberView { get; set; } = 0;

        [Column("amountRam")]
        public byte? AmountRam { get; set; }

        [Column("amountDisk")]
        public byte? AmountDisk { get; set; }

        [Column("weight")]
        public decimal? Weight { get; set; }

        [Column("dimension")]
        public string? Dimension { get; set; }

        [Column("color")]
        public string? Color { get; set; }

        [Column("material")]
        public string? Material { get; set; }

        [Column("operatingSystem")]
        public string? OperatingSystem { get; set; }

        [Column("touchpad")]
        public string? Touchpad { get; set; }

        [Column("keyboard")]
        public string? Keyboard { get; set; }

        [Column("speakers")]
        public string? Speakers { get; set; }

        [Column("battery")]
        public string? Battery { get; set; }

        [Column("camera")]
        public string? Camera { get; set; }

        [Column("connectivityNetwork")]
        public string? ConnectivityNetwork { get; set; }

        [Column("standardPorts")]
        public string? StandardPorts { get; set; }

        [ForeignKey("Category")]
        [Column("categoryID")]
        public Guid CategoryID { get; set; }
        [Column("categoryName")]
        public string CategoryName { get; set; }
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;

    }
}
