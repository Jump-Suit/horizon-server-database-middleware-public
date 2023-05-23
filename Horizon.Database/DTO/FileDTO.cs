using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Horizon.Database.DTO
{
    public class FileDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AppId { get; set; }
        public string FileName { get; set; }
        public string ServerChecksum { get; set; }
        public int FileID { get; set; }
        public int FileSize { get; set; }
        public int CreationTimeStamp { get; set; }
        public int OwnerID { get; set; }
        public int GroupID { get; set; }
        public ushort OwnerPermissionRWX { get; set; }
        public ushort GroupPermissionRWX { get; set; }
        public ushort GlobalPermissionRWX { get; set; }
        public ushort ServerOperationID { get; set; }
        public FileAttributesDTO fileAttributesDTO { get; set; }
        public FileMetaDataDTO fileMetaDataDTO { get; set; }
        public DateTime CreateDt { get; set; }
    }
    public partial class FileAttributesDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AppId { get; set; }
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public int LastChangedTimeStamp { get; set; }
        public int LastChangedByUserID { get; set; }
        public int NumberAccesses { get; set; }
        public int StreamableFlag { get; set; }
        public int StreamingDataRate { get; set; }
        public DateTime CreateDt { get; set; }
    }

    public partial class FileMetaDataDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AppId { get; set; }
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreateDt { get; set; }
    }
}