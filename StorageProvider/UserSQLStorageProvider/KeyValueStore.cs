using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageProvider.UserSQLStorageProvider
{
    class KeyValueStore
    {
        [Key]
        [MaxLength(1024)]
        public string GrainKeyId { get; set; }
        [MaxLength]
        public byte[] BinaryContent { get; set; }
        [MaxLength]
        public string JsonContext { get; set; }
        [MaxLength(100)]
        public string ETag { get; set; }

    }
}
