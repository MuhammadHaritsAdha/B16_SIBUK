using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBUK
{
    internal class LaporanModel
    {
        public string transaksiId { get; set; }
        public DateTime tanggal { get; set; }
        public string judul { get; set; }
        public int jumlah { get; set; }
        public decimal hargaSatuan { get; set; }
        public decimal totalHarga { get; set; }
        public string statusBayar { get; set; }
    }
}
